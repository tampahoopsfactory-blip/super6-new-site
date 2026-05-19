from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt
from datetime import datetime
import structlog

logger = structlog.get_logger()
hardware_bp = Blueprint('hardware', __name__)

# In-memory device registry (replace with DB in production)
_devices: dict = {}


@hardware_bp.route('/devices', methods=['GET'])
@jwt_required()
def list_devices():
    try:
        return jsonify({
            'devices': list(_devices.values()),
            'total': len(_devices)
        }), 200
    except Exception as e:
        logger.error('list_devices error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices', methods=['POST'])
@jwt_required()
def register_device():
    try:
        data = request.get_json() or {}
        required = ['name', 'type', 'ip_address']
        for field in required:
            if not data.get(field):
                return jsonify({'error': f'{field} is required'}), 400

        device_id = str(len(_devices) + 1)
        device = {
            'id': device_id,
            'name': data['name'],
            'type': data['type'],
            'ip_address': data['ip_address'],
            'port': data.get('port', 8000),
            'status': 'offline',
            'registered_at': datetime.utcnow().isoformat()
        }
        _devices[device_id] = device
        logger.info('Device registered', device_id=device_id, name=device['name'])
        return jsonify({'device': device}), 201
    except Exception as e:
        logger.error('register_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<device_id>', methods=['GET'])
@jwt_required()
def get_device(device_id):
    try:
        device = _devices.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404
        return jsonify({'device': device}), 200
    except Exception as e:
        logger.error('get_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<device_id>', methods=['PUT'])
@jwt_required()
def update_device(device_id):
    try:
        device = _devices.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404
        data = request.get_json() or {}
        device.update({k: v for k, v in data.items() if k not in ('id', 'registered_at')})
        return jsonify({'device': device}), 200
    except Exception as e:
        logger.error('update_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<device_id>', methods=['DELETE'])
@jwt_required()
def delete_device(device_id):
    try:
        if device_id not in _devices:
            return jsonify({'error': 'Device not found'}), 404
        del _devices[device_id]
        return jsonify({'deleted': True}), 200
    except Exception as e:
        logger.error('delete_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<device_id>/ping', methods=['POST'])
@jwt_required()
def ping_device(device_id):
    try:
        device = _devices.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404
        # Stub: in production, attempt TCP connection to device
        device['last_seen'] = datetime.utcnow().isoformat()
        return jsonify({'device_id': device_id, 'reachable': False, 'message': 'Ping stub — no real hardware connected'}), 200
    except Exception as e:
        logger.error('ping_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<device_id>/command', methods=['POST'])
@jwt_required()
def send_command(device_id):
    try:
        device = _devices.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404
        data = request.get_json() or {}
        command = data.get('command')
        if not command:
            return jsonify({'error': 'command is required'}), 400
        logger.info('Hardware command sent', device_id=device_id, command=command)
        return jsonify({'device_id': device_id, 'command': command, 'sent': True}), 200
    except Exception as e:
        logger.error('send_command error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500
