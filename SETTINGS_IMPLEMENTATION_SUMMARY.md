# Access Control Settings - Implementation Summary
## EventShield Pro System Administration

---

## Executive Summary

After reviewing the DS-F8881 manufacturer documentation, SDK files, and access control manuals, I have identified the comprehensive list of general settings that should be added to the **SYSTEM ADMINISTRATION > Settings** section.

The screenshot shows an SMS/Email alert settings interface. The access control settings should be added as a separate section or integrated into this existing Settings area.

---

## Settings That Should Be Added

### 1. **DEVICE MANAGEMENT SETTINGS**
These settings control the physical access control hardware (DS-F8881 and similar devices):

#### Device Connection Settings
- **Device IP Address**: Network address of access control device
- **TCP Port**: Default 8000 for device communication
- **UDP Port**: Default 8101 for device discovery
- **Device Serial Number**: 16-character identifier (Model + Serial)
- **Communication Password**: 8-character authentication password (default: FFFFFFFF)
- **Controller Type**: Advanced (8900) or Standard (8800)
- **Connection Timeout**: Milliseconds to wait for device response (default: 5000ms)
- **Reconnect Attempts**: Number of retry attempts (default: 0)

**Navigation Link**: `/settings/devices` or `/settings/access-control/devices`

---

### 2. **ACCESS POINT CONFIGURATION**
Settings for each physical access point (doors, gates, turnstiles):

#### Per Access Point (4 doors/points):
- **Access Point Name**: Descriptive name (e.g., "Main Entrance", "VIP Gate")
- **Access Point Type**: 
  - Main Entrance / VIP Entrance / Staff Entrance
  - Emergency Exit / Parking Entrance / Turnstile / Gate / Door
- **Location**: Physical location description
- **Floor Level**: Building floor (e.g., "Ground", "2nd Floor")
- **Building**: Building identifier
- **GPS Coordinates**: Latitude and Longitude
- **Device ID**: Unique hardware identifier
- **Hardware Model**: DS-F8881, DSN-50P, etc.
- **Serial Number**: Device serial number
- **Is Active**: Enable/Disable this access point
- **Is Emergency Exit**: Mark as emergency egress
- **Requires Authentication**: Enable/disable authentication requirement
- **Max Capacity**: Maximum occupancy limit
- **Operating Hours**: JSON configuration for restricted access times
- **Allowed User Roles**: JSON array of permitted role IDs

#### Supported Authentication Methods (per access point):
- ☐ Facial Recognition
- ☐ QR Code Scanning
- ☐ RFID Card
- ☐ Biometric (Fingerprint/Palm)
- ☐ PIN Code
- ☐ Manual Override

**Navigation Link**: `/settings/access-points` or `/settings/access-control/points`

---

### 3. **BIOMETRIC & RECOGNITION SETTINGS**
Configure facial recognition and biometric authentication:

#### Facial Recognition (DS-F8881)
- **Confidence Threshold**: 
  - High: ≥ 80% (recommended for high security)
  - Medium: 60-79% (balanced)
  - Low: < 60% (not recommended)
- **Recognition Timeout**: 1000-10000ms (default: 5000ms)
- **Image Quality Minimum**: 0.0-1.0 score
- **Lighting Conditions**: Minimum acceptable lighting level
- **Face Detection Confidence**: Minimum confidence for face detection
- **Processing Time Warning Threshold**: Alert if recognition takes longer than X ms
- **Save Recognition Images**: Enable/disable image logging

#### General Biometric Settings
- **Validation Methods Priority**: Order of authentication methods to try
- **Multi-Factor Required**: Require multiple authentication methods
- **Biometric Template Storage**: Local vs Cloud storage option

**Navigation Link**: `/settings/biometric` or `/settings/access-control/biometric`

---

### 4. **SECURITY & ACCESS CONTROL POLICIES**

#### Door Interlock Settings
- **Door 1 Interlock**: ☐ Enabled - Prevents opening if other doors open
- **Door 2 Interlock**: ☐ Enabled
- **Door 3 Interlock**: ☐ Enabled
- **Door 4 Interlock**: ☐ Enabled
- **Purpose**: Prevent multiple doors from opening simultaneously (security zones)

#### Anti-Passback Settings
- **Anti-Passback Mode**: 
  - Per Door: Individual tracking per access point
  - Unified Controller: System-wide tracking
- **Purpose**: Prevent credential sharing/passback through access points

