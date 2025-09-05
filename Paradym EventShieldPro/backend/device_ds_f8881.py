#!/usr/bin/env python3
"""
EventShield Pro - DS-F8881 Device Communication Layer
Full control implementation for face recognition and access control device
"""

import socket
import struct
import time
import json
import threading
from typing import Dict, List, Optional, Tuple, Any
from dataclasses import dataclass
from enum import Enum
import logging

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class CommunicationMode(Enum):
    """Device communication modes"""
    DISABLED = 0
    UDP = 1
    TCP_CLIENT = 2
    TCP_CLIENT_TLS = 3
    MQTT_TCP = 4
    MQTT_TCP_TLS = 5

class ConnectionStatus(Enum):
    """Device connection status"""
    DISCONNECTED = 0
    CONNECTED = 1
    UDP_DISCONNECTED = 2
    DISABLED = 255

@dataclass
class DeviceConfig:
    """Device configuration parameters"""
    ip: str
    port: int = 8000
    sn: str = "0000000000000000"
    password: str = "FFFFFFFF"
    communication_mode: CommunicationMode = CommunicationMode.TCP_CLIENT
    timeout: int = 5000
    retry_count: int = 3

@dataclass
class DeviceStatus:
    """Device status information"""
    online: bool = False
    last_keepalive: Optional[float] = None
    connection_status: ConnectionStatus = ConnectionStatus.DISCONNECTED
    client_model: CommunicationMode = CommunicationMode.DISABLED
    server_ip: str = ""
    mac_address: str = ""
    firmware_version: str = ""

