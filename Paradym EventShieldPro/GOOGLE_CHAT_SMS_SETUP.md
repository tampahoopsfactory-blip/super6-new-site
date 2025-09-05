# Google Chat SMS Integration Setup Guide

This guide will help you set up SMS alerts through Google Chat for EventShield Pro.

## Overview

EventShield Pro can send SMS alerts through Google Chat, which can then be forwarded to your phone via the Google Chat mobile app. This provides a cost-effective way to receive real-time alerts without requiring a dedicated SMS service.

## Prerequisites

- Google Workspace account (or personal Google account)
- Google Chat access
- EventShield Pro application running

## Setup Steps

### Step 1: Create a Google Chat Space

1. Open Google Chat (chat.google.com)
2. Click the "+" button to create a new space
3. Name it "EventShield Pro Alerts" or similar
4. Add yourself as a member

### Step 2: Create a Webhook

1. In your Google Chat space, click the space name at the top
2. Select "Apps & integrations"
3. Click "Manage webhooks"
4. Click "Add webhook"
5. Name it "EventShield Pro SMS"
6. Copy the webhook URL (it will look like: `https://chat.googleapis.com/v1/spaces/SPACE_ID/messages?key=API_KEY&token=TOKEN`)

### Step 3: Configure EventShield Pro

1. Open EventShield Pro
2. Go to Settings
3. Find the "SMS Alert Settings" section
4. Click "Advanced" button
5. Enter your webhook URL in the "Google Chat Webhook URL" field
6. Extract the Space ID from your webhook URL and enter it
7. Optionally enter your Google API Key
8. Enable the alert types you want
9. Click "Save Configuration"

### Step 4: Test the Integration

1. In the SMS Alert Settings section, click "Test SMS"
2. You should receive a test message in your Google Chat space
3. If successful, you'll see a confirmation message

### Step 5: Set Up Mobile Notifications

1. Install Google Chat mobile app on your phone
2. Sign in with the same Google account
3. Join the "EventShield Pro Alerts" space
4. Enable push notifications for the space
5. Configure notification settings as desired

## Alert Types

EventShield Pro can send the following types of alerts:

### 🚨 Criminal Database Alerts
- Sent when a criminal database match is found
- High priority alerts for security threats
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

Alerts are sent as rich Google Chat cards with:
- EventShield Pro branding
- Priority indicators (🔵 Low, 🟡 Normal, 🟠 High, 🔴 Critical)
- Alert type icons
- Timestamp
- Detailed message content
- Footer with system name

## Troubleshooting

### Common Issues

1. **Webhook URL Invalid**
   - Ensure the URL is copied correctly
   - Check that the space exists and you have access

2. **No Messages Received**
   - Verify the webhook is active
   - Check Google Chat space notifications
   - Test with the "Test SMS" button

3. **Permission Denied**
   - Ensure you have admin access to the space
   - Check that the webhook has proper permissions

### Testing

Use the built-in test function:
1. Go to Settings → SMS Alert Settings
2. Click "Test SMS"
3. Check your Google Chat space for the test message

## Security Considerations

- Keep your webhook URL secure
- Don't share the webhook URL publicly
- Regularly rotate API keys if using them
- Monitor the alert space for unauthorized access

## Advanced Configuration

### Custom Alert Messages

You can customize alert messages by modifying the SMS service configuration in the backend.

### Multiple Recipients

To send alerts to multiple people:
1. Add them to the Google Chat space
2. They will receive all alerts sent to that space

### Alert Filtering

You can enable/disable specific alert types in the Settings screen:
- Criminal Database Alerts
- Payment Alerts
- Access Control Alerts
- Device Status Alerts

## Support

If you encounter issues:
1. Check the EventShield Pro logs
2. Verify Google Chat space settings
3. Test the webhook URL manually
4. Contact support with specific error messages

## API Endpoints

The SMS service provides these API endpoints:

- `GET /api/sms/config` - Get current configuration
- `POST /api/sms/config` - Update configuration
- `POST /api/sms/test` - Test SMS connection
- `POST /api/sms/send` - Send custom alert
- `POST /api/sms/criminal-alert` - Send criminal alert
- `POST /api/sms/payment-alert` - Send payment alert
- `POST /api/sms/access-alert` - Send access alert
- `POST /api/sms/device-alert` - Send device alert
- `GET /api/sms/status` - Get service status