#### Capacity Limits
- **Global Capacity Limit**: Total people allowed in facility (0 = unlimited)
- **Door 1 Capacity**: Maximum through this door
- **Door 2 Capacity**: Maximum through this door
- **Door 3 Capacity**: Maximum through this door  
- **Door 4 Capacity**: Maximum through this door
- **Current Occupancy Display**: Show real-time count
- **Capacity Alert Threshold**: Alert at X% capacity
- **Purpose**: Occupancy management and fire safety compliance

**Navigation Link**: `/settings/security` or `/settings/access-control/security`

---

### 5. **ALARM & EMERGENCY CONFIGURATION**

#### Fire Alarm Settings
- **Fire Alarm Mode**:
  - ☐ Disabled
  - ☐ Alarm output + open all doors (software release only)
  - ☐ Alarm output only (software release only)
  - ☐ Alarm + auto open/close with signal
  - ☐ Open door once on alarm signal
- **Purpose**: Emergency egress during fire events

#### Duress/Panic Alarm Settings
- **Duress Alarm Mode**:
  - ☐ Disabled
  - ☐ Lock all doors + alarm (no buzzer, software release only)
  - ☐ Alarm + buzzer (card or software release)
  - ☐ Alarm while button pressed (button release only)
- **Purpose**: Security response for duress situations

#### Smoke Alarm Settings
- **Smoke Alarm Mode**:
  - ☐ Disabled (default)
  - ☐ Drive smoke alarm relay (signal-triggered)
  - ☐ Drive smoke alarm + all doors + board alarm
  - ☐ Drive smoke alarm + lock all doors + board alarm
- **Purpose**: Smoke detection response

#### Intelligent Anti-Theft Host
- **Enabled**: ☐ Yes ☐ No
- **Entry Delay**: 1-255 seconds
- **Exit Delay**: 1-255 seconds
- **Arm Password**: ________ (8 digits)
- **Disarm Password**: ________ (8 digits)
- **Alarm Duration**: 0-65535 seconds
- **Purpose**: Burglar alarm system integration

**Navigation Link**: `/settings/alarms` or `/settings/access-control/alarms`

---

### 6. **READER & DEVICE CONFIGURATION**

#### Card Reader Settings
- **Reader Password Keyboard Enable**: 
  - Reader 1: ☐ Reader 2: ☐ Reader 3: ☐ Reader 4: ☐
  - Reader 5: ☐ Reader 6: ☐ Reader 7: ☐ Reader 8: ☐
