import { useState, useEffect, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Search, Download, QrCode, RotateCcw, ShieldCheck, X, Minus, Check, Users, ChevronDown, ChevronUp } from 'lucide-react';

const TICKET_TYPES = [
  { key: 'DAILY', label: 'Adult Daily', color: '#0066CC', desc: 'Single day' },
  { key: 'WEEKEND', label: 'Adult Weekend', color: '#5856D6', desc: 'Sat & Sun' },
  { key: 'KIDS', label: 'Kids Daily', color: '#FF9500', desc: 'Single day (child)' },
  { key: 'KIDS_WEEKEND', label: 'Kids Weekend', color: '#FF3B30', desc: 'Sat & Sun (child)' },
  { key: 'COACH', label: 'Coach', color: '#5AC8FA', desc: 'Never expires' },
  { key: 'STAFF', label: 'Staff', color: '#34C759', desc: 'Never expires' },
];

const TYPE_COLORS = {
  DAILY: 'badge-info', WEEKEND: 'badge-info',
  KIDS: 'badge-warning', KIDS_WEEKEND: 'badge-warning',
  COACH: 'badge-info', STAFF: 'badge-success',
};

const STATUS_COLORS = {
  ACTIVE: 'badge-success', EXPIRED: 'badge-danger',
  REFUNDED: 'badge-warning', INVALID: 'badge-danger',
};

