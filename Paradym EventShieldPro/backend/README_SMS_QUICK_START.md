# 📱 SMS Quick Start Guide

## TL;DR - Get SMS Working in 5 Minutes

### For Gmail Users (Easiest)

1. **Edit `.env` file:**
   ```bash
   nano .env
   ```

2. **Add your Gmail:**
   ```
   SENDER_EMAIL=your_email@gmail.com
   SENDER_PASSWORD=your_16_char_app_password
   ```

3. **Get Gmail App Password:**
   - Go to: https://myaccount.google.com/apppasswords
   - Click "Generate"
   - Copy the 16-character password
   - Paste it in the `.env` file

4. **Test it:**
   ```bash
   python3 send_test_sms.py
   ```

5. **Done!** Check your phone (8132702754)

---

## For Twilio Users (Most Reliable)

1. **Sign up:** https://www.twilio.com/try-twilio (Free trial)

2. **Get credentials** from https://console.twilio.com/

3. **Edit `.env`:**
   ```
   TWILIO_ACCOUNT_SID=ACxxxxx
   TWILIO_AUTH_TOKEN=your_token
   TWILIO_PHONE_NUMBER=+12345678900
   ```

4. **Install SDK:**
   ```bash
   pip install twilio
   ```

5. **Test:**
   ```bash
   python3 sms_test_twilio.py
   ```

---

## Files You Need to Know

| File | Purpose |
|------|---------|
| `.env` | **Your credentials go here** |
| `send_test_sms.py` | Send test SMS to 8132702754 |
| `quick_sms_test.py` | Test all SMS methods |
| `configure_sms.py` | Interactive setup wizard |

---

## Troubleshooting

### "Authentication Failed" with Gmail
- ✅ Use App Password (not regular password)
- ✅ Enable 2-factor authentication first
- ✅ Generate new App Password if old one doesn't work

### "Message Not Received"
- ✅ Wait 1-2 minutes (SMS can be slow)
- ✅ Check carrier is correct (Verizon for 8132702754)
- ✅ Try Twilio instead (more reliable)

### "Twilio SDK Not Found"
```bash
pip install twilio
```

---

## Need More Help?

Run the interactive wizard:
```bash
python3 configure_sms.py
```

Or read the full guide:
```bash
cat sms_setup_instructions.md
```

---

**Quick Test:** `python3 send_test_sms.py`  
**Your Phone:** 8132702754 (Verizon)
