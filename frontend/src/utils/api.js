const BASE = '/api';

function getHeaders() {
  const token = localStorage.getItem('token');
  const headers = { 'Content-Type': 'application/json' };
  if (token) headers['Authorization'] = `Bearer ${token}`;
  return headers;
}

async function request(path, options = {}) {
  const res = await fetch(`${BASE}${path}`, {
    ...options,
    headers: { ...getHeaders(), ...options.headers },
  });
  if (res.status === 401) {
    localStorage.removeItem('token');
    window.location.href = '/login';
    return null;
  }
  if (!res.ok) {
    const err = await res.json().catch(() => ({ detail: 'Request failed' }));
    throw new Error(err.detail || 'Request failed');
  }
  return res.json();
}

export const api = {
  // Auth
  login: (username, password) =>
    request('/auth/login', { method: 'POST', body: JSON.stringify({ username, password }) }),

  // Dashboard
  getStats: () => request('/dashboard/stats'),
  getRecentAccess: (limit = 20) => request(`/dashboard/recent-access?limit=${limit}`),
  getAccessLog: (params = {}) => {
    const q = new URLSearchParams(params).toString();
    return request(`/dashboard/access-log?${q}`);
  },
  getAlerts: () => request('/dashboard/alerts'),

  // Events
  getEvents: () => request('/events'),
  getActiveEvent: () => request('/events/active'),
  createEvent: (data) => request('/events', { method: 'POST', body: JSON.stringify(data) }),
  activateEvent: (id) => request(`/events/${id}/activate`, { method: 'POST' }),
  endEvent: (id) => request(`/events/${id}/end`, { method: 'POST' }),

  // Tickets
  getTickets: (params = {}) => {
    const q = new URLSearchParams(params).toString();
    return request(`/tickets?${q}`);
  },
  getTicket: (id) => request(`/tickets/${id}`),
  getTicketQR: (id) => request(`/tickets/${id}/qr`),
  createTicket: (data) => request('/tickets', { method: 'POST', body: JSON.stringify(data) }),
  createGroupOrder: (data) => request('/tickets/group', { method: 'POST', body: JSON.stringify(data) }),
  refundTicket: (id) => request(`/tickets/${id}/refund`, { method: 'POST' }),
  overrideTicket: (id) => request(`/tickets/${id}/override`, { method: 'POST' }),

  // Devices (X05)
  getDevices: () => request('/devices'),
  createDevice: (data) => request('/devices', { method: 'POST', body: JSON.stringify(data) }),
  updateDevice: (id, data) => request(`/devices/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
  deleteDevice: (id) => request(`/devices/${id}`, { method: 'DELETE' }),
  pingDevice: (id) => request(`/devices/${id}/ping`, { method: 'POST' }),
  regenerateDeviceToken: (id) => request(`/devices/${id}/regenerate-token`, { method: 'POST' }),

  // Settings
  getSettings: () => request('/settings'),
  updateSetting: (key, value) =>
    request('/settings', { method: 'PUT', body: JSON.stringify({ key, value }) }),
  getPricing: () => request('/settings/pricing'),
  updatePricing: (prices) =>
    request('/settings/pricing', { method: 'PUT', body: JSON.stringify(prices) }),
  testSms: () => request('/settings/test-sms', { method: 'POST' }),
  testEmail: () => request('/settings/test-email', { method: 'POST' }),

  // Health
  getHealth: () => fetch('/health').then((r) => r.json()),
};
