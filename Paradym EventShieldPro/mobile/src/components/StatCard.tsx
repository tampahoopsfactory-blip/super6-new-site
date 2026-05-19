import React from 'react'
import { View, Text, StyleSheet } from 'react-native'
import { colors } from '../utils/colors'

interface Props { label: string; value: string | number; accent?: string }

export default function StatCard({ label, value, accent = colors.primary }: Props) {
  return (
    <View style={styles.card}>
      <Text style={[styles.value, { color: accent }]}>{value}</Text>
      <Text style={styles.label}>{label}</Text>
    </View>
  )
}

const styles = StyleSheet.create({
  card: {
    flex: 1, backgroundColor: colors.surface, borderRadius: 12,
    padding: 16, margin: 6, alignItems: 'center',
    shadowColor: '#000', shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06, shadowRadius: 4, elevation: 2,
  },
  value: { fontSize: 28, fontWeight: '700', marginBottom: 4 },
  label: { fontSize: 12, color: colors.textSec, textAlign: 'center' },
})
