import { useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import {
  Users, Ticket, DollarSign, ShieldAlert, AlertTriangle, CheckCircle,
  ArrowUpRight, Radio, ScanFace, QrCode, ShieldCheck, Wifi, WifiOff, Battery, Signal, Fingerprint, CreditCard, Eye
} from 'lucide-react';

const eventTypeIcon = {
  QR_ENTRY: QrCode,
  FACE_ENTRY: ScanFace,
  IRIS_ENTRY: Eye,
  FINGER_ENTRY: Fingerprint,
  NFC_ENTRY: CreditCard,
  QR_DENIED: ShieldAlert,
  FACE_DENIED: ShieldAlert,
  IRIS_DENIED: ShieldAlert,
  FINGER_DENIED: ShieldAlert,
  NFC_DENIED: ShieldAlert,
  MANUAL_OVERRIDE: ShieldCheck,
};

const eventTypeLabel = {
  QR_ENTRY: 'QR Entry',
  FACE_ENTRY: 'Face Entry',
  IRIS_ENTRY: 'Iris Entry',
  FINGER_ENTRY: 'Finger Entry',
  NFC_ENTRY: 'NFC Entry',
  QR_DENIED: 'QR Denied',
  FACE_DENIED: 'Face Denied',
  IRIS_DENIED: 'Iris Denied',
  FINGER_DENIED: 'Finger Denied',
  NFC_DENIED: 'NFC Denied',
  MANUAL_OVERRIDE: 'Manual Override',
};

const alertTypeColors = {
  TAMPER: '#FF3B30',
  TAILGATE: '#FF9500',
  FORCED_ENTRY: '#FF3B30',
  DEVICE_ERROR: '#FF3B30',
  OFFLINE: '#8e8e93',
};

function formatTime(ts) {
  if (!ts) return '--';
  const d = new Date(ts);
  return d.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
}

export default function LiveMonitor() {
  const fetchStats = useCallback(() => api.getStats(), []);
  const fetchFeed = useCallback(() => api.getRecentAccess(20), []);
  const fetchDevices = useCallback(() => api.getDevices(), []);
  const fetchAlerts = useCallback(() => api.getAlerts(), []);

  const { data: stats, loading: statsLoading } = usePolling(fetchStats, 5000);
  const { data: feed } = usePolling(fetchFeed, 5000);
  const { data: devices } = usePolling(fetchDevices, 15000);
  const { data: alerts } = usePolling(fetchAlerts, 8000);

  const s = stats || {};
  const activeAlerts = (alerts || []).filter((a) => !a.acknowledged);

  return (
    <>
      <div className="page-header">
        <div>
          <h2>Live Monitor</h2>
          {s.active_event && (
            <span className="text-sm text-muted" style={{ marginTop: 2, display: 'block' }}>
              {s.active_event.name} — {s.active_event.status}
            </span>
          )}
        </div>
        <div className="flex gap-2 items-center">
          {activeAlerts.length > 0 && (
            <span className="badge badge-danger" style={{ display: 'flex', alignItems: 'center', gap: 4 }}>
              <AlertTriangle size={11} /> {activeAlerts.length} Alert{activeAlerts.length !== 1 ? 's' : ''}
            </span>
          )}
          {!s.active_event && (
            <span className="badge badge-warning">No Active Event</span>
          )}
        </div>
      </div>

      <div className="page-body">
        {/* Stats Grid */}
        <div className="stats-grid">
          <div className="stat-card">
            <div className="stat-label">Patrons Inside</div>
            <div className="stat-value">{s.patrons_inside ?? 0}</div>
            <div className="stat-sub">Current venue count</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Tickets Sold</div>
            <div className="stat-value">{s.total_tickets_sold ?? 0}</div>
            <div className="stat-sub">Today's event</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Revenue</div>
            <div className="stat-value">${(s.revenue ?? 0).toLocaleString('en-US', { minimumFractionDigits: 2 })}</div>
            <div className="stat-sub">Total collected</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Total Entries</div>
            <div className="stat-value">{s.total_entries ?? 0}</div>
            <div className="stat-sub">All gates combined</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Denied</div>
            <div className="stat-value" style={{ color: s.denied_count > 0 ? '#FF3B30' : undefined }}>
              {s.denied_count ?? 0}
            </div>
            <div className="stat-sub">Access denials</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Devices</div>
            <div className="stat-value">
              {s.devices_online ?? 0}<span className="text-sm text-muted">/{s.devices_total ?? 0}</span>
            </div>
            <div className="stat-sub">Online / Total</div>
          </div>
        </div>

        {/* Alerts Panel */}
        {activeAlerts.length > 0 && (
          <div className="card mb-6" style={{ borderLeft: '4px solid #FF3B30' }}>
            <div className="card-header">
              <h3 style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                <AlertTriangle size={16} color="#FF3B30" /> Device Alerts
              </h3>
              <span className="badge badge-danger">{activeAlerts.length}</span>
            </div>
            <div style={{ maxHeight: 240, overflow: 'auto' }}>
              {activeAlerts.map((alert) => (
                <div key={alert.id} style={{
                  padding: '14px 20px',
                  minHeight: 48,
                  borderBottom: '1px solid var(--color-gray-100)',
                  display: 'flex',
                  alignItems: 'center',
                  gap: 12,
                  fontSize: 14,
                }}>
                  <span style={{
                    width: 8, height: 8, borderRadius: '50%',
                    background: alertTypeColors[alert.alert_type] || '#FF3B30',
                    flexShrink: 0,
                  }} />
                  <span style={{ fontWeight: 600, minWidth: 100, color: alertTypeColors[alert.alert_type] || '#FF3B30' }}>
                    {alert.alert_type}
                  </span>
                  <span style={{ flex: 1, color: '#3a3a3c' }}>{alert.message}</span>
                  <span className="text-xs text-muted">{alert.device_id}</span>
                  <span className="text-xs text-muted">{formatTime(alert.timestamp)}</span>
                </div>
              ))}
            </div>
          </div>
        )}

        <div className="grid-2">
          {/* Live Feed */}
          <div className="card">
            <div className="card-header">
              <h3>Live Access Feed</h3>
              <span className="badge badge-success" style={{ display: 'flex', alignItems: 'center', gap: 4 }}>
                <span className="status-dot online" style={{ margin: 0 }} />
                Live
              </span>
            </div>
            <div className="live-feed">
              {(!feed || feed.length === 0) && (
                <div className="feed-item text-muted text-sm" style={{ justifyContent: 'center', padding: 32 }}>
                  No access events yet
                </div>
              )}
              {feed?.map((item) => {
                const Icon = eventTypeIcon[item.event_type] || ShieldAlert;
                const isGranted = item.event_type.includes('ENTRY') || item.event_type === 'MANUAL_OVERRIDE';
                return (
                  <div className="feed-item" key={item.log_id}>
                    <span className="feed-time">{formatTime(item.timestamp)}</span>
                    <Icon size={16} color={isGranted ? '#34C759' : '#FF3B30'} />
                    <span style={{ flex: 1, color: '#1D1D1F' }}>{eventTypeLabel[item.event_type] || item.event_type}</span>
                    <span className="text-xs text-muted">{item.gate_id}</span>
                    <span className={`badge ${isGranted ? 'badge-success' : 'badge-danger'}`}>
                      {isGranted ? 'GRANTED' : 'DENIED'}
                    </span>
                  </div>
                );
              })}
            </div>
          </div>

          {/* Device Status */}
          <div className="card">
            <div className="card-header">
              <h3>Device Status</h3>
            </div>
            <div className="card-body">
              {(!devices || devices.length === 0) && (
                <div className="text-muted text-sm text-center" style={{ padding: 32 }}>
                  No devices registered
                </div>
              )}
              <div className="device-grid">
                {devices?.map((d) => (
                  <div className="device-tile" key={d.device_id}>
                    <div className="flex items-center gap-2">
                      <span className={`status-dot ${d.status.toLowerCase()}`} />
                      <span className="device-name">{d.device_name}</span>
                    </div>
                    <div className="device-meta">
                      Gate: {d.gate_name || d.gate_id}<br />
                      {d.ip_address && <>IP: {d.ip_address}<br /></>}
                      Faces: {d.face_count}/20,000<br />
                      <div className="flex gap-3" style={{ marginTop: 4 }}>
                        {d.wifi_connected && <span className="text-xs" style={{ color: '#34C759' }}><Wifi size={10} /> WiFi</span>}
                        {d.cellular_connected && <span className="text-xs" style={{ color: '#0066CC' }}><Signal size={10} /> 4G</span>}
                        {d.battery_level != null && (
                          <span className="text-xs" style={{ color: d.battery_level < 20 ? '#FF3B30' : '#8e8e93' }}>
                            <Battery size={10} /> {d.battery_level}%
                          </span>
                        )}
                      </div>
                      {d.last_heartbeat && <>Last ping: {formatTime(d.last_heartbeat)}</>}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
