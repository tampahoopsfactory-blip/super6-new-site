import { useState, useEffect, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Plus, Play, Square, X, Calendar, Clock, Users, DollarSign } from 'lucide-react';

const STATUS_COLORS = {
  UPCOMING: 'badge-info',
  ACTIVE: 'badge-success',
  ENDED: 'badge-neutral',
};

export default function Events() {
  const fetchEvents = useCallback(() => api.getEvents(), []);
  const { data: events, loading, refetch } = usePolling(fetchEvents, 15000);
  const [showCreate, setShowCreate] = useState(false);

  const handleActivate = async (id) => {
    if (!confirm('Activate this event? Any other active event will be ended.')) return;
    await api.activateEvent(id);
    refetch();
  };

  const handleEnd = async (id) => {
    if (!confirm('End this event? All active tickets will be expired.')) return;
    await api.endEvent(id);
    refetch();
  };

  const hasEvents = events && events.length > 0;

  return (
    <>
      <div className="page-header">
        <h2>Events</h2>
        <button className="btn btn-primary btn-sm" onClick={() => setShowCreate(true)}>
          <Plus size={14} /> New Event
        </button>
      </div>

      <div className="page-body">
        {/* Empty State */}
        {!loading && !hasEvents && (
          <div style={{
            textAlign: 'center',
            padding: '80px 24px',
            background: '#fff',
            borderRadius: 16,
            border: '2px dashed #d1d5db',
          }}>
            <div style={{
              width: 80, height: 80, borderRadius: '50%',
              background: '#e6f0ff', display: 'flex',
              alignItems: 'center', justifyContent: 'center',
              margin: '0 auto 20px',
            }}>
              <Calendar size={36} color="#006aff" />
            </div>
            <h2 style={{ fontSize: 22, fontWeight: 700, marginBottom: 8, color: '#1a1a2e' }}>
              No Events Yet
            </h2>
            <p style={{ color: '#6b7280', fontSize: 15, marginBottom: 28, maxWidth: 400, margin: '0 auto 28px' }}>
              Create your first event to start selling tickets and managing access control at your venue.
            </p>
            <button
              className="btn btn-primary btn-lg"
              onClick={() => setShowCreate(true)}
              style={{ padding: '14px 32px', fontSize: 16, borderRadius: 10 }}
            >
              <Plus size={18} /> Create Your First Event
            </button>
          </div>
        )}

        {/* Loading */}
        {loading && !events && (
          <div className="card" style={{ padding: 48, textAlign: 'center' }}>
            <p className="text-muted">Loading events...</p>
          </div>
        )}

        {/* Events List */}
        {hasEvents && (
          <div style={{ display: 'grid', gap: 16 }}>
            {events.map((ev) => (
              <div key={ev.event_id} className="card" style={{
                borderLeft: ev.status === 'ACTIVE' ? '4px solid #00d4aa' :
                            ev.status === 'UPCOMING' ? '4px solid #006aff' : '4px solid #e2e8f0',
              }}>
                <div style={{ padding: '20px 24px', display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: 16 }}>
                  {/* Left: Event info */}
                  <div style={{ flex: 1, minWidth: 240 }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 10, marginBottom: 8 }}>
                      <h3 style={{ fontSize: 17, fontWeight: 700, color: '#1a1a2e', margin: 0 }}>{ev.name}</h3>
                      <span className={`badge ${STATUS_COLORS[ev.status]}`}>{ev.status}</span>
                    </div>
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: 20, fontSize: 13, color: '#6b7280' }}>
                      <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                        <Calendar size={14} /> {ev.event_date}
                      </span>
                      <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                        <Clock size={14} />
                        {new Date(ev.start_time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                        {' — '}
                        {new Date(ev.end_time).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                      </span>
                      <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                        <Users size={14} /> {ev.current_capacity}/{ev.max_capacity || '∞'}
                      </span>
                      <span style={{ display: 'flex', alignItems: 'center', gap: 5 }}>
                        <DollarSign size={14} /> ${Number(ev.admission_price).toFixed(2)}
                      </span>
                    </div>
                  </div>

                  {/* Right: Actions */}
                  <div className="flex gap-2">
                    {ev.status === 'UPCOMING' && (
                      <button className="btn btn-primary btn-sm" onClick={() => handleActivate(ev.event_id)}
                        style={{ padding: '8px 16px' }}>
                        <Play size={14} /> Activate
                      </button>
                    )}
                    {ev.status === 'ACTIVE' && (
                      <button className="btn btn-danger btn-sm" onClick={() => handleEnd(ev.event_id)}
                        style={{ padding: '8px 16px' }}>
                        <Square size={14} /> End Event
                      </button>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {showCreate && (
        <CreateEventModal onClose={() => setShowCreate(false)} onCreated={() => { setShowCreate(false); refetch(); }} />
      )}
    </>
  );
}


/* ============================================================
   CREATE EVENT MODAL
   ============================================================ */
function CreateEventModal({ onClose, onCreated }) {
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

  const handleSubmit = async (e) => {
    e.preventDefault();
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
      onCreated();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const set = (key, val) => setForm((f) => ({ ...f, [key]: val }));

  return (
    <div style={{
      position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)',
      display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000
    }} onClick={onClose}>
      <div className="card" style={{ width: 520, maxHeight: '92vh', overflow: 'auto' }} onClick={(e) => e.stopPropagation()}>
        <div className="card-header">
          <h3>Create New Event</h3>
          <button className="btn btn-secondary btn-sm" onClick={onClose}><X size={14} /></button>
        </div>
        <div className="card-body" style={{ padding: '20px 24px' }}>
          <form onSubmit={handleSubmit}>
            {/* Event Name */}
            <div className="form-group">
              <label className="form-label">Event Name</label>
              <input className="form-input" value={form.name}
                onChange={(e) => set('name', e.target.value)}
                placeholder="e.g. Lakers vs Warriors — Friday Night" required
                autoFocus
              />
            </div>

            {/* Date + Capacity */}
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">Event Date</label>
                <input className="form-input" type="date" value={form.event_date}
                  onChange={(e) => set('event_date', e.target.value)} required />
              </div>
              <div className="form-group">
                <label className="form-label">Max Capacity</label>
                <input className="form-input" type="number" value={form.max_capacity}
                  onChange={(e) => set('max_capacity', parseInt(e.target.value) || 0)} />
              </div>
            </div>

            {/* Start + End Time */}
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">Doors Open</label>
                <input className="form-input" type="time" value={form.start_time}
                  onChange={(e) => set('start_time', e.target.value)} required />
              </div>
              <div className="form-group">
                <label className="form-label">Event Ends</label>
                <input className="form-input" type="time" value={form.end_time}
                  onChange={(e) => set('end_time', e.target.value)} required />
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
            <div className="grid-2">
              <div className="form-group">
                <label className="form-label">Re-Entry Allowed?</label>
                <div style={{ display: 'flex', gap: 8 }}>
                  <button type="button" onClick={() => set('multi_entry', true)}
                    style={{
                      flex: 1, padding: '10px 0', borderRadius: 8, fontWeight: 600, fontSize: 13,
                      border: form.multi_entry ? '2px solid #006aff' : '2px solid #e5e7eb',
                      background: form.multi_entry ? '#e6f0ff' : '#fff',
                      color: form.multi_entry ? '#006aff' : '#6b7280',
                      cursor: 'pointer',
                    }}>
                    Yes
                  </button>
                  <button type="button" onClick={() => set('multi_entry', false)}
                    style={{
                      flex: 1, padding: '10px 0', borderRadius: 8, fontWeight: 600, fontSize: 13,
                      border: !form.multi_entry ? '2px solid #006aff' : '2px solid #e5e7eb',
                      background: !form.multi_entry ? '#e6f0ff' : '#fff',
                      color: !form.multi_entry ? '#006aff' : '#6b7280',
                      cursor: 'pointer',
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

            {error && <div style={{ color: 'var(--color-danger)', fontSize: 'var(--font-size-sm)', marginBottom: 12, fontWeight: 600 }}>{error}</div>}

            <button type="submit" className="btn btn-primary"
              style={{ width: '100%', justifyContent: 'center', padding: '14px 0', fontSize: 16, marginTop: 8 }}
              disabled={loading}>
              {loading ? 'Creating...' : 'Create Event'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
