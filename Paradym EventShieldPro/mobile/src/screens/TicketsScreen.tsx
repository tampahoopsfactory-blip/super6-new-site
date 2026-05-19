import React, { useState, useEffect, useCallback } from 'react'
import {
  View, Text, FlatList, StyleSheet, RefreshControl,
  TextInput, TouchableOpacity
} from 'react-native'
import { useNavigation } from '@react-navigation/native'
import { ticketsApi } from '../services/api'
import TicketCard from '../components/TicketCard'
import { colors } from '../utils/colors'

export default function TicketsScreen() {
  const navigation = useNavigation<any>()
  const [tickets, setTickets] = useState<any[]>([])
  const [refreshing, setRefreshing] = useState(false)
  const [search, setSearch] = useState('')

  const load = useCallback(async () => {
    try {
      const res = await ticketsApi.list()
      setTickets(res.data.tickets ?? [])
    } catch { /* keep existing */ }
  }, [])

  useEffect(() => { load() }, [load])

  const onRefresh = async () => {
    setRefreshing(true)
    await load()
    setRefreshing(false)
  }

  const filtered = tickets.filter(t =>
    !search || t.ticket_number?.toLowerCase().includes(search.toLowerCase())
  )

  return (
    <View style={styles.container}>
      <View style={styles.searchBar}>
        <TextInput
          style={styles.searchInput}
          placeholder="Search ticket number..."
          placeholderTextColor={colors.textSec}
          value={search}
          onChangeText={setSearch}
        />
      </View>
      <FlatList
        data={filtered}
        keyExtractor={item => String(item.id)}
        renderItem={({ item }) => (
          <TicketCard
            ticket={item}
            onPress={() => navigation.navigate('TicketDetail', { ticket: item })}
          />
        )}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}
        ListEmptyComponent={
          <View style={styles.empty}>
            <Text style={styles.emptyText}>{search ? 'No tickets match your search' : 'No tickets yet'}</Text>
          </View>
        }
        contentContainerStyle={{ paddingBottom: 24 }}
      />
    </View>
  )
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.bg },
  searchBar: { padding: 12, paddingBottom: 4 },
  searchInput: {
    height: 44, backgroundColor: colors.surface, borderRadius: 10,
    paddingHorizontal: 14, fontSize: 14, color: colors.text,
    borderWidth: 1, borderColor: colors.border,
  },
  empty: { alignItems: 'center', paddingTop: 60 },
  emptyText: { color: colors.textSec, fontSize: 15 },
})
