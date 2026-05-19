import React from 'react'
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native'
import { colors } from '../utils/colors'

interface Ticket {
  id: number
  ticket_number: string
  status: string
  final_price: string
}

interface Props { ticket: Ticket; onPress?: () => void }

const statusColor = (s: string) => {
  if (s === 'valid' || s === 'paid') return colors.success
  if (s === 'used') return colors.textSec
  if (s === 'cancelled' || s === 'expired') return colors.error
  return colors.warning
}

export default function TicketCard({ ticket, onPress }: Props) {
  return (
    <TouchableOpacity style={styles.card} onPress={onPress} activeOpacity={0.7}>
      <View style={styles.row}>
        <Text style={styles.number}>{ticket.ticket_number}</Text>
        <View style={[styles.badge, { backgroundColor: statusColor(ticket.status) + '22' }]}>
          <Text style={[styles.badgeText, { color: statusColor(ticket.status) }]}>
            {ticket.status.toUpperCase()}
          </Text>
        </View>
      </View>
      <Text style={styles.price}>${ticket.final_price}</Text>
    </TouchableOpacity>
  )
}

const styles = StyleSheet.create({
  card: {
    backgroundColor: colors.surface, borderRadius: 12, padding: 16,
    marginHorizontal: 16, marginVertical: 6,
    shadowColor: '#000', shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06, shadowRadius: 4, elevation: 2,
  },
  row: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 4 },
  number: { fontSize: 15, fontWeight: '600', color: colors.text },
  price: { fontSize: 13, color: colors.textSec },
  badge: { borderRadius: 6, paddingHorizontal: 8, paddingVertical: 2 },
  badgeText: { fontSize: 11, fontWeight: '600' },
})
