#!/usr/bin/env python3
"""
EventShield Pro - DS-F8881 Device API Endpoints
Flask API for device management and control
"""

from flask import Flask, jsonify, request, Blueprint
from flask_cors import CORS
import json
import logging
from typing import Dict, List, Any, Optional
from datetime import datetime

# Import our device communication layer
from device_ds_f8881 import (
    DSF8881Device, 
    DeviceManager, 
    DeviceConfig, 
    CommunicationMode, 
    ConnectionStatus
)

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Create Flask blueprint for device API
device_api = Blueprint('device_api', __name__, url_prefix='/api/devices')

# Global device manager instance
device_manager = DeviceManager()

# CORS setup
CORS(device_api)

# Error handling
class DeviceAPIError(Exception):
    """Custom exception for device API errors"""
    def __init__(self, message: str, status_code: int = 400):
        self.message = message
        self.status_code = status_code
        super().__init__(self.message)

@device_api.errorhandler(DeviceAPIError)
def handle_device_api_error(error):
    """Handle device API errors"""
    return jsonify({
        'error': True,
        'message': error.message,
        'timestamp': datetime.now().isoformat()
    }), error.status_code

@device_api.errorhandler(Exception)
def handle_generic_error(error):
    """Handle generic errors"""
    logger.error(f"Unexpected error: {error}")
    return jsonify({
        'error': True,
        'message': 'Internal server error',
        'timestamp': datetime.now().isoformat()
    }), 500

# Utility functions
def validate_device_config(data: Dict[str, Any]) -> DeviceConfig:
    """Validate and create device configuration from request data"""
    required_fields = ['ip']
    
    for field in required_fields:
        if field not in data:
            raise DeviceAPIError(f"Missing required field: {field}")
    
    # Create device configuration with defaults
    config = DeviceConfig(
        ip=data['ip'],
        port=data.get('port', 8000),
        sn=data.get('sn', '0000000000000000'),
        password=data.get('password', 'FFFFFFFF'),
        communication_mode=CommunicationMode(data.get('communication_mode', 2)),
        timeout=data.get('timeout', 5000),
        retry_count=data.get('retry_count', 3)
    )
    
    return config

def device_to_dict(device: DSF8881Device) -> Dict[str, Any]:
    """Convert device object to dictionary for JSON response"""
    config = device.get_config()
    status = device.get_status()
    
    return {
        'id': f"{config.ip}:{config.port}",
        'ip': config.ip,
        'port': config.port,
        'sn': config.sn,
        'password': config.password,
        'communication_mode': config.communication_mode.value,
        'communication_mode_name': config.communication_mode.name,
        'timeout': config.timeout,
        'retry_count': config.retry_count,
        'connected': device.is_connected(),
        'status': {
            'online': status.online,
            'last_keepalive': status.last_keepalive,
            'connection_status': status.connection_status.value,
            'connection_status_name': status.connection_status.name,
            'client_model': status.client_model.value,
            'client_model_name': status.client_model.name,
            'server_ip': status.server_ip,
            'mac_address': status.mac_address,
            'firmware_version': status.firmware_version
        }
    }

# Device Management Endpoints

