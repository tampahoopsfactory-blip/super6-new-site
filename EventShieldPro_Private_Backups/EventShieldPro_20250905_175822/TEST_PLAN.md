## EventShield Pro Validation & Test Plan

### Objectives
- Verify ≤3s registration-to-device sync under nominal conditions
- Validate daily/weekend expiry logic (America/New_York, DST-aware)
- Confirm time sync ≤2s drift on all devices
- Exercise network loss and recovery
- Verify webhook signature enforcement (no screenshots)

### Environment
- DS‑F881 terminals (min 2), DSN‑50P turnstile
- Windows host running Sync Service (net8+) with vendor SDK DLL
- Backend API (ASP.NET Core) with webhook endpoints

### Tests
1. Enrollment & Sync (25 guests)
   - Steps: WPF enroll → capture face/fingerprint → issue ticket (Daily, Weekend)
   - Assert: Device user + biometrics present within 3s; relay pulses on grant
2. Expiry Logic
   - Daily: issue before midnight; assert expiry tooltip shows 23:59:59 ET; device auto-purges after expiry
   - Weekend: issue Fri/Sat; assert expiry Sunday 23:59:59 ET
3. Time Sync
   - Manually skew device by +10s; Sync Service corrects to ≤2s; log time_drift_seconds
4. Payments
   - Send valid Stripe/PayPal webhooks (HMAC OK) → ticket activates
   - Send invalid signature → ticket denied; fraud log captures phone, IP, timestamp
5. Watchlist
   - Flag guest; attempt entry; assert deny + MMS alert sent with snapshot and gate info
6. Network Loss
   - Disconnect device network; attempt enroll; queue formed; reconnect → batch flush within 60s

### Device SDK Functional Checks
1. Native Client Handshake
   - Configure `DeviceConnectionOptions` with correct SN → `ConnectAsync` succeeds.
   - Re-run with mismatched SN → expect `InvalidOperationException` and log entry.
2. Time Synchronisation
   - Skew device clock +15s; call `EnsureTimeSyncAsync`; assert drift ≤2s afterward via `ReadTime`.
3. Personnel Lifecycle
   - Execute `UpsertUserAsync` with face/palm flags; validate person appears on device with expected card code and feature flags.
   - Invoke `UploadFaceAsync` (≤122 KB JPEG) followed by `UploadPalmTemplateAsync`; confirm status codes == success; device shows image/template present.
   - Run `RemoveUserAsync`; assert SDK command returns success and device purges user.
4. Transaction Polling
   - Trigger gate event; call `ReadTransactionsAsync(lastPointer)`; verify returned sequence increments and maps back to expected GUID via `_userIdByUserCode`.
   - Repeat to ensure subsequent call with updated pointer returns empty list.
5. Relay Control
   - Call `ControlRelayAsync(relayPort:1, TimeSpan.FromSeconds(2))`; confirm relay energises and auto-closes after duration.

### Acceptance Criteria
- 95th percentile sync ≤3s (25 enrollments)
- All devices drift ≤2s for 1 hour test window
- 0 accepted payments without valid webhook signatures
- 0 data loss across disconnect/reconnect cycles

