import React, { useState } from 'react'
import { Box, Card, TextField, Button, Typography, Alert, CircularProgress } from '@mui/material'
import { useAuth } from '../contexts/AuthContext'

const colors = {
  primary: '#007AFF',
  bg: '#F2F2F7',
  text: '#1D1D1F',
  textSec: '#86868B',
  surface: '#FFFFFF',
  error: '#FF3B30'
}

export default function LoginScreen() {
  const { login } = useAuth()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [tenantSlug, setTenantSlug] = useState('demo')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      await login(email, password, tenantSlug)
    } catch (err: any) {
      setError(err.response?.data?.error ?? 'Login failed. Check your credentials.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box sx={{ minHeight: '100vh', bgcolor: colors.bg, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <Card sx={{ p: 5, width: '100%', maxWidth: 400, borderRadius: 3, boxShadow: '0 4px 24px rgba(0,0,0,0.08)' }}>
        <Typography variant="h5" sx={{ fontWeight: 700, color: colors.text, mb: 0.5 }}>EventShield Pro</Typography>
        <Typography sx={{ color: colors.textSec, fontSize: 13, mb: 4 }}>Sign in to your account</Typography>
        {error && <Alert severity="error" sx={{ mb: 2, fontSize: 12 }}>{error}</Alert>}
        <form onSubmit={handleSubmit}>
          <TextField fullWidth label="Email" type="email" value={email} onChange={e => setEmail(e.target.value)}
            required sx={{ mb: 2 }} />
          <TextField fullWidth label="Password" type="password" value={password} onChange={e => setPassword(e.target.value)}
            required sx={{ mb: 2 }} />
          <TextField fullWidth label="Organization" value={tenantSlug} onChange={e => setTenantSlug(e.target.value)}
            helperText="Your organization slug (e.g. 'demo')" sx={{ mb: 3 }} />
          <Button fullWidth type="submit" variant="contained" disabled={loading}
            sx={{ bgcolor: colors.primary, py: 1.5, fontWeight: 600, fontSize: 15 }}>
            {loading ? <CircularProgress size={20} color="inherit" /> : 'Sign In'}
          </Button>
        </form>
      </Card>
    </Box>
  )
}
