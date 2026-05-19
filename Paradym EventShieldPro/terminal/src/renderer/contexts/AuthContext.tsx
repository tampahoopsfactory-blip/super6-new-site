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
  isLoading: boolean
  login: (email: string, password: string, tenantSlug: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  const [token, setToken] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const savedToken = localStorage.getItem('esp_terminal_token')
    const savedUser = localStorage.getItem('esp_terminal_user')
    if (savedToken && savedUser) {
      setToken(savedToken)
      try { setUser(JSON.parse(savedUser)) } catch { /* corrupted */ }
    }
    setIsLoading(false)
  }, [])

  const login = async (email: string, password: string, tenantSlug: string) => {
    const res = await authApi.login(email, password, tenantSlug)
    const { access_token, user: userData } = res.data
    localStorage.setItem('esp_terminal_token', access_token)
    localStorage.setItem('esp_terminal_user', JSON.stringify(userData))
    setToken(access_token)
    setUser(userData)
  }

  const logout = () => {
    localStorage.removeItem('esp_terminal_token')
    localStorage.removeItem('esp_terminal_user')
    setToken(null)
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, token, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export const useAuth = () => useContext(AuthContext)
