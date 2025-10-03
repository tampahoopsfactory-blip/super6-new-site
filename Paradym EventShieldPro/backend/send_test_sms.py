#!/usr/bin/env python3
"""
Send Test SMS - EventShield Pro
This script sends a test SMS to verify the SMS functionality
"""

import os
import sys
from email_sms_service import EmailSMSService

def main():
    print("=" * 70)
    print(" EventShield Pro - SMS Test Message Sender")
    print("=" * 70)
    print()
    
    # Get configuration
    phone_number = "8132702754"
    carrier = "verizon"
    
    print(f"📱 Target Phone: {phone_number}")
    print(f"📡 Carrier: {carrier}")
    print()
    
    # Initialize SMS service
    sms_service = EmailSMSService()
    
    # Check if service is configured
    print("🔍 Checking SMS Service Configuration...")
    print(f"   ✓ SMTP Server: {sms_service.smtp_server}")
    print(f"   ✓ SMTP Port: {sms_service.smtp_port}")
    print(f"   ✓ Email: {'✓ Configured' if sms_service.email else '✗ NOT configured'}")
    print(f"   ✓ Password: {'✓ Configured' if sms_service.password else '✗ NOT configured'}")
    print()
    
    if not sms_service.enabled:
        print("❌ ERROR: SMS Service is NOT properly configured!")
        print()
        print("Please configure the following environment variables in .env file:")
        print("  SENDER_EMAIL=your_email@gmail.com")
        print("  SENDER_PASSWORD=your_app_password")
        print()
        print("📝 For Gmail users:")
        print("  1. Enable 2-factor authentication")
        print("  2. Go to https://myaccount.google.com/apppasswords")
        print("  3. Generate an app password for 'Mail'")
        print("  4. Use that password in .env file")
        print()
        return 1
    
    # Prepare test message
    test_message = """
EventShield Pro SMS Test

This is a test message from EventShield Pro security system.

If you received this message, the SMS functionality is working correctly!

Test conducted at: {time}
Phone: {phone}
Carrier: {carrier}

EventShield Pro - Security Made Simple
""".format(
        time=__import__('datetime').datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
        phone=phone_number,
        carrier=carrier.upper()
    )
    
    print("📤 Sending test SMS message...")
    print()
    
    # Send the SMS
    result = sms_service.send_sms_alert(
        message=test_message,
        phone_number=phone_number,
        carrier=carrier,
        priority='normal',
        alert_type='system'
    )
    
    print("-" * 70)
    print("📊 RESULT:")
    print("-" * 70)
    
    if result.get('success'):
        print("✅ SUCCESS! SMS sent successfully!")
        print()
        print(f"   Message sent to: {result.get('phone_number')}")
        print(f"   Carrier: {result.get('carrier')}")
        print(f"   Timestamp: {result.get('timestamp')}")
        print()
        print("Please check your phone for the test message.")
        print("It may take 1-2 minutes to arrive.")
        print()
        return 0
    else:
        print("❌ FAILED! SMS could not be sent.")
        print()
        print(f"   Error: {result.get('error')}")
        print()
        print("Troubleshooting:")
        print("  1. Verify your email credentials in .env")
        print("  2. Make sure you're using an App Password (not regular password)")
        print("  3. Check that 2-factor authentication is enabled")
        print("  4. Verify the carrier is correct for the phone number")
        print()
        return 1
    
    print("=" * 70)

if __name__ == '__main__':
    try:
        exit_code = main()
        sys.exit(exit_code)
    except KeyboardInterrupt:
        print("\n\n⏹️  Test cancelled by user")
        sys.exit(1)
    except Exception as e:
        print(f"\n\n❌ Unexpected error: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)
