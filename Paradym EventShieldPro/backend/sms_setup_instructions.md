# EventShield Pro - SMS Setup Instructions

## Current Status
The SMS functionality in EventShield Pro is implemented but **requires configuration** to work. None of the SMS services are currently configured with credentials.

## Available SMS Methods

### Method 1: Email-to-SMS Gateway (Recommended for Testing)
**Pros:**
- Free
- Works with any email account
- No signup required for external services

**Cons:**
- May be less reliable
- Carrier-dependent delivery
- Some carriers block email-to-SMS

**Setup Steps:**
1. Edit the `.env` file in the backend directory
2. Add your Gmail credentials:
   ```
   SENDER_EMAIL=your_email@gmail.com
   SENDER_PASSWORD=your_app_password
   ```
3. For Gmail App Password:
   - Enable 2-factor authentication: https://myaccount.google.com/security
   - Generate app password: https://myaccount.google.com/apppasswords
   - Use the 16-character password in the .env file

4. Test the SMS:
   ```bash
   python3 send_test_sms.py
   ```

### Method 2: Twilio (Recommended for Production)
**Pros:**
- Very reliable
- Professional service
- Good delivery rates
- Free trial available ($15 credit)

**Cons:**
- Requires signup
- Costs money after trial

**Setup Steps:**
1. Sign up for Twilio: https://www.twilio.com/try-twilio
2. Get a phone number (during signup)
3. Get your Account SID and Auth Token from: https://console.twilio.com/
4. Add to `.env`:
   ```
   TWILIO_ACCOUNT_SID=your_account_sid
   TWILIO_AUTH_TOKEN=your_auth_token
   TWILIO_PHONE_NUMBER=+1234567890
   ```
5. Install Twilio SDK:
   ```bash
   pip install twilio
   ```
6. Test:
   ```bash
   python3 sms_test_twilio.py
   ```

### Method 3: Google Voice (Not Recommended)
**Status:** Requires Google Voice API which is not publicly available.

**Note:** This method is implemented in the code but requires unofficial APIs that may not work reliably.

## Quick Test

To test the current Email-to-SMS setup:

```bash
# 1. Configure your email in .env
nano .env  # or use your preferred editor

# 2. Run the test
python3 send_test_sms.py
```

The test will send an SMS to: **8132702754** (Verizon carrier)

## Troubleshooting

### Email-to-SMS Issues:
1. **"Authentication failed"**
   - Use an App Password, not your regular password
   - Enable 2-factor authentication first

2. **"Message not received"**
   - Verify the carrier is correct (Verizon for 8132702754)
   - Some carriers block email-to-SMS
   - Try Twilio instead

3. **"Connection timeout"**
   - Check firewall settings
   - Verify SMTP port 587 is not blocked

### Twilio Issues:
1. **"Unverified number"**
   - During trial, you can only send to verified numbers
   - Verify your number in the Twilio console

2. **"Insufficient funds"**
   - Add credit to your Twilio account
   - Or use email-to-SMS method

## Current Implementation

The application has **THREE** SMS implementations:

1. **`email_sms_service.py`** - Email-to-SMS gateway ✅ Easiest to set up
2. **`google_voice_sms.py`** - Google Voice (not working)
3. **`sms_service.py`** - Google Chat (not for SMS)

## Recommended Next Steps

1. ✅ Use Email-to-SMS for immediate testing (configure .env)
2. ✅ Switch to Twilio for production use
3. ❌ Don't use Google Voice or Google Chat methods

## Testing Your Phone Number

Phone: **8132702754**  
Carrier: **Verizon**

To send a test SMS:
```bash
python3 send_test_sms.py
```

This will send a test message to verify the SMS functionality is working.
