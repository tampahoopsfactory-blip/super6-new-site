import { useState, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Wifi, WifiOff, Trash2, Zap, X, Battery, Signal, Key, Fingerprint, CreditCard, Eye } from 'lucide-react';

export default function Devices() {
  const fetchDevices = useCallback(() => api.getDevices(), []);
  const { data: devices, loading, refetch } = usePolling(fetchDevices, 10000);
  const [showCreate, setShowCreate] = useState(false);
  const [pingResult, setPingResult] = useState(null);

  const handlePing = async (id) => {
    setPingResult({ id, status: 'testing' });
    try {
      const result = await api.pingDevice(id);
      setPingResult({ id, ...result });
    } catch (err) {
      setPingResult({ id, status: 'FAILED', error: err.message });
    }
  };

  const handleDelete = async (id) => {
    if (!confirm(`Delete device ${id}? This will remove it from the system.`)) return;
    await api.deleteDevice(id);
    refetch();
  };

  const handleRegenToken = async (id) => {
    if (!confirm('Regenerate API token? The device will need to re-register.')) return;
    const result = await api.regenerateDeviceToken(id);
    alert(`New token: ${result.api_token.slice(0, 16)}...`);
    refetch();
  };

  return (
    <>
      <div className="page-header">
        <h2>X05 Devices</h2>
        <button className="btn btn-primary btn-sm" onClick={() => setShowCreate(true)}>
          <Plus size={14} /> Register Device
        </button>
      </div>

      <div className="page-body">
        {loading && !devices && <div className="text-center text-muted" style={{ padding: 48 }}>Loading devices...</div>}

        {devices?.length === 0 && (
          <div className="card">
            <div className="card-body text-center" style={{ padding: 48 }}>
              <Radio size={40} color="var(--color-gray-300)" style={{ margin: '0 auto 16px' }} />
              <p className="text-muted">No X05 devices registered yet</p>
              <p className="text-xs text-muted" style={{ marginTop: 8, maxWidth: 400, margin: '8px auto 24px' }}>
                X05 devices self-register when they connect. You can also register them manually below.
              </p>
              <button className="btn btn-primary mt-4" onClick={() => setShowCreate(true)}>
                <Plus size={14} /> Register First Device
              </button>
            </div>
          </div>
        )}

        <div className="device-grid">
          {devices?.map((d) => (
            <div className="device-tile" key={d.device_id}>
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-2">
                  {d.status === 'ONLINE' ? (
                    <Wifi size={18} color="var(--color-success)" />
                  ) : (
                    <WifiOff size={18} color="var(--color-danger)" />
                  )}
                  <span className="device-name">{d.device_name}</span>
                </div>
                <span className={`badge ${d.status === 'ONLINE' ? 'badge-success' : d.status === 'MAINTENANCE' ? 'badge-warning' : 'badge-danger'}`}>
                  {d.status}
                </span>
              </div>

              <div className="device-meta">
                <div><strong>Device ID:</strong> {d.device_id}</div>
                <div><strong>Gate:</strong> {d.gate_name || d.gate_id}</div>
                <div><strong>Model:</strong> {d.model || 'X05'}</div>
                {d.serial_number && <div><strong>SN:</strong> {d.serial_number}</div>}
                {d.ip_address && <div><strong>IP:</strong> {d.ip_address}</div>}
                <div><strong>Faces:</strong> {d.face_count}/20,000</div>
                {d.iris_count > 0 && <div><strong>Iris Templates:</strong> {d.iris_count}</div>}
                {d.finger_count > 0 && <div><strong>Fingerprints:</strong> {d.finger_count}</div>}
                {d.app_version && <div><strong>App:</strong> v{d.app_version}</div>}
                {d.firmware_version && <div><strong>FW:</strong> {d.firmware_version}</div>}

                {/* Connectivity */}
                <div className="flex gap-3" style={{ marginTop: 4 }}>
                  {d.wifi_connected && <span className="text-xs" style={{ color: 'var(--color-success)' }}><Wifi size={10} /> WiFi</span>}
                  {d.cellular_connected && <span className="text-xs" style={{ color: 'var(--color-info)' }}><Signal size={10} /> 4G</span>}
                  {d.battery_level != null && (
                    <span className="text-xs" style={{ color: d.battery_level < 20 ? 'var(--color-danger)' : '#6b7280' }}>
                      <Battery size={10} /> {d.battery_level}%
                    </span>
                  )}
                </div>

                {/* Biometric & Hardware Capabilities */}
                {(d.has_fingerprint || d.has_iris || d.has_nfc || d.wiegand_enabled || d.rs485_enabled) && (
                  <div className="flex gap-2 flex-wrap" style={{ marginTop: 6 }}>
                    {d.has_fingerprint && (
                      <span className="text-xs" style={{
                        background: '#f0fdf4', color: '#166534', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        <Fingerprint size={10} /> Finger{d.finger_sdk_version ? ` v${d.finger_sdk_version}` : ''}
                      </span>
                    )}
                    {d.has_iris && (
                      <span className="text-xs" style={{
                        background: '#fff7ed', color: '#c2410c', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        <Eye size={10} /> Iris{d.iris_sdk_version ? ` v${d.iris_sdk_version}` : ''}
                      </span>
                    )}
                    {d.has_nfc && (
                      <span className="text-xs" style={{
                        background: '#eff6ff', color: '#1e40af', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        <CreditCard size={10} /> NFC
                      </span>
                    )}
                    {d.wiegand_enabled && (
                      <span className="text-xs" style={{
                        background: '#fdf4ff', color: '#7e22ce', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        WG
                      </span>
                    )}
                    {d.rs485_enabled && (
                      <span className="text-xs" style={{
                        background: '#fefce8', color: '#a16207', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        RS485
                      </span>
                    )}
                    {d.relay_mode && d.relay_mode !== 'NO' && (
                      <span className="text-xs" style={{
                        background: '#fef2f2', color: '#991b1b', padding: '2px 6px',
                        borderRadius: 4, fontWeight: 600,
                      }}>
                        Relay: {d.relay_mode}
                      </span>
                    )}
                  </div>
                )}

                {d.last_heartbeat && (
                  <div style={{ marginTop: 4 }}><strong>Last Ping:</strong> {new Date(d.last_heartbeat).toLocaleTimeString()}</div>
                )}
              </div>

              {pingResult?.id === d.device_id && (
                <div
                  className="mt-4 text-xs"
                  style={{
                    padding: '8px 12px',
                    borderRadius: 6,
                    background: pingResult.status === 'ONLINE' ? 'var(--color-success-bg)' : pingResult.status === 'testing' ? 'var(--color-info-bg)' : 'var(--color-danger-bg)',
                  }}
                >
                  {pingResult.status === 'testing' ? 'Checking heartbeat...' :
                   pingResult.status === 'ONLINE' ? `Online — last heartbeat ${pingResult.seconds_ago}s ago` :
                   pingResult.status === 'UNKNOWN' ? 'Device has never connected' :
                   `Offline — last seen ${pingResult.seconds_ago}s ago`}
                </div>
              )}

              <div className="flex gap-2 mt-4 flex-wrap">
                <button className="btn btn-primary btn-sm" onClick={() => handlePing(d.device_id)}>
                  <Zap size={12} /> Ping
                </button>
                <button className="btn btn-secondary btn-sm" onClick={() => handleRegenToken(d.device_id)} title="Regenerate API Token">
                  <Key size={12} /> Token
                </button>
                <button className="btn btn-danger btn-sm" onClick={() => handleDelete(d.device_id)}>
                  <Trash2 size={12} />
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>

      {showCreate && (
        <CreateDeviceModal onClose={() => setShowCreate(false)} onCreated={() => { setShowCreate(false); refetch(); }} />
      )}
    </>
  );
}

function Radio({ size, color, style }) {
  return <div style={{ width: size, height: size, ...style }}>
    <svg viewBox="0 0 24 24" fill="none" stroke={color} strokeWidth="2" width={size} height={size}>
      <circle cx="12" cy="12" r="2"/><path d="M16.24 7.76a6 6 0 0 1 0 8.49m-8.48-.01a6 6 0 0 1 0-8.49m11.31-2.82a10 10 0 0 1 0 14.14m-14.14 0a10 10 0 0 1 0-14.14"/>
    </svg>
  </div>;
}

function CreateDeviceModal({ onClose, onCreated }) {
  const [form, setForm] = useState({
    device_id: '',
    device_name: '',
    gate_id: '',
    gate_name: '',
    ip_address: '',
    serial_number: '',
    model: 'X05',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await api.createDevice(form);
      onCreated();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{
      position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)',
      display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000
    }} onClick={onClose}>
      <div className="card" style={{ width: 480, maxHeight: '90vh', overflow: 'auto' }} onClick={(e) => e.stopPropagation()}>
        <div className="card-header">
          <h3>Register X05 Device</h3>
          <button className="btn btn-secondary btn-sm" onClick={onClose}><X size={14} /></button>
        </div>
        <div className="card-body">
          <p className="text-xs text-muted" style={{ marginBottom: 16 }}>
            X05 devices self-register via /api/x05/register with face, iris, fingerprint (FingerSDK v2.0.1), and NFC capabilities.
          </p>
          <form onSubmit={handleSubmit}>
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">Device ID</label>
                <input className="form-input" value={form.device_id} onChange={(e) => setForm({ ...form, device_id: e.target.value })} placeholder="e.g. x05_gate01" required />
              </div>
              <div className="form-group">
                <label className="form-label">Device Name</label>
                <input className="form-input" value={form.device_name} onChange={(e) => setForm({ ...form, device_name: e.target.value })} placeholder="e.g. Main Entrance" required />
              </div>
            </div>
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">Gate ID</label>
                <input className="form-input" value={form.gate_id} onChange={(e) => setForm({ ...form, gate_id: e.target.value })} placeholder="gate_01" required />
              </div>
              <div className="form-group">
                <label className="form-label">Gate Name</label>
                <input className="form-input" value={form.gate_name} onChange={(e) => setForm({ ...form, gate_name: e.target.value })} placeholder="Main Gate" />
              </div>
            </div>
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">IP Address</label>
                <input className="form-input" value={form.ip_address} onChange={(e) => setForm({ ...form, ip_address: e.target.value })} placeholder="Optional — device reports this" />
              </div>
              <div className="form-group">
                <label className="form-label">Serial Number</label>
                <input className="form-input" value={form.serial_number} onChange={(e) => setForm({ ...form, serial_number: e.target.value })} placeholder="Optional" />
              </div>
            </div>
            {error && <div style={{ color: 'var(--color-danger)', fontSize: 'var(--font-size-sm)', marginBottom: 12 }}>{error}</div>}
            <button type="submit" className="btn btn-primary" style={{ width: '100%', justifyContent: 'center' }} disabled={loading}>
              {loading ? 'Registering...' : 'Register Device'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
