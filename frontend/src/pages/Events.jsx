import { useState, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Play, Square, Calendar, Clock, Users, DollarSign, Check, ChevronDown, ChevronUp } from 'lucide-react';

const STATUS_COLORS = {
  UPCOMING: 'badge-info',
  ACTIVE: 'badge-success',
  ENDED: 'badge-neutral',
};

export default function Events() {
  const fetchEvents = useCallback(() => api.getEvents(), []);
  const { data: events, loading, refetch } = usePolling(fetchEvents, 15000);
  const [showCreate, setShowCreate] = useState(true);
  const [showList, setShowList] = useState(false);
  const [confirmAction, setConfirmAction] = useState(null); // { type: 'activate'|'end', id, name }

  const handleActivate = (ev) => {
    setConfirmAction({ type: 'activate', id: ev.event_id, name: ev.name });
  };

  const handleEnd = (ev) => {
    setConfirmAction({ type: 'end', id: ev.event_id, name: ev.name });
  };

  const executeConfirm = async () => {
    if (!confirmAction) return;
    if (confirmAction.type === 'activate') {
      await api.activateEvent(confirmAction.id);
    } else {
      await api.endEvent(confirmAction.id);
    }
    setConfirmAction(null);
    refetch();
  };

  const hasEvents = events && events.length > 0;

  return (
    <>
      {/* ====== INLINE CREATE EVENT FORM — always visible ====== */}
      <div className="page-body" style={{ paddingBottom: 0 }}>
        <InlineCreateEventForm
          show={showCreate}
          onToggle={() => setShowCreate(!showCreate)}
          onCreated={refetch}
        />
      </div>

      {/* ====== INLINE CONFIRM ACTION ====== */}
      {confirmAction && (
        <div className="page-body" style={{ paddingTop: 12, paddingBottom: 0 }}>
          <div className="card" style={{
            textAlign: 'center', padding: 24,
            border: `2px solid ${confirmAction.type === 'activate' ? '#5AC8FA' : '#ef4444'}`,
            borderRadius: 14,
            background: confirmAction.type === 'activate' ? '#EAF6FE' : '#fef2f2',
          }}>
            <h3 style={{ color: confirmAction.type === 'activate' ? '#5AC8FA' : '#dc2626', marginBottom: 8, fontSize: 18 }}>
              {confirmAction.type === 'activate' ? 'Activate Event?' : 'End Event?'}
            </h3>
            <p style={{ fontSize: 15, color: '#6b7280', marginBottom: 16 }}>
              {confirmAction.type === 'activate'
                ? `Activate "${confirmAction.name}"? Any other active event will be ended.`
                : `End "${confirmAction.name}"? All active tickets will be expired.`}
            </p>
            <div style={{ display: 'flex', gap: 12, justifyContent: 'center' }}>
              <button className="btn btn-secondary" onClick={() => setConfirmAction(null)}
                style={{ padding: '12px 28px', fontSize: 16 }}>
                Cancel
              </button>
              <button
                className={confirmAction.type === 'activate' ? 'btn btn-primary' : 'btn btn-danger'}
                onClick={executeConfirm}
                style={{ padding: '12px 28px', fontSize: 16 }}
              >
                {confirmAction.type === 'activate' ? 'Yes, Activate' : 'Yes, End Event'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* ====== EVENT LIST (collapsible) ====== */}
      <div className="page-body" style={{ paddingTop: 12 }}>
        <button
          className="btn btn-secondary"
          onClick={() => setShowList(!showList)}
          style={{ width: '100%', justifyContent: 'center', padding: '12px 0', fontSize: 15, marginBottom: 12 }}
        >
          {showList ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
          {showList ? ' Hide Events' : ` Show Events (${events?.length || 0})`}
        </button>

        {showList && (
          <>
            {loading && !events && (
              <div className="card" style={{ padding: 48, textAlign: 'center' }}>
                <p className="text-muted">Loading events...</p>
              </div>
            )}

            {!loading && !hasEvents && (
              <div className="card" style={{ textAlign: 'center', padding: 48 }}>
                <Calendar size={36} color="#5AC8FA" style={{ margin: '0 auto 16px' }} />
                <p className="text-muted">No events yet — create one above</p>
              </div>
            )}

            {hasEvents && (
              <div style={{ display: 'grid', gap: 16 }}>
                {events.map((ev) => (
                  <div key={ev.event_id} className="card" style={{
                    borderLeft: ev.status === 'ACTIVE' ? '4px solid #00d4aa' :
                                ev.status === 'UPCOMING' ? '4px solid #5AC8FA' : '4px solid #e2e8f0',
                  }}>
                    <div style={{ padding: '20px 24px', display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: 16 }}>
                      <div style={{ flex: 1, minWidth: 240 }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 8 }}>
                          <h3 style={{ fontSize: 17, fontWeight: 700, color: '#1a1a2e', margin: 0 }}>{ev.name}</h3>
                          <span className={`badge ${STATUS_COLORS[ev.status]}`}>{ev.status}</span>
                        </div>
                        <div style={{ display: 'flex', flexWrap: 'wrap', gap: 20, fontSize: 14, color: '#6b7280' }}>
                          <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                            <Calendar size={16} /> {ev.event_date}
                          </span>
                          <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                            <Clock size={16} />
                            {new Date(ev.start_time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                            {' — '}
                            {new Date(ev.end_time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                          </span>
                          <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                            <Users size={16} /> {ev.current_capacity}/{ev.max_capacity || '∞'}
                          </span>
                          <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                            <DollarSign size={16} /> ${Number(ev.admission_price).toFixed(2)}
                          </span>
                        </div>
                      </div>
                      <div className="flex gap-2">
                        {ev.status === 'UPCOMING' && (
                          <button className="btn btn-primary" onClick={() => handleActivate(ev)}
                            style={{ padding: '12px 20px', fontSize: 15 }}>
                            <Play size={16} /> Activate
                          </button>
                        )}
                        {ev.status === 'ACTIVE' && (
                          <button className="btn btn-danger" onClick={() => handleEnd(ev)}
                            style={{ padding: '12px 20px', fontSize: 15 }}>
                            <Square size={16} /> End Event
                          </button>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </>
        )}
      </div>
    </>
  );
}


/* ============================================================
   INLINE CREATE EVENT FORM — renders directly on page, no popup
   ============================================================ */
function InlineCreateEventForm({ show, onToggle, onCreated }) {
  const [form, setForm] = useState({
    name: '',
    event_date: new Date().toISOString().split('T')[0],
    start_time: '',
    end_time: '',
    admission_price: 15,
    max_capacity: 500,
    multi_entry: true,
    ticket_expiry_hours: 24,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [result, setResult] = useState(null);

  const resetForm = () => {
    setForm({
      name: '',
      event_date: new Date().toISOString().split('T')[0],
      start_time: '',
      end_time: '',
      admission_price: 15,
      max_capacity: 500,
      multi_entry: true,
      ticket_expiry_hours: 24,
    });
    setError('');
    setResult(null);
  };

  const handleSubmit = async () => {
    if (!form.name.trim()) { setError('Event name is required'); return; }
    if (!form.start_time) { setError('Start time is required'); return; }
    if (!form.end_time) { setError('End time is required'); return; }
    setLoading(true);
    setError('');
    try {
      const payload = {
        ...form,
        start_time: new Date(`${form.event_date}T${form.start_time}`).toISOString(),
        end_time: new Date(`${form.event_date}T${form.end_time}`).toISOString(),
      };
      await api.createEvent(payload);
      setResult({ name: form.name });
      onCreated();
      setTimeout(() => resetForm(), 2000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const set = (key, val) => setForm((f) => ({ ...f, [key]: val }));

  return (
    <div className="card" style={{ border: '2px solid #5AC8FA', borderRadius: 16 }}>
      <div
        style={{
          background: '#5AC8FA', color: '#fff', padding: '14px 24px',
          borderRadius: show ? '14px 14px 0 0' : 14,
          textAlign: 'center', cursor: 'pointer',
          display: 'flex', alignItems: 'center', justifyContent: 'center', gap: 8,
        }}
        onClick={onToggle}
      >
        <h2 style={{ margin: 0, fontSize: 22, fontWeight: 800, letterSpacing: 0.5 }}>New Event</h2>
        {show ? <ChevronUp size={20} /> : <ChevronDown size={20} />}
      </div>

      {show && (
        <div style={{ padding: '20px 28px' }}>
          {/* Success state */}
          {result && (
            <div style={{ textAlign: 'center', padding: '24px 0' }}>
              <div style={{ width: 64, height: 64, borderRadius: '50%', background: '#e6f9ed', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 16px' }}>
                <Check size={32} color="#059669" />
              </div>
              <h3 style={{ marginBottom: 4 }}>Event Created!</h3>
              <p className="text-muted">"{result.name}" is ready</p>
              <button className="btn btn-primary" onClick={resetForm} style={{ marginTop: 12, fontSize: 16, padding: '12px 32px' }}>
                <Plus size={16} /> Create Another
              </button>
            </div>
          )}

          {!result && (
            <>
              {/* Event Name */}
              <div className="form-group">
                <label className="form-label">Event Name</label>
                <input className="form-input" value={form.name}
                  onChange={(e) => set('name', e.target.value)}
                  placeholder="e.g. Lakers vs Warriors — Friday Night"
                />
              </div>

              {/* Date + Capacity */}
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">Event Date</label>
                  <input className="form-input" type="date" value={form.event_date}
                    onChange={(e) => set('event_date', e.target.value)} />
                </div>
                <div className="form-group">
                  <label className="form-label">Max Capacity</label>
                  <input className="form-input" type="number" value={form.max_capacity}
                    onChange={(e) => set('max_capacity', parseInt(e.target.value) || 0)} />
                </div>
              </div>

              {/* Start + End Time */}
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">Doors Open</label>
                  <input className="form-input" type="time" value={form.start_time}
                    onChange={(e) => set('start_time', e.target.value)} />
                </div>
                <div className="form-group">
                  <label className="form-label">Event Ends</label>
                  <input className="form-input" type="time" value={form.end_time}
                    onChange={(e) => set('end_time', e.target.value)} />
                </div>
              </div>

              {/* Pricing */}
              <div className="form-group">
                <label className="form-label">Default Admission Price ($)</label>
                <input className="form-input" type="number" step="0.01" value={form.admission_price}
                  onChange={(e) => set('admission_price', parseFloat(e.target.value) || 0)}
                  style={{ maxWidth: 200 }} />
                <p className="text-xs text-muted" style={{ marginTop: 4 }}>
                  Per-ticket-type pricing is configured in Settings.
                </p>
              </div>

              {/* Rules */}
              <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
                <div className="form-group">
                  <label className="form-label">Re-Entry Allowed?</label>
                  <div style={{ display: 'flex', gap: 8 }}>
                    <button type="button" onClick={() => set('multi_entry', true)}
                      style={{
                        flex: 1, padding: '12px 0', borderRadius: 10, fontWeight: 700, fontSize: 15,
                        border: form.multi_entry ? '2px solid #5AC8FA' : '2px solid #e5e7eb',
                        background: form.multi_entry ? '#EAF6FE' : '#fff',
                        color: form.multi_entry ? '#5AC8FA' : '#6b7280',
                        cursor: 'pointer', minHeight: 44,
                      }}>
                      Yes
                    </button>
                    <button type="button" onClick={() => set('multi_entry', false)}
                      style={{
                        flex: 1, padding: '12px 0', borderRadius: 10, fontWeight: 700, fontSize: 15,
                        border: !form.multi_entry ? '2px solid #5AC8FA' : '2px solid #e5e7eb',
                        background: !form.multi_entry ? '#EAF6FE' : '#fff',
                        color: !form.multi_entry ? '#5AC8FA' : '#6b7280',
                        cursor: 'pointer', minHeight: 44,
                      }}>
                      No
                    </button>
                  </div>
                </div>
                <div className="form-group">
                  <label className="form-label">Ticket Expiry (hours)</label>
                  <input className="form-input" type="number" value={form.ticket_expiry_hours}
                    onChange={(e) => set('ticket_expiry_hours', parseInt(e.target.value) || 24)} />
                </div>
              </div>

              {error && <div style={{ color: 'var(--color-danger)', fontSize: 14, marginBottom: 12, fontWeight: 600 }}>{error}</div>}

              <button type="button" onClick={handleSubmit}
                className="btn btn-primary"
                style={{ width: '100%', justifyContent: 'center', padding: '16px 0', fontSize: 18, minHeight: 56, fontWeight: 800 }}
                disabled={loading}>
                {loading ? 'Creating...' : 'Create Event'}
              </button>
            </>
          )}
        </div>
      )}
    </div>
  );
}
