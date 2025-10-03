#!/usr/bin/env python3
"""
Test SMS Functionality
This script tests the SMS sending capabilities using email-to-SMS gateway
"""

import sys
import os
from email_sms_service import EmailSMSService

def test_sms_send(phone_number, carrier='verizon'):
    """Test sending SMS to a specific phone number"""
    
    print("=" * 60)
    print("EventShield Pro - SMS Test")
    print("=" * 60)
    print()
    
    # Initialize service
    sms_service = EmailSMSService()
    
    # Check configuration
    print("Checking SMS Service Configuration...")
    print(f"  Email configured: {bool(sms_service.email)}")
    print(f"  Password configured: {bool(sms_service.password)}")
    print(f"  SMTP Server: {sms_service.smtp_server}")
    print(f"  SMTP Port: {sms_service.smtp_port}")
    print(f"  Service Enabled: {sms_service.enabled}")
    print()
    
    if not sms_service.enabled:
        print("❌ SMS Service is NOT configured!")
        print()
        print("To configure email-to-SMS, set these environment variables:")
        print("  SENDER_EMAIL=your_email@gmail.com")
        print("  SENDER_PASSWORD=your_app_password")
        print()
        print("For Gmail, you need to:")
        print("  1. Enable 2-factor authentication")
        print("  2. Generate an App Password")
        print("  3. Use that App Password as SENDER_PASSWORD")
        print()
        return False
    
    # Test send
    print(f"Sending test SMS to: {phone_number} (carrier: {carrier})")
    print()
    
    test_message = "EventShield Pro SMS Test - This is a test message to verify SMS functionality is working correctly!"
    
    result = sms_service.send_sms_alert(
        message=test_message,
        phone_number=phone_number,
        carrier=carrier,
        priority='normal',
        alert_type='system'
    )
    
    print("Result:")
    print(f"  Success: {result.get('success')}")
    
    if result.get('success'):
        print(f"  Message: {result.get('message')}")
        print(f"  Timestamp: {result.get('timestamp')}")
        print(f"  Phone: {result.get('phone_number')}")
        print(f"  Carrier: {result.get('carrier')}")
        print()
        print("✅ SMS sent successfully!")
    else:
        print(f"  Error: {result.get('error')}")
        print()
        print("❌ Failed to send SMS")
    
    print()
    print("=" * 60)
    
    return result.get('success', False)

if __name__ == '__main__':
    # Default test phone number
    phone = '8132702754'
    carrier = 'verizon'
    
    # Override from command line if provided
    if len(sys.argv) > 1:
        phone = sys.argv[1]
    if len(sys.argv) > 2:
        carrier = sys.argv[2]
    
    success = test_sms_send(phone, carrier)
    sys.exit(0 if success else 1)
