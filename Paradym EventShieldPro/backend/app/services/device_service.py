"""
EventShield Pro - Enhanced Device Communication Service
Robust F881 device communication with error handling and recovery
"""

import asyncio
import socket
import struct
import time
import json
from typing import Dict, List, Optional, Any, Tuple
from dataclasses import dataclass
from enum import Enum
import logging
from datetime import datetime, timedelta
import threading
from concurrent.futures import ThreadPoolExecutor

from app.core.exceptions import DeviceConnectionError, NetworkError
from app.core.logging import logger
from app.core.config import get_config


class DeviceStatus(Enum):
    """Device status enumeration"""
    OFFLINE = "offline"
    CONNECTING = "connecting"
    ONLINE = "online"
    ERROR = "error"
    MAINTENANCE = "maintenance"


class CommunicationMode(Enum):
    """Communication mode enumeration"""
    DISABLED = 0
    UDP = 1
    TCP_CLIENT = 2
    TCP_CLIENT_TLS = 3
    MQTT_TCP = 4
    MQTT_TCP_TLS = 5


@dataclass
class DeviceInfo:
    """Device information structure"""
    device_id: str
    device_sn: str
    ip_address: str
    port: int
    password: str
    communication_mode: CommunicationMode
    status: DeviceStatus
    last_seen: Optional[datetime]
    connection_attempts: int = 0
    last_error: Optional[str] = None
    configuration: Dict[str, Any] = None


@dataclass
class DeviceCommand:
    """Device command structure"""
    command_id: str
    command_type: str
    parameters: Dict[str, Any]
    timeout: int = 5000
    retry_count: int = 3
    created_at: datetime = None
    
    def __post_init__(self):
        if self.created_at is None:
            self.created_at = datetime.utcnow()