class DSF8881Device:
    """Main device communication class for DS-F8881"""
    
    def __init__(self, config: DeviceConfig):
        self.config = config
        self.status = DeviceStatus()
        self.tcp_socket = None
        self.udp_socket = None
        self.connected = False
        self.command_queue = []
        self.response_handlers = {}
        self.keepalive_thread = None
        self.monitoring_thread = None
        self._lock = threading.Lock()
        
        # Initialize response handlers
        self._init_response_handlers()
    
    def _init_response_handlers(self):
        """Initialize response handlers for different commands"""
        self.response_handlers = {
            b'\x01': self._handle_ack_response,
            b'\x02': self._handle_nack_response,
            b'\x03': self._handle_data_response,
            b'\x04': self._handle_status_response,
            b'\x05': self._handle_transaction_response,
        }
    
    def connect_tcp(self) -> bool:
        """Establish TCP connection to device"""
        try:
            with self._lock:
                if self.tcp_socket:
                    self.tcp_socket.close()
                
                self.tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                self.tcp_socket.settimeout(self.config.timeout / 1000.0)
                self.tcp_socket.connect((self.config.ip, self.config.port))
                
                # Send authentication packet
                auth_packet = self._create_auth_packet()
                self.tcp_socket.send(auth_packet)
                
                # Wait for authentication response
                response = self.tcp_socket.recv(1024)
                if self._verify_auth_response(response):
                    self.connected = True
                    self.status.connection_status = ConnectionStatus.CONNECTED
                    self.status.online = True
                    logger.info(f"TCP connection established to {self.config.ip}:{self.config.port}")
                    
                    # Start monitoring threads
                    self._start_monitoring()
                    return True
                else:
                    logger.error("Authentication failed")
                    return False
                    
        except Exception as e:
            logger.error(f"TCP connection failed: {e}")
            self.status.connection_status = ConnectionStatus.DISCONNECTED
            self.status.online = False
            return False
    
    def connect_udp(self) -> bool:
        """Establish UDP connection for device discovery and management"""
        try:
            with self._lock:
                if self.udp_socket:
                    self.udp_socket.close()
                
                self.udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
                self.udp_socket.settimeout(self.config.timeout / 1000.0)
                self.udp_socket.bind(('', 0))  # Bind to any available port
                
                logger.info(f"UDP connection established for device discovery")
                return True
                
        except Exception as e:
            logger.error(f"UDP connection failed: {e}")
            return False
    
    def disconnect(self):
        """Disconnect from device"""
        with self._lock:
            if self.tcp_socket:
                self.tcp_socket.close()
                self.tcp_socket = None
            
            if self.udp_socket:
                self.udp_socket.close()
                self.udp_socket = None
            
            self.connected = False
            self.status.connection_status = ConnectionStatus.DISCONNECTED
            self.status.online = False
            
            # Stop monitoring threads
            self._stop_monitoring()
            
            logger.info("Device disconnected")
    
    def _create_auth_packet(self) -> bytes:
        """Create authentication packet"""
        # Format: [Header][SN][Password][Checksum]
        header = b'\xAA\x55'  # Packet header
        sn_bytes = self.config.sn.encode('ascii')
        password_bytes = self.config.password.encode('ascii')
        
        # Pad to required lengths
        sn_bytes = sn_bytes.ljust(16, b'\x00')
        password_bytes = password_bytes.ljust(8, b'\x00')
        
        # Calculate checksum
        data = header + sn_bytes + password_bytes
        checksum = sum(data) & 0xFF
        
        return data + bytes([checksum])
    
    def _verify_auth_response(self, response: bytes) -> bool:
        """Verify authentication response"""
        if len(response) < 3:
            return False
        
        # Check response header and status
        if response[0:2] == b'\xAA\x55' and response[2] == 0x01:
            return True
        return False
    
    def _start_monitoring(self):
        """Start device monitoring threads"""
        if not self.keepalive_thread:
            self.keepalive_thread = threading.Thread(target=self._keepalive_worker, daemon=True)
            self.keepalive_thread.start()
        
        if not self.monitoring_thread:
            self.monitoring_thread = threading.Thread(target=self._monitoring_worker, daemon=True)
            self.monitoring_thread.start()
    
    def _stop_monitoring(self):
        """Stop device monitoring threads"""
        self.keepalive_thread = None
        self.monitoring_thread = None
    
    def _keepalive_worker(self):
        """Keep-alive packet worker thread"""
        while self.keepalive_thread and self.connected:
            try:
                self.send_keepalive()
                time.sleep(30)  # Send keep-alive every 30 seconds
            except Exception as e:
                logger.error(f"Keep-alive error: {e}")
                break
    
    def _monitoring_worker(self):
        """Device monitoring worker thread"""
        while self.monitoring_thread and self.connected:
            try:
                if self.tcp_socket:
                    # Check for incoming data
                    self.tcp_socket.settimeout(1.0)
                    try:
                        data = self.tcp_socket.recv(1024)
                        if data:
                            self._handle_incoming_data(data)
                    except socket.timeout:
                        pass
                    except Exception as e:
                        logger.error(f"Data receive error: {e}")
                        break
                
                time.sleep(0.1)  # Small delay to prevent CPU spinning
                
            except Exception as e:
                logger.error(f"Monitoring error: {e}")
                break
    
    def _handle_incoming_data(self, data: bytes):
        """Handle incoming data from device"""
        try:
            if len(data) < 3:
                return
            
            # Parse packet header
            header = data[0:2]
            if header != b'\xAA\x55':
                return
            
            # Parse packet type
            packet_type = data[2]
            
            # Route to appropriate handler
            if packet_type in self.response_handlers:
                self.response_handlers[packet_type](data)
            else:
                logger.warning(f"Unknown packet type: {packet_type}")
                
        except Exception as e:
            logger.error(f"Error handling incoming data: {e}")
    
    def _handle_ack_response(self, data: bytes):
        """Handle ACK response"""
        logger.debug("Received ACK response")
    
    def _handle_nack_response(self, data: bytes):
        """Handle NACK response"""
        logger.warning("Received NACK response")
    
    def _handle_data_response(self, data: bytes):
        """Handle data response"""
        logger.debug("Received data response")
    
    def _handle_status_response(self, data: bytes):
        """Handle status response"""
        logger.debug("Received status response")
    
    def _handle_transaction_response(self, data: bytes):
        """Handle transaction response"""
        logger.debug("Received transaction response")
    
    def send_command(self, command: bytes) -> bool:
        """Send command to device"""
        if not self.connected or not self.tcp_socket:
            logger.error("Device not connected")
            return False
        
        try:
            with self._lock:
                self.tcp_socket.send(command)
                logger.debug(f"Command sent: {command.hex()}")
                return True
        except Exception as e:
            logger.error(f"Failed to send command: {e}")
            return False
    
    def read_device_sn(self) -> Optional[str]:
        """Read device serial number"""
        try:
            # Command: Read SN
            command = self._create_read_sn_command()
            if self.send_command(command):
                # Wait for response and parse
                response = self._wait_for_response(timeout=5.0)
                if response:
                    sn = self._parse_sn_response(response)
                    if sn:
                        self.config.sn = sn
                        logger.info(f"Device SN: {sn}")
                        return sn
            
            return None
        except Exception as e:
            logger.error(f"Failed to read device SN: {e}")
            return None
    
    def read_tcp_settings(self) -> Optional[Dict[str, Any]]:
        """Read device TCP settings"""
        try:
            # Command: Read TCP Settings
            command = self._create_read_tcp_command()
            if self.send_command(command):
                response = self._wait_for_response(timeout=5.0)
                if response:
                    settings = self._parse_tcp_response(response)
                    if settings:
                        logger.info(f"TCP Settings: {settings}")
                        return settings
            
            return None
        except Exception as e:
            logger.error(f"Failed to read TCP settings: {e}")
            return None
    
    def write_tcp_settings(self, settings: Dict[str, Any]) -> bool:
        """Write device TCP settings"""
        try:
            # Command: Write TCP Settings
            command = self._create_write_tcp_command(settings)
            if self.send_command(command):
                response = self._wait_for_response(timeout=5.0)
                if response and self._verify_write_response(response):
                    logger.info("TCP settings updated successfully")
                    return True
            
            return False
        except Exception as e:
            logger.error(f"Failed to write TCP settings: {e}")
            return False
    
    def read_client_work_mode(self) -> Optional[CommunicationMode]:
        """Read client work mode"""
        try:
            command = self._create_read_work_mode_command()
            if self.send_command(command):
                response = self._wait_for_response(timeout=5.0)
                if response:
                    mode = self._parse_work_mode_response(response)
                    if mode is not None:
                        self.config.communication_mode = mode
                        logger.info(f"Client work mode: {mode}")
                        return mode
            
            return None
        except Exception as e:
            logger.error(f"Failed to read work mode: {e}")
            return None
    
    def write_client_work_mode(self, mode: CommunicationMode) -> bool:
        """Write client work mode"""
        try:
            command = self._create_write_work_mode_command(mode)
            if self.send_command(command):
                response = self._wait_for_response(timeout=5.0)
                if response and self._verify_write_response(response):
                    self.config.communication_mode = mode
                    logger.info(f"Client work mode updated to: {mode}")
                    return True
            
            return False
        except Exception as e:
            logger.error(f"Failed to write work mode: {e}")
            return False
    
    def send_keepalive(self) -> bool:
        """Send keep-alive packet"""
        try:
            command = self._create_keepalive_command()
            if self.send_command(command):
                response = self._wait_for_response(timeout=2.0)
                if response:
                    self.status.last_keepalive = time.time()
                    logger.debug("Keep-alive sent successfully")
                    return True
            
            return False
        except Exception as e:
            logger.error(f"Failed to send keep-alive: {e}")
            return False
    
    def read_client_status(self) -> Optional[DeviceStatus]:
        """Read client status"""
        try:
            command = self._create_read_status_command()
            if self.send_command(command):
                response = self._wait_for_response(timeout=5.0)
                if response:
                    status_data = self._parse_status_response(response)
                    if status_data:
                        self._update_status(status_data)
                        return self.status
            
            return None
        except Exception as e:
            logger.error(f"Failed to read client status: {e}")
            return None
    
    def discover_devices(self) -> List[Dict[str, Any]]:
        """Discover devices on network using UDP broadcast"""
        devices = []
        
        try:
            if not self.udp_socket:
                if not self.connect_udp():
                    return devices
            
            # Send broadcast discovery packet
            discovery_packet = self._create_discovery_packet()
            broadcast_addr = ('255.255.255.255', 8101)
            
            self.udp_socket.sendto(discovery_packet, broadcast_addr)
            
            # Listen for responses
            start_time = time.time()
            timeout = 5.0
            
            while time.time() - start_time < timeout:
                try:
                    self.udp_socket.settimeout(1.0)
                    data, addr = self.udp_socket.recvfrom(1024)
                    
                    if self._is_valid_discovery_response(data):
                        device_info = self._parse_discovery_response(data, addr)
                        if device_info:
                            devices.append(device_info)
                            
                except socket.timeout:
                    continue
                except Exception as e:
                    logger.error(f"Discovery response error: {e}")
                    continue
            
            logger.info(f"Discovered {len(devices)} devices")
            return devices
            
        except Exception as e:
            logger.error(f"Device discovery failed: {e}")
            return devices
    
    def _wait_for_response(self, timeout: float) -> Optional[bytes]:
        """Wait for response from device"""
        if not self.tcp_socket:
            return None
        
        start_time = time.time()
        original_timeout = self.tcp_socket.gettimeout()
        
        try:
            self.tcp_socket.settimeout(timeout)
            response = self.tcp_socket.recv(1024)
            return response if response else None
        except socket.timeout:
            return None
        except Exception as e:
            logger.error(f"Response wait error: {e}")
            return None
        finally:
            self.tcp_socket.settimeout(original_timeout)
    
    def _create_read_sn_command(self) -> bytes:
        """Create read SN command packet"""
        # Command format: [Header][Command][Length][Data][Checksum]
        header = b'\xAA\x55'
        command = b'\x01'  # Read SN command
        length = b'\x00'   # No data
        data = b''
        
        # Calculate checksum
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_read_tcp_command(self) -> bytes:
        """Create read TCP settings command packet"""
        header = b'\xAA\x55'
        command = b'\x02'  # Read TCP command
        length = b'\x00'
        data = b''
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_write_tcp_command(self, settings: Dict[str, Any]) -> bytes:
        """Create write TCP settings command packet"""
        header = b'\xAA\x55'
        command = b'\x03'  # Write TCP command
        
        # Build settings data
        data = self._build_tcp_settings_data(settings)
        length = bytes([len(data)])
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_read_work_mode_command(self) -> bytes:
        """Create read work mode command packet"""
        header = b'\xAA\x55'
        command = b'\x04'  # Read work mode command
        length = b'\x00'
        data = b''
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_write_work_mode_command(self, mode: CommunicationMode) -> bytes:
        """Create write work mode command packet"""
        header = b'\xAA\x55'
        command = b'\x05'  # Write work mode command
        length = b'\x01'
        data = bytes([mode.value])
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_keepalive_command(self) -> bytes:
        """Create keep-alive command packet"""
        header = b'\xAA\x55'
        command = b'\x06'  # Keep-alive command
        length = b'\x00'
        data = b''
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_read_status_command(self) -> bytes:
        """Create read status command packet"""
        header = b'\xAA\x55'
        command = b'\x07'  # Read status command
        length = b'\x00'
        data = b''
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _create_discovery_packet(self) -> bytes:
        """Create device discovery packet"""
        header = b'\xAA\x55'
        command = b'\x08'  # Discovery command
        length = b'\x00'
        data = b''
        
        packet_data = header + command + length + data
        checksum = sum(packet_data) & 0xFF
        
        return packet_data + bytes([checksum])
    
    def _build_tcp_settings_data(self, settings: Dict[str, Any]) -> bytes:
        """Build TCP settings data packet"""
        data = b''
        
        # IP address (4 bytes)
        if 'ip' in settings:
            ip_parts = settings['ip'].split('.')
            for part in ip_parts:
                data += bytes([int(part)])
        
        # Port (2 bytes)
        if 'port' in settings:
            data += struct.pack('>H', settings['port'])
        
        # Subnet mask (4 bytes)
        if 'subnet_mask' in settings:
            mask_parts = settings['subnet_mask'].split('.')
            for part in mask_parts:
                data += bytes([int(part)])
        
        # Gateway (4 bytes)
        if 'gateway' in settings:
            gw_parts = settings['gateway'].split('.')
            for part in gw_parts:
                data += bytes([int(part)])
        
        return data
    
    def _parse_sn_response(self, response: bytes) -> Optional[str]:
        """Parse SN response packet"""
        try:
            if len(response) < 20:
                return None
            
            # Extract SN from response (16 bytes starting at position 3)
            sn_bytes = response[3:19]
            sn = sn_bytes.decode('ascii').rstrip('\x00')
            
            return sn if sn else None
        except Exception as e:
            logger.error(f"Failed to parse SN response: {e}")
            return None
    
    def _parse_tcp_response(self, response: bytes) -> Optional[Dict[str, Any]]:
        """Parse TCP settings response packet"""
        try:
            if len(response) < 20:
                return None
            
            settings = {}
            
            # Parse IP address (4 bytes)
            ip_bytes = response[3:7]
            settings['ip'] = '.'.join([str(b) for b in ip_bytes])
            
            # Parse port (2 bytes)
            port_bytes = response[7:9]
            settings['port'] = struct.unpack('>H', port_bytes)[0]
            
            # Parse subnet mask (4 bytes)
            mask_bytes = response[9:13]
            settings['subnet_mask'] = '.'.join([str(b) for b in mask_bytes])
            
            # Parse gateway (4 bytes)
            gw_bytes = response[13:17]
            settings['gateway'] = '.'.join([str(b) for b in gw_bytes])
            
            return settings
        except Exception as e:
            logger.error(f"Failed to parse TCP response: {e}")
            return None
    
    def _parse_work_mode_response(self, response: bytes) -> Optional[CommunicationMode]:
        """Parse work mode response packet"""
        try:
            if len(response) < 4:
                return None
            
            mode_value = response[3]
            return CommunicationMode(mode_value)
        except Exception as e:
            logger.error(f"Failed to parse work mode response: {e}")
            return None
    
    def _parse_status_response(self, response: bytes) -> Optional[Dict[str, Any]]:
        """Parse status response packet"""
        try:
            if len(response) < 20:
                return None
            
            status = {}
            
            # Parse work mode
            status['work_mode'] = response[3]
            
            # Parse server IP
            ip_bytes = response[4:8]
            status['server_ip'] = '.'.join([str(b) for b in ip_bytes])
            
            # Parse connection status
            status['connection_status'] = response[8]
            
            # Parse last keepalive time
            time_bytes = response[9:13]
            status['last_keepalive'] = struct.unpack('>I', time_bytes)[0]
            
            return status
        except Exception as e:
            logger.error(f"Failed to parse status response: {e}")
            return None
    
    def _verify_write_response(self, response: bytes) -> bool:
        """Verify write command response"""
        try:
            if len(response) < 3:
                return False
            
            # Check for ACK response
            return response[2] == 0x01
        except Exception as e:
            logger.error(f"Failed to verify write response: {e}")
            return False
    
    def _is_valid_discovery_response(self, data: bytes) -> bool:
        """Check if discovery response is valid"""
        try:
            if len(data) < 3:
                return False
            
            # Check header and command
            return data[0:2] == b'\xAA\x55' and data[2] == 0x88
        except Exception:
            return False
    
    def _parse_discovery_response(self, data: bytes, addr: Tuple[str, int]) -> Optional[Dict[str, Any]]:
        """Parse device discovery response"""
        try:
            if len(data) < 20:
                return None
            
            device_info = {
                'ip': addr[0],
                'port': addr[1],
                'sn': data[3:19].decode('ascii').rstrip('\x00'),
                'model': 'DS-F8881',
                'firmware_version': f"{data[19]}.{data[20]}"
            }
            
            return device_info
        except Exception as e:
            logger.error(f"Failed to parse discovery response: {e}")
            return None
    
    def _update_status(self, status_data: Dict[str, Any]):
        """Update device status from response data"""
        try:
            if 'work_mode' in status_data:
                self.status.client_model = CommunicationMode(status_data['work_mode'])
            
            if 'server_ip' in status_data:
                self.status.server_ip = status_data['server_ip']
            
            if 'connection_status' in status_data:
                self.status.connection_status = ConnectionStatus(status_data['connection_status'])
            
            if 'last_keepalive' in status_data:
                self.status.last_keepalive = status_data['last_keepalive']
                
        except Exception as e:
            logger.error(f"Failed to update status: {e}")
    
    def get_status(self) -> DeviceStatus:
        """Get current device status"""
        return self.status
    
    def is_connected(self) -> bool:
        """Check if device is connected"""
        return self.connected
    
    def get_config(self) -> DeviceConfig:
        """Get current device configuration"""
        return self.config
    
    def update_config(self, new_config: DeviceConfig):
        """Update device configuration"""
        self.config = new_config
        logger.info("Device configuration updated")
    
    def __del__(self):
        """Cleanup on destruction"""
        self.disconnect()


