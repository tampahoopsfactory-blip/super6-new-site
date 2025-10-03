#!/usr/bin/env python3
"""
Twilio SMS Test for EventShield Pro
A more reliable SMS option using Twilio API
"""

import os
import sys

def test_twilio_sms():
    print("=" * 70)
    print(" EventShield Pro - Twilio SMS Test")
    print("=" * 70)
    print()
    
    try:
        from twilio.rest import Client
    except ImportError:
        print("❌ Twilio SDK not installed!")
        print()
        print("To install Twilio:")
        print("  pip install twilio")
        print()
        print("Twilio offers a reliable SMS service with a free trial.")
        print("Sign up at: https://www.twilio.com/try-twilio")
        print()
        return 1
    
    # Get credentials from environment
    account_sid = os.getenv('TWILIO_ACCOUNT_SID', '')
    auth_token = os.getenv('TWILIO_AUTH_TOKEN', '')
    from_number = os.getenv('TWILIO_PHONE_NUMBER', '')
    to_number = '8132702754'
    
    print(f"📱 Sending SMS to: {to_number}")
    print()
    
    if not account_sid or not auth_token or not from_number:
        print("❌ Twilio credentials not configured!")
        print()
        print("Please set these environment variables in .env:")
        print("  TWILIO_ACCOUNT_SID=your_account_sid")
        print("  TWILIO_AUTH_TOKEN=your_auth_token")
        print("  TWILIO_PHONE_NUMBER=+1234567890")
        print()
        print("Get your credentials from: https://console.twilio.com/")
        print()
        return 1
    
    print("🔍 Configuration:")
    print(f"   Account SID: {account_sid[:10]}...")
    print(f"   From Number: {from_number}")
    print()
    
    try:
        # Create Twilio client
        client = Client(account_sid, auth_token)
        
        # Send message
        message = client.messages.create(
            body="""EventShield Pro Test Alert

This is a test SMS from EventShield Pro security system.

Your SMS functionality is working correctly!

EventShield Pro - Security Made Simple""",
            from_=from_number,
            to=f'+1{to_number}'
        )
        
        print("✅ SUCCESS! SMS sent via Twilio")
        print()
        print(f"   Message SID: {message.sid}")
        print(f"   Status: {message.status}")
        print(f"   To: +1{to_number}")
        print()
        print("Check your phone for the test message!")
        print()
        return 0
        
    except Exception as e:
        print(f"❌ FAILED: {e}")
        print()
        return 1

if __name__ == '__main__':
    sys.exit(test_twilio_sms())