@device_api.route('/', methods=['GET'])
def get_all_devices():
    """Get all registered devices"""
    try:
        devices = device_manager.get_all_devices()
        device_list = []
        
        for device_id, device in devices.items():
            device_list.append(device_to_dict(device))
        
        return jsonify({
            'success': True,
            'devices': device_list,
            'total': len(device_list),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting devices: {e}")
        raise DeviceAPIError(f"Failed to get devices: {str(e)}")

@device_api.route('/', methods=['POST'])
def add_device():
    """Add a new device"""
    try:
        data = request.get_json()
        if not data:
            raise DeviceAPIError("No data provided")
        
        # Validate and create device configuration
        config = validate_device_config(data)
        
        # Add device to manager
        device_id = device_manager.add_device(config)
        
        # Get the added device
        device = device_manager.get_device(device_id)
        
        return jsonify({
            'success': True,
            'message': 'Device added successfully',
            'device': device_to_dict(device),
            'device_id': device_id,
            'timestamp': datetime.now().isoformat()
        }), 201
    
    except Exception as e:
        logger.error(f"Error adding device: {e}")
        raise DeviceAPIError(f"Failed to add device: {str(e)}")

@device_api.route('/<device_id>', methods=['GET'])
def get_device(device_id: str):
    """Get specific device by ID"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        return jsonify({
            'success': True,
            'device': device_to_dict(device),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to get device: {str(e)}")

@device_api.route('/<device_id>', methods=['PUT'])
def update_device(device_id: str):
    """Update device configuration"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        data = request.get_json()
        if not data:
            raise DeviceAPIError("No data provided")
        
        # Create new configuration
        new_config = validate_device_config(data)
        
        # Update device configuration
        device.update_config(new_config)
        
        return jsonify({
            'success': True,
            'message': 'Device updated successfully',
            'device': device_to_dict(device),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error updating device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to update device: {str(e)}")

@device_api.route('/<device_id>', methods=['DELETE'])
def remove_device(device_id: str):
    """Remove device"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        # Remove device from manager
        device_manager.remove_device(device_id)
        
        return jsonify({
            'success': True,
            'message': 'Device removed successfully',
            'device_id': device_id,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error removing device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to remove device: {str(e)}")

# Device Connection Endpoints

@device_api.route('/<device_id>/connect', methods=['POST'])
def connect_device(device_id: str):
    """Connect to device"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        # Attempt to connect
        success = device_manager.connect_device(device_id)
        
        if success:
            return jsonify({
                'success': True,
                'message': 'Device connected successfully',
                'device': device_to_dict(device),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to connect to device")
    
    except Exception as e:
        logger.error(f"Error connecting to device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to connect to device: {str(e)}")

@device_api.route('/<device_id>/disconnect', methods=['POST'])
def disconnect_device(device_id: str):
    """Disconnect from device"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        # Disconnect device
        device_manager.disconnect_device(device_id)
        
        return jsonify({
            'success': True,
            'message': 'Device disconnected successfully',
            'device': device_to_dict(device),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error disconnecting device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to disconnect device: {str(e)}")

# Device Control Endpoints

@device_api.route('/<device_id>/sn', methods=['GET'])
def read_device_sn(device_id: str):
    """Read device serial number"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        # Read device SN
        sn = device.read_device_sn()
        
        if sn:
            return jsonify({
                'success': True,
                'serial_number': sn,
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to read device serial number")
    
    except Exception as e:
        logger.error(f"Error reading SN for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to read device serial number: {str(e)}")

@device_api.route('/<device_id>/tcp-settings', methods=['GET'])
def read_tcp_settings(device_id: str):
    """Read device TCP settings"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        # Read TCP settings
        settings = device.read_tcp_settings()
        
        if settings:
            return jsonify({
                'success': True,
                'tcp_settings': settings,
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to read TCP settings")
    
    except Exception as e:
        logger.error(f"Error reading TCP settings for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to read TCP settings: {str(e)}")

@device_api.route('/<device_id>/tcp-settings', methods=['PUT'])
def write_tcp_settings(device_id: str):
    """Write device TCP settings"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        data = request.get_json()
        if not data:
            raise DeviceAPIError("No settings data provided")
        
        # Write TCP settings
        success = device.write_tcp_settings(data)
        
        if success:
            return jsonify({
                'success': True,
                'message': 'TCP settings updated successfully',
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to update TCP settings")
    
    except Exception as e:
        logger.error(f"Error writing TCP settings for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to write TCP settings: {str(e)}")

@device_api.route('/<device_id>/work-mode', methods=['GET'])
def read_work_mode(device_id: str):
    """Read device work mode"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        # Read work mode
        mode = device.read_client_work_mode()
        
        if mode is not None:
            return jsonify({
                'success': True,
                'work_mode': mode.value,
                'work_mode_name': mode.name,
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to read work mode")
    
    except Exception as e:
        logger.error(f"Error reading work mode for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to read work mode: {str(e)}")

@device_api.route('/<device_id>/work-mode', methods=['PUT'])
def write_work_mode(device_id: str):
    """Write device work mode"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        data = request.get_json()
        if not data or 'work_mode' not in data:
            raise DeviceAPIError("Work mode not specified")
        
        # Create communication mode enum
        try:
            mode = CommunicationMode(data['work_mode'])
        except ValueError:
            raise DeviceAPIError("Invalid work mode value")
        
        # Write work mode
        success = device.write_client_work_mode(mode)
        
        if success:
            return jsonify({
                'success': True,
                'message': 'Work mode updated successfully',
                'work_mode': mode.value,
                'work_mode_name': mode.name,
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to update work mode")
    
    except Exception as e:
        logger.error(f"Error writing work mode for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to write work mode: {str(e)}")

@device_api.route('/<device_id>/keepalive', methods=['POST'])
def send_keepalive(device_id: str):
    """Send keep-alive packet to device"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        # Send keep-alive
        success = device.send_keepalive()
        
        if success:
            return jsonify({
                'success': True,
                'message': 'Keep-alive sent successfully',
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to send keep-alive")
    
    except Exception as e:
        logger.error(f"Error sending keep-alive to device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to send keep-alive: {str(e)}")

@device_api.route('/<device_id>/status', methods=['GET'])
def read_device_status(device_id: str):
    """Read device status"""
    try:
        device = device_manager.get_device(device_id)
        if not device:
            raise DeviceAPIError("Device not found", 404)
        
        if not device.is_connected():
            raise DeviceAPIError("Device not connected")
        
        # Read device status
        status = device.read_client_status()
        
        if status:
            return jsonify({
                'success': True,
                'status': {
                    'online': status.online,
                    'last_keepalive': status.last_keepalive,
                    'connection_status': status.connection_status.value,
                    'connection_status_name': status.connection_status.name,
                    'client_model': status.client_model.value,
                    'client_model_name': status.client_model.name,
                    'server_ip': status.server_ip,
                    'mac_address': status.mac_address,
                    'firmware_version': status.firmware_version
                },
                'device_id': device_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise DeviceAPIError("Failed to read device status")
    
    except Exception as e:
        logger.error(f"Error reading status for device {device_id}: {e}")
        raise DeviceAPIError(f"Failed to read device status: {str(e)}")

# Device Discovery Endpoints

@device_api.route('/discover', methods=['POST'])
def discover_devices():
    """Discover devices on network"""
    try:
        # Get discovery parameters
        data = request.get_json() or {}
        timeout = data.get('timeout', 5.0)
        
        # Use first device for discovery if available
        devices = device_manager.get_all_devices()
        if not devices:
            raise DeviceAPIError("No devices available for discovery")
        
        # Get first device and use it for discovery
        first_device = list(devices.values())[0]
        
        # Discover devices
        discovered = first_device.discover_devices()
        
        return jsonify({
            'success': True,
            'discovered_devices': discovered,
            'total_discovered': len(discovered),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error discovering devices: {e}")
        raise DeviceAPIError(f"Failed to discover devices: {str(e)}")

@device_api.route('/discovery/start', methods=['POST'])
def start_discovery():
    """Start automatic device discovery"""
    try:
        device_manager.start_discovery()
        
        return jsonify({
            'success': True,
            'message': 'Device discovery started',
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error starting discovery: {e}")
        raise DeviceAPIError(f"Failed to start discovery: {str(e)}")

@device_api.route('/discovery/stop', methods=['POST'])
def stop_discovery():
    """Stop automatic device discovery"""
    try:
        device_manager.stop_discovery()
        
        return jsonify({
            'success': True,
            'message': 'Device discovery stopped',
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error stopping discovery: {e}")
        raise DeviceAPIError(f"Failed to stop discovery: {str(e)}")

# Device Status Summary Endpoint

@device_api.route('/status/summary', methods=['GET'])
def get_devices_status_summary():
    """Get status summary of all devices"""
    try:
        devices = device_manager.get_all_devices()
        status_summary = device_manager.get_all_status()
        
        summary = {
            'total_devices': len(devices),
            'connected_devices': 0,
            'disconnected_devices': 0,
            'devices_by_status': {},
            'devices_by_mode': {}
        }
        
        for device_id, status in status_summary.items():
            # Count by connection status
            if status.connection_status == ConnectionStatus.CONNECTED:
                summary['connected_devices'] += 1
            else:
                summary['disconnected_devices'] += 1
            
            # Group by connection status
            status_name = status.connection_status.name
            if status_name not in summary['devices_by_status']:
                summary['devices_by_status'][status_name] = []
            summary['devices_by_status'][status_name].append(device_id)
            
            # Group by communication mode
            mode_name = status.client_model.name
            if mode_name not in summary['devices_by_mode']:
                summary['devices_by_mode'][mode_name] = []
            summary['devices_by_mode'][mode_name].append(device_id)
        
        return jsonify({
            'success': True,
            'summary': summary,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting status summary: {e}")
        raise DeviceAPIError(f"Failed to get status summary: {str(e)}")

# Health Check Endpoint

@device_api.route('/health', methods=['GET'])
def device_api_health():
    """Health check for device API"""
    try:
        devices = device_manager.get_all_devices()
        
        return jsonify({
            'success': True,
            'status': 'healthy',
            'service': 'DS-F8881 Device API',
            'total_devices': len(devices),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Health check failed: {e}")
        return jsonify({
            'success': False,
            'status': 'unhealthy',
            'service': 'DS-F8881 Device API',
            'error': str(e),
            'timestamp': datetime.now().isoformat()
        }), 500

# Export the blueprint
__all__ = ['device_api']

