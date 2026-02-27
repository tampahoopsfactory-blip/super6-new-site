import { useState, useEffect, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Search, Download, QrCode, RotateCcw, ShieldCheck, X, Minus, Check, Users } from 'lucide-react';

const TICKET_TYPES = [
  { key: 'DAILY', label: 'Daily', color: '#006aff', desc: 'Single day' },
  { key: 'WEEKEND', label: 'Weekend', color: '#7c3aed', desc: 'Sat & Sun' },
  { key: 'KIDS', label: 'Kids Daily', color: '#f59e0b', desc: 'Single day (child)' },
  { key: 'KIDS_WEEKEND', label: 'Kids Weekend', color: '#d97706', desc: 'Sat & Sun (child)' },
  { key: 'STAFF', label: 'Staff', color: '#059669', desc: 'Never expires' },
];

const TYPE_COLORS = {
  DAILY: 'badge-info', WEEKEND: 'badge-info',
  KIDS: 'badge-warning', KIDS_WEEKEND: 'badge-warning',
  STAFF: 'badge-success',
};

const STATUS_COLORS = {
  ACTIVE: 'badge-success', EXPIRED: 'badge-danger',
  REFUNDED: 'badge-warning', INVALID: 'badge-danger',
};

export default function Tickets() {
  const [filter, setFilter] = useState({ page: '1', per_page: '50' });
  const [search, setSearch] = useState('');
  const [showCreate, setShowCreate] = useState(false);
  const [showQR, setShowQR] = useState(null);
  const [events, setEvents] = useState([]);

  const fetchTickets = useCallback(() => api.getTickets(filter), [filter]);
  const { data: tickets, loading, refetch } = usePolling(fetchTickets, 10000);

  useEffect(() => {
    api.getEvents().then(setEvents).catch(() => {});
  }, []);

  const activeEvent = events.find((e) => e.status === 'ACTIVE');

  const handleRefund = async (id) => {
    if (!confirm('Refund this ticket? This will revoke access.')) return;
    await api.refundTicket(id);
    refetch();
  };

  const handleOverride = async (id) => {
    await api.overrideTicket(id);
    refetch();
  };

  const handleViewQR = async (id) => {
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
      <div className="page-header">
        <h2>Tickets</h2>
        <div className="flex gap-2">
          <button className="btn btn-secondary btn-sm" onClick={handleExportCSV}>
            <Download size={14} /> Export CSV
          </button>
          <button className="btn btn-primary btn-sm" onClick={() => setShowCreate(true)}>
            <Plus size={14} /> New Order
          </button>
        </div>
      </div>

      <div className="page-body">
        {/* Filters */}
        <div className="card mb-6">
          <div className="card-body flex gap-3 flex-wrap items-center">
            <div style={{ flex: 1, minWidth: 200 }}>
              <div style={{ position: 'relative' }}>
                <Search size={16} style={{ position: 'absolute', left: 12, top: 10, color: 'var(--color-gray-400)' }} />
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
                        <button className="btn btn-secondary btn-sm" onClick={() => handleViewQR(t.ticket_id)} title="View QR"><QrCode size={12} /></button>
                        {t.status === 'ACTIVE' && (
                          <>
                            <button className="btn btn-secondary btn-sm" onClick={() => handleOverride(t.ticket_id)} title="Grant Access"><ShieldCheck size={12} /></button>
                            <button className="btn btn-danger btn-sm" onClick={() => handleRefund(t.ticket_id)} title="Refund"><RotateCcw size={12} /></button>
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
      </div>

      {/* QR Modal */}
      {showQR && (
        <div style={{
          position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)',
          display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000
        }} onClick={() => setShowQR(null)}>
          <div className="card" style={{ padding: 32, textAlign: 'center', maxWidth: 360 }} onClick={(e) => e.stopPropagation()}>
            <h3 style={{ marginBottom: 16 }}>Ticket QR Code</h3>
            <img src={`data:image/png;base64,${showQR.qr_base64}`} alt="QR Code" style={{ width: 250, height: 250 }} />
            <p className="text-xs text-muted" style={{ marginTop: 12, fontFamily: 'monospace' }}>{showQR.ticket_id}</p>
            <button className="btn btn-secondary mt-4" onClick={() => setShowQR(null)}>Close</button>
          </div>
        </div>
      )}

      {/* Create Order Modal */}
      {showCreate && (
        <CreateOrderModal
          events={events}
          activeEvent={activeEvent}
          onClose={() => setShowCreate(false)}
          onCreated={() => { setShowCreate(false); refetch(); }}
        />
      )}
    </>
  );
}


/* ============================================================
   CREATE ORDER MODAL
   Push-button ticket type selection with +/- quantity counters.
   Prices loaded from admin Settings (read-only here).
   ============================================================ */
function CreateOrderModal({ events, activeEvent, onClose, onCreated }) {
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
  const [eventId, setEventId] = useState(activeEvent?.event_id || '');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [result, setResult] = useState(null);

  useEffect(() => {
    api.getPricing().then((p) => {
      setPrices(p);
      setPricesLoaded(true);
    }).catch(() => {
      setPrices({ DAILY: 15, WEEKEND: 25, KIDS: 10, KIDS_WEEKEND: 18, STAFF: 0 });
      setPricesLoaded(true);
    });
  }, []);

  const increment = (key) => setCounts((c) => ({ ...c, [key]: c[key] + 1 }));
  const decrement = (key) => setCounts((c) => ({ ...c, [key]: Math.max(0, c[key] - 1) }));

  const totalTickets = Object.values(counts).reduce((a, b) => a + b, 0);
  const totalAmount = TICKET_TYPES.reduce((sum, t) => sum + counts[t.key] * (prices[t.key] || 0), 0);
  const selectedTypes = TICKET_TYPES.filter((t) => counts[t.key] > 0);

  const handleSubmit = async () => {
    if (totalTickets === 0) { setError('Add at least one ticket'); return; }
    if (!eventId) { setError('Select an event first'); return; }
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
      setTimeout(() => onCreated(), 1500);
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
      <div className="card" style={{ width: 520, maxHeight: '92vh', overflow: 'auto' }} onClick={(e) => e.stopPropagation()}>
        <div className="card-header">
          <h3>{totalTickets > 1 ? `Group Order (${totalTickets} tickets)` : 'New Ticket'}</h3>
          <button className="btn btn-secondary btn-sm" onClick={onClose}><X size={14} /></button>
        </div>
        <div className="card-body" style={{ padding: '16px 20px' }}>

          {/* Success state */}
          {result && (
            <div style={{ textAlign: 'center', padding: '32px 0' }}>
              <div style={{ width: 64, height: 64, borderRadius: '50%', background: '#e6f9ed', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px' }}>
                <Check size={32} color="#059669" />
              </div>
              <h3 style={{ marginBottom: 4 }}>{result.count} Ticket{result.count > 1 ? 's' : ''} Created</h3>
              <p className="text-muted">Total: ${result.total.toFixed(2)}</p>
            </div>
          )}

          {!result && (
            <>
              {/* Event select */}
              <div className="form-group" style={{ marginBottom: 16 }}>
                <label className="form-label">Event</label>
                <select className="form-input" value={eventId} onChange={(e) => setEventId(e.target.value)} required>
                  <option value="">Select event</option>
                  {events.map((ev) => <option key={ev.event_id} value={ev.event_id}>{ev.name} ({ev.status})</option>)}
                </select>
              </div>

              {/* Ticket Type Push Buttons */}
              <label className="form-label" style={{ marginBottom: 8 }}>Ticket Types</label>
              {!pricesLoaded ? (
                <p className="text-muted text-center" style={{ padding: 16 }}>Loading prices...</p>
              ) : (
                <div style={{ display: 'grid', gridTemplateColumns: 'repeat(2, 1fr)', gap: 8, marginBottom: 20 }}>
                  {TICKET_TYPES.map((t) => {
                    const count = counts[t.key];
                    const isSelected = count > 0;
                    const price = prices[t.key] || 0;
                    return (
                      <div key={t.key} style={{
                        border: `2px solid ${isSelected ? t.color : '#e5e7eb'}`,
                        borderRadius: 10,
                        padding: '10px 12px',
                        background: isSelected ? `${t.color}08` : '#fff',
                        transition: 'all 0.15s',
                      }}>
                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 6 }}>
                          <div>
                            <span style={{
                              fontWeight: 700, fontSize: 14, color: isSelected ? t.color : '#374151',
                            }}>{t.label}</span>
                            <span style={{ fontSize: 11, color: '#9ca3af', marginLeft: 6 }}>{t.desc}</span>
                          </div>
                          {isSelected && <Check size={16} color={t.color} strokeWidth={3} />}
                        </div>

                        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                          <div style={{ display: 'flex', alignItems: 'center', gap: 0 }}>
                            <button type="button" onClick={() => decrement(t.key)}
                              style={{
                                width: 32, height: 32, borderRadius: '8px 0 0 8px',
                                border: '1px solid #d1d5db', background: count > 0 ? '#f3f4f6' : '#f9fafb',
                                display: 'flex', alignItems: 'center', justifyContent: 'center',
                                cursor: 'pointer', color: '#374151',
                              }}>
                              <Minus size={14} />
                            </button>
                            <div style={{
                              width: 40, height: 32, display: 'flex', alignItems: 'center', justifyContent: 'center',
                              borderTop: '1px solid #d1d5db', borderBottom: '1px solid #d1d5db',
                              fontWeight: 700, fontSize: 16, color: isSelected ? t.color : '#374151',
                              background: '#fff',
                            }}>{count}</div>
                            <button type="button" onClick={() => increment(t.key)}
                              style={{
                                width: 32, height: 32, borderRadius: '0 8px 8px 0',
                                border: '1px solid #d1d5db', background: '#f3f4f6',
                                display: 'flex', alignItems: 'center', justifyContent: 'center',
                                cursor: 'pointer', color: '#374151',
                              }}>
                              <Plus size={14} />
                            </button>
                          </div>

                          <span style={{
                            fontSize: 15, fontWeight: 700,
                            color: price > 0 ? '#1a1a2e' : '#9ca3af',
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
                  background: '#f0f4ff', borderRadius: 10, padding: '12px 16px',
                  marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center'
                }}>
                  <div>
                    <span style={{ fontWeight: 700, fontSize: 14 }}>
                      {totalTickets} ticket{totalTickets > 1 ? 's' : ''}
                    </span>
                    {totalTickets > 1 && (
                      <span style={{ fontSize: 12, color: '#6b7280', marginLeft: 8 }}>
                        <Users size={12} style={{ verticalAlign: -2, marginRight: 2 }} />
                        Group Order
                      </span>
                    )}
                    <div style={{ fontSize: 11, color: '#6b7280', marginTop: 2 }}>
                      {selectedTypes.map((t) => `${counts[t.key]}x ${t.label}`).join(' + ')}
                    </div>
                  </div>
                  <span style={{ fontWeight: 800, fontSize: 20, color: '#006aff' }}>
                    ${totalAmount.toFixed(2)}
                  </span>
                </div>
              )}

              {/* Patron Info */}
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 8, marginBottom: 8 }}>
                <div className="form-group" style={{ marginBottom: 0 }}>
                  <label className="form-label" style={{ fontSize: 11 }}>Buyer Name</label>
                  <input className="form-input" value={patronName} onChange={(e) => setPatronName(e.target.value)} placeholder="Optional" style={{ fontSize: 13 }} />
                </div>
                <div className="form-group" style={{ marginBottom: 0 }}>
                  <label className="form-label" style={{ fontSize: 11 }}>Phone</label>
                  <input className="form-input" value={patronPhone} onChange={(e) => setPatronPhone(e.target.value)} placeholder="+1..." style={{ fontSize: 13 }} />
                </div>
              </div>
              <div className="form-group" style={{ marginBottom: 16 }}>
                <label className="form-label" style={{ fontSize: 11 }}>Email</label>
                <input className="form-input" type="email" value={patronEmail} onChange={(e) => setPatronEmail(e.target.value)} placeholder="Optional" style={{ fontSize: 13 }} />
              </div>

              {error && <div style={{ color: 'var(--color-danger)', fontSize: 'var(--font-size-sm)', marginBottom: 12 }}>{error}</div>}

              <button type="button" onClick={handleSubmit}
                className="btn btn-primary"
                style={{ width: '100%', justifyContent: 'center', padding: '14px 0', fontSize: 16 }}
                disabled={loading || totalTickets === 0}>
                {loading ? 'Creating...' : totalTickets > 1
                  ? `Create ${totalTickets} Tickets — $${totalAmount.toFixed(2)}`
                  : totalTickets === 1
                    ? `Create Ticket — $${totalAmount.toFixed(2)}`
                    : 'Select Ticket Type'}
              </button>

              <p className="text-xs text-muted" style={{ textAlign: 'center', marginTop: 8 }}>
                Prices are set by admin in Settings
              </p>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
