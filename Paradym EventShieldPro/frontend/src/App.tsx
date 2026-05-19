import React, { useState } from 'react'
import {
  Box, Container, Paper, Typography, Button, Card, Grid, TextField, IconButton, Alert, Snackbar, List, ListItem, ListItemIcon, ListItemText,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Chip, Dialog, DialogTitle, DialogContent, DialogActions,
  FormControl, InputLabel, Select, MenuItem
} from '@mui/material'
import {
  Dashboard as DashboardIcon, ConfirmationNumber as ConfirmationNumberIcon,
  Search as SearchIcon, PersonAdd as PersonAddIcon, ArrowBack as ArrowBackIcon,
  Settings as SettingsIcon, AttachMoney as AttachMoneyIcon, Add as AddIcon,
  Edit as EditIcon, Delete as DeleteIcon, Visibility as ViewIcon
} from '@mui/icons-material'

// Apple-style theme colors
const appleColors = {
  primary: '#007AFF',
  secondary: '#5856D6',
  success: '#34C759',
  warning: '#FF9500',
  error: '#FF3B30',
  background: '#F2F2F7',
  surface: '#FFFFFF',
  text: '#1D1D1F',
  textSecondary: '#86868B',
  border: '#E5E5EA'
}

const ticketTypes = {
  daily: { name: 'Daily Pass', price: '$25', duration: 'Valid same day only', color: '#007AFF' },
  weekend: { name: 'Weekend Pass', price: '$40', duration: 'Valid Sat & Sun', color: '#5856D6' },
  coach: { name: 'Coach Pass', price: 'FREE', duration: 'Coaches & staff', color: '#34C759' },
  staff: { name: 'Staff Pass', price: 'FREE', duration: 'Event staff', color: '#FF9500' }
}

