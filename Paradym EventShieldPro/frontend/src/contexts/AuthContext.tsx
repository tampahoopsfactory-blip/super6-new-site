import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react'
import { authApi } from '../services/api'

interface User {
  id: number
  email: string
  first_name: string
  last_name: string
  roles: string[]
  tenant_id: number
  tenant_name: string
}

interface AuthContextType {
  user: User | null
  token: string | null
  login: (email: string, password: string, tenantSlug: string) => Promise<void>
  logout: () => void
  isLoading: boolean
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  const [token, setToken] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const savedToken = localStorage.getItem('esp_token')
    const savedUser = localStorage.getItem('esp_user')
    if (savedToken && savedUser) {
      setToken(savedToken)
      setUser(JSON.parse(savedUser))
    }
    setIsLoading(false)
  }, [])

  const login = async (email: string, password: string, tenantSlug: string) => {
    const res = await authApi.login(email, password, tenantSlug)
    const { access_token, user: userData } = res.data
    localStorage.setItem('esp_token', access_token)
    localStorage.setItem('esp_user', JSON.stringify(userData))
    setToken(access_token)
    setUser(userData)
  }

  const logout = () => {
    authApi.logout().catch(() => {})
    localStorage.removeItem('esp_token')
    localStorage.removeItem('esp_user')
    setToken(null)
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)
