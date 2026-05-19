import React, { useState, useEffect, useCallback } from 'react'
import {
  Box, Container, Paper, Typography, Button, Card, Grid, TextField,
  IconButton, Alert, Snackbar, Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Chip, Dialog, DialogTitle, DialogContent, DialogActions,
  FormControl, InputLabel, Select, MenuItem, Switch, FormControlLabel, List,
  ListItem, ListItemIcon, ListItemText, Divider, LinearProgress, CircularProgress
} from '@mui/material'
import { AuthProvider, useAuth } from './contexts/AuthContext'
import LoginScreen from './LoginScreen'
import { analyticsApi, accessApi, hardwareApi, ticketsApi } from './services/api'
import {
  Dashboard as DashboardIcon,
  ConfirmationNumber as TicketIcon,
  Search as SearchIcon,
  PersonAdd as PersonAddIcon,
  Settings as SettingsIcon,
  Warning as WarningIcon,
  CheckCircle as CheckIcon,
  Error as ErrorIcon,
  Devices as DevicesIcon,
  Lock as LockIcon,
  LockOpen as LockOpenIcon,
  Refresh as RefreshIcon,
  CameraAlt as CameraIcon,
  Router as RouterIcon,
  ArrowBack as BackIcon,
  Wifi as WifiIcon,
  WifiOff as WifiOffIcon
} from '@mui/icons-material'

const colors = {
  primary: '#007AFF',
  success: '#34C759',
  warning: '#FF9500',
  error: '#FF3B30',
  bg: '#1C1C1E',
  surface: '#2C2C2E',
  card: '#3A3A3C',
  text: '#FFFFFF',
  textSec: '#AEAEB2',
  border: '#48484A'
}

// IPC bridge — available only in Electron; falls back gracefully in browser dev
const ipc = (window as any).electronAPI ?? {
  getHardwareStatus: async () => ({ connected: false, facialRecognition: false, turnstile: false }),
  sendTurnstileCommand: async (_: string) => false,
  emergencyOverride: async () => false,
  getConfig: async () => ({}),
  updateConfig: async (_: string, __: any) => true,
  getSerialPorts: async () => [],
  onHardwareEvent: (_ch: string, _cb: (data: any) => void) => () => {},
}

type Screen = 'dashboard' | 'access' | 'devices' | 'tickets' | 'search' | 'settings' | 'emergency'

interface AccessEvent {
  id: number
  name: string
  ticketType: string
  action: 'GRANTED' | 'DENIED' | 'MANUAL'
  timestamp: string
  device: string
}

interface Device {
  id: number
  name: string
  type: 'facial_recognition' | 'turnstile'
  status: 'online' | 'offline' | 'error'
  ip: string
  lastSeen: string
}