class F881DeviceManager:
    """Enhanced F881 device communication manager"""
    
    def __init__(self):
        self.config = get_config()
        self.devices: Dict[str, DeviceInfo] = {}
        self.connections: Dict[str, socket.socket] = {}
        self.command_queue: Dict[str, List[DeviceCommand]] = {}
        self.monitoring_thread: Optional[threading.Thread] = None
        self.is_monitoring = False
        self.executor = ThreadPoolExecutor(max_workers=10)
        self.connection_lock = threading.Lock()
        
        # Start monitoring thread
        self.start_monitoring()
    
    def add_device(self, device_info: DeviceInfo) -> bool:
        """Add device to management"""
        try:
            self.devices[device_info.device_id] = device_info
            self.command_queue[device_info.device_id] = []
            
            logger.device_logger.info(
                "Device Added",
                device_id=device_info.device_id,
                device_sn=device_info.device_sn,
                ip_address=device_info.ip_address,
                port=device_info.port
            )
            
            return True
        except Exception as e:
            logger.device_logger.error(
                "Failed to add device",
                device_id=device_info.device_id,
                error=str(e)
            )
            return False
    
    def remove_device(self, device_id: str) -> bool:
        """Remove device from management"""
        try:
            # Disconnect if connected
            if device_id in self.connections:
                self.disconnect_device(device_id)
            
            # Remove from tracking
            if device_id in self.devices:
                del self.devices[device_id]
            
            if device_id in self.command_queue:
                del self.command_queue[device_id]
            
            logger.device_logger.info(
                "Device Removed",
                device_id=device_id
            )
            
            return True
        except Exception as e:
            logger.device_logger.error(
                "Failed to remove device",
                device_id=device_id,
                error=str(e)
            )
            return False
    
    async def connect_device(self, device_id: str) -> bool:
        """Connect to device with retry logic"""
        if device_id not in self.devices:
            raise DeviceConnectionError(f"Device {device_id} not found")
        
        device = self.devices[device_id]
        device.status = DeviceStatus.CONNECTING
        
        try:
            # Attempt connection based on communication mode
            if device.communication_mode == CommunicationMode.TCP_CLIENT:
                success = await self._connect_tcp(device)
            elif device.communication_mode == CommunicationMode.UDP:
                success = await self._connect_udp(device)
            elif device.communication_mode in [CommunicationMode.MQTT_TCP, CommunicationMode.MQTT_TCP_TLS]:
                success = await self._connect_mqtt(device)
            else:
                raise DeviceConnectionError(f"Unsupported communication mode: {device.communication_mode}")
            
            if success:
                device.status = DeviceStatus.ONLINE
                device.last_seen = datetime.utcnow()
                device.connection_attempts = 0
                device.last_error = None
                
                logger.device_logger.info(
                    "Device Connected",
                    device_id=device_id,
                    device_sn=device.device_sn,
                    communication_mode=device.communication_mode.value
                )
            else:
                device.status = DeviceStatus.ERROR
                device.connection_attempts += 1
                
                logger.device_logger.error(
                    "Device Connection Failed",
                    device_id=device_id,
                    device_sn=device.device_sn,
                    attempt=device.connection_attempts
                )
            
            return success
            
        except Exception as e:
            device.status = DeviceStatus.ERROR
            device.connection_attempts += 1
            device.last_error = str(e)
            
            logger.device_logger.error(
                "Device Connection Error",
                device_id=device_id,
                device_sn=device.device_sn,
                error=str(e),
                attempt=device.connection_attempts
            )
            
            raise DeviceConnectionError(
                f"Failed to connect to device {device_id}",
                device_id=device_id,
                device_sn=device.device_sn,
                connection_type=device.communication_mode.value,
                details={"error": str(e), "attempt": device.connection_attempts}
            )
    
    async def _connect_tcp(self, device: DeviceInfo) -> bool:
        """Establish TCP connection to device"""
        try:
            sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            sock.settimeout(self.config.device.connection_timeout / 1000)
            
            # Connect to device
            sock.connect((device.ip_address, device.port))
            
            # Store connection
            with self.connection_lock:
                self.connections[device.device_id] = sock
            
            # Send authentication if required
            if device.password:
                await self._authenticate_device(device)
            
            return True
            
        except socket.timeout:
            raise DeviceConnectionError(
                f"Connection timeout to device {device.device_id}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                connection_type="TCP"
            )
        except socket.error as e:
            raise DeviceConnectionError(
                f"Socket error connecting to device {device.device_id}: {e}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                connection_type="TCP"
            )
    
    async def _connect_udp(self, device: DeviceInfo) -> bool:
        """Establish UDP connection to device"""
        try:
            sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            sock.settimeout(self.config.device.connection_timeout / 1000)
            
            # Store connection
            with self.connection_lock:
                self.connections[device.device_id] = sock
            
            # Send discovery packet
            discovery_packet = self._create_discovery_packet(device)
            sock.sendto(discovery_packet, (device.ip_address, device.port))
            
            # Wait for response
            response, addr = sock.recvfrom(1024)
            
            if self._validate_discovery_response(response, device):
                return True
            else:
                return False
                
        except socket.timeout:
            raise DeviceConnectionError(
                f"UDP discovery timeout for device {device.device_id}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                connection_type="UDP"
            )
        except socket.error as e:
            raise DeviceConnectionError(
                f"UDP socket error for device {device.device_id}: {e}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                connection_type="UDP"
            )
    
    async def _connect_mqtt(self, device: DeviceInfo) -> bool:
        """Establish MQTT connection to device"""
        # This would implement MQTT connection
        # For now, return False as MQTT implementation is not complete
        logger.device_logger.warning(
            "MQTT connection not implemented",
            device_id=device.device_id,
            communication_mode=device.communication_mode.value
        )
        return False
    
    async def _authenticate_device(self, device: DeviceInfo) -> bool:
        """Authenticate with device using password"""
        try:
            # Create authentication packet
            auth_packet = self._create_auth_packet(device.password)
            
            # Send authentication
            sock = self.connections.get(device.device_id)
            if sock:
                sock.send(auth_packet)
                
                # Wait for authentication response
                response = sock.recv(1024)
                
                if self._validate_auth_response(response):
                    return True
            
            return False
            
        except Exception as e:
            logger.device_logger.error(
                "Device authentication failed",
                device_id=device.device_id,
                error=str(e)
            )
            return False
    
    def _create_discovery_packet(self, device: DeviceInfo) -> bytes:
        """Create UDP discovery packet"""
        # This would create the actual discovery packet based on F881 protocol
        # For now, return a placeholder
        return b"DISCOVERY_PACKET"
    
    def _validate_discovery_response(self, response: bytes, device: DeviceInfo) -> bool:
        """Validate UDP discovery response"""
        # This would validate the actual discovery response
        # For now, return True as placeholder
        return True
    
    def _create_auth_packet(self, password: str) -> bytes:
        """Create authentication packet"""
        # This would create the actual authentication packet based on F881 protocol
        # For now, return a placeholder
        return password.encode('utf-8')
    
    def _validate_auth_response(self, response: bytes) -> bool:
        """Validate authentication response"""
        # This would validate the actual authentication response
        # For now, return True as placeholder
        return True
    
    async def disconnect_device(self, device_id: str) -> bool:
        """Disconnect from device"""
        try:
            if device_id in self.connections:
                sock = self.connections[device_id]
                sock.close()
                
                with self.connection_lock:
                    del self.connections[device_id]
            
            if device_id in self.devices:
                self.devices[device_id].status = DeviceStatus.OFFLINE
                self.devices[device_id].last_seen = None
            
            logger.device_logger.info(
                "Device Disconnected",
                device_id=device_id
            )
            
            return True
            
        except Exception as e:
            logger.device_logger.error(
                "Failed to disconnect device",
                device_id=device_id,
                error=str(e)
            )
            return False
    
    async def send_command(self, device_id: str, command: DeviceCommand) -> Dict[str, Any]:
        """Send command to device with retry logic"""
        if device_id not in self.devices:
            raise DeviceConnectionError(f"Device {device_id} not found")
        
        device = self.devices[device_id]
        
        # Check if device is online
        if device.status != DeviceStatus.ONLINE:
            raise DeviceConnectionError(
                f"Device {device_id} is not online",
                device_id=device_id,
                device_sn=device.device_sn
            )
        
        # Add command to queue
        self.command_queue[device_id].append(command)
        
        # Execute command with retry logic
        for attempt in range(command.retry_count):
            try:
                result = await self._execute_command(device, command)
                
                logger.device_logger.info(
                    "Command Executed Successfully",
                    device_id=device_id,
                    command_id=command.command_id,
                    command_type=command.command_type,
                    attempt=attempt + 1
                )
                
                return result
                
            except Exception as e:
                logger.device_logger.warning(
                    "Command Execution Failed",
                    device_id=device_id,
                    command_id=command.command_id,
                    command_type=command.command_type,
                    attempt=attempt + 1,
                    error=str(e)
                )
                
                if attempt == command.retry_count - 1:
                    # Final attempt failed
                    raise DeviceConnectionError(
                        f"Command execution failed after {command.retry_count} attempts",
                        device_id=device_id,
                        device_sn=device.device_sn,
                        details={"command_id": command.command_id, "error": str(e)}
                    )
                
                # Wait before retry
                await asyncio.sleep(1)
    
    async def _execute_command(self, device: DeviceInfo, command: DeviceCommand) -> Dict[str, Any]:
        """Execute command on device"""
        sock = self.connections.get(device.device_id)
        if not sock:
            raise DeviceConnectionError(
                f"No connection to device {device.device_id}",
                device_id=device.device_id,
                device_sn=device.device_sn
            )
        
        try:
            # Create command packet
            command_packet = self._create_command_packet(command)
            
            # Send command
            sock.send(command_packet)
            
            # Wait for response
            sock.settimeout(command.timeout / 1000)
            response = sock.recv(4096)
            
            # Parse response
            result = self._parse_command_response(response, command)
            
            return result
            
        except socket.timeout:
            raise DeviceConnectionError(
                f"Command timeout for device {device.device_id}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                details={"command_id": command.command_id, "timeout": command.timeout}
            )
        except Exception as e:
            raise DeviceConnectionError(
                f"Command execution error for device {device.device_id}: {e}",
                device_id=device.device_id,
                device_sn=device.device_sn,
                details={"command_id": command.command_id, "error": str(e)}
            )
    
    def _create_command_packet(self, command: DeviceCommand) -> bytes:
        """Create command packet based on F881 protocol"""
        # This would create the actual command packet based on F881 protocol
        # For now, return a placeholder
        packet_data = {
            "command_id": command.command_id,
            "command_type": command.command_type,
            "parameters": command.parameters,
            "timestamp": command.created_at.isoformat()
        }
        return json.dumps(packet_data).encode('utf-8')
    
    def _parse_command_response(self, response: bytes, command: DeviceCommand) -> Dict[str, Any]:
        """Parse command response from device"""
        try:
            # Try to parse as JSON first
            response_data = json.loads(response.decode('utf-8'))
            return response_data
        except (json.JSONDecodeError, UnicodeDecodeError):
            # If not JSON, return raw response
            return {
                "raw_response": response.hex(),
                "command_id": command.command_id,
                "success": True
            }
    
    def start_monitoring(self):
        """Start device monitoring thread"""
        if not self.is_monitoring:
            self.is_monitoring = True
            self.monitoring_thread = threading.Thread(target=self._monitor_devices, daemon=True)
            self.monitoring_thread.start()
            
            logger.device_logger.info("Device monitoring started")
    
    def stop_monitoring(self):
        """Stop device monitoring thread"""
        self.is_monitoring = False
        if self.monitoring_thread:
            self.monitoring_thread.join(timeout=5)
            
        logger.device_logger.info("Device monitoring stopped")
    
    def _monitor_devices(self):
        """Monitor device health and connectivity"""
        while self.is_monitoring:
            try:
                for device_id, device in self.devices.items():
                    # Check if device needs reconnection
                    if device.status == DeviceStatus.ERROR and device.connection_attempts < 5:
                        # Attempt reconnection
                        asyncio.run(self.connect_device(device_id))
                    
                    # Check if device is online but hasn't been seen recently
                    elif device.status == DeviceStatus.ONLINE:
                        if device.last_seen and (datetime.utcnow() - device.last_seen).seconds > 60:
                            # Device hasn't been seen for 60 seconds, mark as offline
                            device.status = DeviceStatus.OFFLINE
                            logger.device_logger.warning(
                                "Device marked as offline due to inactivity",
                                device_id=device_id,
                                last_seen=device.last_seen
                            )
                
                # Sleep for monitoring interval
                time.sleep(self.config.device.keep_alive_interval)
                
            except Exception as e:
                logger.device_logger.error(
                    "Error in device monitoring",
                    error=str(e)
                )
                time.sleep(5)  # Wait before retrying
    
    def get_device_status(self, device_id: str) -> Optional[Dict[str, Any]]:
        """Get device status information"""
        if device_id not in self.devices:
            return None
        
        device = self.devices[device_id]
        return {
            "device_id": device.device_id,
            "device_sn": device.device_sn,
            "ip_address": device.ip_address,
            "port": device.port,
            "status": device.status.value,
            "communication_mode": device.communication_mode.value,
            "last_seen": device.last_seen.isoformat() if device.last_seen else None,
            "connection_attempts": device.connection_attempts,
            "last_error": device.last_error,
            "is_connected": device_id in self.connections
        }
    
    def get_all_devices_status(self) -> List[Dict[str, Any]]:
        """Get status of all devices"""
        return [self.get_device_status(device_id) for device_id in self.devices.keys()]
    
    def discover_devices(self, network_range: str = "192.168.1.0/24") -> List[Dict[str, Any]]:
        """Discover F881 devices on network"""
        discovered_devices = []
        
        try:
            # This would implement actual device discovery
            # For now, return empty list as placeholder
            logger.device_logger.info(
                "Device discovery started",
                network_range=network_range
            )
            
            # Placeholder implementation
            # In real implementation, this would:
            # 1. Send UDP broadcast packets
            # 2. Listen for device responses
            # 3. Parse device information from responses
            # 4. Return list of discovered devices
            
        except Exception as e:
            logger.device_logger.error(
                "Device discovery failed",
                network_range=network_range,
                error=str(e)
            )
        
        return discovered_devices
    
    def __del__(self):
        """Cleanup on destruction"""
        self.stop_monitoring()
        if hasattr(self, 'executor'):
            self.executor.shutdown(wait=False)


# Global device manager instance
device_manager = F881DeviceManager()
