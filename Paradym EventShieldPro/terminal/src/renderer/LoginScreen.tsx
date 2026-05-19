import React, { useState } from 'react'
import { Box, Card, TextField, Button, Typography, Alert, CircularProgress } from '@mui/material'
import { useAuth } from './contexts/AuthContext'

const colors = { primary: '#007AFF', bg: '#1C1C1E', surface: '#2C2C2E', text: '#FFFFFF', textSec: '#AEAEB2', border: '#48484A' }

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
      setError(err.response?.data?.error ?? 'Login failed — check credentials and API URL in Settings.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box sx={{ minHeight: '100vh', bgcolor: colors.bg, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <Card sx={{ p: 5, width: '100%', maxWidth: 380, borderRadius: 3, bgcolor: colors.surface, border: `1px solid ${colors.border}` }}>
        <Typography sx={{ fontSize: 20, fontWeight: 700, color: colors.text, mb: 0.5 }}>
          EventShield Pro
        </Typography>
        <Typography sx={{ fontSize: 12, color: colors.textSec, mb: 4 }}>Terminal — Sign in to continue</Typography>

        {error && <Alert severity="error" sx={{ mb: 2, fontSize: 12 }}>{error}</Alert>}

        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth label="Email" type="email" value={email} required
            onChange={e => setEmail(e.target.value)}
            sx={{ mb: 2, '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }}
          />
          <TextField
            fullWidth label="Password" type="password" value={password} required
            onChange={e => setPassword(e.target.value)}
            sx={{ mb: 2, '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }}
          />
          <TextField
            fullWidth label="Organization" value={tenantSlug}
            onChange={e => setTenantSlug(e.target.value)}
            helperText="Organization slug (e.g. demo)"
            sx={{ mb: 3, '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec }, '& .MuiFormHelperText-root': { color: colors.textSec } }}
          />
          <Button
            fullWidth type="submit" variant="contained" disabled={loading}
            sx={{ bgcolor: colors.primary, py: 1.5, fontWeight: 600 }}
          >
            {loading ? <CircularProgress size={20} color="inherit" /> : 'Sign In'}
          </Button>
        </form>
      </Card>
    </Box>
  )
}
