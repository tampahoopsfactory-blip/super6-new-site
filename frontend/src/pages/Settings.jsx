import { useState, useEffect } from 'react';
import { api } from '../utils/api';
import { Save, Send, Mail, Phone, DollarSign, Check, Clock } from 'lucide-react';

const TICKET_TYPES = [
  { key: 'DAILY', label: 'Daily Pass', color: '#006aff' },
  { key: 'WEEKEND', label: 'Weekend Pass', color: '#7c3aed' },
  { key: 'KIDS', label: 'Kids Daily', color: '#f59e0b' },
  { key: 'KIDS_WEEKEND', label: 'Kids Weekend', color: '#d97706' },
  { key: 'COACH', label: 'Coach', color: '#0891b2' },
  { key: 'STAFF', label: 'Staff', color: '#059669' },
];

// Expiry rules — matches backend TICKET_EXPIRY_RULES
const EXPIRY_RULES = {
  DAILY:        { rule: 'fixed', label: 'End of day (11:59 PM)' },
  WEEKEND:      { rule: 'fixed', label: 'End of Sunday (11:59 PM)' },
  KIDS:         { rule: 'fixed', label: 'End of day (11:59 PM)' },
  KIDS_WEEKEND: { rule: 'fixed', label: 'End of Sunday (11:59 PM)' },
  COACH:        { rule: 'fixed', label: 'Never expires' },
  STAFF:        { rule: 'fixed', label: 'Never expires' },
};

