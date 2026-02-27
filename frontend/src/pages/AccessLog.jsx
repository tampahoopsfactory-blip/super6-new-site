import { useState, useCallback } from 'react';
import { api } from '../utils/api';
import { usePolling } from '../hooks/usePolling';
import { Download, Filter } from 'lucide-react';

const TYPE_COLORS = {
  QR_ENTRY: 'badge-success',
  FACE_ENTRY: 'badge-success',
  QR_DENIED: 'badge-danger',
  FACE_DENIED: 'badge-danger',
  MANUAL_OVERRIDE: 'badge-info',
};

export default function AccessLogPage() {
  const [filter, setFilter] = useState({ page: '1', per_page: '100' });

  const fetchLogs = useCallback(() => api.getAccessLog(filter), [filter]);
  const { data: logs, loading } = usePolling(fetchLogs, 10000);

  const handleExportCSV = () => {
    if (!logs?.length) return;
    const headers = ['Timestamp', 'Gate', 'Event Type', 'Result', 'Reason', 'Confidence', 'Ticket ID'];
    const rows = logs.map((l) => [
      new Date(l.timestamp).toLocaleString(),
      l.gate_id,
      l.event_type,
      l.event_type.includes('ENTRY') || l.event_type === 'MANUAL_OVERRIDE' ? 'GRANTED' : 'DENIED',
      l.reason_code || '',
      l.confidence_score || '',
      l.ticket_id ? `...${l.ticket_id.slice(-8)}` : '',
    ]);
    const csv = [headers, ...rows].map((r) => r.join(',')).join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'access-log.csv';
    a.click();
  };

  return (
    <>
      <div className="page-header">
        <h2>Access Log</h2>
        <button className="btn btn-secondary btn-sm" onClick={handleExportCSV}>
          <Download size={14} /> Export CSV
        </button>
      </div>

      <div className="page-body">
        {/* Filters */}
        <div className="card mb-6">
          <div className="card-body flex gap-3 flex-wrap items-center">
            <Filter size={16} color="var(--color-gray-400)" />
            <select
              className="form-input"
              style={{ width: 160 }}
              value={filter.gate || ''}
              onChange={(e) => setFilter({ ...filter, gate: e.target.value || undefined })}
            >
              <option value="">All Gates</option>
              <option value="gate_01">Gate 1</option>
              <option value="gate_02">Gate 2</option>
              <option value="gate_03">Gate 3</option>
              <option value="MANUAL">Manual</option>
            </select>
            <select
              className="form-input"
              style={{ width: 160 }}
              value={filter.event_type || ''}
              onChange={(e) => setFilter({ ...filter, event_type: e.target.value || undefined })}
            >
              <option value="">All Events</option>
              <option value="QR_ENTRY">QR Entry</option>
              <option value="FACE_ENTRY">Face Entry</option>
              <option value="QR_DENIED">QR Denied</option>
              <option value="FACE_DENIED">Face Denied</option>
              <option value="MANUAL_OVERRIDE">Manual Override</option>
            </select>
          </div>
        </div>

        {/* Table */}
        <div className="card">
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Timestamp</th>
                  <th>Gate</th>
                  <th>Event Type</th>
                  <th>Result</th>
                  <th>Reason</th>
                  <th>Confidence</th>
                  <th>Ticket</th>
                </tr>
              </thead>
              <tbody>
                {loading && !logs && (
                  <tr><td colSpan={7} className="text-center text-muted" style={{ padding: 32 }}>Loading...</td></tr>
                )}
                {logs?.length === 0 && (
                  <tr><td colSpan={7} className="text-center text-muted" style={{ padding: 32 }}>No access events recorded</td></tr>
                )}
                {logs?.map((l) => {
                  const granted = l.event_type.includes('ENTRY') || l.event_type === 'MANUAL_OVERRIDE';
                  return (
                    <tr key={l.log_id}>
                      <td className="text-xs" style={{ fontVariantNumeric: 'tabular-nums' }}>
                        {new Date(l.timestamp).toLocaleString()}
                      </td>
                      <td>{l.gate_id}</td>
                      <td><span className={`badge ${TYPE_COLORS[l.event_type] || 'badge-neutral'}`}>{l.event_type}</span></td>
                      <td><span className={`badge ${granted ? 'badge-success' : 'badge-danger'}`}>{granted ? 'GRANTED' : 'DENIED'}</span></td>
                      <td className="text-muted">{l.reason_code || '—'}</td>
                      <td>{l.confidence_score ? `${(l.confidence_score * 100).toFixed(0)}%` : '—'}</td>
                      <td style={{ fontFamily: 'monospace', fontSize: 'var(--font-size-xs)' }}>
                        {l.ticket_id ? `...${l.ticket_id.slice(-8)}` : '—'}
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </>
  );
}
