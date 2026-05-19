import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react'
import { authApi } from '../services/api'
import { storage } from '../services/storage'

interface User {
  id: number
  email: string
  first_name: string
  last_name: string
  roles: string[]
  tenant_id: number
}

interface AuthContextType {
  user: User | null
  isLoading: boolean
  login: (email: string, password: string, tenantSlug: string) => Promise<void>
  logout: () => Promise<void>
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    storage.getJSON<User>('esp_user').then(u => {
      setUser(u)
      setIsLoading(false)
    })
  }, [])

  const login = async (email: string, password: string, tenantSlug: string) => {
    const res = await authApi.login(email, password, tenantSlug)
    const { access_token, user: userData } = res.data
    await storage.set('esp_token', access_token)
    await storage.setJSON('esp_user', userData)
    setUser(userData)
  }

  const logout = async () => {
    authApi.logout().catch(() => {})
    await storage.remove('esp_token')
    await storage.remove('esp_user')
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)
