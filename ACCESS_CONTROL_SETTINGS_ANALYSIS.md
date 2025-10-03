# Access Control Settings Analysis
## EventShield Pro - DS-F8881 Integration

### Document Sources Reviewed
1. **DS-F8881 SDK Documentation** (`DS-F8881 Documents/SDK-230522 4/`)
   - Java Door Access Control Board User Manual
   - SDK Code Examples (.NET and Java)
   - Access Controller Documentation

2. **Backend Models**
   - `Paradym EventShieldPro/backend/app/models/access_control.py`
   - Defines AccessPoint, AccessLog, AccessPermission, FacialRecognitionLog, TurnstileLog

3. **Private Backup**
   - EventShieldPro_20250905_175822/AccessControl.Backend (C#/.NET 8)

---

## General Access Control Settings Required

Based on the manufacturer documentation and SDK, the following general settings should be added to the **SYSTEM ADMINISTRATION > Settings** section:

### 1. System Configuration Settings

#### 1.1 Record Storage Settings
- **Record Storage Mode**
  - Options: Loop (when full) / Stop (when full)
  - Default: Loop
  - Purpose: Controls how the system handles access logs when storage is full

#### 1.2 Reader/Device Configuration
- **Reader Password Keyboard Enable**
  - 8 readers/channels (bit flags for each)
  - Purpose: Enable/disable password keyboard functionality for each card reader
  
- **Reader Verification Mode**
  - Options: Disabled / Enabled / Enabled Silent
  - Purpose: Enable card reader data verification
  
- **Reader Interval Time**
  - Range: 0-65535 seconds (0 = unlimited)
  - Purpose: Minimum time between consecutive card reads

#### 1.3 Device Status & Monitoring
- **Main Board Buzzer**
  - Options: Enabled / Disabled
  - Purpose: Enable/disable audible feedback on main controller

- **Voice Broadcast Settings**
  - 80 configurable voice prompts
  - Individual enable/disable for each prompt type
  - Purpose: Configure audio feedback for different access events

### 2. Security & Access Control Settings

#### 2.1 Door Interlock Settings
- **Door Interlock Enable** (per door)
  - 4 doors configurable
  - Purpose: Prevent multiple doors from opening simultaneously

#### 2.2 Anti-Passback Settings
- **Anti-Passback Mode**
  - Options: Per Door / Unified Controller
  - Purpose: Prevent users from passing credentials back through access point

#### 2.3 Door Capacity Limits
- **Global Capacity Limit**
  - Maximum people allowed in facility
  
- **Per-Door Capacity Limits** (4 doors)
  - Individual limits for each access point
  - Current count tracking
  - Purpose: Occupancy management and safety compliance

### 3. Alarm & Emergency Settings

#### 3.1 Fire Alarm Configuration
- **Fire Alarm Mode**
  - 0: Disabled
  - 1: Alarm output + open all doors (software release only)
  - 2: Alarm output only (software release only)
  - 3: Alarm + auto open/close with signal
  - 4: Open door once on alarm signal
  - Purpose: Emergency egress during fire events

#### 3.2 Duress/Panic Alarm Configuration
- **Duress Alarm Mode**
  - 0: Disabled
  - 1: Lock all doors + alarm output (no buzzer, software release only)
  - 2: Alarm output + buzzer (card or software release)
  - 3: Alarm while button pressed (button release only)
  - Purpose: Security response for duress situations

#### 3.3 Smoke Alarm Configuration
- **Smoke Alarm Mode**
  - 0: Disabled (default)
  - 1: Drive smoke alarm relay (signal-triggered)
  - 2: Drive smoke alarm + all doors + board alarm
  - 3: Drive smoke alarm + lock all doors + board alarm
  - Purpose: Smoke detection response

#### 3.4 Intelligent Anti-Theft Host
- **Enable Status**: On/Off
- **Entry Delay**: 1-255 seconds
- **Exit Delay**: 1-255 seconds
- **Arm Password**: 8-digit numeric
- **Disarm Password**: 8-digit numeric
- **Alarm Duration**: 0-65535 seconds
- Purpose: Burglar alarm system integration

### 4. Time & Access Schedule Settings

#### 4.1 Card Expiration Settings
- **Card Expiration Reminder**
  - Options: Enabled / Disabled
  - Purpose: Alert users when credentials are near expiration

#### 4.2 Scheduled Voice Announcements
- **Enable Status**: On/Off
- **Message Index**: 1 (rent due), 2 (maintenance fee due)
- **Start DateTime**: BCD format (YYYYMMDDHH)
- **End DateTime**: BCD format
- Purpose: Automated reminders for facility management

### 5. Network & Communication Settings

#### 5.1 TCP/IP Configuration
- **IP Address**
- **Subnet Mask**
- **Gateway**
- **TCP Port** (default: 8000)
- **UDP Port** (default: 8101)
- Purpose: Network connectivity for device management

#### 5.2 Device Identity
- **Device Serial Number** (16 characters)
  - First 8: Model (e.g., FC-8940H)
  - Last 8: Serial (e.g., 47124309)
  
- **Communication Password** (8 characters, default: FFFFFFFF)
- **Controller Type**: Advanced (8900) / Standard (8800)
- Purpose: Device authentication and identification

### 6. Biometric & Authentication Settings

#### 6.1 Facial Recognition (DS-F8881 Specific)
- **Confidence Threshold**
  - High: ≥ 0.8
  - Medium: 0.6 - 0.8
  - Low: < 0.6
  - Purpose: Set minimum confidence for facial recognition matches

- **Recognition Timeout**
  - Default: 5000ms
  - Range: 1000-10000ms
  - Purpose: Maximum time for facial recognition attempt

- **Image Quality Settings**
  - Minimum quality score
  - Lighting condition requirements
  - Purpose: Ensure consistent biometric capture quality

#### 6.2 Validation Methods
- **Supported Methods** (enable/disable per access point)
  - QR Scan
  - Facial Recognition
  - RFID
  - Biometric (fingerprint/palm)
  - PIN Code
  - Manual Override

### 7. Access Point Settings

#### 7.1 Per Access Point Configuration
For each access point (up to 4 doors):

- **Access Point Name**
- **Access Point Type**
  - Main Entrance / VIP Entrance / Staff Entrance
  - Emergency Exit / Parking / Turnstile / Gate / Door
  
- **Location Information**
  - Location name
  - Floor level
  - Building
  - GPS coordinates (lat/lng)

- **Hardware Configuration**
  - Device ID (unique)
  - Device Type
  - Hardware Model (e.g., DS-F881)
  - Serial Number

- **Capabilities**
  - Supports Facial Recognition
  - Supports QR Scanning
  - Supports RFID
  - Supports Biometric
  - Supports Manual Override

- **Status Settings**
  - Is Active
  - Is Emergency Exit
  - Requires Authentication
  - Max Capacity
  - Operating Hours (restricted hours JSON)
  - Allowed User Roles (JSON array)

#### 7.2 Door/Relay Control Settings
- **Unlock Duration**: Time door stays unlocked (seconds)
- **Auto-Lock Settings**: Automatic locking after closure
- **Sensor Alarm Settings**: Door sensor monitoring
- **Emergency Unlock**: Force door open in emergency

### 8. Operational Settings

#### 8.1 Access Logging
- **Log Level**: Verbose / Normal / Errors Only
- **Log Retention Period**: Days to keep logs
- **Log Export Format**: CSV / JSON / Database

#### 8.2 Performance Monitoring
- **Processing Time Thresholds**
  - Warning threshold (ms)
  - Error threshold (ms)
  
- **Device Heartbeat**
  - Heartbeat interval (seconds)
  - Offline threshold (seconds)
  
- **Maintenance Schedule**
  - Next maintenance date
  - Maintenance interval

---

## Settings UI Structure Recommendation

### SYSTEM ADMINISTRATION Section Structure

```
├── Reports
├── SYSTEM ADMINISTRATION
│   ├── Settings (← Add Access Control Settings here)
│   │   ├── General System Settings
│   │   │   ├── Record Storage
│   │   │   ├── Reader Configuration
│   │   │   └── Device Monitoring
│   │   ├── Security Settings
│   │   │   ├── Door Interlock
│   │   │   ├── Anti-Passback
│   │   │   └── Capacity Limits
│   │   ├── Alarm Configuration
│   │   │   ├── Fire Alarm
│   │   │   ├── Duress Alarm
│   │   │   ├── Smoke Alarm
│   │   │   └── Anti-Theft Host
│   │   ├── Access Point Configuration
│   │   │   └── Per-Door Settings (4 doors)
│   │   ├── Biometric Settings
│   │   │   ├── Facial Recognition Thresholds
│   │   │   └── Authentication Methods
│   │   ├── Network Settings
│   │   │   ├── TCP/IP Configuration
│   │   │   └── Device Identity
│   │   └── Operational Settings
│   │       ├── Logging
│   │       └── Performance Monitoring
│   └── Password Test
└── (other sections)
```

---

## Implementation Notes

### Priority Settings (Must Have)
1. **Access Point Configuration** - Define where access control occurs
2. **Biometric Settings** - Configure facial recognition thresholds
3. **Network Settings** - Device connectivity
4. **Security Settings** - Door interlock, anti-passback
5. **Alarm Configuration** - Emergency response

### Secondary Settings (Should Have)
6. **Reader Configuration** - Card reader behavior
7. **Capacity Limits** - Occupancy management
8. **Operational Settings** - Logging and monitoring

### Optional Settings (Nice to Have)
9. **Scheduled Announcements** - Voice prompts
10. **Card Expiration** - Credential management
11. **Anti-Theft Host** - Extended security features

---

## API Endpoints Needed

### Backend Routes Required:
```python
# System Settings
GET    /api/access-control/settings/system
PUT    /api/access-control/settings/system

# Security Settings  
GET    /api/access-control/settings/security
PUT    /api/access-control/settings/security

# Alarm Settings
GET    /api/access-control/settings/alarms
PUT    /api/access-control/settings/alarms

# Access Point Settings
GET    /api/access-control/access-points
GET    /api/access-control/access-points/{id}
PUT    /api/access-control/access-points/{id}
POST   /api/access-control/access-points

# Biometric Settings
GET    /api/access-control/settings/biometric
PUT    /api/access-control/settings/biometric

# Network/Device Settings
GET    /api/access-control/settings/network
PUT    /api/access-control/settings/network

# Operational Settings
GET    /api/access-control/settings/operational
PUT    /api/access-control/settings/operational
```

---

## Database Schema Additions

### Access Control Settings Table
```sql
CREATE TABLE access_control_settings (
    id INTEGER PRIMARY KEY,
    tenant_id INTEGER NOT NULL,
    
    -- System Settings
    record_storage_mode VARCHAR(20) DEFAULT 'loop',
    reader_keyboard_enable INTEGER DEFAULT 0,
    reader_verification_mode INTEGER DEFAULT 0,
    reader_interval_time INTEGER DEFAULT 0,
    board_buzzer_enabled BOOLEAN DEFAULT true,
    
    -- Security Settings
    door_interlock_config JSON,
    anti_passback_mode VARCHAR(20) DEFAULT 'per_door',
    global_capacity_limit INTEGER,
    
    -- Alarm Settings
    fire_alarm_mode INTEGER DEFAULT 0,
    duress_alarm_mode INTEGER DEFAULT 0,
    smoke_alarm_mode INTEGER DEFAULT 0,
    anti_theft_enabled BOOLEAN DEFAULT false,
    anti_theft_config JSON,
    
    -- Biometric Settings
    face_confidence_threshold FLOAT DEFAULT 0.8,
    recognition_timeout_ms INTEGER DEFAULT 5000,
    
    -- Network Settings
    device_ip_address VARCHAR(45),
    device_tcp_port INTEGER DEFAULT 8000,
    device_udp_port INTEGER DEFAULT 8101,
    communication_password VARCHAR(8) DEFAULT 'FFFFFFFF',
    
    -- Metadata
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);
```

---

## Next Steps

1. **Create Settings API Routes** - Backend implementation in Flask/Python
2. **Create Settings UI Components** - Frontend forms for each settings category
3. **Integrate with DS-F8881 SDK** - Apply settings to actual hardware
4. **Add Settings Validation** - Ensure values are within acceptable ranges
5. **Implement Settings Export/Import** - Backup and restore capability
6. **Add Settings Audit Log** - Track who changed what settings when
7. **Create Settings Migration Scripts** - Database schema updates

---

## Testing Requirements

1. **Unit Tests** - Test each settings category independently
2. **Integration Tests** - Test settings application to devices
3. **UI Tests** - Test settings forms and validation
4. **Performance Tests** - Ensure settings changes don't impact system performance
5. **Security Tests** - Validate permission requirements for settings changes

---

## Documentation Required

1. **User Guide** - How to configure access control settings
2. **Admin Guide** - Best practices for security settings
3. **API Documentation** - Endpoint specifications
4. **Integration Guide** - How settings map to DS-F8881 commands

---

## Conclusion

The Access Control Settings should provide comprehensive configuration for:
- **Hardware devices** (DS-F8881 and other access controllers)
- **Security policies** (alarms, interlocks, capacity)
- **Biometric authentication** (thresholds, methods)
- **Network connectivity** (TCP/IP, device identity)
- **Operational monitoring** (logging, performance)

All settings are based on the DS-F8881 manufacturer SDK documentation and align with the existing backend models in the EventShield Pro codebase.
