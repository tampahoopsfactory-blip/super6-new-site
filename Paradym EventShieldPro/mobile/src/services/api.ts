import axios from 'axios'
import { storage } from './storage'

const BASE_URL = 'http://localhost:5000/api/v1'

const api = axios.create({ baseURL: BASE_URL, timeout: 10000 })

api.interceptors.request.use(async config => {
  const token = await storage.get('esp_token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

export default api

export const authApi = {
  login: (email: string, password: string, tenantSlug: string) =>
    api.post('/auth/login', { email, password, tenant_slug: tenantSlug }),
  me: () => api.get('/auth/me'),
  logout: () => api.post('/auth/logout'),
}

export const ticketsApi = {
  list: (params?: Record<string, any>) => api.get('/tickets/', { params }),
  get: (id: number) => api.get(`/tickets/${id}`),
  validate: (id: number) => api.post(`/tickets/${id}/validate`),
}

export const analyticsApi = {
  dashboard: () => api.get('/analytics/dashboard'),
}

export const accessApi = {
  logs: (params?: Record<string, any>) => api.get('/access-control/logs', { params }),
  grant: (data: Record<string, any>) => api.post('/access-control/grant', data),
}
