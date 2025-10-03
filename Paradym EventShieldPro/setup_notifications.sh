#!/bin/bash

# EventShield Pro - Notification Setup Script
# This script helps you quickly configure SMS/Email notifications

set -e

echo "================================================================================"
echo "EventShield Pro - Notification Setup Wizard"
echo "================================================================================"
echo ""
echo "This wizard will help you configure SMS and Email notifications."
echo "You'll need:"
echo "  1. A Gmail account"
echo "  2. A Gmail App Password (we'll show you how to get one)"
echo ""
echo "Press ENTER to continue or CTRL+C to cancel..."
read

# Check if .env already exists
if [ -f ".env" ]; then
    echo ""
    echo "⚠️  WARNING: .env file already exists!"
    echo ""
    read -p "Do you want to overwrite it? (yes/no): " overwrite
    if [ "$overwrite" != "yes" ]; then
        echo "Setup cancelled. Your existing .env file was not modified."
        exit 0
    fi
    # Backup existing .env
    cp .env .env.backup.$(date +%Y%m%d_%H%M%S)
    echo "✓ Backed up existing .env file"
fi

# Copy template
if [ ! -f ".env.example" ]; then
    echo "❌ Error: .env.example not found!"
    exit 1
fi

cp .env.example .env
echo "✓ Created .env file from template"
echo ""

# Get Gmail credentials
echo "================================================================================  "
echo "Step 1: Gmail Configuration"
echo "================================================================================"
echo ""
echo "You need a Gmail App Password (NOT your regular Gmail password)."
echo ""
echo "To get an App Password:"
echo "  1. Go to: https://myaccount.google.com/security"
echo "  2. Enable 2-Step Verification (if not already enabled)"
echo "  3. Go to: https://myaccount.google.com/apppasswords"
echo "  4. Select 'Mail' and generate a password"
echo "  5. Copy the 16-character password (remove spaces)"
echo ""
echo "Press ENTER when you're ready to enter your credentials..."
read
echo ""

# Get email
read -p "Enter your Gmail address: " gmail_address
while [ -z "$gmail_address" ]; do
    read -p "Email address cannot be empty. Enter your Gmail address: " gmail_address
done

# Get app password
echo ""
echo "Enter your Gmail App Password (16 characters, no spaces):"
read -s app_password
while [ -z "$app_password" ]; then
    echo ""
    read -s -p "App Password cannot be empty. Enter your Gmail App Password: " app_password
done
echo ""
echo "✓ Credentials received"

# Update .env file
echo ""
echo "Updating .env file..."

# Use sed to replace the placeholder values
sed -i.bak "s|SENDER_EMAIL=your-email@gmail.com|SENDER_EMAIL=$gmail_address|g" .env
sed -i.bak "s|SENDER_PASSWORD=your-16-character-app-password|SENDER_PASSWORD=$app_password|g" .env

# Remove backup file
rm -f .env.bak

echo "✓ Configuration saved to .env"
echo ""

# Test configuration
echo "================================================================================"
echo "Step 2: Testing Configuration"
echo "================================================================================"
echo ""
read -p "Do you want to test the configuration now? (yes/no): " test_now

if [ "$test_now" = "yes" ]; then
    echo ""
    echo "Running test..."
    cd backend
    python3 test_sms_config.py
    cd ..
fi

echo ""
echo "================================================================================"
echo "✅ Setup Complete!"
echo "================================================================================"
echo ""
echo "Your notification system is now configured!"
echo ""
echo "Next steps:"
echo "  1. Start/restart the EventShield Pro backend:"
echo "     cd backend && python3 app.py"
echo ""
echo "  2. Test from the web interface:"
echo "     - Go to System Administration → SMS Configuration"
echo "     - Click 'TEST CARRIER SMS' or 'SEND ALERT TEST'"
echo ""
echo "  3. If you have issues, see NOTIFICATION_SETUP_GUIDE.md"
echo ""
echo "Your credentials are stored in .env (this file is gitignored)"
echo "================================================================================"
