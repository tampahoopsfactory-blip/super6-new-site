import axios from 'axios'

// Config is loaded from electron-store via IPC; falls back to localhost
async function getBaseUrl(): Promise<string> {
  try {
    const config = await (window as any).electronAPI?.getConfig?.()
    return config?.server?.apiUrl ?? 'http://localhost:5000'
  } catch {
    return 'http://localhost:5000'
  }
}

const api = axios.create({ timeout: 8000 })

// Attach JWT from localStorage (terminal stores it after login)
api.interceptors.request.use(async config => {
  const token = localStorage.getItem('esp_terminal_token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  if (!config.baseURL) config.baseURL = await getBaseUrl()
  return config
})

export default api

export const authApi = {
  login: async (email: string, password: string, tenantSlug: string) => {
    const base = await getBaseUrl()
    return axios.post(`${base}/api/v1/auth/login`, { email, password, tenant_slug: tenantSlug })
  },
}

export const analyticsApi = {
  dashboard: () => api.get('/api/v1/analytics/dashboard'),
}

export const ticketsApi = {
  list: (params?: Record<string, any>) => api.get('/api/v1/tickets/', { params }),
  create: (data: Record<string, any>) => api.post('/api/v1/tickets/', data),
  validate: (id: number) => api.post(`/api/v1/tickets/${id}/validate`),
  revoke: (id: number) => api.post(`/api/v1/tickets/${id}/revoke`),
}

export const accessApi = {
  logs: (params?: Record<string, any>) => api.get('/api/v1/access-control/logs', { params }),
  grant: (data: Record<string, any>) => api.post('/api/v1/access-control/grant', data),
  deny: (data: Record<string, any>) => api.post('/api/v1/access-control/deny', data),
  stats: () => api.get('/api/v1/access-control/stats'),
}

export const hardwareApi = {
  list: () => api.get('/api/v1/hardware/devices'),
  ping: (id: number) => api.post(`/api/v1/hardware/devices/${id}/ping`),
  command: (id: number, command: string) =>
    api.post(`/api/v1/hardware/devices/${id}/command`, { command }),
}
