import { app, BrowserWindow, Menu, Tray, nativeImage, ipcMain, dialog, shell } from 'electron'
import { join } from 'path'
import { existsSync, readFileSync, writeFileSync, mkdirSync } from 'fs'
import Store from 'electron-store'
import SerialPort from 'serialport'

// Store configuration
const store = new Store({
  name: 'eventshield-terminal-config',
  defaults: {
    windowBounds: { width: 1200, height: 800 },
    hardware: {
      facialRecognition: {
        enabled: false,
        deviceId: '',
        ipAddress: '',
        port: 8000
      },
      turnstile: {
        enabled: false,
        deviceId: '',
        serialPort: '',
        baudRate: 9600
      }
    },
    server: {
      apiUrl: 'http://localhost:5000',
      tenantId: '',
      accessToken: ''
    },
    settings: {
      autoStart: false,
      minimizeToTray: true,
      enableLogging: true,
      logLevel: 'info'
    }
  }
})

// Global references
let mainWindow: BrowserWindow | null = null
let tray: Tray | null = null
let hardwareManager: HardwareManager | null = null

// Hardware manager class
class HardwareManager {
  private facialRecognitionPort: SerialPort | null = null
  private turnstilePort: SerialPort | null = null
  private isConnected = false

  constructor() {
    this.initializeHardware()
  }

  private async initializeHardware() {
    try {
      const config = store.get('hardware') as any

      // Initialize facial recognition device
      if (config.facialRecognition.enabled && config.facialRecognition.deviceId) {
        await this.connectFacialRecognition(config.facialRecognition)
      }

      // Initialize turnstile device
      if (config.turnstile.enabled && config.turnstile.serialPort) {
        await this.connectTurnstile(config.turnstile)
      }

      this.isConnected = true
      this.sendStatusToRenderer('hardware_connected', { connected: true })
    } catch (error) {
      console.error('Hardware initialization failed:', error)
      this.sendStatusToRenderer('hardware_error', { error: error.message })
    }
  }

  private async connectFacialRecognition(config: any) {
    try {
      // For DS-F881 device, we'll use TCP/IP connection
      if (config.ipAddress && config.port) {
        // Simulate connection for now
        console.log(`Connecting to facial recognition device at ${config.ipAddress}:${config.port}`)
        
        // In a real implementation, you would establish TCP connection
        // and handle device communication protocol
        
        this.sendStatusToRenderer('facial_recognition_connected', { connected: true })
      }
    } catch (error) {
      console.error('Facial recognition connection failed:', error)
      throw error
    }
  }

  private async connectTurnstile(config: any) {
    try {
      // For DSN-50P turnstile, we'll use serial connection
      if (config.serialPort) {
        this.turnstilePort = new SerialPort({
          path: config.serialPort,
          baudRate: config.baudRate || 9600,
          dataBits: 8,
          stopBits: 1,
          parity: 'none'
        })

        this.turnstilePort.on('open', () => {
          console.log('Turnstile connected successfully')
          this.sendStatusToRenderer('turnstile_connected', { connected: true })
        })

        this.turnstilePort.on('data', (data) => {
          this.handleTurnstileData(data)
        })

        this.turnstilePort.on('error', (error) => {
          console.error('Turnstile error:', error)
          this.sendStatusToRenderer('turnstile_error', { error: error.message })
        })
      }
    } catch (error) {
      console.error('Turnstile connection failed:', error)
      throw error
    }
  }

  private handleTurnstileData(data: Buffer) {
    try {
      const dataString = data.toString('utf8').trim()
      console.log('Turnstile data received:', dataString)

      // Parse turnstile data (example format: "ENTRY,USER123,2024-01-01T10:00:00Z")
      const parts = dataString.split(',')
      if (parts.length >= 3) {
        const action = parts[0] // ENTRY or EXIT
        const userId = parts[1]
        const timestamp = parts[2]

        this.sendStatusToRenderer('turnstile_event', {
          action,
          userId,
          timestamp,
          data: dataString
        })
      }
    } catch (error) {
      console.error('Error parsing turnstile data:', error)
    }
  }

  public async sendTurnstileCommand(command: string) {
    if (this.turnstilePort && this.turnstilePort.isOpen) {
      try {
        this.turnstilePort.write(command + '\n')
        console.log('Turnstile command sent:', command)
        return true
      } catch (error) {
        console.error('Failed to send turnstile command:', error)
        return false
      }
    }
    return false
  }

