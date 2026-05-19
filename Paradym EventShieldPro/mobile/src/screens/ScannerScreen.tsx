import React, { useState } from 'react'
import { View, Text, StyleSheet, TouchableOpacity, Alert } from 'react-native'
import { colors } from '../utils/colors'

// QR scanner requires a native build — this is the JS-side shell
// react-native-qrcode-scanner is in package.json for when the native build runs
let QRCodeScanner: any = null
try {
  QRCodeScanner = require('react-native-qrcode-scanner').default
} catch { /* not available in JS-only environments */ }

export default function ScannerScreen() {
  const [scanned, setScanned] = useState(false)

  const handleScan = ({ data }: { data: string }) => {
    if (scanned) return
    setScanned(true)
    Alert.alert(
      'QR Code Scanned',
      `Code: ${data}`,
      [{ text: 'Scan Again', onPress: () => setScanned(false) }]
    )
  }

  if (!QRCodeScanner) {
    return (
      <View style={styles.fallback}>
        <Text style={styles.fallbackTitle}>Camera Scanner</Text>
        <Text style={styles.fallbackText}>
          QR scanning requires a native build.{'\n'}Run `npx react-native run-ios` or `run-android` to enable.
        </Text>
      </View>
    )
  }

  return (
    <QRCodeScanner
      onRead={handleScan}
      reactivate={!scanned}
      reactivateTimeout={2000}
      topContent={<Text style={styles.scanHint}>Point camera at ticket QR code</Text>}
      bottomContent={
        scanned ? (
          <TouchableOpacity style={styles.resetBtn} onPress={() => setScanned(false)}>
            <Text style={styles.resetText}>Scan Again</Text>
          </TouchableOpacity>
        ) : null
      }
    />
  )
}

const styles = StyleSheet.create({
  fallback: { flex: 1, backgroundColor: colors.bg, alignItems: 'center', justifyContent: 'center', padding: 32 },
  fallbackTitle: { fontSize: 20, fontWeight: '700', color: colors.text, marginBottom: 8 },
  fallbackText: { fontSize: 14, color: colors.textSec, textAlign: 'center', lineHeight: 22 },
  scanHint: { fontSize: 14, color: colors.textSec, marginBottom: 16 },
  resetBtn: { backgroundColor: colors.primary, borderRadius: 10, paddingHorizontal: 24, paddingVertical: 12, marginTop: 16 },
  resetText: { color: '#fff', fontWeight: '600' },
})
