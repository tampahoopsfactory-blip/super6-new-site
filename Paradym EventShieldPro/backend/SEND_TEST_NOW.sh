#!/bin/bash
# Quick SMS Test Script
# Sends test SMS to 8132702754

echo ""
echo "╔════════════════════════════════════════════════════════════════╗"
echo "║                                                                ║"
echo "║          EventShield Pro - Send Test SMS Now                  ║"
echo "║                                                                ║"
echo "╚════════════════════════════════════════════════════════════════╝"
echo ""
echo "📱 Sending test SMS to: 8132702754 (Verizon)"
echo ""

# Check if .env exists
if [ ! -f .env ]; then
    echo "❌ ERROR: .env file not found!"
    echo ""
    echo "Please create .env file with your credentials:"
    echo "  cp .env.template .env"
    echo "  nano .env"
    echo ""
    exit 1
fi

# Check if credentials are configured
if grep -q "your_email@gmail.com" .env 2>/dev/null; then
    echo "⚠️  WARNING: .env still has placeholder values"
    echo ""
    echo "Please edit .env and add your real credentials:"
    echo "  nano .env"
    echo ""
    echo "For Gmail:"
    echo "  SENDER_EMAIL=your_email@gmail.com"
    echo "  SENDER_PASSWORD=your_16_char_app_password"
    echo ""
    echo "Get App Password: https://myaccount.google.com/apppasswords"
    echo ""
    exit 1
fi

echo "✅ Configuration found"
echo ""
echo "Sending test message..."
echo ""

# Run the test
python3 send_test_sms.py

echo ""
echo "════════════════════════════════════════════════════════════════"
echo ""
echo "If successful, check your phone for the test message!"
echo "It may take 1-2 minutes to arrive."
echo ""
