# EventShield Pro - Notification Setup Guide

## 🚨 IMPORTANT: Why Your Notifications Aren't Working

Your notification system is currently **NOT CONFIGURED**. The system requires email/SMS credentials to send alerts, but none are currently set. This guide will help you fix this issue completely.

## 📋 Quick Setup (5 minutes)

### Step 1: Get Gmail App Password

The easiest way to enable notifications is using Gmail's Email-to-SMS feature.

1. **Go to your Google Account Security page:**
   - Visit: https://myaccount.google.com/security

2. **Enable 2-Step Verification** (if not already enabled)
   - Click "2-Step Verification"
   - Follow the prompts to enable it

3. **Generate an App Password:**
   - Visit: https://myaccount.google.com/apppasswords
   - Select "Mail" as the app
   - Select your device or "Other" and name it "EventShield Pro"
   - Click "Generate"
   - **COPY THE 16-CHARACTER PASSWORD** (remove spaces if any)
   - Example: `abcd efgh ijkl mnop` → `abcdefghijklmnop`

### Step 2: Configure EventShield Pro

1. **Navigate to the EventShield Pro directory:**
   ```bash
   cd "/workspace/Paradym EventShieldPro"
   ```

2. **Create your .env file:**
   ```bash
   cp .env.example .env
   ```

3. **Edit the .env file:**
   ```bash
   nano .env
   # OR
   vi .env
   # OR use any text editor
   ```

4. **Add your credentials:**
   ```bash
   # Replace with YOUR actual values
   SENDER_EMAIL=your-actual-email@gmail.com
   SENDER_PASSWORD=your-16-char-app-password
   
   # These are fine as defaults for Gmail
   SMTP_SERVER=smtp.gmail.com
   SMTP_PORT=587
   ```

5. **Save and exit**

### Step 3: Test Your Configuration

1. **Run the test script:**
   ```bash
   cd backend
   python3 test_sms_config.py
   ```

2. **Follow the prompts:**
   - Enter your phone number (10 digits: 8132702754)
   - Select your carrier (Verizon, AT&T, T-Mobile, Sprint, etc.)
   - Wait for the test SMS

3. **Check your phone:**
   - You should receive a test message within 1-2 minutes
   - If not, check the error messages and troubleshooting section below

### Step 4: Restart the Backend

```bash
cd /workspace/Paradym EventShieldPro/backend
python3 app.py
```

## 📱 Supported Carriers

The Email-to-SMS service supports these carriers:

### Major Carriers (SMS)
- **Verizon:** Works with `verizon` or `verizon sms`
- **AT&T:** Works with `att` or `at&t`
- **T-Mobile:** Works with `tmobile` or `t-mobile`
- **Sprint:** Works with `sprint`

### Major Carriers (MMS - with images)
- **Verizon MMS:** Use `verizon mms`
- **AT&T MMS:** Use `att mms`
- **T-Mobile MMS:** Use `tmobile mms`
- **Sprint MMS:** Use `sprint mms`

### Other Supported Carriers
- US Cellular, Boost Mobile, Cricket, Metro PCS, Virgin Mobile
- Straight Talk, TracFone, Republic Wireless, Google Fi
- Mint Mobile, Visible, Xfinity Mobile, Spectrum Mobile
- And many more (see email_sms_service.py for full list)

## 🔧 Troubleshooting

### Problem: "Authentication failed" or "Username and Password not accepted"

**Solution:**
- Make sure you're using an **App Password**, NOT your regular Gmail password
- Verify 2-Step Verification is enabled on your Google account
- Double-check you copied the entire 16-character app password
- Remove any spaces in the app password

### Problem: "SMTP connection failed"

**Solution:**
- Check your internet connection
- Verify SMTP_SERVER is set to `smtp.gmail.com`
- Try changing SMTP_PORT:
  - Port 587 with STARTTLS (default)
  - Port 465 with SSL (automatic fallback)
- Check if your firewall is blocking SMTP connections

### Problem: SMS not received

**Solution:**
- Wait 2-5 minutes (carrier gateways can be slow)
- Verify your phone number is correct (10 digits, no dashes)
- Try a different carrier gateway
- Some carriers block email-to-SMS from unknown senders
- Contact your carrier to enable email-to-SMS

### Problem: "Invalid phone number"

**Solution:**
- Use 10 digits only: `8132702754`
- Don't include: dashes, parentheses, country code
- Example of WRONG: `(813) 270-2754` or `+1-813-270-2754`
- Example of CORRECT: `8132702754`

### Problem: Service shows as "Not configured"

**Solution:**
- Make sure `.env` file exists (not `.env.example`)
- Check that SENDER_EMAIL and SENDER_PASSWORD are set
- Restart the backend after changing `.env`
- Run `test_sms_config.py` to verify configuration

## 📊 Testing from the Web UI

Once configured, you can test from the EventShield Pro web interface:

1. Navigate to **System Administration** or **Settings**
2. Go to the **SMS/Notification Configuration** section
3. Enter a test phone number (e.g., `8132702754`)
4. Select your carrier (e.g., `Verizon`)
5. Click **"TEST CARRIER SMS"** or **"SEND ALERT TEST"**
6. Check your phone for the test message

## 🔄 Alternative: Using Google Voice or Google Chat

If Email-to-SMS doesn't work for you, you can use:

### Option 2: Google Voice (Advanced)
Requires Google Voice API access and OAuth2 credentials.
See `GOOGLE_VOICE_SMS_SETUP.md` for details.

### Option 3: Google Chat (Advanced)
Requires Google Chat webhook URL.
See `GOOGLE_CHAT_SMS_SETUP.md` for details.

## ✅ Verification Checklist

- [ ] Created `.env` file from `.env.example`
- [ ] Set SENDER_EMAIL to your Gmail address
- [ ] Set SENDER_PASSWORD to your Gmail App Password (16 chars)
- [ ] Ran `test_sms_config.py` successfully
- [ ] Received test SMS on your phone
- [ ] Restarted the EventShield Pro backend
- [ ] Tested from the web UI

## 📞 Support

If you're still having issues after following this guide:

1. Check the backend logs for detailed error messages
2. Run `test_sms_config.py` and share the output
3. Verify your Gmail account settings
4. Contact your system administrator

## 🔐 Security Notes

- **Never** commit your `.env` file to version control
- **Never** share your App Password with anyone
- The `.env` file is already in `.gitignore`
- You can revoke App Passwords anytime from your Google Account

---

**Last Updated:** 2025-10-03
**Version:** 2.0.0