function AppInner() {
  const { user, logout, isLoading } = useAuth()
  const [screen, setScreen] = useState<Screen>('dashboard')
  const [hwStatus, setHwStatus] = useState({ connected: false, facialRecognition: false, turnstile: false })
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' | 'warning' })
  const [emergencyActive, setEmergencyActive] = useState(false)
  const [apiStats, setApiStats] = useState<any>(null)

  const [accessLog, setAccessLog] = useState<AccessEvent[]>([])
  const [devices, setDevices] = useState<Device[]>([])

  // Fetch live data from API when logged in
  const refreshData = useCallback(async () => {
    if (!user) return
    try {
      const [statsRes, logsRes, devsRes] = await Promise.allSettled([
        analyticsApi.dashboard(),
        accessApi.logs({ per_page: 50 }),
        hardwareApi.list(),
      ])
      if (statsRes.status === 'fulfilled') setApiStats(statsRes.value.data)
      if (logsRes.status === 'fulfilled') {
        const logs = logsRes.value.data.logs ?? []
        setAccessLog(logs.map((l: any) => ({
          id: l.id,
          name: l.user?.name ?? l.person_name ?? 'Unknown',
          ticketType: l.ticket_type ?? '—',
          action: l.access_status === 'granted' ? 'GRANTED' : 'DENIED',
          timestamp: l.access_time ?? new Date().toISOString(),
          device: l.device_name ?? l.device_id ?? 'Unknown',
        })))
      }
      if (devsRes.status === 'fulfilled') {
        const devs = devsRes.value.data.devices ?? []
        setDevices(devs.map((d: any) => ({
          id: d.id,
          name: d.name,
          type: d.device_type?.includes('facial') ? 'facial_recognition' : 'turnstile',
          status: d.status ?? 'offline',
          ip: d.ip_address ?? '',
          lastSeen: d.last_heartbeat ? new Date(d.last_heartbeat).toLocaleTimeString() : 'Unknown',
        })))
      }
    } catch { /* keep existing data on network error */ }
  }, [user])

  useEffect(() => {
    if (!isLoading && !user) return
    refreshData()
    const interval = setInterval(refreshData, 30000)
    return () => clearInterval(interval)
  }, [refreshData, isLoading, user])

  // Auth guard
  if (isLoading) return (
    <Box sx={{ minHeight: '100vh', bgcolor: colors.bg, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <CircularProgress sx={{ color: colors.primary }} />
    </Box>
  )
  if (!user) return <LoginScreen />

  useEffect(() => {
    ipc.getHardwareStatus().then(setHwStatus).catch(() => {})

    // Listen for real-time hardware events
    const unsub = ipc.onHardwareEvent('turnstile_event', (data: any) => {
      setAccessLog(prev => [{
        id: Date.now(),
        name: data.userId ?? 'Unknown',
        ticketType: '—',
        action: data.action === 'ENTRY' ? 'GRANTED' : 'DENIED',
        timestamp: data.timestamp ?? new Date().toISOString(),
        device: 'Turnstile'
      }, ...prev.slice(0, 49)])
    })

    return unsub
  }, [])

  const handleEmergencyOverride = async () => {
    const result = await ipc.emergencyOverride()
    if (result) {
      setEmergencyActive(true)
      setSnackbar({ open: true, message: 'Emergency override activated — all turnstiles open', severity: 'warning' })
    } else {
      setSnackbar({ open: true, message: 'Emergency override failed — check hardware connection', severity: 'error' })
    }
  }

  const statsToday = {
    granted: apiStats?.today?.entries_granted ?? accessLog.filter(e => e.action === 'GRANTED').length,
    denied: apiStats?.today?.entries_denied ?? accessLog.filter(e => e.action === 'DENIED').length,
    manual: accessLog.filter(e => e.action === 'MANUAL').length,
    devicesOnline: apiStats?.devices_online ?? devices.filter(d => d.status === 'online').length,
  }

  const nav = (s: Screen) => setScreen(s)

  const sidebar = (
    <Box sx={{ width: 220, bgcolor: colors.surface, borderRight: `1px solid ${colors.border}`, display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <Box sx={{ p: 3, borderBottom: `1px solid ${colors.border}` }}>
        <Typography sx={{ fontSize: 13, fontWeight: 700, color: colors.primary, letterSpacing: 1, textTransform: 'uppercase' }}>
          EventShield Pro
        </Typography>
        <Typography sx={{ fontSize: 11, color: colors.textSec, mt: 0.5 }}>Terminal v1.0</Typography>
      </Box>

      {/* Hardware status pill */}
      <Box sx={{ px: 2, py: 1.5 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, bgcolor: colors.card, borderRadius: 1, px: 1.5, py: 1 }}>
          {hwStatus.connected ? <WifiIcon sx={{ fontSize: 14, color: colors.success }} /> : <WifiOffIcon sx={{ fontSize: 14, color: colors.error }} />}
          <Typography sx={{ fontSize: 11, color: hwStatus.connected ? colors.success : colors.error }}>
            {hwStatus.connected ? 'Hardware connected' : 'No hardware'}
          </Typography>
        </Box>
      </Box>

      <List sx={{ flex: 1, px: 1, py: 0 }}>
        {([
          ['dashboard', 'Dashboard', DashboardIcon],
          ['access', 'Access Log', CheckIcon],
          ['devices', 'Devices', DevicesIcon],
          ['tickets', 'Tickets', TicketIcon],
          ['search', 'Search', SearchIcon],
          ['settings', 'Settings', SettingsIcon],
        ] as [Screen, string, any][]).map(([id, label, Icon]) => (
          <ListItem
            key={id}
            onClick={() => nav(id)}
            sx={{
              borderRadius: 1, mb: 0.5, cursor: 'pointer',
              bgcolor: screen === id ? colors.primary : 'transparent',
              '&:hover': { bgcolor: screen === id ? colors.primary : colors.card }
            }}
          >
            <ListItemIcon sx={{ minWidth: 36 }}><Icon sx={{ fontSize: 18, color: screen === id ? '#fff' : colors.textSec }} /></ListItemIcon>
            <ListItemText primary={label} primaryTypographyProps={{ sx: { fontSize: 13, color: screen === id ? '#fff' : colors.text } }} />
          </ListItem>
        ))}
      </List>

      {/* Emergency override button */}
      <Box sx={{ p: 2, borderTop: `1px solid ${colors.border}` }}>
        <Button
          fullWidth
          variant="contained"
          startIcon={<WarningIcon />}
          onClick={() => nav('emergency')}
          sx={{
            bgcolor: emergencyActive ? colors.warning : colors.error,
            fontSize: 12, fontWeight: 700,
            '&:hover': { bgcolor: emergencyActive ? '#e08800' : '#cc2200' }
          }}
        >
          {emergencyActive ? 'OVERRIDE ACTIVE' : 'EMERGENCY'}
        </Button>
      </Box>
    </Box>
  )

  const renderScreen = () => {
    switch (screen) {
      case 'dashboard':
        return <DashboardScreen stats={statsToday} log={accessLog} onNavigate={nav} />
      case 'access':
        return <AccessLogScreen log={accessLog} />
      case 'devices':
        return <DevicesScreen devices={devices} />
      case 'tickets':
        return <TicketsScreen />
      case 'search':
        return <SearchScreen />
      case 'settings':
        return <SettingsScreen />
      case 'emergency':
        return <EmergencyScreen onActivate={handleEmergencyOverride} isActive={emergencyActive} onBack={() => nav('dashboard')} />
      default:
        return null
    }
  }

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh', bgcolor: colors.bg, color: colors.text }}>
      {sidebar}
      <Box sx={{ flex: 1, overflow: 'auto' }}>
        {renderScreen()}
      </Box>
      <Snackbar open={snackbar.open} autoHideDuration={5000} onClose={() => setSnackbar(s => ({ ...s, open: false }))}>
        <Alert severity={snackbar.severity} onClose={() => setSnackbar(s => ({ ...s, open: false }))}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  )
}

export default function App() {
  return (
    <AuthProvider>
      <AppInner />
    </AuthProvider>
  )
}

// ─── Dashboard ───────────────────────────────────────────────────────────────

function DashboardScreen({ stats, log, onNavigate }: { stats: any; log: AccessEvent[]; onNavigate: (s: Screen) => void }) {
  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 1, color: colors.text }}>Dashboard</Typography>
      <Typography sx={{ color: colors.textSec, mb: 4, fontSize: 13 }}>
        {new Date().toLocaleDateString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
      </Typography>

      <Grid container spacing={2} sx={{ mb: 4 }}>
        {[
          { label: 'Entries Granted', value: stats.granted, color: colors.success },
          { label: 'Entries Denied', value: stats.denied, color: colors.error },
          { label: 'Manual Override', value: stats.manual, color: colors.warning },
          { label: 'Devices Online', value: stats.devicesOnline, color: colors.primary },
        ].map(({ label, value, color }) => (
          <Grid item xs={6} md={3} key={label}>
            <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}` }}>
              <Typography sx={{ fontSize: 32, fontWeight: 700, color }}>{value}</Typography>
              <Typography sx={{ fontSize: 12, color: colors.textSec, mt: 0.5 }}>{label}</Typography>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Grid container spacing={2}>
        <Grid item xs={12} md={8}>
          <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}` }}>
            <Typography sx={{ fontWeight: 600, mb: 2, fontSize: 14 }}>Recent Access Events</Typography>
            <TableContainer>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11 }}>Time</TableCell>
                    <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11 }}>Name</TableCell>
                    <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11 }}>Ticket</TableCell>
                    <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11 }}>Result</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {log.slice(0, 8).map(e => (
                    <TableRow key={e.id}>
                      <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>
                        {new Date(e.timestamp).toLocaleTimeString()}
                      </TableCell>
                      <TableCell sx={{ color: colors.text, borderColor: colors.border, fontSize: 12 }}>{e.name}</TableCell>
                      <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{e.ticketType}</TableCell>
                      <TableCell sx={{ borderColor: colors.border }}>
                        <Chip
                          label={e.action}
                          size="small"
                          sx={{
                            fontSize: 10, height: 20,
                            bgcolor: e.action === 'GRANTED' ? `${colors.success}22` : e.action === 'DENIED' ? `${colors.error}22` : `${colors.warning}22`,
                            color: e.action === 'GRANTED' ? colors.success : e.action === 'DENIED' ? colors.error : colors.warning
                          }}
                        />
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Card>
        </Grid>

        <Grid item xs={12} md={4}>
          <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}`, mb: 2 }}>
            <Typography sx={{ fontWeight: 600, mb: 2, fontSize: 14 }}>Quick Actions</Typography>
            {[
              ['New Registration', 'tickets', PersonAddIcon],
              ['Search Guest', 'search', SearchIcon],
              ['Device Status', 'devices', DevicesIcon],
            ].map(([label, target, Icon]: any) => (
              <Button
                key={label}
                fullWidth
                variant="outlined"
                startIcon={<Icon sx={{ fontSize: 16 }} />}
                onClick={() => onNavigate(target)}
                sx={{
                  mb: 1, justifyContent: 'flex-start', borderColor: colors.border,
                  color: colors.text, fontSize: 12, textTransform: 'none',
                  '&:hover': { borderColor: colors.primary, bgcolor: `${colors.primary}11` }
                }}
              >
                {label}
              </Button>
            ))}
          </Card>
        </Grid>
      </Grid>
    </Box>
  )
}

// ─── Access Log ───────────────────────────────────────────────────────────────

function AccessLogScreen({ log }: { log: AccessEvent[] }) {
  const [filter, setFilter] = useState<'all' | 'GRANTED' | 'DENIED'>('all')
  const filtered = filter === 'all' ? log : log.filter(e => e.action === filter)

  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 1 }}>Access Log</Typography>
      <Typography sx={{ color: colors.textSec, mb: 3, fontSize: 13 }}>Real-time entry / exit events</Typography>

      <Box sx={{ display: 'flex', gap: 1, mb: 3 }}>
        {(['all', 'GRANTED', 'DENIED'] as const).map(f => (
          <Button
            key={f}
            size="small"
            variant={filter === f ? 'contained' : 'outlined'}
            onClick={() => setFilter(f)}
            sx={{
              fontSize: 11, textTransform: 'none',
              bgcolor: filter === f ? colors.primary : 'transparent',
              borderColor: colors.border, color: filter === f ? '#fff' : colors.textSec
            }}
          >
            {f === 'all' ? 'All' : f === 'GRANTED' ? 'Granted' : 'Denied'}
          </Button>
        ))}
      </Box>

      <Card sx={{ bgcolor: colors.surface, borderRadius: 2, border: `1px solid ${colors.border}` }}>
        <TableContainer>
          <Table size="small">
            <TableHead>
              <TableRow>
                {['Time', 'Name', 'Ticket Type', 'Device', 'Result'].map(h => (
                  <TableCell key={h} sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11, fontWeight: 600 }}>{h}</TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {filtered.map(e => (
                <TableRow key={e.id} sx={{ '&:hover': { bgcolor: colors.card } }}>
                  <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>
                    {new Date(e.timestamp).toLocaleTimeString()}
                  </TableCell>
                  <TableCell sx={{ color: colors.text, borderColor: colors.border, fontSize: 12 }}>{e.name}</TableCell>
                  <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{e.ticketType}</TableCell>
                  <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{e.device}</TableCell>
                  <TableCell sx={{ borderColor: colors.border }}>
                    <Chip
                      label={e.action}
                      size="small"
                      sx={{
                        fontSize: 10, height: 20,
                        bgcolor: e.action === 'GRANTED' ? `${colors.success}22` : `${colors.error}22`,
                        color: e.action === 'GRANTED' ? colors.success : colors.error
                      }}
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Card>
    </Box>
  )
}

// ─── Devices ─────────────────────────────────────────────────────────────────

function DevicesScreen({ devices }: { devices: Device[] }) {
  const sendCommand = async (deviceId: number, cmd: string) => {
    await ipc.sendTurnstileCommand(`${deviceId}:${cmd}`)
  }

  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 1 }}>Devices</Typography>
      <Typography sx={{ color: colors.textSec, mb: 3, fontSize: 13 }}>Hardware status and control</Typography>

      <Grid container spacing={2}>
        {devices.map(d => (
          <Grid item xs={12} md={6} key={d.id}>
            <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${d.status === 'online' ? colors.border : colors.error}` }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                  {d.type === 'facial_recognition' ? <CameraIcon sx={{ color: colors.primary, fontSize: 20 }} /> : <RouterIcon sx={{ color: colors.warning, fontSize: 20 }} />}
                  <Box>
                    <Typography sx={{ fontWeight: 600, fontSize: 13 }}>{d.name}</Typography>
                    <Typography sx={{ color: colors.textSec, fontSize: 11 }}>{d.type === 'facial_recognition' ? 'DS-F881 Facial Recognition' : 'DSN-50P Turnstile'}</Typography>
                  </Box>
                </Box>
                <Chip
                  label={d.status.toUpperCase()}
                  size="small"
                  sx={{
                    fontSize: 10, height: 20,
                    bgcolor: d.status === 'online' ? `${colors.success}22` : `${colors.error}22`,
                    color: d.status === 'online' ? colors.success : colors.error
                  }}
                />
              </Box>

              <Box sx={{ mb: 2 }}>
                <Typography sx={{ fontSize: 11, color: colors.textSec }}>IP: {d.ip}</Typography>
                <Typography sx={{ fontSize: 11, color: colors.textSec }}>Last seen: {d.lastSeen}</Typography>
              </Box>

              <Box sx={{ display: 'flex', gap: 1 }}>
                {d.type === 'turnstile' && (
                  <>
                    <Button size="small" variant="outlined" onClick={() => sendCommand(d.id, 'OPEN')}
                      sx={{ fontSize: 11, borderColor: colors.success, color: colors.success, textTransform: 'none' }}>
                      Open
                    </Button>
                    <Button size="small" variant="outlined" onClick={() => sendCommand(d.id, 'LOCK')}
                      sx={{ fontSize: 11, borderColor: colors.error, color: colors.error, textTransform: 'none' }}>
                      Lock
                    </Button>
                  </>
                )}
                <Button size="small" variant="outlined" onClick={() => sendCommand(d.id, 'PING')}
                  sx={{ fontSize: 11, borderColor: colors.border, color: colors.textSec, textTransform: 'none' }}>
                  Ping
                </Button>
                <Button size="small" variant="outlined" onClick={() => sendCommand(d.id, 'RESTART')}
                  sx={{ fontSize: 11, borderColor: colors.warning, color: colors.warning, textTransform: 'none' }}>
                  Restart
                </Button>
              </Box>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  )
}