- **Reader Verification Mode**:
  - ☐ Disabled
  - ☐ Enabled
  - ☐ Enabled Silent (verify but don't alert)
- **Card Reading Interval Time**: 0-65535 seconds (0 = unlimited)
- **Purpose**: Control card reader behavior and security

#### Device Monitoring
- **Main Board Buzzer**: ☐ Enabled ☐ Disabled
- **Voice Broadcast Enabled**: ☐ Yes ☐ No
- **Voice Prompts Configuration**: Configure 80 individual voice prompts
- **Device Heartbeat Interval**: Seconds between status checks
- **Offline Alert Threshold**: Seconds before device marked offline
- **Purpose**: Device status monitoring and user feedback

**Navigation Link**: `/settings/readers` or `/settings/access-control/readers`

---

### 7. **OPERATIONAL SETTINGS**

#### Record Storage
- **Record Storage Mode**:
  - ☐ Loop (overwrite oldest when full)
  - ☐ Stop (stop recording when full)
- **Purpose**: Control how access logs are stored when storage is full

#### Logging Configuration
- **Log Level**:
  - ☐ Verbose (all events)
  - ☐ Normal (standard events)
  - ☐ Errors Only (errors and critical events)
- **Log Retention Period**: Days to keep logs (default: 90)
- **Log Export Format**: CSV, JSON, or Database
- **Include Images in Logs**: ☐ Yes ☐ No (facial recognition images)
- **Log Processing Time**: ☐ Yes ☐ No (track performance metrics)

#### Performance Monitoring
- **Processing Time Warning Threshold**: Milliseconds (alert if exceeded)
- **Processing Time Error Threshold**: Milliseconds (error if exceeded)
- **Statistics Collection**: ☐ Enabled ☐ Disabled
- **Real-time Monitoring Dashboard**: ☐ Enabled ☐ Disabled

**Navigation Link**: `/settings/operational` or `/settings/access-control/operational`

---

### 8. **TIME & SCHEDULE SETTINGS**

#### Card Expiration
- **Card Expiration Reminder**: ☐ Enabled ☐ Disabled
- **Reminder Days Before Expiration**: Days to alert before card expires
- **Purpose**: Alert users when credentials are near expiration

#### Scheduled Announcements
- **Enabled**: ☐ Yes ☐ No
- **Message Type**: 
  - ☐ Rent Due
  - ☐ Maintenance Fee Due
  - ☐ Custom Message
- **Start Date/Time**: YYYY-MM-DD HH:MM (BCD format: YYYYMMDDHH)
- **End Date/Time**: YYYY-MM-DD HH:MM
- **Purpose**: Automated reminders for facility management

#### Access Time Restrictions (per user/role)
- **Valid From Date/Time**: Start of access permission
- **Valid Until Date/Time**: End of access permission
- **Allowed Days**: Mon, Tue, Wed, Thu, Fri, Sat, Sun
- **Allowed Hours**: Start time - End time ranges
- **Purpose**: Temporal access control

**Navigation Link**: `/settings/schedule` or `/settings/access-control/schedule`

---

## Navigation Links Summary

Based on the screenshot showing the left sidebar, the navigation structure should be:

```
Reports
  └─ [Reports Dashboard]

SYSTEM ADMINISTRATION
  ├─ Settings
  │   ├─ Device Management
  │   │   └─ Link: /settings/access-control/devices
  │   ├─ Access Points
  │   │   └─ Link: /settings/access-control/points
  │   ├─ Biometric Settings
  │   │   └─ Link: /settings/access-control/biometric
  │   ├─ Security Policies
  │   │   └─ Link: /settings/access-control/security
  │   ├─ Alarm Configuration
  │   │   └─ Link: /settings/access-control/alarms
  │   ├─ Reader Configuration
  │   │   └─ Link: /settings/access-control/readers
  │   ├─ Operational Settings
  │   │   └─ Link: /settings/access-control/operational
  │   ├─ Time & Schedule
  │   │   └─ Link: /settings/access-control/schedule
  │   └─ SMS/Email Alerts (existing)
  │       └─ Link: /settings/alerts
  └─ Password Test
      └─ Link: /settings/password-test
```

---

## Link Verification Checklist

### Current Links to Verify:
- ☐ **Reports** → `/reports` or `/admin/reports`
- ☐ **SYSTEM ADMINISTRATION** → `/admin` or `/system`
- ☐ **Settings** → `/settings` or `/admin/settings`
- ☐ **Password Test** → `/settings/password-test` or `/admin/password-test`

### New Links to Add:
- ☐ **Device Management** → `/settings/access-control/devices`
- ☐ **Access Points** → `/settings/access-control/points`
- ☐ **Biometric Settings** → `/settings/access-control/biometric`
- ☐ **Security Policies** → `/settings/access-control/security`
- ☐ **Alarm Configuration** → `/settings/access-control/alarms`
- ☐ **Reader Configuration** → `/settings/access-control/readers`
- ☐ **Operational Settings** → `/settings/access-control/operational`
- ☐ **Time & Schedule** → `/settings/access-control/schedule`

---

## Backend API Endpoints Required

```python
# Device Management
GET    /api/access-control/settings/devices
PUT    /api/access-control/settings/devices
POST   /api/access-control/devices/test-connection

# Access Points
GET    /api/access-control/access-points
GET    /api/access-control/access-points/{id}
POST   /api/access-control/access-points
PUT    /api/access-control/access-points/{id}
DELETE /api/access-control/access-points/{id}

# Biometric Settings
GET    /api/access-control/settings/biometric
PUT    /api/access-control/settings/biometric

# Security Settings
GET    /api/access-control/settings/security
PUT    /api/access-control/settings/security

# Alarm Settings
GET    /api/access-control/settings/alarms
PUT    /api/access-control/settings/alarms
POST   /api/access-control/alarms/test

# Reader Settings
GET    /api/access-control/settings/readers
PUT    /api/access-control/settings/readers

# Operational Settings
GET    /api/access-control/settings/operational
PUT    /api/access-control/settings/operational

# Schedule Settings
GET    /api/access-control/settings/schedule
PUT    /api/access-control/settings/schedule
```

---

## Database Tables Required

### 1. access_control_system_settings
Stores global system settings (device connection, readers, operational)

### 2. access_control_security_settings
Stores security policies (interlock, anti-passback, capacity)

### 3. access_control_alarm_settings
Stores alarm configurations (fire, duress, smoke, anti-theft)

### 4. access_control_biometric_settings
Stores biometric configuration (thresholds, timeouts, methods)

### 5. access_control_schedule_settings
Stores time-based settings (expiration, announcements)

**Note**: `access_points` table already exists in `backend/app/models/access_control.py`

---

## UI Components Needed

### Forms
1. **Device Connection Form** - IP, ports, credentials
2. **Access Point Form** - Location, type, capabilities (with map selector)
3. **Biometric Threshold Sliders** - Visual confidence adjustment
4. **Security Policy Toggles** - Interlock, anti-passback switches
5. **Alarm Configuration Form** - Mode dropdowns, time inputs
6. **Reader Configuration Grid** - 8-reader enable/disable matrix
7. **Operational Settings Form** - Log level, retention, monitoring
8. **Schedule Editor** - Date/time pickers, day selectors

### Visual Components
1. **Access Point Map** - Visual representation of facility layout
2. **Capacity Dashboard** - Real-time occupancy display
3. **Device Status Indicators** - Online/offline status per device
4. **Alarm Status Panel** - Active alarms and history
5. **Performance Metrics Dashboard** - Processing times, throughput

---

## Priority Implementation Order

### Phase 1 (Must Have - Core Functionality)
1. Device Management Settings
2. Access Point Configuration
3. Biometric Settings
4. Security Policies (Interlock, Capacity)

### Phase 2 (Should Have - Safety & Compliance)
5. Alarm Configuration (Fire, Duress)
6. Operational Settings (Logging)
7. Time & Schedule Settings

### Phase 3 (Nice to Have - Enhanced Features)
8. Reader Configuration (Advanced)
9. Scheduled Announcements
10. Advanced Monitoring & Analytics

---

## Integration with DS-F8881 SDK

### SDK Commands to Implement

#### System Settings
- `ReadAllSystemSetting` - Read all current settings from device
- `WriteRecordMode` - Set record storage mode
- `ReadKeyboard` / `WriteKeyboard` - Reader keyboard configuration
- `ReadTCPSetting` / `WriteTCPSetting` - Network configuration

#### Security Settings
- `ReadLockInteraction` / `WriteLockInteraction` - Door interlock
- `ReadAntiPassback` / `WriteAntiPassback` - Anti-passback mode
- `ReadEnterDoorLimit` / `WriteEnterDoorLimit` - Capacity limits

#### Alarm Settings
- `ReadFireAlarmOption` / `WriteFireAlarmOption` - Fire alarm mode
- `ReadOpenAlarmOption` / `WriteOpenAlarmOption` - Duress alarm mode
- `ReadSmogAlarmOption` / `WriteSmogAlarmOption` - Smoke alarm mode
- `ReadTheftAlarmSetting` / `WriteTheftAlarmSetting` - Anti-theft configuration

### C# Implementation Required
Per the master prompt, all device communication MUST be implemented in C#/.NET 8 in the `EventShieldPro_Private_Backups/EventShieldPro_20250905_175822/AccessControl.Backend` project.

---

## Testing Checklist

### Functionality Tests
- ☐ All settings can be read from devices
- ☐ All settings can be written to devices
- ☐ Settings persist across system restarts
- ☐ Settings validation prevents invalid values
- ☐ Settings changes are logged in audit trail

### Integration Tests
- ☐ Settings synchronize with DS-F8881 hardware
- ☐ Multiple access points can be configured
- ☐ Alarm modes trigger correct behavior
- ☐ Capacity limits enforce correctly
- ☐ Biometric thresholds affect recognition

### UI/UX Tests
- ☐ All navigation links work correctly
- ☐ Forms validate user input
- ☐ Settings save successfully with feedback
- ☐ Help text explains each setting
- ☐ Responsive design on mobile/tablet

### Security Tests
- ☐ Only authorized users can change settings
- ☐ Communication passwords are encrypted
- ☐ Settings changes require authentication
- ☐ Audit log cannot be tampered with

---

## Documentation Required

1. **User Guide** - Step-by-step configuration instructions
2. **Admin Manual** - Best practices and security recommendations
3. **API Documentation** - Complete endpoint specifications
4. **SDK Integration Guide** - How to use DS-F8881 SDK commands
5. **Troubleshooting Guide** - Common issues and solutions

---

## Conclusion

All settings identified above are derived directly from:
1. **DS-F8881 SDK Documentation** (Java/C# SDK)
2. **Access Controller User Manual** (manufacturer documentation)
3. **Existing Backend Models** (`access_control.py`, `AccessPoint`, etc.)

The settings provide comprehensive control over:
- Physical hardware devices
- Security policies and access control
- Emergency and alarm response
- Biometric authentication
- System monitoring and logging

Implementation should follow the existing architecture:
- **Backend**: Python Flask (current) + C#/.NET 8 (device communication)
- **Frontend**: React/Material-UI (based on `main_complete.py y`)
- **Database**: SQLAlchemy models with PostgreSQL/MySQL

All navigation links should follow RESTful patterns and integrate seamlessly with the existing SYSTEM ADMINISTRATION interface shown in the screenshot.
