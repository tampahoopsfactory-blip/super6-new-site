"""
SMS Alert Routes for EventShield Pro
Handles SMS configuration and alert sending through Google Chat
"""

from flask import Blueprint, request, jsonify
from sms_service import sms_service, test_sms_connection, send_criminal_alert, send_payment_alert, send_access_alert, send_device_alert
import logging

# Configure logging
logger = logging.getLogger(__name__)

# Create Blueprint
sms_bp = Blueprint('sms', __name__, url_prefix='/api/sms')

@sms_bp.route('/config', methods=['GET'])
def get_sms_config():
    """Get current SMS configuration"""
    try:
        config = {
            'enabled': sms_service.enabled,
            'webhook_configured': bool(sms_service.webhook_url),
            'space_configured': bool(sms_service.space_id),
            'api_key_configured': bool(sms_service.api_key)
        }
        return jsonify({
            'success': True,
            'config': config
        })
    except Exception as e:
        logger.error(f"Error getting SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/config', methods=['POST'])
def update_sms_config():
    """Update SMS configuration"""
    try:
        data = request.get_json()
        
        # Update configuration
        if 'webhook_url' in data:
            sms_service.webhook_url = data['webhook_url']
        if 'space_id' in data:
            sms_service.space_id = data['space_id']
        if 'api_key' in data:
            sms_service.api_key = data['api_key']
        
        # Re-evaluate enabled status
        sms_service.enabled = bool(sms_service.webhook_url and sms_service.space_id)
        
        return jsonify({
            'success': True,
            'message': 'SMS configuration updated successfully',
            'enabled': sms_service.enabled
        })
    except Exception as e:
        logger.error(f"Error updating SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/test', methods=['POST'])
def test_sms():
    """Test SMS connection"""
    try:
        result = test_sms_connection()
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error testing SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/send', methods=['POST'])
def send_sms():
    """Send custom SMS alert"""
    try:
        data = request.get_json()
        
        required_fields = ['message']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = sms_service.send_sms_alert(
            message=data['message'],
            priority=data.get('priority', 'normal'),
            alert_type=data.get('alert_type', 'system'),
            recipient_phone=data.get('recipient_phone')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/criminal-alert', methods=['POST'])
def send_criminal_sms():
    """Send criminal database alert"""
    try:
        data = request.get_json()
        
        if 'person_name' not in data or 'match_details' not in data:
            return jsonify({
                'success': False,
                'error': 'Missing required fields: person_name, match_details'
            }), 400
        
        result = send_criminal_alert(
            person_name=data['person_name'],
            match_details=data['match_details']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending criminal alert: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/payment-alert', methods=['POST'])
def send_payment_sms():
    """Send payment alert"""
    try:
        data = request.get_json()
        
        required_fields = ['amount', 'method', 'status']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_payment_alert(
            amount=float(data['amount']),
            method=data['method'],
            status=data['status']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending payment alert: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/access-alert', methods=['POST'])
def send_access_sms():
    """Send access control alert"""
    try:
        data = request.get_json()
        
        required_fields = ['person_name', 'access_type', 'status']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_access_alert(
            person_name=data['person_name'],
            access_type=data['access_type'],
            status=data['status']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending access alert: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/device-alert', methods=['POST'])
def send_device_sms():
    """Send device alert"""
    try:
        data = request.get_json()
        
        required_fields = ['device_name', 'status']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_device_alert(
            device_name=data['device_name'],
            status=data['status'],
            issue=data.get('issue')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending device alert: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@sms_bp.route('/status', methods=['GET'])
def get_sms_status():
    """Get SMS service status"""
    try:
        status = {
            'enabled': sms_service.enabled,
            'webhook_url': bool(sms_service.webhook_url),
            'space_id': bool(sms_service.space_id),
            'api_key': bool(sms_service.api_key),
            'service_ready': sms_service.enabled
        }
        
        return jsonify({
            'success': True,
            'status': status
        })
    except Exception as e:
        logger.error(f"Error getting SMS status: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500
