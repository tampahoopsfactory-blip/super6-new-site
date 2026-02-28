import { useState, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Wifi, WifiOff, Trash2, Zap, Battery, Signal, Key, Fingerprint, CreditCard, Eye, Check, ChevronDown, ChevronUp, Copy } from 'lucide-react';

export default function Devices() {
  const fetchDevices = useCallback(() => api.getDevices(), []);
  const { data: devices, loading, refetch } = usePolling(fetchDevices, 10000);
  const [showCreate, setShowCreate] = useState(false);
  const [showList, setShowList] = useState(true);
  const [pingResult, setPingResult] = useState(null);
  const [confirmAction, setConfirmAction] = useState(null);
  const [tokenResult, setTokenResult] = useState(null);

  const handlePing = async (id) => {
    setPingResult({ id, status: 'testing' });
    try {
      const result = await api.pingDevice(id);
      setPingResult({ id, ...result });
    } catch (err) {
      setPingResult({ id, status: 'FAILED', error: err.message });
    }
  };

  const handleDelete = (d) => {
    setConfirmAction({ type: 'delete', id: d.device_id, name: d.device_name });
  };

  const handleRegenToken = (d) => {
    setConfirmAction({ type: 'regen', id: d.device_id, name: d.device_name });
  };

  const executeConfirm = async () => {
    if (!confirmAction) return;
    if (confirmAction.type === 'delete') {
      await api.deleteDevice(confirmAction.id);
      setConfirmAction(null);
      refetch();
    } else if (confirmAction.type === 'regen') {
      const result = await api.regenerateDeviceToken(confirmAction.id);
      setConfirmAction(null);
      setTokenResult({ id: confirmAction.id, token: result.api_token });
      refetch();
    }
  };

  return (
    <>
      {/* ====== INLINE REGISTER DEVICE FORM ====== */}
      <div className="page-body" style={{ paddingBottom: 0 }}>
        <InlineRegisterDeviceForm
          show={showCreate}
          onToggle={() => setShowCreate(!showCreate)}
          onCreated={refetch}
        />
      </div>

      {/* ====== INLINE CONFIRM ACTION ====== */}
      {confirmAction && (
        <div className="page-body" style={{ paddingTop: 12, paddingBottom: 0 }}>
          <div className="card" style={{
            textAlign: 'center', padding: 28,
            border: '2px solid #FF3B30', borderRadius: 16, background: 'var(--color-danger-bg)',
          }}>
            <h3 style={{ color: '#FF3B30', marginBottom: 8, fontSize: 18, fontWeight: 700 }}>
              {confirmAction.type === 'delete' ? 'Delete Device?' : 'Regenerate Token?'}
            </h3>
            <p style={{ fontSize: 15, color: '#48484a', marginBottom: 20 }}>
              {confirmAction.type === 'delete'
                ? `Delete "${confirmAction.name}" (${confirmAction.id})? This will remove it from the system.`
                : `Regenerate API token for "${confirmAction.name}"? The device will need to re-register.`}
            </p>
            <div style={{ display: 'flex', gap: 12, justifyContent: 'center' }}>
              <button className="btn btn-secondary" onClick={() => setConfirmAction(null)}
                style={{ padding: '12px 28px' }}>
                Cancel
              </button>
              <button className="btn btn-danger" onClick={executeConfirm}
                style={{ padding: '12px 28px' }}>
                {confirmAction.type === 'delete' ? 'Yes, Delete' : 'Yes, Regenerate'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* ====== INLINE TOKEN RESULT ====== */}
      {tokenResult && (
        <div className="page-body" style={{ paddingTop: 12, paddingBottom: 0 }}>
          <div className="card" style={{
            textAlign: 'center', padding: 28,
            border: '2px solid #34C759', borderRadius: 16, background: 'var(--color-success-bg)',
          }}>
            <h3 style={{ color: '#248a3d', marginBottom: 12, fontSize: 18, fontWeight: 700 }}>New Token Generated</h3>
            <div style={{
              fontFamily: 'monospace', fontSize: 13, padding: '14px 16px',
              background: '#fff', borderRadius: 10, border: '1px solid #d1d1d6',
              wordBreak: 'break-all', marginBottom: 16,
            }}>
              {tokenResult.token}
            </div>
            <button className="btn btn-secondary" onClick={() => setTokenResult(null)}
              style={{ padding: '10px 24px' }}>
              Dismiss
            </button>
          </div>
        </div>
      )}

      {/* ====== DEVICE LIST (collapsible) ====== */}
      <div className="page-body" style={{ paddingTop: 12 }}>
        <button
          className="btn btn-secondary"
          onClick={() => setShowList(!showList)}
          style={{ width: '100%', justifyContent: 'center', padding: '12px 0', fontSize: 15, marginBottom: 12 }}
        >
          {showList ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
          {showList ? ' Hide Devices' : ` Show Devices (${devices?.length || 0})`}
        </button>

        {showList && (
          <>
            {loading && !devices && <div className="text-center text-muted" style={{ padding: 48 }}>Loading devices...</div>}

            {devices?.length === 0 && (
              <div className="card" style={{ textAlign: 'center', padding: 48 }}>
                <Radio size={40} color="#8e8e93" style={{ margin: '0 auto 16px' }} />
                <p className="text-muted">No X05 devices registered yet</p>
                <p className="text-xs text-muted" style={{ marginTop: 8 }}>
                  X05 devices self-register when they connect. You can also register them above.
                </p>
              </div>
            )}

            <div className="device-grid">
              {devices?.map((d) => (
                <div className="device-tile" key={d.device_id}>
                  <div className="flex items-center justify-between" style={{ marginBottom: 14 }}>
                    <div className="flex items-center gap-2">
                      {d.status === 'ONLINE' ? (
                        <Wifi size={18} color="#34C759" />
                      ) : (
                        <WifiOff size={18} color="#FF3B30" />
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

                    <div className="flex gap-3" style={{ marginTop: 6 }}>
                      {d.wifi_connected && <span style={{ fontSize: 12, color: '#34C759' }}><Wifi size={11} /> WiFi</span>}
                      {d.cellular_connected && <span style={{ fontSize: 12, color: '#0066CC' }}><Signal size={11} /> 4G</span>}
                      {d.battery_level != null && (
                        <span style={{ fontSize: 12, color: d.battery_level < 20 ? '#FF3B30' : '#8e8e93' }}>
                          <Battery size={11} /> {d.battery_level}%
                        </span>
                      )}
                    </div>

                    {(d.has_fingerprint || d.has_iris || d.has_nfc || d.wiegand_enabled || d.rs485_enabled) && (
                      <div className="flex gap-2 flex-wrap" style={{ marginTop: 8 }}>
                        {d.has_fingerprint && (
                          <span style={{
                            background: '#eafbf0', color: '#248a3d', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            <Fingerprint size={11} /> Finger{d.finger_sdk_version ? ` v${d.finger_sdk_version}` : ''}
                          </span>
                        )}
                        {d.has_iris && (
                          <span style={{
                            background: '#fff4e6', color: '#c93400', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            <Eye size={11} /> Iris{d.iris_sdk_version ? ` v${d.iris_sdk_version}` : ''}
                          </span>
                        )}
                        {d.has_nfc && (
                          <span style={{
                            background: '#E8F1FC', color: '#0066CC', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            <CreditCard size={11} /> NFC
                          </span>
                        )}
                        {d.wiegand_enabled && (
                          <span style={{
                            background: '#f3ecff', color: '#5856D6', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            WG
                          </span>
                        )}
                        {d.rs485_enabled && (
                          <span style={{
                            background: '#fff4e6', color: '#FF9500', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            RS485
                          </span>
                        )}
                        {d.relay_mode && d.relay_mode !== 'NO' && (
                          <span style={{
                            background: 'var(--color-danger-bg)', color: '#FF3B30', padding: '3px 8px',
                            borderRadius: 6, fontWeight: 600, fontSize: 11,
                          }}>
                            Relay: {d.relay_mode}
                          </span>
                        )}
                      </div>
                    )}

                    {d.last_heartbeat && (
                      <div style={{ marginTop: 6 }}><strong>Last Ping:</strong> {new Date(d.last_heartbeat).toLocaleTimeString()}</div>
                    )}
                  </div>

                  {pingResult?.id === d.device_id && (
                    <div
                      style={{
                        marginTop: 12, fontSize: 13, padding: '10px 14px', borderRadius: 10,
                        background: pingResult.status === 'ONLINE' ? 'var(--color-success-bg)' : pingResult.status === 'testing' ? '#E8F1FC' : 'var(--color-danger-bg)',
                        color: pingResult.status === 'ONLINE' ? '#248a3d' : pingResult.status === 'testing' ? '#0066CC' : '#FF3B30',
                      }}
                    >
                      {pingResult.status === 'testing' ? 'Checking heartbeat...' :
                       pingResult.status === 'ONLINE' ? `Online — last heartbeat ${pingResult.seconds_ago}s ago` :
                       pingResult.status === 'UNKNOWN' ? 'Device has never connected' :
                       `Offline — last seen ${pingResult.seconds_ago}s ago`}
                    </div>
                  )}

                  <div className="flex gap-2 flex-wrap" style={{ marginTop: 14 }}>
                    <button className="btn btn-primary btn-sm" onClick={() => handlePing(d.device_id)}>
                      <Zap size={14} /> Ping
                    </button>
                    <button className="btn btn-secondary btn-sm" onClick={() => handleRegenToken(d)}>
                      <Key size={14} /> Token
                    </button>
                    <button className="btn btn-danger btn-sm" onClick={() => handleDelete(d)}>
                      <Trash2 size={14} /> Delete
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}
      </div>
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


/* ============================================================
   INLINE REGISTER DEVICE FORM — Apple-inspired
   ============================================================ */
function InlineRegisterDeviceForm({ show, onToggle, onCreated }) {
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
  const [result, setResult] = useState(null);

  const resetForm = () => {
    setForm({ device_id: '', device_name: '', gate_id: '', gate_name: '', ip_address: '', serial_number: '', model: 'X05' });
    setError('');
    setResult(null);
  };

  const handleSubmit = async () => {
    setLoading(true);
    setError('');
    try {
      await api.createDevice(form);
      setResult({ name: form.device_name });
      onCreated();
      setTimeout(() => resetForm(), 2000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card" style={{ borderRadius: 18, overflow: 'hidden' }}>
      <div
        style={{
          background: '#0066CC', color: '#fff', padding: '16px 28px',
          borderRadius: show ? '16px 16px 0 0' : 16,
          textAlign: 'center', cursor: 'pointer',
          display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 8,
        }}
        onClick={onToggle}
      >
        <h2 style={{ margin: 0, fontSize: 20, fontWeight: 700, letterSpacing: -0.3 }}>Register Device</h2>
        {show ? <ChevronUp size={18} /> : <ChevronDown size={18} />}
      </div>

      {show && (
        <div style={{ padding: '24px 28px' }}>
          {result && (
            <div style={{ textAlign: 'center', padding: '28px 0' }}>
              <div style={{ width: 56, height: 56, borderRadius: '50%', background: 'var(--color-success-bg)', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px' }}>
                <Check size={28} color="#34C759" strokeWidth={2.5} />
              </div>
              <h3 style={{ marginBottom: 4, color: '#1D1D1F', fontWeight: 700 }}>Device Registered!</h3>
              <p className="text-muted">"{result.name}" added to the system</p>
              <button className="btn btn-primary" onClick={resetForm} style={{ marginTop: 12, padding: '12px 32px' }}>
                <Plus size={16} /> Register Another
              </button>
            </div>
          )}

          {!result && (
            <>
              <p className="text-muted" style={{ marginBottom: 16, fontSize: 13 }}>
                X05 devices self-register via /api/x05/register. You can also register them manually here.
              </p>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">Device ID</label>
                  <input className="form-input" value={form.device_id} onChange={(e) => setForm({ ...form, device_id: e.target.value })} placeholder="e.g. x05_gate01" />
                </div>
                <div className="form-group">
                  <label className="form-label">Device Name</label>
                  <input className="form-input" value={form.device_name} onChange={(e) => setForm({ ...form, device_name: e.target.value })} placeholder="e.g. Main Entrance" />
                </div>
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">Gate ID</label>
                  <input className="form-input" value={form.gate_id} onChange={(e) => setForm({ ...form, gate_id: e.target.value })} placeholder="gate_01" />
                </div>
                <div className="form-group">
                  <label className="form-label">Gate Name</label>
                  <input className="form-input" value={form.gate_name} onChange={(e) => setForm({ ...form, gate_name: e.target.value })} placeholder="Main Gate" />
                </div>
              </div>
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">IP Address</label>
                  <input className="form-input" value={form.ip_address} onChange={(e) => setForm({ ...form, ip_address: e.target.value })} placeholder="Optional" />
                </div>
                <div className="form-group">
                  <label className="form-label">Serial Number</label>
                  <input className="form-input" value={form.serial_number} onChange={(e) => setForm({ ...form, serial_number: e.target.value })} placeholder="Optional" />
                </div>
              </div>
              {error && <div style={{ color: 'var(--color-danger)', fontSize: 14, marginBottom: 12, fontWeight: 600 }}>{error}</div>}
              <button type="button" onClick={handleSubmit}
                className="btn btn-primary"
                style={{ width: '100%', justifyContent: 'center', padding: '16px 0', fontSize: 18, minHeight: 56, fontWeight: 700, borderRadius: 12, letterSpacing: -0.3 }}
                disabled={loading}>
                {loading ? 'Registering...' : 'Register Device'}
              </button>
            </>
          )}
        </div>
      )}
    </div>
  );
}
