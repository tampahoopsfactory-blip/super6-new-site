"""
SMS Alert Service using Google Chat API
Integrates with Google Chat to send SMS alerts for EventShield Pro
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

class GoogleChatSMSService:
    """Service for sending SMS alerts through Google Chat API"""
    
    def __init__(self):
        self.webhook_url = os.getenv('GOOGLE_CHAT_WEBHOOK_URL', '')
        self.space_id = os.getenv('GOOGLE_CHAT_SPACE_ID', '')
        self.api_key = os.getenv('GOOGLE_CHAT_API_KEY', '')
        self.enabled = bool(self.webhook_url and self.space_id)
        
    def send_sms_alert(self, 
                      message: str, 
                      priority: str = 'normal',
                      alert_type: str = 'system',
                      recipient_phone: Optional[str] = None) -> Dict:
        """
        Send SMS alert through Google Chat
        
        Args:
            message: The alert message to send
            priority: Alert priority (low, normal, high, critical)
            alert_type: Type of alert (system, security, payment, access)
            recipient_phone: Optional specific phone number to send to
            
        Returns:
            Dict with success status and details
        """
        if not self.enabled:
            return {
                'success': False,
                'error': 'Google Chat SMS service not configured'
            }
        
        try:
            # Format the message with EventShield Pro branding
            formatted_message = self._format_message(message, priority, alert_type)
            
            # Send to Google Chat
            response = self._send_to_google_chat(formatted_message)
            
            if response.get('success'):
                logger.info(f"SMS alert sent successfully: {alert_type} - {priority}")
                return {
                    'success': True,
                    'message': 'SMS alert sent successfully',
                    'timestamp': datetime.now().isoformat(),
                    'alert_type': alert_type,
                    'priority': priority
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
    
    def _format_message(self, message: str, priority: str, alert_type: str) -> Dict:
        """Format the message for Google Chat with proper styling"""
        
        # Priority emojis and colors
        priority_config = {
            'low': {'emoji': '🔵', 'color': '#4285F4'},
            'normal': {'emoji': '🟡', 'color': '#FBBC04'},
            'high': {'emoji': '🟠', 'color': '#FF8C00'},
            'critical': {'emoji': '🔴', 'color': '#EA4335'}
        }
        
        # Alert type emojis
        type_config = {
            'system': '⚙️',
            'security': '🔒',
            'payment': '💳',
            'access': '🚪',
            'device': '📱',
            'criminal': '🚨'
        }
        
        config = priority_config.get(priority, priority_config['normal'])
        type_emoji = type_config.get(alert_type, '📢')
        
        # Create Google Chat message format
        chat_message = {
            "cards": [
                {
                    "header": {
                        "title": f"{type_emoji} EventShield Pro Alert",
                        "subtitle": f"{config['emoji']} {priority.upper()} Priority",
                        "imageUrl": "https://via.placeholder.com/40x40/007AFF/FFFFFF?text=ES",
                        "imageStyle": "AVATAR"
                    },
                    "sections": [
                        {
                            "widgets": [
                                {
                                    "textParagraph": {
                                        "text": f"<b>Alert Type:</b> {alert_type.title()}<br/>"
                                               f"<b>Priority:</b> {priority.title()}<br/>"
                                               f"<b>Time:</b> {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}"
                                    }
                                },
                                {
                                    "textParagraph": {
                                        "text": f"<b>Message:</b><br/>{message}"
                                    }
                                }
                            ]
                        }
                    ],
                    "footer": "EventShield Pro Security System"
                }
            ]
        }
        
        return chat_message
    
    def _send_to_google_chat(self, message: Dict) -> Dict:
        """Send message to Google Chat webhook"""
        try:
            headers = {
                'Content-Type': 'application/json',
                'Authorization': f'Bearer {self.api_key}' if self.api_key else None
            }
            
            # Remove None values from headers
            headers = {k: v for k, v in headers.items() if v is not None}
            
            response = requests.post(
                self.webhook_url,
                json=message,
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
    
    def send_criminal_database_alert(self, person_name: str, match_details: str) -> Dict:
        """Send specific criminal database alert"""
        message = f"🚨 CRIMINAL DATABASE MATCH ALERT 🚨\n\n"
        message += f"Person: {person_name}\n"
        message += f"Match Details: {match_details}\n"
        message += f"Action Required: Review and deny access if confirmed"
        
        return self.send_sms_alert(
            message=message,
            priority='critical',
            alert_type='criminal'
        )
    
    def send_payment_alert(self, amount: float, method: str, status: str) -> Dict:
        """Send payment-related alert"""
        message = f"💳 PAYMENT ALERT\n\n"
        message += f"Amount: ${amount:.2f}\n"
        message += f"Method: {method}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            priority='high' if status.lower() == 'failed' else 'normal',
            alert_type='payment'
        )
    
    def send_access_alert(self, person_name: str, access_type: str, status: str) -> Dict:
        """Send access control alert"""
        message = f"🚪 ACCESS CONTROL ALERT\n\n"
        message += f"Person: {person_name}\n"
        message += f"Access Type: {access_type}\n"
        message += f"Status: {status}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            priority='high' if status.lower() == 'denied' else 'normal',
            alert_type='access'
        )
    
    def send_device_alert(self, device_name: str, status: str, issue: str = None) -> Dict:
        """Send device status alert"""
        message = f"📱 DEVICE ALERT\n\n"
        message += f"Device: {device_name}\n"
        message += f"Status: {status}\n"
        if issue:
            message += f"Issue: {issue}\n"
        message += f"Time: {datetime.now().strftime('%H:%M:%S')}"
        
        return self.send_sms_alert(
            message=message,
            priority='high' if status.lower() == 'offline' else 'normal',
            alert_type='device'
        )
    
    def test_connection(self) -> Dict:
        """Test the Google Chat connection"""
        test_message = "🔧 EventShield Pro SMS Test\n\nThis is a test message to verify SMS alert functionality."
        
        return self.send_sms_alert(
            message=test_message,
            priority='low',
            alert_type='system'
        )

# Global instance
sms_service = GoogleChatSMSService()

# Convenience functions for easy integration
def send_criminal_alert(person_name: str, match_details: str) -> Dict:
    """Send criminal database alert"""
    return sms_service.send_criminal_database_alert(person_name, match_details)

def send_payment_alert(amount: float, method: str, status: str) -> Dict:
    """Send payment alert"""
    return sms_service.send_payment_alert(amount, method, status)

def send_access_alert(person_name: str, access_type: str, status: str) -> Dict:
    """Send access control alert"""
    return sms_service.send_access_alert(person_name, access_type, status)

def send_device_alert(device_name: str, status: str, issue: str = None) -> Dict:
    """Send device alert"""
    return sms_service.send_device_alert(device_name, status, issue)

def test_sms_connection() -> Dict:
    """Test SMS connection"""
    return sms_service.test_connection()
