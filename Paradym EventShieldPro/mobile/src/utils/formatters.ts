export const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })

export const formatTime = (iso: string) =>
  new Date(iso).toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' })

export const formatPrice = (cents: number) =>
  `$${(cents / 100).toFixed(2)}`

export const formatPhone = (digits: string) => {
  const d = digits.replace(/\D/g, '')
  if (d.length <= 3) return `(${d}`
  if (d.length <= 6) return `(${d.slice(0,3)}) ${d.slice(3)}`
  return `(${d.slice(0,3)}) ${d.slice(3,6)}-${d.slice(6,10)}`
}
