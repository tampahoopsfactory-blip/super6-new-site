"""
Email-to-SMS Routes for EventShield Pro
Handles SMS configuration and alert sending through email-to-SMS
"""

from flask import Blueprint, request, jsonify
from email_sms_service import sms_service, test_sms_connection, send_criminal_alert, send_payment_alert, send_access_alert, send_device_alert
import logging

# Configure logging
logger = logging.getLogger(__name__)

# Create Blueprint
email_sms_bp = Blueprint('email_sms', __name__, url_prefix='/api/email-sms')

@email_sms_bp.route('/config', methods=['GET'])
def get_sms_config():
    """Get current email-to-SMS configuration"""
    try:
        config = {
            'enabled': sms_service.enabled,
            'email_configured': bool(sms_service.email),
            'password_configured': bool(sms_service.password),
            'smtp_server': sms_service.smtp_server,
            'smtp_port': sms_service.smtp_port
        }
        return jsonify({
            'success': True,
            'config': config
        })
    except Exception as e:
        logger.error(f"Error getting email SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/config', methods=['POST'])
def update_sms_config():
    """Update email-to-SMS configuration"""
    try:
        data = request.get_json()
        
        # Update configuration
        if 'email' in data:
            sms_service.email = data['email']
        if 'password' in data:
            sms_service.password = data['password']
        if 'smtp_server' in data:
            sms_service.smtp_server = data['smtp_server']
        if 'smtp_port' in data:
            sms_service.smtp_port = int(data['smtp_port'])
        
        # Re-evaluate enabled status
        sms_service.enabled = bool(sms_service.email and sms_service.password)
        
        return jsonify({
            'success': True,
            'message': 'Email-to-SMS configuration updated successfully',
            'enabled': sms_service.enabled
        })
    except Exception as e:
        logger.error(f"Error updating email SMS config: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/test', methods=['POST'])
def test_sms():
    """Test email-to-SMS connection"""
    try:
        data = request.get_json()
        
        required_fields = ['phone_number']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        result = test_sms_connection(
            phone_number=data['phone_number'],
            carrier=data.get('carrier', 'verizon')
        )
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error testing email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/send', methods=['POST'])
def send_sms():
    """Send custom email-to-SMS alert"""
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
            carrier=data.get('carrier', 'verizon'),
            priority=data.get('priority', 'normal'),
            alert_type=data.get('alert_type', 'system')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/criminal-alert', methods=['POST'])
def send_criminal_sms():
    """Send criminal database alert via email-to-SMS"""
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
            phone_number=data['phone_number'],
            carrier=data.get('carrier', 'verizon')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending criminal alert via email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/payment-alert', methods=['POST'])
def send_payment_sms():
    """Send payment alert via email-to-SMS"""
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
            phone_number=data['phone_number'],
            carrier=data.get('carrier', 'verizon')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending payment alert via email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/access-alert', methods=['POST'])
def send_access_sms():
    """Send access control alert via email-to-SMS"""
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
            phone_number=data['phone_number'],
            carrier=data.get('carrier', 'verizon')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending access alert via email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/device-alert', methods=['POST'])
def send_device_sms():
    """Send device alert via email-to-SMS"""
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
            phone_number=data['phone_number'],
            carrier=data.get('carrier', 'verizon')
        )
        
        return jsonify(result)
    except Exception as e:
        logger.error(f"Error sending device alert via email SMS: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/status', methods=['GET'])
def get_sms_status():
    """Get email-to-SMS service status"""
    try:
        status = {
            'enabled': sms_service.enabled,
            'email': bool(sms_service.email),
            'password': bool(sms_service.password),
            'smtp_server': sms_service.smtp_server,
            'smtp_port': sms_service.smtp_port,
            'service_ready': sms_service.enabled
        }
        
        return jsonify({
            'success': True,
            'status': status
        })
    except Exception as e:
        logger.error(f"Error getting email SMS status: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500

@email_sms_bp.route('/send-email', methods=['POST'])
def send_email_alert():
    """Send regular email alert (not SMS)"""
    try:
        data = request.get_json()
        
        required_fields = ['email', 'subject', 'message']
        for field in required_fields:
            if field not in data:
                return jsonify({
                    'success': False,
                    'error': f'Missing required field: {field}'
                }), 400
        
        # Import email sending functionality
        import smtplib
        from email.mime.text import MIMEText
        from email.mime.multipart import MIMEMultipart
        
        # Create message
        msg = MIMEMultipart('alternative')
        msg['From'] = sms_service.email
        msg['To'] = data['email']
        msg['Subject'] = data['subject']
        
        # Add HTML and plain text versions
        html_message = data.get('html_message', data['message'])
        text_part = MIMEText(data['message'], 'plain')
        html_part = MIMEText(html_message, 'html')
        
        msg.attach(text_part)
        msg.attach(html_part)
        
        # Send email
        try:
            server = smtplib.SMTP(sms_service.smtp_server, sms_service.smtp_port)
            server.set_debuglevel(1)
            server.ehlo()
            if server.has_extn('STARTTLS'):
                server.starttls()
                server.ehlo()
            server.login(sms_service.email, sms_service.password)
            server.sendmail(sms_service.email, data['email'], msg.as_string())
            server.quit()
            
            return jsonify({
                'success': True,
                'message': 'Email sent successfully'
            })
        except Exception as smtp_error:
            logger.error(f"SMTP error: {str(smtp_error)}")
            # Try SSL fallback
            try:
                server = smtplib.SMTP_SSL(sms_service.smtp_server, 465)
                server.set_debuglevel(1)
                server.login(sms_service.email, sms_service.password)
                server.sendmail(sms_service.email, data['email'], msg.as_string())
                server.quit()
                
                return jsonify({
                    'success': True,
                    'message': 'Email sent successfully via SSL'
                })
            except Exception as ssl_error:
                logger.error(f"SSL error: {str(ssl_error)}")
                raise ssl_error
                
    except Exception as e:
        logger.error(f"Error sending email alert: {str(e)}")
        return jsonify({
            'success': False,
            'error': str(e)
        }), 500
