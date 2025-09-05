"""
Google Voice SMS Routes for EventShield Pro
Handles SMS configuration and alert sending through Google Voice
"""

from flask import Blueprint, request, jsonify
from google_voice_sms import sms_service, test_sms_connection, send_criminal_alert, send_payment_alert, send_access_alert, send_device_alert
import logging

# Configure logging
logger = logging.getLogger(__name__)

# Create Blueprint
google_voice_bp = Blueprint('google_voice_sms', __name__, url_prefix='/api/google-voice-sms')

@google_voice_bp.route('/config', methods=['GET'])
def get_sms_config():
    """Get current Google Voice SMS configuration"""
    try:
        config = {
            'enabled': sms_service.enabled,
            'api_key_configured': bool(sms_service.api_key),
            'voice_number_configured': bool(sms_service.voice_number)
        }
        return jsonify({
            'success': True,
            'config': config
        })
    except Exception as e:
        logger.error(f"Error getting Google Voice SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/config', methods=['POST'])
def update_sms_config():
    """Update Google Voice SMS configuration"""
    try:
        data = request.get_json()
        
        # Update configuration
        if 'api_key' in data:
            sms_service.api_key = data['api_key']
        if 'voice_number' in data:
            sms_service.voice_number = data['voice_number']
        
        # Re-evaluate enabled status
        sms_service.enabled = bool(sms_service.api_key and sms_service.voice_number)
        
        return jsonify({
            'success': True,
            'message': 'Google Voice SMS configuration updated successfully',
            'enabled': sms_service.enabled
        })
    except Exception as e:
        logger.error(f"Error updating Google Voice SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/test', methods=['POST'])
def test_sms():
    """Test Google Voice SMS connection"""
    try:
        data = request.get_json()
        
        if 'phone_number' not in data:
            return jsonify({
                'success': False,
                'error': 'Missing required field: phone_number'
            }), 400
        
        result = test_sms_connection(data['phone_number'])
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error testing Google Voice SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/send', methods=['POST'])
def send_sms():
    """Send custom Google Voice SMS alert"""
    try:
        data = request.get_json()
        
        required_fields = ['message', 'phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = sms_service.send_sms_alert(
            message=data['message'],
            phone_number=data['phone_number'],
            priority=data.get('priority', 'normal'),
            alert_type=data.get('alert_type', 'system')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending Google Voice SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/criminal-alert', methods=['POST'])
def send_criminal_sms():
    """Send criminal database alert via Google Voice"""
    try:
        data = request.get_json()
        
        required_fields = ['person_name', 'match_details', 'phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_criminal_alert(
            person_name=data['person_name'],
            match_details=data['match_details'],
            phone_number=data['phone_number']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending criminal alert via Google Voice: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/payment-alert', methods=['POST'])
def send_payment_sms():
    """Send payment alert via Google Voice"""
    try:
        data = request.get_json()
        
        required_fields = ['amount', 'method', 'status', 'phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_payment_alert(
            amount=float(data['amount']),
            method=data['method'],
            status=data['status'],
            phone_number=data['phone_number']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending payment alert via Google Voice: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/access-alert', methods=['POST'])
def send_access_sms():
    """Send access control alert via Google Voice"""
    try:
        data = request.get_json()
        
        required_fields = ['person_name', 'access_type', 'status', 'phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_access_alert(
            person_name=data['person_name'],
            access_type=data['access_type'],
            status=data['status'],
            phone_number=data['phone_number']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending access alert via Google Voice: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/device-alert', methods=['POST'])
def send_device_sms():
    """Send device alert via Google Voice"""
    try:
        data = request.get_json()
        
        required_fields = ['device_name', 'status', 'phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = send_device_alert(
            device_name=data['device_name'],
            status=data['status'],
            issue=data.get('issue'),
            phone_number=data['phone_number']
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending device alert via Google Voice: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@google_voice_bp.route('/status', methods=['GET'])
def get_sms_status():
    """Get Google Voice SMS service status"""
    try:
        status = {
            'enabled': sms_service.enabled,
            'api_key': bool(sms_service.api_key),
            'voice_number': bool(sms_service.voice_number),
            'service_ready': sms_service.enabled
        }
        
        return jsonify({
            'success': True,
            'status': status
        })
    except Exception as e:
        logger.error(f"Error getting Google Voice SMS status: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500
