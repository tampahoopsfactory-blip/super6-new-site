import axios from 'axios'

const api = axios.create({ baseURL: '/api/v1' })

// Attach JWT token from localStorage to every request
api.interceptors.request.use(config => {
  const token = localStorage.getItem('esp_token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// On 401, clear token and redirect to login
api.interceptors.response.use(
  res => res,
  err => {
    if (err.response?.status === 401) {
      localStorage.removeItem('esp_token')
      localStorage.removeItem('esp_user')
      window.location.href = '/login'
    }
    return Promise.reject(err)
  }
)

export default api

// Auth
export const authApi = {
  login: (email: string, password: string, tenantSlug: string) =>
    api.post('/auth/login', { email, password, tenant_slug: tenantSlug }),
  me: () => api.get('/auth/me'),
  logout: () => api.post('/auth/logout'),
}

// Tickets
export const ticketsApi = {
  list: (params?: Record<string, any>) => api.get('/tickets/', { params }),
  create: (data: Record<string, any>) => api.post('/tickets/', data),
  get: (id: number) => api.get(`/tickets/${id}`),
  validate: (id: number) => api.post(`/tickets/${id}/validate`),
  revoke: (id: number) => api.post(`/tickets/${id}/revoke`),
}

// Access control
export const accessApi = {
  logs: (params?: Record<string, any>) => api.get('/access-control/logs', { params }),
  grant: (data: Record<string, any>) => api.post('/access-control/grant', data),
  deny: (data: Record<string, any>) => api.post('/access-control/deny', data),
  stats: () => api.get('/access-control/stats'),
}

// Analytics
export const analyticsApi = {
  dashboard: () => api.get('/analytics/dashboard'),
}

// Hardware
export const hardwareApi = {
  list: () => api.get('/hardware/devices'),
  register: (data: Record<string, any>) => api.post('/hardware/devices', data),
  ping: (id: string) => api.post(`/hardware/devices/${id}/ping`),
  command: (id: string, command: string) => api.post(`/hardware/devices/${id}/command`, { command }),
}

// Users
export const usersApi = {
  list: () => api.get('/users/'),
  search: (q: string) => api.get('/users/', { params: { search: q } }),
}
