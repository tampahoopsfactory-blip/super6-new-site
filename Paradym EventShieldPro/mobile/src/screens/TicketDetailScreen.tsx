import React from 'react'
import { View, Text, StyleSheet, TouchableOpacity, Alert } from 'react-native'
import { useRoute, useNavigation } from '@react-navigation/native'
import { ticketsApi } from '../services/api'
import { colors } from '../utils/colors'

export default function TicketDetailScreen() {
  const route = useRoute<any>()
  const navigation = useNavigation()
  const { ticket } = route.params

  const handleValidate = async () => {
    try {
      await ticketsApi.validate(ticket.id)
      Alert.alert('Success', 'Ticket validated — entry granted.')
      navigation.goBack()
    } catch (err: any) {
      Alert.alert('Failed', err.response?.data?.error ?? 'Could not validate ticket.')
    }
  }

  return (
    <View style={styles.container}>
      <View style={styles.card}>
        <Text style={styles.number}>{ticket.ticket_number}</Text>
        <View style={styles.row}>
          <Text style={styles.label}>Status</Text>
          <Text style={[styles.value, { color: ticket.status === 'valid' ? colors.success : colors.textSec }]}>
            {ticket.status?.toUpperCase()}
          </Text>
        </View>
        <View style={styles.row}>
          <Text style={styles.label}>Price</Text>
          <Text style={styles.value}>${ticket.final_price}</Text>
        </View>
        <View style={styles.row}>
          <Text style={styles.label}>Currency</Text>
          <Text style={styles.value}>{ticket.currency}</Text>
        </View>
        {ticket.qr_code && (
          <View style={styles.row}>
            <Text style={styles.label}>QR Code</Text>
            <Text style={[styles.value, { fontSize: 11 }]}>{ticket.qr_code}</Text>
          </View>
        )}
      </View>

      {(ticket.status === 'valid' || ticket.status === 'paid') && (
        <TouchableOpacity style={styles.validateBtn} onPress={handleValidate}>
          <Text style={styles.validateText}>Grant Entry</Text>
        </TouchableOpacity>
      )}
    </View>
  )
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.bg, padding: 16 },
  card: { backgroundColor: colors.surface, borderRadius: 16, padding: 20, marginBottom: 16 },
  number: { fontSize: 20, fontWeight: '700', color: colors.text, marginBottom: 16 },
  row: { flexDirection: 'row', justifyContent: 'space-between', paddingVertical: 10, borderBottomWidth: 1, borderBottomColor: colors.border },
  label: { fontSize: 14, color: colors.textSec },
  value: { fontSize: 14, fontWeight: '600', color: colors.text },
  validateBtn: { backgroundColor: colors.success, borderRadius: 12, height: 52, alignItems: 'center', justifyContent: 'center' },
  validateText: { color: '#fff', fontSize: 16, fontWeight: '700' },
})
