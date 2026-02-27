# HFSecurity X05 SDK & Documentation - Complete Inventory

## Overview
This folder contains all HFSecurity X05 device SDK files, manuals, and server setup software needed for EventShield Pro development. The X05 device is capable of running your entire custom ticketing system with face recognition and QR code integration.

**Status:** All essential files located and organized. Ready for developer handoff.

---

## 📁 Folder Structure

### `/Manuals/`
**All technical documentation for the X05 device and system**

- `HFSecurity Software User Manual 20230609.doc` - Main user manual for X05 device
- `Access Control Software User Manual.pdf` - Complete access control system guide
- `DS-F881 Face Access Controller User Manual.pdf` - Hardware-specific technical specs

### `/Server_Setup/`
**HF-IMS Server installation and configuration**

- `HF-IMS(Heystar Software)/` - Complete server software folder
  - `HFIMSServerV1Setup_202411231400.exe` - Windows server installer (latest: Nov 28, 2024)
  - `HFIMSServerV1 install manual 1.0.pdf` - Server installation guide
  - `Heystar connect to HFIMS video/` - Video tutorials (LAN & WAN setup)
  - `Back up HFIMS data/` - Database backup utilities

### `/SDK_Archives/`
**Complete Android SDK and API packages for custom development**

#### Primary SDK (Start Here)
- `Face+System+3.2.004_01.zip` (187MB) - **Latest facial recognition system SDK. Use this.**
  - Full Android source code for X05
  - Face recognition APIs
  - QR code integration libraries
  - Ready for EventShield Pro customization

#### Alternative/Backup SDKs
- `New 2024 SDK-230522.zip` (91MB) - 2024 version, comprehensive build
- `3568-API.zip` (12MB) - Pure API documentation and reference

#### Legacy/Additional
- In Google Drive: `/All SDK Software for facial scanner 5 21 23/`
  - `SDK Files.zip` (47MB)
  - `SDK 5.23.zip` (14MB)
  - `New SDK from Old Machine Full Code 5:5:23.zip` (47MB)

### `/Server_Setup/`
- `demo.apk` (1.5MB) - Demo Android application showing X05 capabilities

---

## 🚀 Developer Quick Start

### Phase 1: Environment Setup
1. **Install HF-IMS Server** (Windows/Linux)
   - Run: `HFIMSServerV1Setup_202411231400.exe`
   - Follow: `HFIMSServerV1 install manual 1.0.pdf`
   - Watch: Videos in `Heystar connect to HFIMS video/`

2. **Extract SDK Archive**
   ```bash
   unzip Face+System+3.2.004_01.zip
   # Explore the project structure
   ```

3. **Review Documentation**
   - Start: `HFSecurity Software User Manual 20230609.doc`
   - Reference: `Access Control Software User Manual.pdf`
   - Hardware specs: `DS-F881 Face Access Controller User Manual.pdf`

### Phase 2: Android App Development
The X05 runs Android 11 with open-source access. Your developer needs to:

1. **Build custom Android app** using the Face System SDK
2. **Implement EventShield Pro requirements:**
   - Ticket type logic (DAILY/WEEKEND/STAFF rules)
   - QR code validation against your database
   - USED status stamping and sync
   - Face recognition enrollment flow
   - Turnstile relay/Wiegand control
   - TCP/IP communication to your ticketing server

### Phase 3: Hardware Integration
- X05 connects via TCP/IP (POE powered or battery 3-6 hrs)
- WiFi + 4G cellular backup built in
- Wiegand/RS485 output for turnstile control
- Supports external turnstile (ZKTeco recommended, ~$300-800)

---

## 🔗 Critical Developer Resources

### APIs Available in SDK
- Face recognition and enrollment
- Iris recognition
- Fingerprint capture
- QR code scanning
- RFID/NFC card reading
- Wiegand output control
- TCP/IP socket communication
- Local SQLite database access

### Supported Development Languages
- Java (primary)
- C/C++
- Android SDK (API level 30+)

### Database Integration
Supports: Oracle, MS SQL Server, MySQL, PostgreSQL

### Hardware Connectivity
- **Local Storage:** 20,000 face templates on device
- **Network:** TCP/IP, WiFi, 4G LTE
- **Outputs:** Wiegand (multiple formats), RS485, RS232, relay control
- **Inputs:** 4G, WiFi, Bluetooth, NFC, RFID

---

## 📋 What's NOT in this Folder (But Available)

If your developer needs additional resources:

1. **Original Dropbox links** (from Linda at HFSecurity - currently deleted)
   - Contact: `info@hfcctv.com` or `zoe@hfsecuritytech.com`
   - Status: Need to request fresh links

2. **Additional SDK versions**
   - Location: `/Library/CloudStorage/GoogleDrive-tampahoopsfactory@gmail.com/My Drive/All SDK Software for facial scanner 5 21 23/`
   - Includes: Legacy versions, backup builds, extended documentation

3. **Sample Projects**
   - Demo APK is included
   - Full source examples in Face System SDK archive

---

## ⚠️ Known Configuration Notes

1. **X05 Device Specifics**
   - Running Android 11 with full open-source access
   - 20,000 user capacity local database
   - Built-in anti-spoofing (rejects fake photos/videos)
   - Works offline with local face database

2. **Server Architecture**
   - HF-IMS runs on Windows or Linux
   - Acts as central enrollment and management hub
   - Can sync face databases across multiple devices
   - Includes backup/restore utilities

3. **Turnstile Integration**
   - X05 does NOT include turnstile hardware
   - Must connect external turnstile via Wiegand cable
   - Recommended: ZKTeco (global leader, US support)

---

## 📞 Support & Contact

### HFSecurity Direct Contact
- **Email:** `info@hfcctv.com` or `zoe@hfsecuritytech.com`
- **WhatsApp:** +86 18598053400
- **Status:** You're a priority customer (device owner)

### What to Ask For If Needed
- Android SDK updates
- QR code integration examples
- Turnstile control documentation
- API reference guides
- Sample Android app source code

---

## ✅ Checklist for Developer Handoff

- [ ] Extract Face+System+3.2.004_01.zip
- [ ] Read HFSecurity Software User Manual
- [ ] Install HF-IMS Server locally
- [ ] Review Access Control Software Manual
- [ ] Examine demo.apk for UI/UX reference
- [ ] Set up Android Studio with SDK
- [ ] Create EventShield Pro custom app project
- [ ] Implement QR code scanning
- [ ] Implement face enrollment flow
- [ ] Build ticket validation logic
- [ ] Integrate with your ticketing database
- [ ] Test Wiegand output (turnstile control)
- [ ] Deploy to X05 device

---

## 🎯 Development Timeline Estimate

Based on SDK scope:
- **SDK Setup & Familiarization:** 1-2 weeks
- **Core App Development:** 4-6 weeks
- **Integration & Testing:** 2-3 weeks
- **Hardware Testing:** 1 week
- **Total:** 8-12 weeks for production-ready system

You're starting with 75-80% of the hardware already working. Customization is focused on the 20-25% business logic layer (tickets, USED status, date rules, database sync).

---

## 📝 Last Updated
February 27, 2026 - All files verified and organized for EventShield Pro development.
