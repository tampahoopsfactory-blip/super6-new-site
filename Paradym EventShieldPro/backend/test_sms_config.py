#!/usr/bin/env python3
"""
Test SMS Configuration for EventShield Pro
This script helps you verify your SMS/email configuration is working correctly.
"""

import os
import sys
from email_sms_service import sms_service

def main():
    print("=" * 80)
    print("EventShield Pro - SMS Configuration Test")
    print("=" * 80)
    print()
    
    # Check environment variables
    print("1. Checking Environment Variables...")
    print("-" * 80)
    
    sender_email = os.getenv('SENDER_EMAIL') or os.getenv('SMTP_EMAIL') or os.getenv('EMAIL_ADDRESS')
    sender_password = os.getenv('SENDER_PASSWORD') or os.getenv('SMTP_PASSWORD') or os.getenv('EMAIL_PASSWORD')
    smtp_server = os.getenv('SMTP_SERVER', 'smtp.gmail.com')
    smtp_port = os.getenv('SMTP_PORT', '587')
    
    print(f"   SENDER_EMAIL: {'✓ Set' if sender_email else '✗ NOT SET'}")
    if sender_email:
        print(f"   Email: {sender_email}")
    
    print(f"   SENDER_PASSWORD: {'✓ Set' if sender_password else '✗ NOT SET'}")
    if sender_password:
        # Show first 4 characters only
        print(f"   Password: {sender_password[:4]}{'*' * (len(sender_password) - 4)}")
    
    print(f"   SMTP_SERVER: {smtp_server}")
    print(f"   SMTP_PORT: {smtp_port}")
    print()
    
    # Check service status
    print("2. Checking SMS Service Status...")
    print("-" * 80)
    print(f"   Service Enabled: {'✓ YES' if sms_service.enabled else '✗ NO'}")
    print(f"   Email Configured: {'✓ YES' if sms_service.email else '✗ NO'}")
    print(f"   Password Configured: {'✓ YES' if sms_service.password else '✗ NO'}")
    print()
    
    if not sms_service.enabled:
        print("❌ SMS Service is NOT configured!")
        print()
        print("To fix this:")
        print("1. Copy .env.example to .env")
        print("2. Edit .env and set SENDER_EMAIL and SENDER_PASSWORD")
        print("3. For Gmail, use an App Password (see instructions in .env.example)")
        print("4. Restart the EventShield Pro backend")
        print()
        return False
    
    print("✅ SMS Service is configured and ready!")
    print()
    
    # Test sending
    print("3. Testing SMS Send...")
    print("-" * 80)
    
    # Get test phone number from user
    phone = input("Enter your phone number (10 digits, e.g., 8132702754): ").strip()
    if not phone:
        print("❌ No phone number provided. Skipping send test.")
        return True
    
    # Get carrier
    print()
    print("Common carriers:")
    print("  1. Verizon")
    print("  2. AT&T")
    print("  3. T-Mobile")
    print("  4. Sprint")
    print()
    carrier_input = input("Enter carrier name or number (default: Verizon): ").strip()
    
    carrier_map = {
        '1': 'verizon',
        '2': 'att',
        '3': 'tmobile',
        '4': 'sprint'
    }
    
    carrier = carrier_map.get(carrier_input, carrier_input.lower() or 'verizon')
    
    print()
    print(f"Sending test SMS to {phone} via {carrier}...")
    
    result = sms_service.test_connection(phone, carrier)
    
    if result.get('success'):
        print("✅ Test SMS sent successfully!")
        print(f"   Check your phone for the test message.")
        print(f"   Timestamp: {result.get('timestamp')}")
    else:
        print("❌ Test SMS failed!")
        print(f"   Error: {result.get('error')}")
        print()
        print("Common issues:")
        print("  - Gmail: Make sure you're using an App Password, not your regular password")
        print("  - Check that 2-Step Verification is enabled on your Google account")
        print("  - Verify the phone number is correct (10 digits, no dashes or spaces)")
        print("  - Try a different carrier if the first one doesn't work")
    
    print()
    return result.get('success', False)

if __name__ == '__main__':
    try:
        success = main()
        sys.exit(0 if success else 1)
    except KeyboardInterrupt:
        print("\n\nTest cancelled by user.")
        sys.exit(1)
    except Exception as e:
        print(f"\n\n❌ Error: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)