  public async emergencyOverride() {
    try {
      // Send emergency override command to turnstile
      if (this.turnstilePort) {
        await this.sendTurnstileCommand('EMERGENCY_OVERRIDE')
      }

      // Send emergency override command to facial recognition
      // This would be implemented based on the specific device protocol

      this.sendStatusToRenderer('emergency_override_activated', { timestamp: new Date().toISOString() })
      return true
    } catch (error) {
      console.error('Emergency override failed:', error)
      return false
    }
  }

  public async disconnect() {
    try {
      if (this.turnstilePort) {
        this.turnstilePort.close()
        this.turnstilePort = null
      }

      // Close facial recognition connection
      // This would be implemented based on the specific device protocol

      this.isConnected = false
      this.sendStatusToRenderer('hardware_disconnected', { connected: false })
    } catch (error) {
      console.error('Hardware disconnection failed:', error)
    }
  }

  private sendStatusToRenderer(channel: string, data: any) {
    if (mainWindow && !mainWindow.isDestroyed()) {
      mainWindow.webContents.send(channel, data)
    }
  }

  public getStatus() {
    return {
      connected: this.isConnected,
      facialRecognition: this.facialRecognitionPort !== null,
      turnstile: this.turnstilePort !== null && this.turnstilePort.isOpen
    }
  }
}

// Create main window
function createMainWindow() {
  const windowBounds = store.get('windowBounds') as any

  mainWindow = new BrowserWindow({
    width: windowBounds.width,
    height: windowBounds.height,
    minWidth: 800,
    minHeight: 600,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      enableRemoteModule: false,
      preload: join(__dirname, 'preload.js')
    },
    icon: join(__dirname, '../assets/icon.png'),
    titleBarStyle: 'default',
    show: false,
    autoHideMenuBar: true
  })

  // Load the app
  if (process.env.NODE_ENV === 'development') {
    mainWindow.loadURL('http://localhost:3000')
    mainWindow.webContents.openDevTools()
  } else {
    mainWindow.loadFile(join(__dirname, '../renderer/index.html'))
  }

  // Show window when ready
  mainWindow.once('ready-to-show', () => {
    mainWindow?.show()
    
    // Initialize hardware manager
    if (!hardwareManager) {
      hardwareManager = new HardwareManager()
    }
  })

  // Save window bounds
  mainWindow.on('resize', () => {
    if (mainWindow) {
      const bounds = mainWindow.getBounds()
      store.set('windowBounds', bounds)
    }
  })

  // Handle window close
  mainWindow.on('close', (event) => {
    const settings = store.get('settings') as any
    
    if (settings.minimizeToTray && tray) {
      event.preventDefault()
      mainWindow?.hide()
    } else {
      app.quit()
    }
  })

  // Handle window closed
  mainWindow.on('closed', () => {
    mainWindow = null
  })

  // Handle external links
  mainWindow.webContents.setWindowOpenHandler(({ url }) => {
    shell.openExternal(url)
    return { action: 'deny' }
  })
}

// Create system tray
function createTray() {
  const iconPath = join(__dirname, '../assets/tray-icon.png')
  const icon = nativeImage.createFromPath(iconPath)
  
  tray = new Tray(icon)
  tray.setToolTip('EventShield Pro Terminal')

  const contextMenu = Menu.buildFromTemplate([
    {
      label: 'Show Terminal',
      click: () => {
        if (mainWindow) {
          mainWindow.show()
          mainWindow.focus()
        }
      }
    },
    {
      label: 'Hardware Status',
      click: () => {
        if (hardwareManager) {
          const status = hardwareManager.getStatus()
          dialog.showMessageBox(mainWindow!, {
            type: 'info',
            title: 'Hardware Status',
            message: `Hardware Status:\nConnected: ${status.connected}\nFacial Recognition: ${status.facialRecognition}\nTurnstile: ${status.turnstile}`
          })
        }
      }
    },
    {
      label: 'Emergency Override',
      click: async () => {
        if (hardwareManager) {
          const result = await hardwareManager.emergencyOverride()
          if (result) {
            dialog.showMessageBox(mainWindow!, {
              type: 'info',
              title: 'Emergency Override',
              message: 'Emergency override activated successfully'
            })
          } else {
            dialog.showMessageBox(mainWindow!, {
              type: 'error',
              title: 'Emergency Override',
              message: 'Failed to activate emergency override'
            })
          }
        }
      }
    },
    { type: 'separator' },
    {
      label: 'Settings',
      click: () => {
        if (mainWindow) {
          mainWindow.show()
          mainWindow.focus()
          mainWindow.webContents.send('open_settings')
        }
      }
    },
    { type: 'separator' },
    {
      label: 'Quit',
      click: () => {
        app.quit()
      }
    }
  ])

  tray.setContextMenu(contextMenu)

  // Handle tray click
  tray.on('click', () => {
    if (mainWindow) {
      mainWindow.show()
      mainWindow.focus()
    }
  })
}