export default function Settings() {
  const [settings, setSettings] = useState({});
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(null);
  const [message, setMessage] = useState('');

  // Pricing state
  const [pricing, setPricing] = useState({});
  const [pricingLoading, setPricingLoading] = useState(true);
  const [pricingSaving, setPricingSaving] = useState(false);
  const [pricingSaved, setPricingSaved] = useState(false);

  useEffect(() => {
    api.getSettings().then((s) => { setSettings(s); setLoading(false); }).catch(() => setLoading(false));
    api.getPricing().then((p) => { setPricing(p); setPricingLoading(false); }).catch(() => setPricingLoading(false));
  }, []);

  const handleSave = async (key, value) => {
    setSaving(key);
    setMessage('');
    try {
      await api.updateSetting(key, value);
      setMessage(`${key} updated`);
    } catch (err) {
      setMessage(`Error: ${err.message}`);
    } finally {
      setSaving(null);
      setTimeout(() => setMessage(''), 3000);
    }
  };

  const handleSavePricing = async () => {
    setPricingSaving(true);
    setPricingSaved(false);
    try {
      await api.updatePricing(pricing);
      setPricingSaved(true);
      setTimeout(() => setPricingSaved(false), 3000);
    } catch (err) {
      setMessage(`Error saving pricing: ${err.message}`);
    } finally {
      setPricingSaving(false);
    }
  };

  const handleTestSMS = async () => {
    setSaving('test_sms');
    const result = await api.testSms();
    setMessage(result.success ? 'Test SMS sent!' : `SMS failed: ${result.error || 'Unknown error'}`);
    setSaving(null);
    setTimeout(() => setMessage(''), 5000);
  };

  const handleTestEmail = async () => {
    setSaving('test_email');
    const result = await api.testEmail();
    setMessage(result.success ? 'Test email sent!' : `Email failed: ${result.error || 'Unknown error'}`);
    setSaving(null);
    setTimeout(() => setMessage(''), 5000);
  };

  if (loading) return <div className="page-body text-center text-muted" style={{ padding: 48 }}>Loading settings...</div>;

  return (
    <>
      <div className="page-header">
        <h2>Settings</h2>
        {message && (
          <span className={`badge ${message.startsWith('Error') ? 'badge-danger' : 'badge-success'}`}>
            {message}
          </span>
        )}
      </div>

      <div className="page-body">

        {/* ====== TICKET PRICING ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3><DollarSign size={16} style={{ marginRight: 8, verticalAlign: -2 }} />Ticket Pricing</h3>
            <div className="flex gap-2 items-center">
              {pricingSaved && (
                <span className="badge badge-success">
                  <Check size={12} style={{ marginRight: 4 }} /> Saved
                </span>
              )}
              <button
                className="btn btn-primary btn-sm"
                onClick={handleSavePricing}
                disabled={pricingSaving}
              >
                <Save size={12} /> {pricingSaving ? 'Saving...' : 'Save All Prices'}
              </button>
            </div>
          </div>
          <div className="card-body">
            <p className="text-sm text-muted" style={{ marginBottom: 16 }}>
              Set default prices for each ticket type. These prices are used when creating new orders. Only admins can change pricing.
            </p>
            {pricingLoading ? (
              <p className="text-muted text-center" style={{ padding: 24 }}>Loading pricing...</p>
            ) : (
              <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(220px, 1fr))', gap: 12 }}>
                {TICKET_TYPES.map((t) => (
                  <div key={t.key} style={{
                    border: '1px solid #e5e7eb',
                    borderRadius: 10,
                    padding: '14px 16px',
                    borderLeft: `4px solid ${t.color}`,
                  }}>
                    <label style={{ fontSize: 13, fontWeight: 700, color: t.color, display: 'block', marginBottom: 6 }}>
                      {t.label}
                    </label>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 6 }}>
                      <span style={{ fontSize: 18, fontWeight: 700, color: '#374151' }}>$</span>
                      <input
                        type="number"
                        step="0.01"
                        min="0"
                        value={pricing[t.key] ?? 0}
                        onChange={(e) => setPricing((p) => ({ ...p, [t.key]: parseFloat(e.target.value) || 0 }))}
                        style={{
                          width: '100%', height: 44, border: '1px solid #d1d5db', borderRadius: 8,
                          padding: '0 14px', fontSize: 18, fontWeight: 700, color: '#1a1a2e',
                          outline: 'none', background: '#f9fafb',
                        }}
                      />
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* ====== TICKET EXPIRY RULES ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3><Clock size={16} style={{ marginRight: 8, verticalAlign: -2 }} />Ticket Expiry Rules</h3>
          </div>
          <div className="card-body">
            <p className="text-sm text-muted" style={{ marginBottom: 16 }}>
              Each ticket type has a fixed expiry rule. These are automatic and cannot be changed.
            </p>
            <div style={{ display: 'grid', gap: 8 }}>
              {TICKET_TYPES.map((t) => {
                const rule = EXPIRY_RULES[t.key];
                if (!rule) return null;
                return (
                  <div key={t.key} style={{
                    display: 'flex', alignItems: 'center', justifyContent: 'space-between',
                    padding: '12px 16px', borderRadius: 8,
                    border: '1px solid #e5e7eb', background: '#fafafa',
                  }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                      <div style={{ width: 4, height: 28, borderRadius: 4, background: t.color }} />
                      <span style={{ fontWeight: 700, fontSize: 13, color: '#1a1a2e', minWidth: 130 }}>{t.label}</span>
                    </div>
                    <span style={{ fontSize: 13, color: '#6b7280', fontStyle: 'italic' }}>
                      {rule.label}
                    </span>
                  </div>
                );
              })}
            </div>
          </div>
        </div>

        {/* ====== ADMISSION RULES ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3>Admission Rules</h3>
          </div>
          <div className="card-body">
            <div className="form-group">
              <label className="form-label">Default Entry Mode</label>
              <select
                className="form-input"
                value={settings.multi_entry || 'true'}
                onChange={(e) => {
                  setSettings({ ...settings, multi_entry: e.target.value });
                  handleSave('multi_entry', e.target.value);
                }}
                style={{ maxWidth: 340 }}
              >
                <option value="true">Multi-entry (re-entry allowed)</option>
                <option value="false">Single-entry only</option>
              </select>
            </div>
          </div>
        </div>

        {/* ====== SMS SETTINGS ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3><Phone size={16} style={{ marginRight: 8, verticalAlign: -2 }} />SMS Delivery (Twilio)</h3>
            <div className="flex gap-2 items-center">
              <label className="text-sm" style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                <input
                  type="checkbox"
                  checked={settings.sms_enabled === 'true'}
                  onChange={(e) => {
                    const v = e.target.checked ? 'true' : 'false';
                    setSettings({ ...settings, sms_enabled: v });
                    handleSave('sms_enabled', v);
                  }}
                />
                Enabled
              </label>
              <button className="btn btn-secondary btn-sm" onClick={handleTestSMS} disabled={saving === 'test_sms'}>
                <Send size={12} /> {saving === 'test_sms' ? 'Sending...' : 'Test SMS'}
              </button>
            </div>
          </div>
          <div className="card-body">
            <div className="grid-2">
              <SettingField label="Account SID" settingKey="twilio_account_sid" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} type="password" />
              <SettingField label="Auth Token" settingKey="twilio_auth_token" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} type="password" />
            </div>
            <div className="grid-2">
              <SettingField label="From Number" settingKey="twilio_from_number" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} placeholder="+15551234567" />
              <SettingField label="Admin Phone (for testing)" settingKey="admin_phone" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} placeholder="+1..." />
            </div>
          </div>
        </div>

        {/* ====== EMAIL SETTINGS ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3><Mail size={16} style={{ marginRight: 8, verticalAlign: -2 }} />Email Delivery (SendGrid)</h3>
            <div className="flex gap-2 items-center">
              <label className="text-sm" style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                <input
                  type="checkbox"
                  checked={settings.email_enabled === 'true'}
                  onChange={(e) => {
                    const v = e.target.checked ? 'true' : 'false';
                    setSettings({ ...settings, email_enabled: v });
                    handleSave('email_enabled', v);
                  }}
                />
                Enabled
              </label>
              <button className="btn btn-secondary btn-sm" onClick={handleTestEmail} disabled={saving === 'test_email'}>
                <Send size={12} /> {saving === 'test_email' ? 'Sending...' : 'Test Email'}
              </button>
            </div>
          </div>
          <div className="card-body">
            <div className="grid-2">
              <SettingField label="SendGrid API Key" settingKey="sendgrid_api_key" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} type="password" />
              <SettingField label="From Email" settingKey="sendgrid_from_email" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} placeholder="tickets@example.com" />
            </div>
            <SettingField label="Admin Email (for testing)" settingKey="admin_email" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} placeholder="admin@example.com" />
          </div>
        </div>

        {/* ====== SQUARE SETTINGS ====== */}
        <div className="card mb-6">
          <div className="card-header">
            <h3>Square Terminal</h3>
          </div>
          <div className="card-body">
            <div className="grid-2">
              <SettingField label="Application ID" settingKey="square_application_id" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} />
              <SettingField label="Access Token" settingKey="square_access_token" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} type="password" />
            </div>
            <SettingField label="Webhook Signature Key" settingKey="square_webhook_key" settings={settings} setSettings={setSettings} onSave={handleSave} saving={saving} type="password" />
          </div>
        </div>

        {/* ====== ADMIN CREDENTIALS ====== */}
        <div className="card">
          <div className="card-header">
            <h3>Admin Account</h3>
          </div>
          <div className="card-body">
            <p className="text-sm text-muted mb-4">
              Change admin credentials in the .env file or environment variables.
              Default: admin / changeme123
            </p>
          </div>
        </div>
      </div>
    </>
  );
}

function SettingField({ label, settingKey, settings, setSettings, onSave, saving, type = 'text', placeholder = '' }) {
  return (
    <div className="form-group">
      <label className="form-label">{label}</label>
      <div className="flex gap-2">
        <input
          className="form-input"
          type={type}
          value={settings[settingKey] || ''}
          onChange={(e) => setSettings({ ...settings, [settingKey]: e.target.value })}
          placeholder={placeholder}
          style={{ flex: 1, minHeight: 44 }}
        />
        <button
          className="btn btn-secondary"
          onClick={() => onSave(settingKey, settings[settingKey] || '')}
          disabled={saving === settingKey}
          style={{ minWidth: 48, minHeight: 44 }}
        >
          <Save size={16} />
        </button>
      </div>
    </div>
  );
}
