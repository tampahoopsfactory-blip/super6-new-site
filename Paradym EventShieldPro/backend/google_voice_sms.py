"""
Google Voice SMS Service for EventShield Pro
Sends SMS alerts through Google Voice API
"""

import requests
import json
import logging
from typing import Dict, List, Optional
from datetime import datetime
import os

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class GoogleVoiceSMSService:
    """Service for sending SMS alerts through Google Voice API"""
    
    def __init__(self):
        self.api_key = os.getenv('GOOGLE_VOICE_API_KEY', '')
        self.voice_number = os.getenv('GOOGLE_VOICE_NUMBER', '')
        self.enabled = bool(self.api_key and self.voice_number)
        
    def send_sms_alert(self, 
                      message: str, 
                      phone_number: str,
                      priority: str = 'normal',
                      alert_type: str = 'system') -> Dict:
        """
        Send SMS alert through Google Voice
        
        Args:
            message: The alert message to send
            phone_number: Target phone number (e.g., +1234567890)
            priority: Alert priority (low, normal, high, critical)
            alert_type: Type of alert (system, security, payment, access)
            
        Returns:
            Dict with success status and details
        """
        if not self.enabled:
            return {
                'success': False,
                'error': 'Google Voice SMS service not configured'
            }
        
        try:
            # Format the message with EventShield Pro branding
            formatted_message = self._format_message(message, priority, alert_type)
            
            # Send via Google Voice API
            response = self._send_via_google_voice(formatted_message, phone_number)
            
            if response.get('success'):
                logger.info(f"SMS alert sent successfully: {alert_type} - {priority}")
                return {
                    'success': True,
                    'message': 'SMS alert sent successfully',
                    'timestamp': datetime.now().isoformat(),
                    'alert_type': alert_type,
                    'priority': priority,
                    'phone_number': phone_number
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
        
        # Format SMS message
        formatted = f"{type_emoji} EventShield Pro Alert\n"
        formatted += f"{priority_emoji} {priority.upper()} Priority\n\n"
        formatted += f"{message}\n\n"
        formatted += f"Time: {datetime.now().strftime('%H:%M:%S')}\n"
        formatted += "EventShield Pro Security System"
        
        return formatted
    
    def _send_via_google_voice(self, message: str, phone_number: str) -> Dict:
        """Send message via Google Voice API"""
        try:
            # Google Voice API endpoint
            url = "https://voice.googleapis.com/v1/voice:calls"
            
            headers = {
                'Authorization': f'Bearer {self.api_key}',
                'Content-Type': 'application/json'
            }
            
            # Google Voice API payload
            payload = {
                "destination": phone_number,
                "text": message,
                "source": self.voice_number
            }
            
            response = requests.post(
                url,
                json=payload,
                headers=headers,
                timeout=10
            )
            
            if response.status_code == 200:
                return {'success': True}
            else:
                return {
                    'success': False,
                    'error': f'HTTP {response.status_code}: {response.text}'
                }
                
        except requests.exceptions.RequestException as e:
            return {
                'success': False,
                'error': f'Request failed: {str(e)}'
            }
    
    def send_criminal_database_alert(self, person_name: str, match_details: str, phone_number: str) -> Dict:
        """Send specific criminal database alert"""
        message = f"🚨 CRIMINAL DATABASE MATCH ALERT 🚨\n\n"
        message += f"Person: {person_name}\n"
        message += f"Match Details: {match_details}\n"
        message += f"Action Required: Review and deny access if confirmed"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            priority='critical',
            alert_type='criminal'
        )
    
    def send_payment_alert(self, amount: float, method: str, status: str, phone_number: str) -> Dict:
        """Send payment-related alert"""
        message = f"💳 PAYMENT ALERT\n\n"
        message += f"Amount: ${amount:.2f}\n"
        message += f"Method: {method}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            priority='high' if status.lower() == 'failed' else 'normal',
            alert_type='payment'
        )
    
    def send_access_alert(self, person_name: str, access_type: str, status: str, phone_number: str) -> Dict:
        """Send access control alert"""
        message = f"🚪 ACCESS CONTROL ALERT\n\n"
        message += f"Person: {person_name}\n"
        message += f"Access Type: {access_type}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            phone_number=phone_number,
            priority='high' if status.lower() == 'denied' else 'normal',
            alert_type='access'
        )
    
    def send_device_alert(self, device_name: str, status: str, issue: str = None, phone_number: str = None) -> Dict:
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
            priority='high' if status.lower() == 'offline' else 'normal',
            alert_type='device'
        )
    
    def test_connection(self, phone_number: str) -> Dict:
        """Test the Google Voice connection"""
        test_message = "🔧 EventShield Pro SMS Test\n\nThis is a test message to verify SMS alert functionality."
        
        return self.send_sms_alert(
            message=test_message,
            phone_number=phone_number,
            priority='low',
            alert_type='system'
        )

# Global instance
sms_service = GoogleVoiceSMSService()

# Convenience functions for easy integration
def send_criminal_alert(person_name: str, match_details: str, phone_number: str) -> Dict:
    """Send criminal database alert"""
    return sms_service.send_criminal_database_alert(person_name, match_details, phone_number)

def send_payment_alert(amount: float, method: str, status: str, phone_number: str) -> Dict:
    """Send payment alert"""
    return sms_service.send_payment_alert(amount, method, status, phone_number)

def send_access_alert(person_name: str, access_type: str, status: str, phone_number: str) -> Dict:
    """Send access control alert"""
    return sms_service.send_access_alert(person_name, access_type, status, phone_number)

def send_device_alert(device_name: str, status: str, issue: str = None, phone_number: str = None) -> Dict:
    """Send device alert"""
    return sms_service.send_device_alert(device_name, status, issue, phone_number)

def test_sms_connection(phone_number: str) -> Dict:
    """Test SMS connection"""
    return sms_service.test_connection(phone_number)