class DeviceManager:
    """Manager for multiple DS-F8881 devices"""
    
    def __init__(self):
        self.devices: Dict[str, DSF8881Device] = {}
        self.discovery_thread = None
        self._running = False
    
    def add_device(self, config: DeviceConfig) -> str:
        """Add a new device"""
        device_id = f"{config.ip}:{config.port}"
        
        if device_id in self.devices:
            logger.warning(f"Device {device_id} already exists")
            return device_id
        
        device = DSF8881Device(config)
        self.devices[device_id] = device
        
        logger.info(f"Device {device_id} added to manager")
        return device_id
    
    def remove_device(self, device_id: str):
        """Remove a device"""
        if device_id in self.devices:
            device = self.devices[device_id]
            device.disconnect()
            del self.devices[device_id]
            logger.info(f"Device {device_id} removed from manager")
    
    def get_device(self, device_id: str) -> Optional[DSF8881Device]:
        """Get device by ID"""
        return self.devices.get(device_id)
    
    def get_all_devices(self) -> Dict[str, DSF8881Device]:
        """Get all devices"""
        return self.devices.copy()
    
    def connect_device(self, device_id: str) -> bool:
        """Connect to a specific device"""
        device = self.get_device(device_id)
        if device:
            return device.connect_tcp()
        return False
    
    def disconnect_device(self, device_id: str):
        """Disconnect a specific device"""
        device = self.get_device(device_id)
        if device:
            device.disconnect()
    
    def start_discovery(self):
        """Start automatic device discovery"""
        if not self.discovery_thread:
            self._running = True
            self.discovery_thread = threading.Thread(target=self._discovery_worker, daemon=True)
            self.discovery_thread.start()
            logger.info("Device discovery started")
    
    def stop_discovery(self):
        """Stop automatic device discovery"""
        self._running = False
        self.discovery_thread = None
        logger.info("Device discovery stopped")
    
    def _discovery_worker(self):
        """Device discovery worker thread"""
        while self._running:
            try:
                # Use first device for discovery if available
                if self.devices:
                    first_device = list(self.devices.values())[0]
                    discovered = first_device.discover_devices()
                    
                    # Add new devices
                    for device_info in discovered:
                        device_id = f"{device_info['ip']}:{device_info['port']}"
                        if device_id not in self.devices:
                            config = DeviceConfig(
                                ip=device_info['ip'],
                                port=device_info['port'],
                                sn=device_info['sn']
                            )
                            self.add_device(config)
                
                # Wait before next discovery cycle
                time.sleep(60)  # Discover every minute
                
            except Exception as e:
                logger.error(f"Discovery worker error: {e}")
                time.sleep(30)  # Wait before retry
    
    def get_all_status(self) -> Dict[str, DeviceStatus]:
        """Get status of all devices"""
        status = {}
        for device_id, device in self.devices.items():
            status[device_id] = device.get_status()
        return status
    
    def __del__(self):
        """Cleanup on destruction"""
        self.stop_discovery()
        for device in self.devices.values():
            device.disconnect()


# Example usage and testing
if __name__ == "__main__":
    # Create device configuration
    config = DeviceConfig(
        ip="192.168.1.150",
        port=8000,
        sn="0000000000000000",
        password="FFFFFFFF"
    )
    
    # Create device instance
    device = DSF8881Device(config)
    
    try:
        # Connect to device
        if device.connect_tcp():
            print("Connected to device successfully!")
            
            # Read device SN
            sn = device.read_device_sn()
            if sn:
                print(f"Device SN: {sn}")
            
            # Read TCP settings
            tcp_settings = device.read_tcp_settings()
            if tcp_settings:
                print(f"TCP Settings: {tcp_settings}")
            
            # Read work mode
            work_mode = device.read_client_work_mode()
            if work_mode:
                print(f"Work Mode: {work_mode}")
            
            # Get status
            status = device.get_status()
            print(f"Device Status: {status}")
            
        else:
            print("Failed to connect to device")
    
    except KeyboardInterrupt:
        print("\nInterrupted by user")
    
    finally:
        device.disconnect()
        print("Device disconnected")

