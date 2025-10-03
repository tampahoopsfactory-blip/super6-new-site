#!/usr/bin/env python3
"""
Quick SMS Test - Tries multiple methods to send test SMS
This is a diagnostic and testing tool
"""

import os
import sys
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

def test_direct_email_sms():
    """Test sending SMS directly via email-to-SMS gateway"""
    print("\n" + "=" * 70)
    print(" Method 1: Direct Email-to-SMS (No Auth Required)")
    print("=" * 70)
    
    phone = "8132702754"
    carrier_gateway = "vtext.com"  # Verizon
    to_address = f"{phone}@{carrier_gateway}"
    
    message = "EventShield Pro Test - If you receive this, SMS is working!"
    
    print(f"\n📱 Sending to: {to_address}")
    print(f"📝 Message: {message}")
    
    # Try sending without authentication (some SMTP servers allow this)
    try:
        # Create message
        msg = MIMEText(message)
        msg['Subject'] = 'EventShield Pro Test'
        msg['To'] = to_address
        msg['From'] = 'noreply@eventshieldpro.local'
        
        # Try local SMTP server first
        try:
            server = smtplib.SMTP('localhost', 25)
            server.send_message(msg)
            server.quit()
            print("✅ SUCCESS via local SMTP server!")
            return True
        except:
            pass
        
        # Try Gmail SMTP (will likely fail without auth)
        try:
            server = smtplib.SMTP('smtp.gmail.com', 587)
            server.starttls()
            server.send_message(msg)
            server.quit()
            print("✅ SUCCESS via Gmail SMTP!")
            return True
        except Exception as e:
            print(f"❌ Failed: {e}")
            return False
            
    except Exception as e:
        print(f"❌ Failed: {e}")
        return False

def test_gmail_smtp():
    """Test Gmail SMTP with credentials from environment"""
    print("\n" + "=" * 70)
    print(" Method 2: Gmail SMTP with Authentication")
    print("=" * 70)
    
    email = os.getenv('SENDER_EMAIL', '')
    password = os.getenv('SENDER_PASSWORD', '')
    
    if not email or not password:
        print("\n❌ No Gmail credentials found in environment")
        print("   Set SENDER_EMAIL and SENDER_PASSWORD in .env file")
        return False
    
    print(f"\n📧 Using email: {email}")
    print(f"🔑 Password: {'*' * len(password)}")
    
    phone = "8132702754"
    carrier_gateway = "vtext.com"
    to_address = f"{phone}@{carrier_gateway}"
    
    try:
        # Create message
        msg = MIMEMultipart()
        msg['From'] = email
        msg['To'] = to_address
        msg['Subject'] = 'EventShield Pro Alert'
        
        body = """EventShield Pro Test Message

This is a test SMS from EventShield Pro security system.

If you received this, your SMS functionality is working!

Time: {time}
""".format(time=__import__('datetime').datetime.now().strftime('%Y-%m-%d %H:%M:%S'))
        
        msg.attach(MIMEText(body, 'plain'))
        
        print(f"\n📤 Connecting to SMTP server...")
        
        # Connect to Gmail SMTP
        server = smtplib.SMTP('smtp.gmail.com', 587)
        server.starttls()
        
        print(f"🔐 Authenticating...")
        server.login(email, password)
        
        print(f"📨 Sending message to {to_address}...")
        text = msg.as_string()
        server.sendmail(email, to_address, text)
        server.quit()
        
        print("\n✅ SUCCESS! SMS sent via Gmail SMTP")
        print(f"   Message sent to: {to_address}")
        print(f"   Check phone {phone} for the message")
        return True
        
    except smtplib.SMTPAuthenticationError:
        print("\n❌ Authentication Failed!")
        print("\n   For Gmail users:")
        print("   1. Enable 2-factor authentication")
        print("   2. Generate App Password: https://myaccount.google.com/apppasswords")
        print("   3. Use App Password in .env file (not regular password)")
        return False
        
    except Exception as e:
        print(f"\n❌ Failed: {e}")
        import traceback
        traceback.print_exc()
        return False

def test_twilio():
    """Test Twilio SMS"""
    print("\n" + "=" * 70)
    print(" Method 3: Twilio SMS API")
    print("=" * 70)
    
    try:
        from twilio.rest import Client
    except ImportError:
        print("\n❌ Twilio SDK not installed")
        print("   Install with: pip install twilio")
        return False
    
    account_sid = os.getenv('TWILIO_ACCOUNT_SID', '')
    auth_token = os.getenv('TWILIO_AUTH_TOKEN', '')
    from_number = os.getenv('TWILIO_PHONE_NUMBER', '')
    
    if not account_sid or not auth_token or not from_number:
        print("\n❌ Twilio credentials not configured")
        print("   Set TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN, and TWILIO_PHONE_NUMBER")
        return False
    
    print(f"\n📱 From: {from_number}")
    print(f"📱 To: +18132702754")
    
    try:
        client = Client(account_sid, auth_token)
        
        message = client.messages.create(
            body="EventShield Pro Test - Your SMS is working via Twilio!",
            from_=from_number,
            to='+18132702754'
        )
        
        print(f"\n✅ SUCCESS! SMS sent via Twilio")
        print(f"   Message SID: {message.sid}")
        print(f"   Status: {message.status}")
        return True
        
    except Exception as e:
        print(f"\n❌ Failed: {e}")
        return False

def main():
    print("\n")
    print("╔════════════════════════════════════════════════════════════════════╗")
    print("║                                                                    ║")
    print("║              EventShield Pro - Quick SMS Test                     ║")
    print("║                                                                    ║")
    print("╚════════════════════════════════════════════════════════════════════╝")
    
    print("\nTarget Phone: 8132702754 (Verizon)")
    print("\nTrying multiple SMS methods...\n")
    
    # Load environment variables
    if os.path.exists('.env'):
        print("📄 Loading .env file...")
        with open('.env', 'r') as f:
            for line in f:
                line = line.strip()
                if line and not line.startswith('#') and '=' in line:
                    key, value = line.split('=', 1)
                    os.environ[key] = value
        print("✅ Environment loaded\n")
    else:
        print("⚠️  No .env file found\n")
    
    # Test methods in order
    success = False
    
    # Method 1: Direct (usually doesn't work but worth trying)
    if not success:
        success = test_direct_email_sms()
    
    # Method 2: Gmail SMTP (most likely to work)
    if not success:
        success = test_gmail_smtp()
    
    # Method 3: Twilio (most reliable if configured)
    if not success:
        success = test_twilio()
    
    # Summary
    print("\n" + "=" * 70)
    print(" TEST SUMMARY")
    print("=" * 70)
    
    if success:
        print("\n✅ SMS functionality is WORKING!")
        print("\nYour phone should receive a test message shortly.")
        print("It may take 1-2 minutes to arrive.\n")
        return 0
    else:
        print("\n❌ All SMS methods FAILED")
        print("\nRecommended actions:")
        print("  1. Run the configuration wizard:")
        print("     python3 configure_sms.py")
        print("\n  2. Or manually edit .env file with your credentials")
        print("\n  3. See sms_setup_instructions.md for detailed help\n")
        return 1

if __name__ == '__main__':
    try:
        sys.exit(main())
    except KeyboardInterrupt:
        print("\n\n⏹️  Test cancelled by user")
        sys.exit(1)
    except Exception as e:
        print(f"\n\n❌ Unexpected error: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)
