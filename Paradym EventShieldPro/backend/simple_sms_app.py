#!/usr/bin/env python3
"""
Simple SMS Backend for EventShield Pro
Handles only SMS functionality without biometric dependencies
"""

from flask import Flask, jsonify, request
from flask_cors import CORS
import json
import logging
import smtplib
from email.mime.text import MIMEText

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Create Flask app
app = Flask(__name__)

# CORS setup
CORS(app)

# Email-to-SMS service
class EmailSMSService:
    CARRIER_GATEWAYS = {
        'att': 'txt.att.net',
        'verizon': 'vtext.com',
        'tmobile': 'tmomail.net',
        'sprint': 'messaging.sprintpcs.com',
        'boost mobile': 'sms.myboostmobile.com',
        'cricket': 'sms.cricketwireless.net',
        'us cellular': 'email.uscc.net',
        'virgin mobile': 'vmobl.com',
    }

    def __init__(self, sender_email=None, sender_password=None):
        self.sender_email = sender_email
        self.sender_password = sender_password

    def get_sms_gateway(self, carrier):
        return self.CARRIER_GATEWAYS.get(carrier.lower())

    def send_sms(self, to_phone_number, carrier, message, alert_type="INFO"):
        if not self.sender_email or not self.sender_password:
            return {"success": False, "error": "Email sender credentials not configured."}
        
        gateway = self.get_sms_gateway(carrier)
        if not gateway:
            return {"success": False, "error": f"Unsupported carrier: {carrier}"}
        
        to_address = f"{to_phone_number}@{gateway}"
        
        msg = MIMEText(f"EventShield Pro Alert ({alert_type}): {message}")
        msg['Subject'] = f"EventShield Pro Alert: {alert_type}"
        msg['From'] = self.sender_email
        msg['To'] = to_address

        try:
            with smtplib.SMTP_SSL('smtp.gmail.com', 465) as smtp:
                smtp.login(self.sender_email, self.sender_password)
                smtp.send_message(msg)
            logger.info(f"Email-to-SMS sent to {to_address} successfully.")
            return {"success": True, "message": "Email-to-SMS sent successfully."}
        except Exception as e:
            logger.error(f"Error sending email-to-SMS: {e}")
            return {"success": False, "error": f"Failed to send email-to-SMS: {str(e)}"}

    def send_sms_to_multiple(self, recipients, message, alert_type="INFO"):
        """Send SMS to multiple recipients"""
        if not self.sender_email or not self.sender_password:
            return {"success": False, "error": "Email sender credentials not configured."}
        
        results = []
        successful_sends = 0
        failed_sends = 0
        
        for recipient in recipients:
            phone_number = recipient.get('phoneNumber', '')
            carrier = recipient.get('carrier', '')
            name = recipient.get('name', 'Unknown')
            
            if not phone_number or not carrier:
                results.append({
                    "recipient": name,
                    "phone": phone_number,
                    "success": False,
                    "error": "Missing phone number or carrier"
                })
                failed_sends += 1
                continue
            
            # Send individual SMS
            result = self.send_sms(phone_number, carrier, message, alert_type)
            
            if result['success']:
                successful_sends += 1
                results.append({
                    "recipient": name,
                    "phone": phone_number,
                    "success": True,
                    "message": result['message']
                })
            else:
                failed_sends += 1
                results.append({
                    "recipient": name,
                    "phone": phone_number,
                    "success": False,
                    "error": result['error']
                })
        
        return {
            "success": successful_sends > 0,
            "total_recipients": len(recipients),
            "successful_sends": successful_sends,
            "failed_sends": failed_sends,
            "results": results
        }

# Global config storage
email_sms_config = {
    "phoneNumber": "4078669451",
    "carrier": "att"
}

# Recipients storage — Moto G Play (AT&T Prepaid) pre-loaded
sms_recipients = [
    {
        "id": "1",
        "name": "Quantum SMS Device (Moto G Play)",
        "phoneNumber": "4078669451",
        "carrier": "att",
        "role": "Primary Alert Device",
        "enabled": True
    }
]

# Routes
@app.route('/api/email-sms/config', methods=['POST'])
def configure_email_sms():
    data = request.get_json()
    global email_sms_config
    email_sms_config = data
    logger.info(f"SMS configuration updated: {data}")
    return jsonify({"success": True, "message": "Email-to-SMS configuration saved."})

@app.route('/api/email-sms/test', methods=['POST'])
def test_email_sms():
    data = request.get_json()
    phone_number = data.get('phone_number')
    carrier = data.get('carrier')
    message = data.get('message', "This is a test SMS from EventShield Pro via Email-to-SMS!")
    alert_type = data.get('alert_type', 'INFO')

    if not phone_number or not carrier:
        return jsonify({"success": False, "error": "Phone number and carrier are required for testing."})

    service = EmailSMSService(
        sender_email=email_sms_config.get('email'),
        sender_password=email_sms_config.get('password')
    )
    result = service.send_sms(phone_number, carrier, message, alert_type)
    return jsonify(result)