export default function Tickets() {
  const [filter, setFilter] = useState({ page: '1', per_page: '50' });
  const [search, setSearch] = useState('');
  const [showQR, setShowQR] = useState(null);
  const [confirmRefund, setConfirmRefund] = useState(null);
  const [showHistory, setShowHistory] = useState(false);

  const fetchTickets = useCallback(() => api.getTickets(filter), [filter]);
  const { data: tickets, loading, refetch } = usePolling(fetchTickets, 10000);

  const handleRefund = async (id) => {
    setConfirmRefund(id);
  };

  const executeRefund = async () => {
    if (!confirmRefund) return;
    await api.refundTicket(confirmRefund);
    setConfirmRefund(null);
    refetch();
  };

  const handleOverride = async (id) => {
    await api.overrideTicket(id);
    refetch();
  };

  const handleViewQR = async (id) => {
    if (showQR && showQR.ticket_id === id) { setShowQR(null); return; }
    const data = await api.getTicketQR(id);
    setShowQR(data);
  };

  const handleExportCSV = () => {
    if (!tickets?.length) return;
    const headers = ['Ticket ID', 'Group', 'Patron', 'Type', 'Amount', 'Status', 'Entries', 'Created'];
    const rows = tickets.map((t) => [
      t.ticket_id.slice(-8), t.group_id ? t.group_id.slice(-6) : '-',
      t.patron_name || 'N/A', t.admission_type, t.amount_paid,
      t.status, t.entry_count, new Date(t.created_at).toLocaleString(),
    ]);
    const csv = [headers, ...rows].map((r) => r.join(',')).join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'tickets.csv';
    a.click();
  };

  const filteredTickets = tickets?.filter((t) => {
    if (!search) return true;
    const s = search.toLowerCase();
    return (
      t.ticket_id.toLowerCase().includes(s) ||
      (t.patron_name && t.patron_name.toLowerCase().includes(s)) ||
      (t.patron_email && t.patron_email.toLowerCase().includes(s)) ||
      (t.group_id && t.group_id.toLowerCase().includes(s))
    );
  });

  return (
    <>
      {/* ====== INLINE TICKET ENTRY FORM ====== */}
      <div className="page-body" style={{ paddingBottom: 0 }}>
        <InlineTicketForm onCreated={refetch} />
      </div>

      {/* ====== TICKET HISTORY (collapsible) ====== */}
      <div className="page-body" style={{ paddingTop: 8 }}>
        <button
          className="btn btn-secondary"
          onClick={() => setShowHistory(!showHistory)}
          style={{ width: '100%', justifyContent: 'center', padding: '10px 0', fontSize: 14, marginBottom: 12 }}
        >
          {showHistory ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
          {showHistory ? ' Hide Ticket History' : ` Show Ticket History (${tickets?.length || 0})`}
        </button>

        {showHistory && (
          <>
            {/* Filters */}
            <div className="card mb-6">
              <div className="card-body flex gap-3 flex-wrap items-center">
                <div style={{ flex: 1, minWidth: 200 }}>
                  <div style={{ position: 'relative' }}>
                    <Search size={16} style={{ position: 'absolute', left: 12, top: 14, color: 'var(--color-gray-400)' }} />
                    <input
                      className="form-input"
                      placeholder="Search by name, email, ticket ID, or group..."
                      value={search}
                      onChange={(e) => setSearch(e.target.value)}
                      style={{ paddingLeft: 36 }}
                    />
                  </div>
                </div>
                <select className="form-input" style={{ width: 160 }}
                  value={filter.admission_type || ''}
                  onChange={(e) => setFilter({ ...filter, admission_type: e.target.value || undefined })}>
                  <option value="">All Types</option>
                  {TICKET_TYPES.map((t) => <option key={t.key} value={t.key}>{t.label}</option>)}
                </select>
                <select className="form-input" style={{ width: 140 }}
                  value={filter.status || ''}
                  onChange={(e) => setFilter({ ...filter, status: e.target.value || undefined })}>
                  <option value="">All Status</option>
                  <option value="ACTIVE">Active</option>
                  <option value="EXPIRED">Expired</option>
                  <option value="REFUNDED">Refunded</option>
                </select>
                <button className="btn btn-secondary btn-sm" onClick={handleExportCSV}>
                  <Download size={14} /> Export CSV
                </button>
              </div>
            </div>

            {/* Table */}
            <div className="card">
              <div className="table-wrap">
                <table>
                  <thead>
                    <tr>
                      <th>Ticket ID</th>
                      <th>Group</th>
                      <th>Patron</th>
                      <th>Type</th>
                      <th>Amount</th>
                      <th>Status</th>
                      <th>QR Used</th>
                      <th>Biometrics</th>
                      <th>Entries</th>
                      <th>Created</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {loading && !tickets && (
                      <tr><td colSpan={11} className="text-center text-muted" style={{ padding: 32 }}>Loading...</td></tr>
                    )}
                    {filteredTickets?.length === 0 && (
                      <tr><td colSpan={11} className="text-center text-muted" style={{ padding: 32 }}>No tickets found</td></tr>
                    )}
                    {filteredTickets?.map((t) => (
                      <tr key={t.ticket_id}>
                        <td style={{ fontFamily: 'monospace', fontSize: 'var(--font-size-xs)' }}>
                          ...{t.ticket_id.slice(-8)}
                        </td>
                        <td>
                          {t.group_id ? (
                            <span className="badge badge-neutral" style={{ fontFamily: 'monospace', fontSize: 10 }}>
                              <Users size={10} style={{ marginRight: 2 }} />
                              {t.group_id.slice(-6)}
                            </span>
                          ) : <span className="text-muted">-</span>}
                        </td>
                        <td>{t.patron_name || <span className="text-muted">-</span>}</td>
                        <td><span className={`badge ${TYPE_COLORS[t.admission_type] || 'badge-neutral'}`}>{t.admission_type}</span></td>
                        <td>${Number(t.amount_paid).toFixed(2)}</td>
                        <td><span className={`badge ${STATUS_COLORS[t.status]}`}>{t.status}</span></td>
                        <td>{t.qr_used ? <span className="badge badge-warning">USED</span> : <span className="text-muted">-</span>}</td>
                        <td>
                          <div className="flex gap-1">
                            {t.face_enrolled && <span className="badge badge-success" style={{ fontSize: 10 }}>Face</span>}
                            {t.iris_enrolled && <span className="badge badge-warning" style={{ fontSize: 10 }}>Iris</span>}
                            {t.finger_enrolled && <span className="badge badge-info" style={{ fontSize: 10 }}>Finger</span>}
                            {!t.face_enrolled && !t.iris_enrolled && !t.finger_enrolled && <span className="text-muted">-</span>}
                          </div>
                        </td>
                        <td>{t.entry_count}</td>
                        <td className="text-xs">{new Date(t.created_at).toLocaleDateString()}</td>
                        <td>
                          <div className="flex gap-2">
                            <button className="btn btn-secondary btn-sm" onClick={() => handleViewQR(t.ticket_id)} title="View QR"><QrCode size={14} /></button>
                            {t.status === 'ACTIVE' && (
                              <>
                                <button className="btn btn-secondary btn-sm" onClick={() => handleOverride(t.ticket_id)} title="Grant Access"><ShieldCheck size={14} /></button>
                                <button className="btn btn-danger btn-sm" onClick={() => handleRefund(t.ticket_id)} title="Refund"><RotateCcw size={14} /></button>
                              </>
                            )}
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </>
        )}
      </div>

      {/* Inline QR Display */}
      {showQR && (
        <div className="page-body" style={{ paddingTop: 0 }}>
          <div className="card" style={{ textAlign: 'center', padding: 32, border: '1px solid var(--color-gray-200)', borderRadius: 14 }}>
            <h3 style={{ marginBottom: 16, fontWeight: 600, color: '#1D1D1F' }}>Ticket QR Code</h3>
            <img src={`data:image/png;base64,${showQR.qr_base64}`} alt="QR Code" style={{ width: 200, height: 200, margin: '0 auto', borderRadius: 12 }} />
            <p className="text-xs text-muted" style={{ marginTop: 12, fontFamily: 'monospace' }}>{showQR.ticket_id}</p>
            <button className="btn btn-secondary" onClick={() => setShowQR(null)} style={{ marginTop: 16 }}>
              <X size={14} /> Close
            </button>
          </div>
        </div>
      )}

      {/* Inline Refund Confirmation */}
      {confirmRefund && (
        <div className="page-body" style={{ paddingTop: 0 }}>
          <div className="card" style={{ textAlign: 'center', padding: 32, border: '2px solid var(--color-danger)', borderRadius: 14, background: 'var(--color-danger-bg)' }}>
            <h3 style={{ color: 'var(--color-danger)', marginBottom: 8, fontWeight: 700 }}>Refund Ticket?</h3>
            <p style={{ fontSize: 14, color: '#48484a', marginBottom: 20 }}>This will revoke access for ticket ...{confirmRefund.slice(-8)}</p>
            <div style={{ display: 'flex', gap: 12, justifyContent: 'center' }}>
              <button className="btn btn-secondary" onClick={() => setConfirmRefund(null)} style={{ padding: '10px 28px' }}>Cancel</button>
              <button className="btn btn-danger" onClick={executeRefund} style={{ padding: '10px 28px' }}>Yes, Refund</button>
            </div>
          </div>
        </div>
      )}
    </>
  );
}


/* ============================================================
   INLINE TICKET FORM — Apple-inspired design
   ============================================================ */
function InlineTicketForm({ onCreated }) {
  const [counts, setCounts] = useState(
    Object.fromEntries(TICKET_TYPES.map((t) => [t.key, 0]))
  );
  const [prices, setPrices] = useState(
    Object.fromEntries(TICKET_TYPES.map((t) => [t.key, 0]))
  );
  const [pricesLoaded, setPricesLoaded] = useState(false);
  const [patronName, setPatronName] = useState('');
  const [patronPhone, setPatronPhone] = useState('');
  const [patronEmail, setPatronEmail] = useState('');
  const [eventId, setEventId] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [result, setResult] = useState(null);

  useEffect(() => {
    api.getPricing().then((p) => {
      setPrices(p);
      setPricesLoaded(true);
    }).catch(() => {
      setPrices({ DAILY: 15, WEEKEND: 25, KIDS: 10, KIDS_WEEKEND: 18, COACH: 0, STAFF: 0 });
      setPricesLoaded(true);
    });

    const resolveEvent = async () => {
      try {
        const allEvents = await api.getEvents();
        const active = allEvents.find((e) => e.status === 'ACTIVE');
        if (active) { setEventId(active.event_id); return; }
        if (allEvents.length > 0) { setEventId(allEvents[0].event_id); return; }
        const now = new Date();
        const endOfDay = new Date(now);
        endOfDay.setHours(23, 59, 59, 999);
        const newEvent = await api.createEvent({
          name: `Event ${now.toLocaleDateString()}`,
          event_date: now.toISOString().slice(0, 10),
          start_time: now.toISOString(),
          end_time: endOfDay.toISOString(),
          admission_price: 0,
          max_capacity: 9999,
          multi_entry: true,
          ticket_expiry_hours: 24,
        });
        setEventId(newEvent.event_id);
      } catch { /* silent */ }
    };
    resolveEvent();
  }, []);

  const increment = (key) => setCounts((c) => ({ ...c, [key]: c[key] + 1 }));
  const decrement = (key) => setCounts((c) => ({ ...c, [key]: Math.max(0, c[key] - 1) }));

  const totalTickets = Object.values(counts).reduce((a, b) => a + b, 0);
  const totalAmount = TICKET_TYPES.reduce((sum, t) => sum + counts[t.key] * (prices[t.key] || 0), 0);
  const selectedTypes = TICKET_TYPES.filter((t) => counts[t.key] > 0);

  const resetForm = () => {
    setCounts(Object.fromEntries(TICKET_TYPES.map((t) => [t.key, 0])));
    setPatronName('');
    setPatronPhone('');
    setPatronEmail('');
    setError('');
    setResult(null);
  };

  const handleSubmit = async () => {
    if (totalTickets === 0) { setError('Add at least one ticket'); return; }
    if (!patronPhone.trim()) { setError('Phone number is required'); return; }
    if (!eventId) { setError('Event not ready — please wait a moment and try again'); return; }
    setLoading(true);
    setError('');

    try {
      const items = selectedTypes.map((t) => ({
        admission_type: t.key,
        quantity: counts[t.key],
        unit_price: prices[t.key] || 0,
      }));

      if (totalTickets === 1) {
        const item = items[0];
        await api.createTicket({
          event_id: eventId,
          patron_name: patronName || null,
          patron_phone: patronPhone || null,
          patron_email: patronEmail || null,
          admission_type: item.admission_type,
          amount_paid: item.unit_price,
        });
      } else {
        await api.createGroupOrder({
          event_id: eventId,
          patron_name: patronName || null,
          patron_phone: patronPhone || null,
          patron_email: patronEmail || null,
          items,
          total_paid: totalAmount,
        });
      }
      setResult({ count: totalTickets, total: totalAmount });
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
      {/* Header */}
      <div style={{
        background: '#0066CC', color: '#fff', padding: '16px 28px',
        textAlign: 'center',
      }}>
        <h2 style={{ margin: 0, fontSize: 20, fontWeight: 700, letterSpacing: -0.3 }}>New Ticket</h2>
      </div>

      <div style={{ padding: '24px 28px' }}>
        {/* Success state */}
        {result && (
          <div style={{ textAlign: 'center', padding: '28px 0' }}>
            <div style={{
              width: 56, height: 56, borderRadius: '50%',
              background: 'var(--color-success-bg)', display: 'flex',
              alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px',
            }}>
              <Check size={28} color="#34C759" strokeWidth={2.5} />
            </div>
            <h3 style={{ marginBottom: 4, color: '#1D1D1F', fontWeight: 700 }}>
              {result.count} Ticket{result.count > 1 ? 's' : ''} Created
            </h3>
            <p className="text-muted" style={{ fontSize: 15 }}>Total: ${result.total.toFixed(2)}</p>
            <button className="btn btn-primary" onClick={resetForm} style={{ marginTop: 16, padding: '12px 32px' }}>
              <Plus size={16} /> New Ticket
            </button>
          </div>
        )}

        {!result && (
          <>
            {/* Ticket Type Grid */}
            {!pricesLoaded ? (
              <p className="text-muted text-center" style={{ padding: 16 }}>Loading prices...</p>
            ) : (
              <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: 10, marginBottom: 20 }}>
                {TICKET_TYPES.map((t) => {
                  const count = counts[t.key];
                  const isSelected = count > 0;
                  const price = prices[t.key] || 0;
                  return (
                    <div key={t.key} style={{
                      border: isSelected ? `2px solid ${t.color}` : '1px solid #d1d1d6',
                      borderRadius: 14,
                      padding: '14px 14px',
                      background: isSelected ? `${t.color}0A` : '#fff',
                      transition: 'all 0.2s ease',
                    }}>
                      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 10 }}>
                        <div>
                          <span style={{
                            fontWeight: 600, fontSize: 14, color: isSelected ? t.color : '#1D1D1F',
                            letterSpacing: -0.2,
                          }}>{t.label}</span>
                          <span style={{ fontSize: 11, color: '#8e8e93', display: 'block', marginTop: 1 }}>{t.desc}</span>
                        </div>
                        {isSelected && <Check size={16} color={t.color} strokeWidth={3} />}
                      </div>

                      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 0 }}>
                          <button type="button" onClick={() => decrement(t.key)}
                            style={{
                              width: 40, height: 40, borderRadius: '10px 0 0 10px',
                              border: '1px solid #d1d1d6', background: count > 0 ? '#F5F5F7' : '#fafafa',
                              display: 'flex', alignItems: 'center', justifyContent: 'center',
                              cursor: 'pointer', color: '#3a3a3c',
                            }}>
                            <Minus size={16} />
                          </button>
                          <div style={{
                            width: 44, height: 40, display: 'flex', alignItems: 'center', justifyContent: 'center',
                            borderTop: '1px solid #d1d1d6', borderBottom: '1px solid #d1d1d6',
                            fontWeight: 700, fontSize: 18, color: isSelected ? t.color : '#3a3a3c',
                            background: '#fff',
                          }}>{count}</div>
                          <button type="button" onClick={() => increment(t.key)}
                            style={{
                              width: 40, height: 40, borderRadius: '0 10px 10px 0',
                              border: '1px solid #d1d1d6', background: '#F5F5F7',
                              display: 'flex', alignItems: 'center', justifyContent: 'center',
                              cursor: 'pointer', color: '#3a3a3c',
                            }}>
                            <Plus size={16} />
                          </button>
                        </div>

                        <span style={{
                          fontSize: 15, fontWeight: 700,
                          color: price > 0 ? '#1D1D1F' : '#8e8e93',
                        }}>
                          {price > 0 ? `$${price.toFixed(2)}` : 'FREE'}
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}

            {/* Order Summary */}
            {totalTickets > 0 && (
              <div style={{
                background: '#E8F1FC', borderRadius: 12, padding: '14px 18px',
                marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center',
              }}>
                <div>
                  <span style={{ fontWeight: 600, fontSize: 14, color: '#1D1D1F' }}>
                    {totalTickets} ticket{totalTickets > 1 ? 's' : ''}
                  </span>
                  {totalTickets > 1 && (
                    <span style={{ fontSize: 12, color: '#8e8e93', marginLeft: 8 }}>
                      <Users size={12} style={{ verticalAlign: -2, marginRight: 2 }} />
                      Group Order
                    </span>
                  )}
                  <div style={{ fontSize: 11, color: '#8e8e93', marginTop: 2 }}>
                    {selectedTypes.map((t) => `${counts[t.key]}x ${t.label}`).join(' + ')}
                  </div>
                </div>
                <span style={{ fontWeight: 700, fontSize: 22, color: '#0066CC', letterSpacing: -0.5 }}>
                  ${totalAmount.toFixed(2)}
                </span>
              </div>
            )}

            {/* Customer Info */}
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: 12, marginBottom: 16 }}>
              <div className="form-group" style={{ marginBottom: 0 }}>
                <label className="form-label" style={{ fontSize: 12 }}>Phone <span style={{ color: '#FF3B30' }}>*</span></label>
                <input className="form-input" value={patronPhone} onChange={(e) => setPatronPhone(e.target.value)} placeholder="+1..." required style={{ fontSize: 15, height: 44, padding: '0 14px' }} />
              </div>
              <div className="form-group" style={{ marginBottom: 0 }}>
                <label className="form-label" style={{ fontSize: 12 }}>Customer Name</label>
                <input className="form-input" value={patronName} onChange={(e) => setPatronName(e.target.value)} placeholder="Optional" style={{ fontSize: 15, height: 44, padding: '0 14px' }} />
              </div>
              <div className="form-group" style={{ marginBottom: 0 }}>
                <label className="form-label" style={{ fontSize: 12 }}>Email</label>
                <input className="form-input" type="email" value={patronEmail} onChange={(e) => setPatronEmail(e.target.value)} placeholder="Optional" style={{ fontSize: 15, height: 44, padding: '0 14px' }} />
              </div>
            </div>

            {error && <div style={{ color: 'var(--color-danger)', fontSize: 'var(--font-size-sm)', marginBottom: 12, fontWeight: 600 }}>{error}</div>}

            <button type="button" onClick={handleSubmit}
              className="btn btn-primary"
              style={{
                width: '100%', justifyContent: 'center', padding: '16px 0',
                fontSize: 18, minHeight: 56, fontWeight: 700, borderRadius: 12,
                letterSpacing: -0.3,
              }}
              disabled={loading || totalTickets === 0}>
              {loading ? 'Creating...' : totalTickets > 1
                ? `Create ${totalTickets} Tickets — $${totalAmount.toFixed(2)}`
                : totalTickets === 1
                  ? `Create Ticket — $${totalAmount.toFixed(2)}`
                  : 'Select Ticket Type'}
            </button>
          </>
        )}
      </div>
    </div>
  );
}
