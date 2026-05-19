from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required
from datetime import datetime
from uuid import uuid4
import structlog

from app import db
from app.models.hardware import HardwareDevice, DeviceType, DeviceStatus, CommunicationProtocol

logger = structlog.get_logger()
hardware_bp = Blueprint('hardware', __name__)

_TYPE_MAP = {
    'facial_recognition': DeviceType.FACIAL_RECOGNITION_CAMERA,
    'facial_recognition_camera': DeviceType.FACIAL_RECOGNITION_CAMERA,
    'turnstile': DeviceType.TURNSTILE,
    'rfid': DeviceType.RFID_READER,
    'rfid_reader': DeviceType.RFID_READER,
    'biometric': DeviceType.BIOMETRIC_READER,
    'biometric_reader': DeviceType.BIOMETRIC_READER,
    'qr_scanner': DeviceType.QR_SCANNER,
    'gate': DeviceType.GATE,
    'door_lock': DeviceType.DOOR_LOCK,
    'access_control_panel': DeviceType.ACCESS_CONTROL_PANEL,
}


def _serialize(device):
    return {
        'id': device.id,
        'device_id': device.device_id,
        'name': device.name,
        'device_type': device.device_type.value if device.device_type else None,
        'model': device.model.value if device.model else None,
        'ip_address': device.ip_address,
        'port': device.port,
        'status': device.status.value if device.status else None,
        'last_heartbeat': device.last_heartbeat.isoformat() if device.last_heartbeat else None,
        'created_at': device.created_at.isoformat() if device.created_at else None,
    }


@hardware_bp.route('/devices', methods=['GET'])
@jwt_required()
def list_devices():
    try:
        devices = HardwareDevice.query.all()
        return jsonify({'devices': [_serialize(d) for d in devices], 'total': len(devices)}), 200
    except Exception as e:
        logger.error('list_devices error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices', methods=['POST'])
@jwt_required()
def register_device():
    try:
        data = request.get_json() or {}
        for field in ('name', 'type', 'ip_address'):
            if not data.get(field):
                return jsonify({'error': f'{field} is required'}), 400

        device_type = _TYPE_MAP.get(data['type'].lower(), DeviceType.FACIAL_RECOGNITION_CAMERA)

        device = HardwareDevice(
            device_id=str(uuid4()),
            name=data['name'],
            device_type=device_type,
            ip_address=data['ip_address'],
            port=data.get('port', 8000),
            communication_protocol=CommunicationProtocol.TCP_IP,
            status=DeviceStatus.OFFLINE,
            access_point_id=data.get('access_point_id'),
        )

        try:
            db.session.add(device)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        logger.info('Device registered', device_id=device.device_id, name=device.name)
        return jsonify({'device': _serialize(device)}), 201
    except Exception as e:
        logger.error('register_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<int:device_id>', methods=['GET'])
@jwt_required()
def get_device(device_id):
    try:
        device = HardwareDevice.query.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404
        return jsonify({'device': _serialize(device)}), 200
    except Exception as e:
        logger.error('get_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<int:device_id>', methods=['PUT'])
@jwt_required()
def update_device(device_id):
    try:
        device = HardwareDevice.query.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404

        data = request.get_json() or {}
        protected = {'id', 'device_id', 'created_at'}
        for key, value in data.items():
            if key not in protected and hasattr(device, key):
                setattr(device, key, value)

        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        return jsonify({'device': _serialize(device)}), 200
    except Exception as e:
        logger.error('update_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<int:device_id>', methods=['DELETE'])
@jwt_required()
def delete_device(device_id):
    try:
        device = HardwareDevice.query.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404

        try:
            db.session.delete(device)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        return jsonify({'deleted': True}), 200
    except Exception as e:
        logger.error('delete_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<int:device_id>/ping', methods=['POST'])
@jwt_required()
def ping_device(device_id):
    try:
        device = HardwareDevice.query.get(device_id)
        if not device:
            return jsonify({'error': 'Device not found'}), 404

        device.last_heartbeat = datetime.utcnow()
        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        return jsonify({'device_id': device_id, 'reachable': False, 'message': 'Ping stub — no real hardware connected'}), 200
    except Exception as e:
        logger.error('ping_device error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@hardware_bp.route('/devices/<int:device_id>/command', methods=['POST'])
@jwt_required()
def send_command(device_id):
    try:
        device = HardwareDevice.query.get(device_id)
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
