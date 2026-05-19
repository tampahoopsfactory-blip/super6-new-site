import React, { useState } from 'react'
import {
  View, Text, TextInput, TouchableOpacity, StyleSheet,
  KeyboardAvoidingView, Platform, Alert, ActivityIndicator
} from 'react-native'
import { useAuth } from '../contexts/AuthContext'
import { colors } from '../utils/colors'

export default function LoginScreen() {
  const { login } = useAuth()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [tenantSlug, setTenantSlug] = useState('demo')
  const [loading, setLoading] = useState(false)

  const handleLogin = async () => {
    if (!email || !password) {
      Alert.alert('Required', 'Please enter your email and password.')
      return
    }
    setLoading(true)
    try {
      await login(email, password, tenantSlug)
    } catch (err: any) {
      Alert.alert('Login Failed', err.response?.data?.error ?? 'Check your credentials and try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <View style={styles.card}>
        <Text style={styles.title}>EventShield Pro</Text>
        <Text style={styles.subtitle}>Sign in to continue</Text>

        <TextInput style={styles.input} placeholder="Email" placeholderTextColor={colors.textSec}
          value={email} onChangeText={setEmail} autoCapitalize="none" keyboardType="email-address" />
        <TextInput style={styles.input} placeholder="Password" placeholderTextColor={colors.textSec}
          value={password} onChangeText={setPassword} secureTextEntry />
        <TextInput style={styles.input} placeholder="Organization slug" placeholderTextColor={colors.textSec}
          value={tenantSlug} onChangeText={setTenantSlug} autoCapitalize="none" />

        <TouchableOpacity style={styles.button} onPress={handleLogin} disabled={loading}>
          {loading
            ? <ActivityIndicator color="#fff" />
            : <Text style={styles.buttonText}>Sign In</Text>}
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  )
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.bg, alignItems: 'center', justifyContent: 'center', padding: 24 },
  card: { width: '100%', maxWidth: 400, backgroundColor: colors.surface, borderRadius: 16, padding: 28 },
  title: { fontSize: 22, fontWeight: '700', color: colors.text, marginBottom: 4 },
  subtitle: { fontSize: 14, color: colors.textSec, marginBottom: 24 },
  input: {
    height: 48, borderWidth: 1, borderColor: colors.border, borderRadius: 10,
    paddingHorizontal: 14, fontSize: 15, color: colors.text, marginBottom: 12,
  },
  button: {
    height: 50, backgroundColor: colors.primary, borderRadius: 10,
    alignItems: 'center', justifyContent: 'center', marginTop: 8,
  },
  buttonText: { color: '#fff', fontSize: 16, fontWeight: '600' },
})