// Main App Component
const App = () => {
  console.log('App component rendering')

  const [currentScreen, setCurrentScreen] = useState('dashboard')
  const [registrationData, setRegistrationData] = useState<any>(null)
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as any })

  // Mock data for demonstration
  const [tickets, setTickets] = useState([
    { id: 1, guestName: 'John Doe', type: 'Daily Pass', status: 'Active', issued: '2024-01-15', expires: '2024-01-15' },
    { id: 2, guestName: 'Jane Smith', type: 'Weekend Pass', status: 'Active', issued: '2024-01-14', expires: '2024-01-16' },
    { id: 3, guestName: 'Bob Johnson', type: 'Coach Pass', status: 'Active', issued: '2024-01-13', expires: '2024-01-20' }
  ])

  const [guests] = useState([
    { id: 1, name: 'John Doe', phone: '(555) 123-4567', email: 'john@example.com', ticketType: 'Daily Pass', status: 'Active' },
    { id: 2, name: 'Jane Smith', phone: '(555) 234-5678', email: 'jane@example.com', ticketType: 'Weekend Pass', status: 'Active' },
    { id: 3, name: 'Bob Johnson', phone: '(555) 345-6789', email: 'bob@example.com', ticketType: 'Coach Pass', status: 'Active' }
  ])

  const [pricing, setPricing] = useState({
    daily: { name: 'Daily Pass', price: 25, description: 'Valid for same day only' },
    weekend: { name: 'Weekend Pass', price: 40, description: 'Valid Saturday and Sunday' },
    coach: { name: 'Coach Pass', price: 0, description: 'Free for coaches and staff' },
    staff: { name: 'Staff Pass', price: 0, description: 'Free for event staff' }
  })

  const handleNavigation = (screen: string) => {
    setCurrentScreen(screen)
  }

  const handleRegistrationComplete = (data: any) => {
    setRegistrationData(data)
    setCurrentScreen('ticketRegistration')
  }

  // Fast Registration Component
  const FastRegistration = ({ onComplete, onBack }: { onComplete: (data: any) => void, onBack: () => void }) => {
    const [currentStep, setCurrentStep] = useState(1)
    const [formData, setFormData] = useState({
      phone: '',
      firstName: '',
      lastName: '',
      email: '',
      ticketType: 'daily'
    })

    const formatPhoneNumber = (value: string) => {
      const digits = value.replace(/\D/g, '')
      if (digits.length <= 3) {
        return `(${digits}`
      } else if (digits.length <= 6) {
        return `(${digits.slice(0, 3)}) ${digits.slice(3)}`
      } else {
        return `(${digits.slice(0, 3)}) ${digits.slice(3, 6)}-${digits.slice(6, 10)}`
      }
    }

    const handlePhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const formatted = formatPhoneNumber(e.target.value)
      setFormData({ ...formData, phone: formatted })
    }

    const isPhoneValid = () => {
      const phoneRegex = /^\(\d{3}\) \d{3}-\d{4}$/
      return phoneRegex.test(formData.phone)
    }

    const handleNext = () => {
      if (currentStep === 1) {
        if (!isPhoneValid()) {
          alert('Please enter phone number in format: (555) 123-4567')
          return
        }
      }
      if (currentStep < 4) setCurrentStep(currentStep + 1)
    }

    const handleBack = () => {
      if (currentStep > 1) setCurrentStep(currentStep - 1)
    }

    const handleSubmit = () => {
      onComplete(formData)
    }

    const renderStep = () => {
      switch (currentStep) {
        case 1:
          return (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography variant="h4" sx={{ mb: 3, color: appleColors.text }}>
                Enter Phone Number
              </Typography>
              <TextField
                fullWidth
                label="Phone Number"
                value={formData.phone}
                onChange={handlePhoneChange}
                placeholder="(555) 123-4567"
                sx={{ mb: 3, maxWidth: 300 }}
              />
              <Typography variant="body2" sx={{ color: appleColors.textSecondary, mb: 3 }}>
                We'll use this to identify you at the event
              </Typography>
            </Box>
          )
        case 2:
          return (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography variant="h4" sx={{ mb: 3, color: appleColors.text }}>
                Personal Information
              </Typography>
              <Grid container spacing={2} sx={{ maxWidth: 600, mx: 'auto' }}>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="First Name"
                    value={formData.firstName}
                    onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="Last Name"
                    value={formData.lastName}
                    onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Email (Optional)"
                    type="email"
                    value={formData.email}
                    onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                  />
                </Grid>
              </Grid>
            </Box>
          )
        case 3:
          return (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography variant="h4" sx={{ mb: 3, color: appleColors.text }}>
                Select Ticket Type
              </Typography>
              <Grid container spacing={2} sx={{ maxWidth: 600, mx: 'auto' }}>
                {Object.entries(ticketTypes).map(([key, ticket]) => (
                  <Grid item xs={12} sm={6} key={key}>
                    <Card
                      sx={{
                        p: 3,
                        cursor: 'pointer',
                        border: formData.ticketType === key ? `2px solid ${ticket.color}` : '2px solid transparent',
                        '&:hover': { borderColor: ticket.color, opacity: 0.8 }
                      }}
                      onClick={() => setFormData({ ...formData, ticketType: key })}
                    >
                      <Typography variant="h6" sx={{ color: ticket.color, mb: 1 }}>
                        {ticket.name}
                      </Typography>
                      <Typography variant="h4" sx={{ color: ticket.color, mb: 1 }}>
                        {ticket.price}
                      </Typography>
                      <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>
                        {ticket.duration}
                      </Typography>
                    </Card>
                  </Grid>
                ))}
              </Grid>
            </Box>
          )
        case 4:
          return (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography variant="h4" sx={{ mb: 3, color: appleColors.text }}>
                Review & Confirm
              </Typography>
              <Paper sx={{ p: 3, maxWidth: 500, mx: 'auto', mb: 3 }}>
                <Typography variant="h6" sx={{ mb: 2, color: appleColors.text }}>
                  Registration Summary
                </Typography>
                <Box sx={{ textAlign: 'left' }}>
                  <Typography><strong>Phone:</strong> {formData.phone}</Typography>
                  <Typography><strong>Name:</strong> {formData.firstName} {formData.lastName}</Typography>
                  <Typography><strong>Email:</strong> {formData.email || 'Not provided'}</Typography>
                  <Typography><strong>Ticket:</strong> {ticketTypes[formData.ticketType as keyof typeof ticketTypes].name}</Typography>
                </Box>
              </Paper>
            </Box>
          )
        default:
          return null
      }
    }

    return (
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Button
            startIcon={<ArrowBackIcon />}
            onClick={onBack}
            sx={{ mb: 3, color: appleColors.textSecondary }}
          >
            Back to Dashboard
          </Button>
          <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
            Fast Registration
          </Typography>
          <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
            Step {currentStep} of 4
          </Typography>
        </Box>

        {renderStep()}

        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 4 }}>
          <Button
            variant="outlined"
            onClick={handleBack}
            disabled={currentStep === 1}
            sx={{ borderColor: appleColors.border, color: appleColors.text }}
          >
            Back
          </Button>
          <Button
            variant="contained"
            onClick={currentStep === 4 ? handleSubmit : handleNext}
            sx={{ bgcolor: appleColors.primary }}
          >
            {currentStep === 4 ? 'Complete Registration' : 'Next'}
          </Button>
        </Box>
      </Container>
    )
  }

  // Ticket Registration Screen
  const TicketRegistrationScreen = ({ registrationData: regData, onBack }: { registrationData: any, onBack: () => void }) => {
    return (
      <Container maxWidth="md" sx={{ py: 4 }}>
        <Box sx={{ textAlign: 'center' }}>
          <Button
            startIcon={<ArrowBackIcon />}
            onClick={onBack}
            sx={{ mb: 3, color: appleColors.textSecondary }}
          >
            Back to Registration
          </Button>
          <Paper sx={{ p: 6, textAlign: 'center', borderRadius: 3 }}>
            <Box sx={{ mb: 4 }}>
              <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
                Registration Complete!
              </Typography>
              <Box sx={{
                bgcolor: appleColors.success,
                color: 'white',
                p: 4,
                borderRadius: 3,
                mb: 4,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                gap: 2
              }}>
                <Typography variant="h5" sx={{ fontWeight: 500 }}>
                  Welcome to EventShield Pro!
                </Typography>
                <Typography variant="body1" sx={{ opacity: 0.9 }}>
                  Your {ticketTypes[regData.ticketType as keyof typeof ticketTypes].name} has been activated.
                </Typography>
              </Box>
            </Box>
            <Button
              variant="contained"
              onClick={() => handleNavigation('dashboard')}
              sx={{ bgcolor: appleColors.primary }}
            >
              RETURN TO DASHBOARD
            </Button>
          </Paper>
        </Box>
      </Container>
    )
  }

  // Tickets Management Screen
  const TicketsScreen = () => {
    const [openDialog, setOpenDialog] = useState(false)
    const [selectedTicket, setSelectedTicket] = useState<any>(null)

    const handleEditTicket = (ticket: any) => {
      setSelectedTicket(ticket)
      setOpenDialog(true)
    }

    const handleDeleteTicket = (ticketId: number) => {
      setTickets(tickets.filter(t => t.id !== ticketId))
    }

    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
            Ticket Management
          </Typography>
          <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
            Manage all event tickets and access passes
          </Typography>
        </Box>

        <Card sx={{ p: 3, mb: 3 }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
            <Typography variant="h5" sx={{ fontWeight: 500 }}>
              Active Tickets ({tickets.length})
            </Typography>
            <Button
              variant="contained"
              startIcon={<AddIcon />}
              onClick={() => handleNavigation('registration')}
              sx={{ bgcolor: appleColors.primary }}
            >
              New Ticket
            </Button>
          </Box>

          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Guest Name</TableCell>
                  <TableCell>Ticket Type</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Issued</TableCell>
                  <TableCell>Expires</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {tickets.map((ticket) => (
                  <TableRow key={ticket.id}>
                    <TableCell>{ticket.guestName}</TableCell>
                    <TableCell>
                      <Chip
                        label={ticket.type}
                        color={ticket.type.includes('Daily') ? 'primary' : ticket.type.includes('Weekend') ? 'success' : 'warning'}
                        size="small"
                      />
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={ticket.status}
                        color={ticket.status === 'Active' ? 'success' : 'error'}
                        size="small"
                      />
                    </TableCell>
                    <TableCell>{ticket.issued}</TableCell>
                    <TableCell>{ticket.expires}</TableCell>
                    <TableCell>
                      <IconButton size="small" onClick={() => handleEditTicket(ticket)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton size="small" onClick={() => handleDeleteTicket(ticket.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Card>

        <Dialog open={openDialog} onClose={() => setOpenDialog(false)} maxWidth="sm" fullWidth>
          <DialogTitle>Edit Ticket</DialogTitle>
          <DialogContent>
            {selectedTicket && (
              <Box sx={{ pt: 2 }}>
                <TextField
                  fullWidth
                  label="Guest Name"
                  value={selectedTicket.guestName}
                  sx={{ mb: 2 }}
                />
                <FormControl fullWidth sx={{ mb: 2 }}>
                  <InputLabel>Ticket Type</InputLabel>
                  <Select value={selectedTicket.type} label="Ticket Type">
                    <MenuItem value="Daily Pass">Daily Pass</MenuItem>
                    <MenuItem value="Weekend Pass">Weekend Pass</MenuItem>
                    <MenuItem value="Coach Pass">Coach Pass</MenuItem>
                    <MenuItem value="Staff Pass">Staff Pass</MenuItem>
                  </Select>
                </FormControl>
                <TextField
                  fullWidth
                  label="Expires"
                  value={selectedTicket.expires}
                  sx={{ mb: 2 }}
                />
              </Box>
            )}
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setOpenDialog(false)}>Cancel</Button>
            <Button variant="contained" onClick={() => setOpenDialog(false)}>Save</Button>
          </DialogActions>
        </Dialog>
      </Container>
    )
  }

  // Search Screen
  const SearchScreen = () => {
    const [searchTerm, setSearchTerm] = useState('')
    const [searchResults, setSearchResults] = useState<any[]>([])

    const handleSearch = () => {
      if (searchTerm.trim()) {
        const results = guests.filter(guest =>
          guest.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          guest.phone.includes(searchTerm) ||
          guest.email.toLowerCase().includes(searchTerm.toLowerCase())
        )
        setSearchResults(results)
      } else {
        setSearchResults([])
      }
    }

    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
            Guest Search
          </Typography>
          <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
            Find guests by name, phone, or email
          </Typography>
        </Box>

        <Card sx={{ p: 3, mb: 3 }}>
          <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
            <TextField
              fullWidth
              placeholder="Search by name, phone, or email..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
            />
            <Button
              variant="contained"
              onClick={handleSearch}
              sx={{ bgcolor: appleColors.primary }}
            >
              Search
            </Button>
          </Box>

          {searchResults.length > 0 && (
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>Phone</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Ticket Type</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Actions</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {searchResults.map((guest) => (
                    <TableRow key={guest.id}>
                      <TableCell>{guest.name}</TableCell>
                      <TableCell>{guest.phone}</TableCell>
                      <TableCell>{guest.email}</TableCell>
                      <TableCell>
                        <Chip
                          label={guest.ticketType}
                          color={guest.ticketType.includes('Daily') ? 'primary' : guest.ticketType.includes('Weekend') ? 'success' : 'warning'}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={guest.status}
                          color={guest.status === 'Active' ? 'success' : 'error'}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        <IconButton size="small">
                          <ViewIcon />
                        </IconButton>
                        <IconButton size="small">
                          <EditIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}

          {searchTerm && searchResults.length === 0 && (
            <Box sx={{ textAlign: 'center', py: 4 }}>
              <Typography variant="h6" sx={{ color: appleColors.textSecondary }}>
                No guests found matching "{searchTerm}"
              </Typography>
            </Box>
          )}
        </Card>
      </Container>
    )
  }

  // Device Control Screen
  const DeviceControlScreen = () => {
    const [devices] = useState([
      { id: 1, name: 'DS-F881 Main Entrance', type: 'Facial Recognition', status: 'Online', ip: '192.168.1.100', lastSeen: '2 minutes ago' },
      { id: 2, name: 'DS-F881 Side Entrance', type: 'Facial Recognition', status: 'Online', ip: '192.168.1.101', lastSeen: '1 minute ago' },
      { id: 3, name: 'Turnstile 1', type: 'Access Control', status: 'Online', ip: '192.168.1.102', lastSeen: '30 seconds ago' }
    ])

    const handleDeviceAction = (deviceId: number, action: string) => {
      console.log(`${action} device ${deviceId}`)
    }

    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
            Device Control
          </Typography>
          <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
            Monitor and control DS-F881 facial recognition devices and turnstiles
          </Typography>
        </Box>

        <Grid container spacing={3}>
          {devices.map((device) => (
            <Grid item xs={12} md={6} lg={4} key={device.id}>
              <Card sx={{ p: 3, height: '100%' }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                  <Typography variant="h6" sx={{ fontWeight: 500 }}>
                    {device.name}
                  </Typography>
                  <Chip
                    label={device.status}
                    color={device.status === 'Online' ? 'success' : 'error'}
                    size="small"
                  />
                </Box>

                <Typography variant="body2" sx={{ color: appleColors.textSecondary, mb: 1 }}>
                  Type: {device.type}
                </Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary, mb: 1 }}>
                  IP: {device.ip}
                </Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary, mb: 3 }}>
                  Last seen: {device.lastSeen}
                </Typography>

                <Box sx={{ display: 'flex', gap: 1 }}>
                  <Button
                    size="small"
                    variant="outlined"
                    onClick={() => handleDeviceAction(device.id, 'restart')}
                    sx={{ borderColor: appleColors.warning, color: appleColors.warning }}
                  >
                    Restart
                  </Button>
                  <Button
                    size="small"
                    variant="outlined"
                    onClick={() => handleDeviceAction(device.id, 'configure')}
                    sx={{ borderColor: appleColors.primary, color: appleColors.primary }}
                  >
                    Configure
                  </Button>
                  <Button
                    size="small"
                    variant="outlined"
                    onClick={() => handleDeviceAction(device.id, 'monitor')}
                    sx={{ borderColor: appleColors.success, color: appleColors.success }}
                  >
                    Monitor
                  </Button>
                </Box>
              </Card>
            </Grid>
          ))}
        </Grid>

        <Card sx={{ p: 3, mt: 4 }}>
          <Typography variant="h5" sx={{ fontWeight: 500, mb: 3 }}>
            System Status
          </Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.success, mb: 1 }}>3</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Devices Online</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.primary, mb: 1 }}>156</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Active Tickets</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.warning, mb: 1 }}>24</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Today's Entries</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.error, mb: 1 }}>0</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>System Errors</Typography>
              </Box>
            </Grid>
          </Grid>
        </Card>
      </Container>
    )
  }

  // Ticket Pricing Screen
  const TicketPricingScreen = () => {
    const [openDialog, setOpenDialog] = useState(false)
    const [editingPricing, setEditingPricing] = useState<any>(null)

    const handleEditPricing = (key: string) => {
      setEditingPricing({ key, ...pricing[key as keyof typeof pricing] })
      setOpenDialog(true)
    }

    const handleSavePricing = () => {
      if (editingPricing) {
        setPricing({
          ...pricing,
          [editingPricing.key]: editingPricing
        })
        setOpenDialog(false)
        setEditingPricing(null)
      }
    }

    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
            Ticket Pricing
          </Typography>
          <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
            Manage ticket types, pricing, and availability
          </Typography>
        </Box>

        <Grid container spacing={3}>
          {Object.entries(pricing).map(([key, ticket]) => (
            <Grid item xs={12} sm={6} md={3} key={key}>
              <Card sx={{ p: 3, height: '100%', textAlign: 'center' }}>
                <Typography variant="h4" sx={{ color: ticket.price === 0 ? appleColors.success : appleColors.primary, mb: 2 }}>
                  {ticket.price === 0 ? 'FREE' : `$${ticket.price}`}
                </Typography>
                <Typography variant="h6" sx={{ fontWeight: 500, mb: 2 }}>
                  {ticket.name}
                </Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary, mb: 3 }}>
                  {ticket.description}
                </Typography>
                <Button
                  variant="outlined"
                  onClick={() => handleEditPricing(key)}
                  sx={{ borderColor: appleColors.primary, color: appleColors.primary }}
                >
                  Edit Pricing
                </Button>
              </Card>
            </Grid>
          ))}
        </Grid>

        <Card sx={{ p: 3, mt: 4 }}>
          <Typography variant="h5" sx={{ fontWeight: 500, mb: 3 }}>
            Pricing Summary
          </Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.primary, mb: 1 }}>2</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Paid Tickets</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.success, mb: 1 }}>2</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Free Tickets</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.warning, mb: 1 }}>$65</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Total Revenue</Typography>
              </Box>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Box sx={{ textAlign: 'center', p: 2 }}>
                <Typography variant="h4" sx={{ color: appleColors.error, mb: 1 }}>24</Typography>
                <Typography variant="body2" sx={{ color: appleColors.textSecondary }}>Tickets Sold Today</Typography>
              </Box>
            </Grid>
          </Grid>
        </Card>

        <Dialog open={openDialog} onClose={() => setOpenDialog(false)} maxWidth="sm" fullWidth>
          <DialogTitle>Edit Ticket Pricing</DialogTitle>
          <DialogContent>
            {editingPricing && (
              <Box sx={{ pt: 2 }}>
                <TextField
                  fullWidth
                  label="Ticket Name"
                  value={editingPricing.name}
                  onChange={(e) => setEditingPricing({ ...editingPricing, name: e.target.value })}
                  sx={{ mb: 2 }}
                />
                <TextField
                  fullWidth
                  label="Price"
                  type="number"
                  value={editingPricing.price}
                  onChange={(e) => setEditingPricing({ ...editingPricing, price: Number(e.target.value) })}
                  sx={{ mb: 2 }}
                />
                <TextField
                  fullWidth
                  label="Description"
                  multiline
                  rows={3}
                  value={editingPricing.description}
                  onChange={(e) => setEditingPricing({ ...editingPricing, description: e.target.value })}
                />
              </Box>
            )}
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setOpenDialog(false)}>Cancel</Button>
            <Button variant="contained" onClick={handleSavePricing}>Save</Button>
          </DialogActions>
        </Dialog>
      </Container>
    )
  }

  const renderScreen = () => {
    switch (currentScreen) {
      case 'dashboard':
        return (
          <Container maxWidth="xl" sx={{ py: 4 }}>
            <Box sx={{ mb: 6 }}>
              <Typography variant="h3" sx={{ fontWeight: 300, color: appleColors.text, mb: 2 }}>
                Welcome to EventShield Pro
              </Typography>
              <Typography variant="h6" sx={{ color: appleColors.textSecondary, fontWeight: 400 }}>
                Manage your events with speed and precision
              </Typography>
            </Box>
            <Grid container spacing={4}>
              <Grid item xs={12} md={8}>
                <Grid container spacing={3}>
                  <Grid item xs={12} sm={4}>
                    <Card sx={{ p: 4, bgcolor: appleColors.primary, color: 'white', textAlign: 'center', borderRadius: 3 }}>
                      <Typography variant="h2" sx={{ fontWeight: 300, mb: 1 }}>24</Typography>
                      <Typography variant="body1" sx={{ opacity: 0.9 }}>Today's Registrations</Typography>
                    </Card>
                  </Grid>
                  <Grid item xs={12} sm={4}>
                    <Card sx={{ p: 4, bgcolor: appleColors.success, color: 'white', textAlign: 'center', borderRadius: 3 }}>
                      <Typography variant="h2" sx={{ fontWeight: 300, mb: 1 }}>156</Typography>
                      <Typography variant="body1" sx={{ opacity: 0.9 }}>Active Tickets</Typography>
                    </Card>
                  </Grid>
                  <Grid item xs={12} sm={4}>
                    <Card sx={{ p: 4, bgcolor: appleColors.warning, color: 'white', textAlign: 'center', borderRadius: 3 }}>
                      <Typography variant="h2" sx={{ fontWeight: 300, mb: 1 }}>8</Typography>
                      <Typography variant="body1" sx={{ opacity: 0.9 }}>Expired Today</Typography>
                    </Card>
                  </Grid>
                </Grid>
              </Grid>
              <Grid item xs={12} md={4}>
                <Card sx={{ p: 4, height: '100%', borderRadius: 3, border: `1px solid ${appleColors.border}`, bgcolor: appleColors.surface }}>
                  <Typography variant="h5" sx={{ mb: 3, fontWeight: 500, color: appleColors.text }}>
                    Quick Actions
                  </Typography>
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <Button
                      variant="contained"
                      startIcon={<PersonAddIcon />}
                      fullWidth
                      onClick={() => handleNavigation('registration')}
                      sx={{ bgcolor: appleColors.primary, borderRadius: 2, py: 2, textTransform: 'none', fontSize: '1rem', fontWeight: 500 }}
                    >
                      New Registration
                    </Button>
                    <Button
                      variant="outlined"
                      startIcon={<SearchIcon />}
                      fullWidth
                      onClick={() => handleNavigation('search')}
                      sx={{ borderColor: appleColors.border, color: appleColors.text, borderRadius: 2, py: 2, textTransform: 'none', fontSize: '1rem' }}
                    >
                      Search Guest
                    </Button>
                  </Box>
                </Card>
              </Grid>
            </Grid>
          </Container>
        )
      case 'registration':
        return <FastRegistration onComplete={handleRegistrationComplete} onBack={() => handleNavigation('dashboard')} />
      case 'ticketRegistration':
        return <TicketRegistrationScreen registrationData={registrationData} onBack={() => handleNavigation('registration')} />
      case 'tickets':
        return <TicketsScreen />
      case 'search':
        return <SearchScreen />
      case 'device':
        return <DeviceControlScreen />
      case 'pricing':
        return <TicketPricingScreen />
      default:
        return (
          <Container maxWidth="xl" sx={{ py: 4 }}>
            <Typography variant="h4" sx={{ fontWeight: 300, color: appleColors.text }}>
              Coming Soon
            </Typography>
          </Container>
        )
    }
  }

  const navItems = [
    { key: 'dashboard', label: 'Dashboard', icon: <DashboardIcon /> },
    { key: 'registration', label: 'Registration', icon: <PersonAddIcon /> },
    { key: 'tickets', label: 'Tickets', icon: <ConfirmationNumberIcon /> },
    { key: 'search', label: 'Search', icon: <SearchIcon /> },
    { key: 'device', label: 'Device Control', icon: <SettingsIcon /> },
    { key: 'pricing', label: 'Ticket Pricing', icon: <AttachMoneyIcon /> },
  ]

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh', bgcolor: appleColors.background }}>
      {/* Left Sidebar */}
      <Box sx={{
        width: 250,
        bgcolor: appleColors.surface,
        borderRight: `1px solid ${appleColors.border}`,
        p: 3
      }}>
        <Typography variant="h5" sx={{
          fontWeight: 700,
          color: appleColors.text,
          mb: 4,
          textAlign: 'center'
        }}>
          EventShield Pro
        </Typography>

        <List>
          {navItems.map(({ key, label, icon }) => (
            <ListItem
              key={key}
              onClick={() => handleNavigation(key)}
              sx={{
                mb: 1,
                borderRadius: 2,
                cursor: 'pointer',
                bgcolor: currentScreen === key ? appleColors.primary : 'transparent',
                color: currentScreen === key ? 'white' : appleColors.text,
                '&:hover': {
                  bgcolor: currentScreen === key ? appleColors.primary : appleColors.background
                }
              }}
            >
              <ListItemIcon sx={{ color: 'inherit' }}>
                {icon}
              </ListItemIcon>
              <ListItemText primary={label} />
            </ListItem>
          ))}
        </List>
      </Box>

      {/* Main Content */}
      <Box sx={{ flex: 1, bgcolor: appleColors.surface }}>
        {renderScreen()}
      </Box>

      {/* Snackbar for notifications */}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar({ ...snackbar, open: false })}
      >
        <Alert onClose={() => setSnackbar({ ...snackbar, open: false })} severity={snackbar.severity}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  )
}

export default App