// ─── Tickets ─────────────────────────────────────────────────────────────────

const ticketTypes = {
  daily: { name: 'Daily Pass', price: 25, color: colors.primary },
  weekend: { name: 'Weekend Pass', price: 40, color: '#5856D6' },
  coach: { name: 'Coach Pass', price: 0, color: colors.success },
  staff: { name: 'Staff Pass', price: 0, color: colors.warning },
}

function TicketsScreen() {
  const [step, setStep] = useState<'list' | 'register'>('list')
  const [form, setForm] = useState({ phone: '', firstName: '', lastName: '', ticketType: 'daily' })
  const [tickets, setTickets] = useState([
    { id: 1, name: 'John Doe', type: 'Daily Pass', phone: '(555) 123-4567', status: 'Active', issued: new Date().toLocaleDateString() },
    { id: 2, name: 'Jane Smith', type: 'Weekend Pass', phone: '(555) 234-5678', status: 'Active', issued: new Date().toLocaleDateString() },
    { id: 3, name: 'Coach Brown', type: 'Coach Pass', phone: '(555) 345-6789', status: 'Active', issued: new Date().toLocaleDateString() },
  ])

  const handleRegister = () => {
    const tt = ticketTypes[form.ticketType as keyof typeof ticketTypes]
    setTickets(prev => [{
      id: Date.now(),
      name: `${form.firstName} ${form.lastName}`,
      type: tt.name,
      phone: form.phone,
      status: 'Active',
      issued: new Date().toLocaleDateString()
    }, ...prev])
    setForm({ phone: '', firstName: '', lastName: '', ticketType: 'daily' })
    setStep('list')
  }

  if (step === 'register') {
    return (
      <Box sx={{ p: 4, maxWidth: 600 }}>
        <Button startIcon={<BackIcon />} onClick={() => setStep('list')} sx={{ mb: 3, color: colors.textSec, fontSize: 12, textTransform: 'none' }}>
          Back
        </Button>
        <Typography variant="h5" sx={{ fontWeight: 600, mb: 3 }}>New Registration</Typography>

        <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}` }}>
          <TextField fullWidth label="Phone" value={form.phone} onChange={e => setForm(f => ({ ...f, phone: e.target.value }))}
            sx={{ mb: 2, '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }} />
          <Grid container spacing={2} sx={{ mb: 2 }}>
            <Grid item xs={6}>
              <TextField fullWidth label="First Name" value={form.firstName} onChange={e => setForm(f => ({ ...f, firstName: e.target.value }))}
                sx={{ '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }} />
            </Grid>
            <Grid item xs={6}>
              <TextField fullWidth label="Last Name" value={form.lastName} onChange={e => setForm(f => ({ ...f, lastName: e.target.value }))}
                sx={{ '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }} />
            </Grid>
          </Grid>

          <Grid container spacing={2} sx={{ mb: 3 }}>
            {Object.entries(ticketTypes).map(([key, tt]) => (
              <Grid item xs={6} key={key}>
                <Card
                  onClick={() => setForm(f => ({ ...f, ticketType: key }))}
                  sx={{
                    p: 2, cursor: 'pointer', bgcolor: colors.card, textAlign: 'center',
                    border: `2px solid ${form.ticketType === key ? tt.color : 'transparent'}`,
                    '&:hover': { borderColor: tt.color }
                  }}
                >
                  <Typography sx={{ color: tt.color, fontWeight: 700, fontSize: 16 }}>
                    {tt.price === 0 ? 'FREE' : `$${tt.price}`}
                  </Typography>
                  <Typography sx={{ fontSize: 12, color: colors.text }}>{tt.name}</Typography>
                </Card>
              </Grid>
            ))}
          </Grid>

          <Button fullWidth variant="contained" onClick={handleRegister}
            sx={{ bgcolor: colors.primary, fontWeight: 600 }}>
            Complete Registration
          </Button>
        </Card>
      </Box>
    )
  }

  return (
    <Box sx={{ p: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Box>
          <Typography variant="h5" sx={{ fontWeight: 600 }}>Tickets</Typography>
          <Typography sx={{ color: colors.textSec, fontSize: 13 }}>{tickets.length} active tickets</Typography>
        </Box>
        <Button variant="contained" startIcon={<PersonAddIcon />} onClick={() => setStep('register')}
          sx={{ bgcolor: colors.primary, fontSize: 12, textTransform: 'none' }}>
          New Registration
        </Button>
      </Box>

      <Card sx={{ bgcolor: colors.surface, borderRadius: 2, border: `1px solid ${colors.border}` }}>
        <TableContainer>
          <Table size="small">
            <TableHead>
              <TableRow>
                {['Name', 'Phone', 'Ticket Type', 'Issued', 'Status'].map(h => (
                  <TableCell key={h} sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11, fontWeight: 600 }}>{h}</TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {tickets.map(t => (
                <TableRow key={t.id} sx={{ '&:hover': { bgcolor: colors.card } }}>
                  <TableCell sx={{ color: colors.text, borderColor: colors.border, fontSize: 12 }}>{t.name}</TableCell>
                  <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{t.phone}</TableCell>
                  <TableCell sx={{ borderColor: colors.border }}>
                    <Chip label={t.type} size="small" sx={{ fontSize: 10, height: 20, bgcolor: `${colors.primary}22`, color: colors.primary }} />
                  </TableCell>
                  <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{t.issued}</TableCell>
                  <TableCell sx={{ borderColor: colors.border }}>
                    <Chip label={t.status} size="small" sx={{ fontSize: 10, height: 20, bgcolor: `${colors.success}22`, color: colors.success }} />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Card>
    </Box>
  )
}

// ─── Search ───────────────────────────────────────────────────────────────────

function SearchScreen() {
  const [term, setTerm] = useState('')
  const allGuests = [
    { id: 1, name: 'John Doe', phone: '(555) 123-4567', ticket: 'Daily Pass', status: 'Active' },
    { id: 2, name: 'Jane Smith', phone: '(555) 234-5678', ticket: 'Weekend Pass', status: 'Active' },
    { id: 3, name: 'Coach Brown', phone: '(555) 345-6789', ticket: 'Coach Pass', status: 'Active' },
  ]
  const results = term.length > 1
    ? allGuests.filter(g => g.name.toLowerCase().includes(term.toLowerCase()) || g.phone.includes(term))
    : []

  return (
    <Box sx={{ p: 4 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 1 }}>Search</Typography>
      <Typography sx={{ color: colors.textSec, mb: 3, fontSize: 13 }}>Find guests by name or phone number</Typography>

      <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
        <TextField
          fullWidth
          placeholder="Name or phone..."
          value={term}
          onChange={e => setTerm(e.target.value)}
          sx={{
            '& .MuiOutlinedInput-root': { color: colors.text, bgcolor: colors.surface, '& fieldset': { borderColor: colors.border } },
            '& .MuiInputBase-input::placeholder': { color: colors.textSec }
          }}
        />
      </Box>

      {results.length > 0 && (
        <Card sx={{ bgcolor: colors.surface, borderRadius: 2, border: `1px solid ${colors.border}` }}>
          <TableContainer>
            <Table size="small">
              <TableHead>
                <TableRow>
                  {['Name', 'Phone', 'Ticket', 'Status'].map(h => (
                    <TableCell key={h} sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 11, fontWeight: 600 }}>{h}</TableCell>
                  ))}
                </TableRow>
              </TableHead>
              <TableBody>
                {results.map(g => (
                  <TableRow key={g.id}>
                    <TableCell sx={{ color: colors.text, borderColor: colors.border, fontSize: 12 }}>{g.name}</TableCell>
                    <TableCell sx={{ color: colors.textSec, borderColor: colors.border, fontSize: 12 }}>{g.phone}</TableCell>
                    <TableCell sx={{ borderColor: colors.border }}>
                      <Chip label={g.ticket} size="small" sx={{ fontSize: 10, height: 20, bgcolor: `${colors.primary}22`, color: colors.primary }} />
                    </TableCell>
                    <TableCell sx={{ borderColor: colors.border }}>
                      <Chip label={g.status} size="small" sx={{ fontSize: 10, height: 20, bgcolor: `${colors.success}22`, color: colors.success }} />
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Card>
      )}

      {term.length > 1 && results.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 6 }}>
          <Typography sx={{ color: colors.textSec, fontSize: 14 }}>No guests found for "{term}"</Typography>
        </Box>
      )}
    </Box>
  )
}

// ─── Settings ────────────────────────────────────────────────────────────────

function SettingsScreen() {
  const [apiUrl, setApiUrl] = useState('http://localhost:5000')
  const [autoStart, setAutoStart] = useState(false)
  const [minimizeToTray, setMinimizeToTray] = useState(true)

  const save = async () => {
    await ipc.updateConfig('server.apiUrl', apiUrl)
    await ipc.updateConfig('settings.autoStart', autoStart)
    await ipc.updateConfig('settings.minimizeToTray', minimizeToTray)
  }

  return (
    <Box sx={{ p: 4, maxWidth: 600 }}>
      <Typography variant="h5" sx={{ fontWeight: 600, mb: 1 }}>Settings</Typography>
      <Typography sx={{ color: colors.textSec, mb: 3, fontSize: 13 }}>Terminal configuration</Typography>

      <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}`, mb: 2 }}>
        <Typography sx={{ fontWeight: 600, mb: 2, fontSize: 13 }}>Server</Typography>
        <TextField
          fullWidth label="API URL" value={apiUrl} onChange={e => setApiUrl(e.target.value)}
          sx={{ mb: 2, '& .MuiOutlinedInput-root': { color: colors.text, '& fieldset': { borderColor: colors.border } }, '& .MuiInputLabel-root': { color: colors.textSec } }}
        />
      </Card>

      <Card sx={{ bgcolor: colors.surface, p: 3, borderRadius: 2, border: `1px solid ${colors.border}`, mb: 3 }}>
        <Typography sx={{ fontWeight: 600, mb: 2, fontSize: 13 }}>Behavior</Typography>
        <FormControlLabel
          control={<Switch checked={autoStart} onChange={e => setAutoStart(e.target.checked)} size="small" />}
          label={<Typography sx={{ fontSize: 13 }}>Start on login</Typography>}
          sx={{ display: 'flex', mb: 1 }}
        />
        <FormControlLabel
          control={<Switch checked={minimizeToTray} onChange={e => setMinimizeToTray(e.target.checked)} size="small" />}
          label={<Typography sx={{ fontSize: 13 }}>Minimize to system tray on close</Typography>}
          sx={{ display: 'flex' }}
        />
      </Card>

      <Button variant="contained" onClick={save} sx={{ bgcolor: colors.primary, textTransform: 'none' }}>
        Save Settings
      </Button>
    </Box>
  )
}

