"""
Email-to-SMS Service for EventShield Pro
Sends SMS alerts via email that carriers convert to SMS
"""

import smtplib
import logging
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
from typing import Dict, List, Optional
from datetime import datetime
import os

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class EmailSMSService:
    """Service for sending SMS alerts through email-to-SMS"""
    
    def __init__(self):
        self.smtp_server = os.getenv('SMTP_SERVER', 'smtp.gmail.com')
        self.smtp_port = int(os.getenv('SMTP_PORT', '587'))
        self.email = os.getenv('SENDER_EMAIL', '')
        self.password = os.getenv('SENDER_PASSWORD', '')
        self.enabled = bool(self.email and self.password)
        
        # Carrier email-to-SMS mappings
        self.carrier_emails = {
            'verizon': '@vtext.com',
            'att': '@txt.att.net',
            'tmobile': '@tmomail.net',
            'sprint': '@messaging.sprintpcs.com',
            'us_cellular': '@email.uscc.net',
            'boost': '@myboostmobile.com',
            'cricket': '@sms.cricketwireless.net',
            'metropcs': '@mymetropcs.com'
        }
    
    def send_sms_alert(self, 
                      message: str, 
                      phone_number: str,
                      carrier: str = 'verizon',
                      priority: str = 'normal',
                      alert_type: str = 'system') -> Dict:
        """
        Send SMS alert via email-to-SMS
        
        Args:
            message: The alert message to send
            phone_number: Target phone number (e.g., 8132702754)
            carrier: Phone carrier (verizon, att, tmobile, etc.)
            priority: Alert priority (low, normal, high, critical)
            alert_type: Type of alert (system, security, payment, access)
            
        Returns:
            Dict with success status and details
        """
        if not self.enabled:
            return {
                'success': False,
                'error': 'Email SMS service not configured'
            }
        
        try:
            # Format phone number
            clean_phone = ''.join(filter(str.isdigit, phone_number))
            if len(clean_phone) == 10:
                clean_phone = '1' + clean_phone  # Add country code for US
            
            # Get carrier email
            carrier_email = self.carrier_emails.get(carrier.lower(), '@vtext.com')
            sms_email = f"{clean_phone}{carrier_email}"
            
            # Format the message with EventShield Pro branding
            formatted_message = self._format_message(message, priority, alert_type)
            
            # Send via email
            response = self._send_via_email(sms_email, formatted_message, alert_type)
            
            if response.get('success'):
                logger.info(f"SMS alert sent successfully: {alert_type} - {priority}")
                return {
                    'success': True,
                    'message': 'SMS alert sent successfully',
                    'timestamp': datetime.now().isoformat(),
                    'alert_type': alert_type,
                    'priority': priority,
                    'phone_number': phone_number,
                    'carrier': carrier
                }
            else:
                return {
                    'success': False,
                    'error': response.get('error', 'Failed to send SMS alert')
                }
                
        except Exception as e:
            logger.error(f"Error sending SMS alert: {str(e)}")
            return {
                'success': False,
                'error': f'Exception occurred: {str(e)}'
            }
    
    def _format_message(self, message: str, priority: str, alert_type: str) -> str:
        """Format the message for SMS with proper styling"""
        
        # Priority emojis
        priority_emojis = {
            'low': '🔵',
            'normal': '🟡',
            'high': '🟠',
            'critical': '🔴'
        }
        
        # Alert type emojis
        type_emojis = {
            'system': '⚙️',
            'security': '🔒',
            'payment': '💳',
            'access': '🚪',
            'device': '📱',
            'criminal': '🚨'
        }
        
        priority_emoji = priority_emojis.get(priority, '🟡')
        type_emoji = type_emojis.get(alert_type, '📢')
        
        # Format SMS message (keep it short for SMS)
        formatted = f"{type_emoji} EventShield Pro Alert\n"
        formatted += f"{priority_emoji} {priority.upper()} Priority\n\n"
        formatted += f"{message}\n\n"
        formatted += f"Time: {datetime.now().strftime('%H:%M:%S')}\n"
        formatted += "EventShield Pro"
        
        return formatted
    
    def _send_via_email(self, sms_email: str, message: str, alert_type: str) -> Dict:
        """Send message via email-to-SMS"""
        try:
            # Create message
            msg = MIMEMultipart()
            msg['From'] = self.email
            msg['To'] = sms_email
            msg['Subject'] = f"EventShield Pro Alert - {alert_type.title()}"
            
            # Add body
            msg.attach(MIMEText(message, 'plain'))
            
            # Send email
            server = smtplib.SMTP(self.smtp_server, self.smtp_port)
            server.starttls()
            server.login(self.email, self.password)
            text = msg.as_string()
            server.sendmail(self.email, sms_email, text)
            server.quit()
            
            return {'success': True}
            
        except Exception as e:
            return {
                'success': False,
                'error': f'Email send failed: {str(e)}'
            }
    
    def send_criminal_database_alert(self, person_name: str, match_details: str, phone_number: str, carrier: str = 'verizon') -> Dict:
        """Send specific criminal database alert"""
        message = f"🚨 CRIMINAL DATABASE MATCH ALERT 🚨\n\n"
        message += f"Person: {person_name}\n"
        message += f"Match Details: {match_details}\n"
        message += f"Action Required: Review and deny access if confirmed"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            carrier=carrier,
            priority='critical',
            alert_type='criminal'
        )
    
    def send_payment_alert(self, amount: float, method: str, status: str, phone_number: str, carrier: str = 'verizon') -> Dict:
        """Send payment-related alert"""
        message = f"💳 PAYMENT ALERT\n\n"
        message += f"Amount: ${amount:.2f}\n"
        message += f"Method: {method}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            carrier=carrier,
            priority='high' if status.lower() == 'failed' else 'normal',
            alert_type='payment'
        )
    
    def send_access_alert(self, person_name: str, access_type: str, status: str, phone_number: str, carrier: str = 'verizon') -> Dict:
        """Send access control alert"""
        message = f"🚪 ACCESS CONTROL ALERT\n\n"
        message += f"Person: {person_name}\n"
        message += f"Access Type: {access_type}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            carrier=carrier,
            priority='high' if status.lower() == 'denied' else 'normal',
            alert_type='access'
        )
    
    def send_device_alert(self, device_name: str, status: str, issue: str = None, phone_number: str = None, carrier: str = 'verizon') -> Dict:
        """Send device status alert"""
        message = f"📱 DEVICE ALERT\n\n"
        message += f"Device: {device_name}\n"
        message += f"Status: {status}\n"
        if issue:
            message += f"Issue: {issue}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            carrier=carrier,
            priority='high' if status.lower() == 'offline' else 'normal',
            alert_type='device'
        )
    
    def test_connection(self, phone_number: str, carrier: str = 'verizon') -> Dict:
        """Test the email-to-SMS connection"""
        test_message = "🔧 EventShield Pro SMS Test\n\nThis is a test message to verify SMS alert functionality."
        
        return self.send_sms_alert(
            message=test_message,
            phone_number=phone_number,
            carrier=carrier,
            priority='low',
            alert_type='system'
        )

# Global instance
sms_service = EmailSMSService()

# Convenience functions for easy integration
def send_criminal_alert(person_name: str, match_details: str, phone_number: str, carrier: str = 'verizon') -> Dict:
    """Send criminal database alert"""
    return sms_service.send_criminal_database_alert(person_name, match_details, phone_number, carrier)

def send_payment_alert(amount: float, method: str, status: str, phone_number: str, carrier: str = 'verizon') -> Dict:
    """Send payment alert"""
    return sms_service.send_payment_alert(amount, method, status, phone_number, carrier)

def send_access_alert(person_name: str, access_type: str, status: str, phone_number: str, carrier: str = 'verizon') -> Dict:
    """Send access control alert"""
    return sms_service.send_access_alert(person_name, access_type, status, phone_number, carrier)

def send_device_alert(device_name: str, status: str, issue: str = None, phone_number: str = None, carrier: str = 'verizon') -> Dict:
    """Send device alert"""
    return sms_service.send_device_alert(device_name, status, issue, phone_number, carrier)

def test_sms_connection(phone_number: str, carrier: str = 'verizon') -> Dict:
    """Test SMS connection"""
    return sms_service.test_connection(phone_number, carrier)
