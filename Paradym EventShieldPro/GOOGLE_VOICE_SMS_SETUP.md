# Google Voice SMS Integration Setup Guide

This guide will help you set up SMS alerts through Google Voice for EventShield Pro.

## Overview

EventShield Pro can send SMS alerts directly to your phone using Google Voice. This provides a reliable, direct SMS solution without requiring complex webhook configurations.

## Prerequisites

- Google account with Google Voice access
- Google Cloud Console access
- EventShield Pro application running

## Setup Steps

### Step 1: Enable Google Voice API

1. **Go to Google Cloud Console**
   - Visit [console.cloud.google.com](https://console.cloud.google.com)
   - Sign in with your Google account

2. **Create or Select a Project**
   - Create a new project or select an existing one
   - Name it "EventShield Pro" or similar

3. **Enable Google Voice API**
   - Go to "APIs & Services" → "Library"
   - Search for "Google Voice API"
   - Click on it and press "Enable"

### Step 2: Create API Credentials

1. **Go to Credentials**
   - Navigate to "APIs & Services" → "Credentials"
   - Click "Create Credentials" → "API Key"

2. **Configure API Key**
   - Copy the generated API key
   - Click "Restrict Key" for security
   - Under "API restrictions", select "Google Voice API"
   - Save the configuration

3. **Copy Your API Key**
   - Save the API key securely - you'll need it for EventShield Pro

### Step 3: Set Up Google Voice

1. **Get Google Voice Number**
   - Go to [voice.google.com](https://voice.google.com)
   - Sign in with your Google account
   - Choose a Google Voice phone number
   - Complete the setup process

2. **Verify Your Phone**
   - Add your personal phone number to Google Voice
   - Verify it through SMS or call

### Step 4: Configure EventShield Pro

1. **Open EventShield Pro**
   - Go to Settings (gear icon)
   - Find the "Google Voice SMS Alerts" section (green card)

2. **Enter Configuration**
   - **Google Voice API Key**: Paste your API key from Step 2
   - **Google Voice Phone Number**: Enter your Google Voice number (e.g., +1234567890)
   - **Your Phone Number**: Enter your personal phone number for receiving alerts

3. **Enable Alerts**
   - Turn on "Enable SMS Alerts"
   - Select which alert types you want

4. **Save Configuration**
   - Click "Save Configuration"

### Step 5: Test the Setup

1. **Test SMS Function**
   - In the Google Voice SMS Alerts section
   - Click "Test SMS" button
   - You should receive a test message on your phone

2. **Verify Delivery**
   - Check your phone for the test message
   - The message should come from your Google Voice number

## Alert Types

EventShield Pro can send the following types of alerts:

### 🚨 Criminal Database Alerts
- Sent when a criminal database match is found
- Critical priority alerts for security threats
- Includes person name and match details

### 💳 Payment Alerts
- Sent for payment transactions
- Includes amount, method, and status
- High priority for failed payments

### 🚪 Access Control Alerts
- Sent when access is granted or denied
- Includes person name and access type
- High priority for denied access

### 📱 Device Status Alerts
- Sent when devices go offline or have issues
- Includes device name and status
- High priority for offline devices

## Message Format

Alerts are sent as formatted SMS messages with:
- EventShield Pro branding
- Priority indicators (🔵 Low, 🟡 Normal, 🟠 High, 🔴 Critical)
- Alert type icons
- Timestamp
- Detailed message content
- System footer

## Troubleshooting

### Common Issues

1. **API Key Invalid**
   - Verify the API key is correct
   - Check that Google Voice API is enabled
   - Ensure API key restrictions are set correctly

2. **No Messages Received**
   - Verify Google Voice number is correct
   - Check that your phone number is verified in Google Voice
   - Test with the "Test SMS" button

3. **Permission Denied**
   - Ensure Google Voice API is enabled
   - Check API key permissions
   - Verify project settings

### Testing

Use the built-in test function:
1. Go to Settings → Google Voice SMS Alerts
2. Click "Test SMS"
3. Check your phone for the test message

## Security Considerations

- Keep your API key secure
- Don't share your API key publicly
- Regularly rotate API keys if needed
- Monitor API usage in Google Cloud Console

## Advanced Configuration

### Custom Alert Messages

You can customize alert messages by modifying the SMS service configuration in the backend.

### Multiple Recipients

To send alerts to multiple people:
1. Add their phone numbers to your Google Voice contacts
2. Configure multiple phone numbers in the system
3. Each person will receive alerts on their phone

### Alert Filtering

You can enable/disable specific alert types in the Settings screen:
- Criminal Database Alerts
- Payment Alerts
- Access Control Alerts
- Device Status Alerts

## API Endpoints

The Google Voice SMS service provides these API endpoints:

- `GET /api/google-voice-sms/config` - Get current configuration
- `POST /api/google-voice-sms/config` - Update configuration
- `POST /api/google-voice-sms/test` - Test SMS connection
- `POST /api/google-voice-sms/send` - Send custom alert
- `POST /api/google-voice-sms/criminal-alert` - Send criminal alert
- `POST /api/google-voice-sms/payment-alert` - Send payment alert
- `POST /api/google-voice-sms/access-alert` - Send access alert
- `POST /api/google-voice-sms/device-alert` - Send device alert
- `GET /api/google-voice-sms/status` - Get service status

## Support

If you encounter issues:
1. Check the EventShield Pro logs
2. Verify Google Voice settings
3. Test the API key manually
4. Contact support with specific error messages

## Cost Information

- Google Voice API calls may have associated costs
- Check Google Cloud Console for current pricing
- SMS delivery through Google Voice is typically free
- Monitor usage in the Google Cloud Console

## Benefits of Google Voice SMS

- **Direct SMS delivery** to your phone
- **No additional apps** required
- **Reliable delivery** through Google's infrastructure
- **Easy setup** compared to webhook systems
- **Professional formatting** with EventShield Pro branding
- **Multiple alert types** with priority indicators
