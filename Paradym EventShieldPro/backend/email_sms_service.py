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
        # Support multiple environment variable names for flexibility
        self.email = os.getenv('SENDER_EMAIL') or os.getenv('SMTP_EMAIL') or os.getenv('EMAIL_ADDRESS') or ''
        self.password = os.getenv('SENDER_PASSWORD') or os.getenv('SMTP_PASSWORD') or os.getenv('EMAIL_PASSWORD') or ''
        self.enabled = bool(self.email and self.password)
        
        if not self.enabled:
            logger.warning("Email-to-SMS service not configured. Please set SENDER_EMAIL and SENDER_PASSWORD environment variables.")
        
        # Carrier email-to-SMS mappings (SMS only, text-only)
        self.carrier_emails = {
            'verizon': '@vtext.com',
            'verizon sms': '@vtext.com',
            'att': '@txt.att.net',
            'at&t': '@txt.att.net',
            'tmobile': '@tmomail.net',
            't-mobile': '@tmomail.net',
            'sprint': '@messaging.sprintpcs.com',
            'uscellular': '@email.uscc.net',
            'us cellular': '@email.uscc.net',
            'boost': '@smsmyboostmobile.com',
            'boost mobile': '@smsmyboostmobile.com',
            'cricket': '@sms.cricketwireless.net',
            'metropcs': '@mymetropcs.com',
            'metro pcs': '@mymetropcs.com',
            'virgin': '@vmobl.com',
            'virgin mobile': '@vmobl.com',
            'straighttalk': '@vtext.com',
            'straight talk': '@vtext.com',
            'tracfone': '@mmst5.tracfone.com',
            'republic': '@text.republicwireless.com',
            'googlefi': '@msg.fi.google.com',
            'google fi': '@msg.fi.google.com',
            'mint': '@mailmymobile.net',
            'mint mobile': '@mailmymobile.net',
            'visible': '@vtext.com',
            'xfinity': '@vtext.com',
            'xfinity mobile': '@vtext.com',
            'spectrum': '@vtext.com',
            'spectrum mobile': '@vtext.com',
            'altice': '@vtext.com',
            'redpocket': '@vtext.com',
            'red pocket': '@vtext.com',
            'h2o': '@txt.att.net',
            'h2o wireless': '@txt.att.net',
            'ultra': '@tmomail.net',
            'ultra mobile': '@tmomail.net',
            'lycamobile': '@lycamobile.us',
            'lyca mobile': '@lycamobile.us',
            'simple': '@smtext.com',
            'simple mobile': '@smtext.com',
            'net10': '@mmst5.tracfone.com',
            'total': '@vtext.com',
            'total wireless': '@vtext.com',
            'pageplus': '@vtext.com',
            'page plus': '@vtext.com',
            'selectel': '@vtext.com',
            'tello': '@tmomail.net',
            'twigby': '@vtext.com',
            'unreal': '@vtext.com',
            'unreal mobile': '@vtext.com'
        }
        
        # Carrier email-to-MMS mappings (for images/attachments)
        self.carrier_mms_emails = {
            'verizon': '@vzwpix.com',
            'verizon mms': '@vzwpix.com',
            'att': '@mms.att.net',
            'at&t': '@mms.att.net',
            'tmobile': '@tmomail.net',
            't-mobile': '@tmomail.net',
            'sprint': '@pm.sprint.com',
            'uscellular': '@mms.uscc.net',
            'us cellular': '@mms.uscc.net',
            'boost': '@myboostmobile.com',
            'boost mobile': '@myboostmobile.com',
            'cricket': '@mms.cricketwireless.net',
            'metropcs': '@mymetropcs.com',
            'metro pcs': '@mymetropcs.com',
            'virgin': '@vmpix.com',
            'virgin mobile': '@vmpix.com',
            'straighttalk': '@vzwpix.com',
            'straight talk': '@vzwpix.com',
            'tracfone': '@mmst5.tracfone.com',
            'republic': '@text.republicwireless.com',
            'googlefi': '@msg.fi.google.com',
            'google fi': '@msg.fi.google.com',
            'mint': '@mailmymobile.net',
            'mint mobile': '@mailmymobile.net',
            'visible': '@vzwpix.com',
            'xfinity': '@vzwpix.com',
            'xfinity mobile': '@vzwpix.com',
            'spectrum': '@vzwpix.com',
            'spectrum mobile': '@vzwpix.com'
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
            # Format phone number - remove all non-digit characters
            clean_phone = ''.join(filter(str.isdigit, phone_number))
            
            # Remove leading 1 or +1 if present, we just want the 10-digit number
            if len(clean_phone) == 11 and clean_phone.startswith('1'):
                clean_phone = clean_phone[1:]
            
            # Validate 10-digit phone number
            if len(clean_phone) != 10:
                return {
                    'success': False,
                    'error': f'Invalid phone number: {phone_number}. Must be 10 digits (e.g., 8132702754)'
                }
            
            # Determine if MMS should be used based on carrier name
            use_mms = 'mms' in carrier.lower()
            
            # Get carrier email gateway
            if use_mms:
                carrier_email = self.carrier_mms_emails.get(carrier.lower(), self.carrier_mms_emails.get(carrier.lower().replace(' mms', ''), '@vzwpix.com'))
            else:
                carrier_email = self.carrier_emails.get(carrier.lower(), '@vtext.com')
            
            sms_email = f"{clean_phone}{carrier_email}"
            
            logger.info(f"Sending {'MMS' if use_mms else 'SMS'} to {sms_email} (Phone: {phone_number}, Carrier: {carrier})")
            
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
            
            # Try STARTTLS (port 587) first, then fall back to SSL (port 465)
            server = None
            try:
                # Try TLS on port 587
                logger.info(f"Attempting to send email to {sms_email} via {self.smtp_server}:{self.smtp_port}")
                server = smtplib.SMTP(self.smtp_server, self.smtp_port)
                server.set_debuglevel(1)  # Enable debug output
                server.ehlo()
                if server.has_extn('STARTTLS'):
                    server.starttls()
                    server.ehlo()
                server.login(self.email, self.password)
                text = msg.as_string()
                server.sendmail(self.email, sms_email, text)
                server.quit()
                logger.info(f"Successfully sent email to {sms_email}")
                return {'success': True}
                
            except Exception as tls_error:
                logger.warning(f"TLS connection failed: {str(tls_error)}")
                if server:
                    try:
                        server.quit()
                    except:
                        pass
                
                # Try SSL on port 465 as fallback
                try:
                    logger.info("Trying SSL connection on port 465...")
                    server = smtplib.SMTP_SSL(self.smtp_server, 465)
                    server.set_debuglevel(1)
                    server.login(self.email, self.password)
                    text = msg.as_string()
                    server.sendmail(self.email, sms_email, text)
                    server.quit()
                    logger.info(f"Successfully sent email to {sms_email} via SSL")
                    return {'success': True}
                except Exception as ssl_error:
                    logger.error(f"SSL connection also failed: {str(ssl_error)}")
                    raise ssl_error
            
        except Exception as e:
            logger.error(f"Email send failed to {sms_email}: {str(e)}", exc_info=True)
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
