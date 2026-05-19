import { contextBridge, ipcRenderer } from 'electron'

contextBridge.exposeInMainWorld('electronAPI', {
  getHardwareStatus: () => ipcRenderer.invoke('get-hardware-status'),
  sendTurnstileCommand: (command: string) => ipcRenderer.invoke('send-turnstile-command', command),
  emergencyOverride: () => ipcRenderer.invoke('emergency-override'),
  getConfig: () => ipcRenderer.invoke('get-config'),
  updateConfig: (key: string, value: unknown) => ipcRenderer.invoke('update-config', key, value),
  getSerialPorts: () => ipcRenderer.invoke('get-serial-ports'),

  // Subscribe to hardware events from main process
  onHardwareEvent: (channel: string, callback: (data: unknown) => void) => {
    const validChannels = [
      'hardware_connected', 'hardware_disconnected', 'hardware_error',
      'facial_recognition_connected', 'turnstile_connected', 'turnstile_error',
      'turnstile_event', 'emergency_override_activated'
    ]
    if (validChannels.includes(channel)) {
      const listener = (_event: Electron.IpcRendererEvent, data: unknown) => callback(data)
      ipcRenderer.on(channel, listener)
      return () => ipcRenderer.removeListener(channel, listener)
    }
    return () => {}
  }
})
