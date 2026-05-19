from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt
import structlog

logger = structlog.get_logger()
licensing_bp = Blueprint('licensing', __name__)


def _get_tenant_id():
    claims = get_jwt()
    return claims.get('tenant_id')


@licensing_bp.route('/status', methods=['GET'])
@jwt_required()
def get_license_status():
    try:
        tenant_id = _get_tenant_id()
        return jsonify({
            'tenant_id': tenant_id,
            'plan': 'standard',
            'status': 'active',
            'expires_at': '2027-01-01T00:00:00Z',
            'seats_used': 5,
            'seats_total': 25
        }), 200
    except Exception as e:
        logger.error('get_license_status error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@licensing_bp.route('/validate', methods=['POST'])
@jwt_required()
def validate_license():
    try:
        data = request.get_json() or {}
        key = data.get('license_key', '')
        if not key:
            return jsonify({'error': 'license_key is required'}), 400
        # Stub: any non-empty key is treated as valid in dev
        return jsonify({'valid': True, 'plan': 'standard', 'license_key': key}), 200
    except Exception as e:
        logger.error('validate_license error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@licensing_bp.route('/features', methods=['GET'])
@jwt_required()
def get_features():
    try:
        return jsonify({
            'features': {
                'facial_recognition': True,
                'turnstile_control': True,
                'multi_event': True,
                'white_label': False,
                'api_access': True,
                'advanced_analytics': False,
                'sms_notifications': True,
                'max_devices': 10
            }
        }), 200
    except Exception as e:
        logger.error('get_features error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@licensing_bp.route('/activate', methods=['POST'])
@jwt_required()
def activate_license():
    try:
        data = request.get_json() or {}
        key = data.get('license_key', '')
        if not key:
            return jsonify({'error': 'license_key is required'}), 400
        logger.info('License activation attempted', key=key[:8] + '...')
        return jsonify({'activated': True, 'plan': 'standard', 'message': 'License activated successfully'}), 200
    except Exception as e:
        logger.error('activate_license error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500
