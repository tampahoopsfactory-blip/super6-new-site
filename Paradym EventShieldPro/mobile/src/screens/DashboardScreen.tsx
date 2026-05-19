import React, { useState, useEffect, useCallback } from 'react'
import { View, Text, ScrollView, StyleSheet, RefreshControl } from 'react-native'
import { analyticsApi } from '../services/api'
import { useAuth } from '../contexts/AuthContext'
import StatCard from '../components/StatCard'
import { colors } from '../utils/colors'

interface Stats {
  today: { entries_granted: number; entries_denied: number; tickets_issued: number; revenue: number }
  devices_online: number
}

export default function DashboardScreen() {
  const { user } = useAuth()
  const [stats, setStats] = useState<Stats | null>(null)
  const [refreshing, setRefreshing] = useState(false)

  const load = useCallback(async () => {
    try {
      const res = await analyticsApi.dashboard()
      setStats(res.data)
    } catch { /* ignore — show zeros */ }
  }, [])

  useEffect(() => { load() }, [load])

  const onRefresh = async () => {
    setRefreshing(true)
    await load()
    setRefreshing(false)
  }

  return (
    <ScrollView style={styles.container} refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} />}>
      <View style={styles.header}>
        <Text style={styles.greeting}>Welcome, {user?.first_name}</Text>
        <Text style={styles.date}>{new Date().toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric' })}</Text>
      </View>

      <View style={styles.grid}>
        <StatCard label="Entries Granted" value={stats?.today.entries_granted ?? 0} accent={colors.success} />
        <StatCard label="Entries Denied" value={stats?.today.entries_denied ?? 0} accent={colors.error} />
      </View>
      <View style={styles.grid}>
        <StatCard label="Tickets Issued" value={stats?.today.tickets_issued ?? 0} accent={colors.primary} />
        <StatCard label="Devices Online" value={stats?.devices_online ?? 0} accent={colors.warning} />
      </View>
    </ScrollView>
  )
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.bg },
  header: { padding: 20, paddingTop: 8 },
  greeting: { fontSize: 22, fontWeight: '700', color: colors.text },
  date: { fontSize: 13, color: colors.textSec, marginTop: 2 },
  grid: { flexDirection: 'row', paddingHorizontal: 10 },
})