// ─── Emergency ───────────────────────────────────────────────────────────────

function EmergencyScreen({ onActivate, isActive, onBack }: { onActivate: () => void; isActive: boolean; onBack: () => void }) {
  const [confirmed, setConfirmed] = useState(false)

  return (
    <Box sx={{ p: 4, maxWidth: 500 }}>
      <Button startIcon={<BackIcon />} onClick={onBack} sx={{ mb: 3, color: colors.textSec, fontSize: 12, textTransform: 'none' }}>
        Back to Dashboard
      </Button>

      <Card sx={{ bgcolor: colors.surface, p: 4, borderRadius: 2, border: `2px solid ${colors.error}`, textAlign: 'center' }}>
        <WarningIcon sx={{ fontSize: 48, color: colors.error, mb: 2 }} />
        <Typography variant="h5" sx={{ fontWeight: 700, color: colors.error, mb: 1 }}>Emergency Override</Typography>
        <Typography sx={{ color: colors.textSec, fontSize: 13, mb: 3 }}>
          This will immediately open all turnstiles and disable access control. Use only in a life-safety emergency.
        </Typography>

        {isActive ? (
          <Box sx={{ bgcolor: `${colors.warning}22`, border: `1px solid ${colors.warning}`, borderRadius: 2, p: 2 }}>
            <Typography sx={{ color: colors.warning, fontWeight: 700, fontSize: 14 }}>OVERRIDE IS ACTIVE</Typography>
            <Typography sx={{ color: colors.textSec, fontSize: 12, mt: 0.5 }}>All turnstiles are currently open</Typography>
          </Box>
        ) : (
          <>
            <FormControlLabel
              control={<Switch checked={confirmed} onChange={e => setConfirmed(e.target.checked)} />}
              label={<Typography sx={{ fontSize: 13, color: colors.text }}>I confirm this is a life-safety emergency</Typography>}
              sx={{ mb: 3 }}
            />
            <Button
              fullWidth
              variant="contained"
              disabled={!confirmed}
              onClick={onActivate}
              sx={{
                bgcolor: colors.error, fontWeight: 700, fontSize: 14, py: 1.5,
                '&:hover': { bgcolor: '#cc2200' },
                '&:disabled': { bgcolor: colors.card, color: colors.textSec }
              }}
            >
              ACTIVATE EMERGENCY OVERRIDE
            </Button>
          </>
        )}
      </Card>
    </Box>
  )
}