@app.route('/api/email-sms/alert', methods=['POST'])
def send_alert():
    data = request.get_json()
    phone_number = data.get('phone_number')
    carrier = data.get('carrier')
    alert_type = data.get('alert_type', 'INFO')
    message = data.get('message', '')

    if not phone_number or not carrier:
        return jsonify({"success": False, "error": "Phone number and carrier are required."})

    # Define alert messages based on type
    alert_messages = {
        'CRIMINAL_DATABASE': 'Criminal Database Alert: Suspicious activity detected. Please review immediately.',
        'PAYMENT': 'Payment Alert: Payment processing issue detected. Please check system.',
        'ACCESS_CONTROL': 'Access Control Alert: Unauthorized access attempt detected.',
        'DEVICE_STATUS': 'Device Status Alert: Device connectivity issue detected.',
        'INFO': 'General Alert: System notification from EventShield Pro.'
    }

    alert_message = alert_messages.get(alert_type, message)
    if not alert_message:
        alert_message = f"EventShield Pro {alert_type} Alert: {message}"

    service = EmailSMSService(
        sender_email=email_sms_config.get('email'),
        sender_password=email_sms_config.get('password')
    )
    result = service.send_sms(phone_number, carrier, alert_message, alert_type)
    return jsonify(result)

# Recipients management endpoints
@app.route('/api/email-sms/recipients', methods=['GET'])
def get_recipients():
    return jsonify({"success": True, "recipients": sms_recipients})

@app.route('/api/email-sms/recipients', methods=['POST'])
def add_recipient():
    data = request.get_json()
    global sms_recipients
    
    # Validate required fields
    required_fields = ['name', 'phoneNumber', 'carrier']
    for field in required_fields:
        if not data.get(field):
            return jsonify({"success": False, "error": f"Missing required field: {field}"})
    
    # Add unique ID
    recipient = {
        "id": str(len(sms_recipients) + 1),
        "name": data['name'],
        "phoneNumber": data['phoneNumber'],
        "carrier": data['carrier'],
        "role": data.get('role', 'General'),
        "enabled": data.get('enabled', True)
    }
    
    sms_recipients.append(recipient)
    logger.info(f"Added recipient: {recipient['name']}")
    return jsonify({"success": True, "message": "Recipient added successfully", "recipient": recipient})

@app.route('/api/email-sms/recipients/<recipient_id>', methods=['DELETE'])
def delete_recipient(recipient_id):
    global sms_recipients
    
    # Find and remove recipient
    original_count = len(sms_recipients)
    sms_recipients = [r for r in sms_recipients if r['id'] != recipient_id]
    
    if len(sms_recipients) < original_count:
        logger.info(f"Deleted recipient with ID: {recipient_id}")
        return jsonify({"success": True, "message": "Recipient deleted successfully"})
    else:
        return jsonify({"success": False, "error": "Recipient not found"})

@app.route('/api/email-sms/recipients/<recipient_id>', methods=['PUT'])
def update_recipient(recipient_id):
    data = request.get_json()
    global sms_recipients
    
    # Find recipient
    for i, recipient in enumerate(sms_recipients):
        if recipient['id'] == recipient_id:
            # Update fields
            for key, value in data.items():
                if key in recipient:
                    recipient[key] = value
            
            logger.info(f"Updated recipient: {recipient['name']}")
            return jsonify({"success": True, "message": "Recipient updated successfully", "recipient": recipient})
    
    return jsonify({"success": False, "error": "Recipient not found"})

@app.route('/api/email-sms/send-to-all', methods=['POST'])
def send_alert_to_all():
    data = request.get_json()
    alert_type = data.get('alert_type', 'INFO')
    custom_message = data.get('message', '')

    if not sms_recipients:
        return jsonify({"success": False, "error": "No recipients configured"})

    # Define alert messages
    alert_messages = {
        'CRIMINAL_DATABASE': '🚨 CRIMINAL DATABASE ALERT: A person with a criminal record has attempted to register for an event. Immediate security review required.',
        'PAYMENT': '💳 PAYMENT ALERT: A payment issue has occurred. Please check the payment system immediately.',
        'ACCESS_CONTROL': '🔒 ACCESS CONTROL ALERT: Unauthorized access attempt detected. Security team notification required.',
        'DEVICE_STATUS': '📱 DEVICE STATUS ALERT: A security device has gone offline or malfunctioned. Technical support needed.',
        'INFO': 'ℹ️ INFO ALERT: General information notification from EventShield Pro system.'
    }

    message = custom_message or alert_messages.get(alert_type, alert_messages['INFO'])

    # Filter enabled recipients
    enabled_recipients = [r for r in sms_recipients if r.get('enabled', True)]

    service = EmailSMSService(
        sender_email=email_sms_config.get('email'),
        sender_password=email_sms_config.get('password')
    )
    
    result = service.send_sms_to_multiple(enabled_recipients, message, alert_type)
    return jsonify(result)

@app.route('/api/health', methods=['GET'])
def health_check():
    return jsonify({"status": "healthy", "service": "EventShield Pro SMS Backend"})

if __name__ == '__main__':
    print("🚀 EventShield Pro SMS Backend Starting...")
    print("📍 API available at: http://localhost:5001")
    print("📧 Email-to-SMS service ready")
    
    try:
        app.run(debug=True, host='0.0.0.0', port=5001)
    except KeyboardInterrupt:
        print("\n🛑 EventShield Pro SMS Backend stopped by user")
    except Exception as e:
        print(f"❌ EventShield Pro SMS Backend failed to start: {e}")