// IPC handlers
function setupIpcHandlers() {
  // Get hardware status
  ipcMain.handle('get-hardware-status', () => {
    if (hardwareManager) {
      return hardwareManager.getStatus()
    }
    return { connected: false, facialRecognition: false, turnstile: false }
  })

  // Send turnstile command
  ipcMain.handle('send-turnstile-command', async (event, command: string) => {
    if (hardwareManager) {
      return await hardwareManager.sendTurnstileCommand(command)
    }
    return false
  })

  // Emergency override
  ipcMain.handle('emergency-override', async () => {
    if (hardwareManager) {
      return await hardwareManager.emergencyOverride()
    }
    return false
  })

  // Get configuration
  ipcMain.handle('get-config', () => {
    return store.store
  })

  // Update configuration
  ipcMain.handle('update-config', (event, key: string, value: any) => {
    store.set(key, value)
    return true
  })

  // Get available serial ports
  ipcMain.handle('get-serial-ports', async () => {
    try {
      const ports = await SerialPort.list()
      return ports.map(port => ({
        path: port.path,
        manufacturer: port.manufacturer,
        serialNumber: port.serialNumber,
        pnpId: port.pnpId,
        locationId: port.locationId,
        productId: port.productId,
        vendorId: port.vendorId
      }))
    } catch (error) {
      console.error('Failed to get serial ports:', error)
      return []
    }
  })

  // Test hardware connection
  ipcMain.handle('test-hardware-connection', async (event, config: any) => {
    try {
      // Test turnstile connection
      if (config.turnstile.enabled && config.turnstile.serialPort) {
        const testPort = new SerialPort({
          path: config.turnstile.serialPort,
          baudRate: config.turnstile.baudRate || 9600
        })

        return new Promise((resolve) => {
          testPort.on('open', () => {
            testPort.close()
            resolve({ success: true, message: 'Hardware connection test successful' })
          })

          testPort.on('error', (error) => {
            resolve({ success: false, message: `Hardware connection test failed: ${error.message}` })
          })

          // Timeout after 5 seconds
          setTimeout(() => {
            testPort.close()
            resolve({ success: false, message: 'Hardware connection test timed out' })
          }, 5000)
        })
      }

      return { success: true, message: 'No hardware to test' }
    } catch (error) {
      return { success: false, message: `Hardware connection test failed: ${error.message}` }
    }
  })

  // Open file dialog
  ipcMain.handle('open-file-dialog', async (event, options: any) => {
    const result = await dialog.showOpenDialog(mainWindow!, options)
    return result
  })

  // Save file dialog
  ipcMain.handle('save-file-dialog', async (event, options: any) => {
    const result = await dialog.showSaveDialog(mainWindow!, options)
    return result
  })

  // Show message box
  ipcMain.handle('show-message-box', async (event, options: any) => {
    const result = await dialog.showMessageBox(mainWindow!, options)
    return result
  })
}

// App event handlers
app.whenReady().then(() => {
  createMainWindow()
  createTray()
  setupIpcHandlers()

  // Handle macOS activation
  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createMainWindow()
    } else if (mainWindow) {
      mainWindow.show()
      mainWindow.focus()
    }
  })
})

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

app.on('before-quit', async () => {
  if (hardwareManager) {
    await hardwareManager.disconnect()
  }
})

// Handle uncaught exceptions
process.on('uncaughtException', (error) => {
  console.error('Uncaught Exception:', error)
  
  if (mainWindow) {
    dialog.showErrorBox('Application Error', `An unexpected error occurred:\n${error.message}`)
  }
  
  app.quit()
})

// Handle unhandled promise rejections
process.on('unhandledRejection', (reason, promise) => {
  console.error('Unhandled Rejection at:', promise, 'reason:', reason)
  
  if (mainWindow) {
    dialog.showErrorBox('Application Error', `An unhandled promise rejection occurred:\n${reason}`)
  }
})

// Security: Prevent new window creation
app.on('web-contents-created', (event, contents) => {
  contents.on('new-window', (event, navigationUrl) => {
    event.preventDefault()
    shell.openExternal(navigationUrl)
  })
})

