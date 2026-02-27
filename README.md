# EventShield Pro

Smart Event Access Control System integrating Square Terminal payments, F8881 facial recognition devices, and turnstile hardware for basketball event venues.

## Architecture

```
Square Terminal ──webhook──> Local Server ──TCP──> F8881 Devices ──relay──> Turnstiles
                                  │
                            Admin Dashboard
                           (React + FastAPI)
```

**Stack:**
- **Backend:** Python 3.11+ / FastAPI / SQLAlchemy / APScheduler
- **Frontend:** React 18 / Vite / Lucide Icons
- **Database:** SQLite (dev) / PostgreSQL (production)
- **Hardware:** DS-F8881 facial recognition / DSN-50P turnstiles
- **Services:** Square (payments), Twilio (SMS), SendGrid (email)

## Quick Start

### Prerequisites
- Python 3.10+
- Node.js 18+
- Git

### Setup

```bash
# Clone and run setup
cd eventshield-pro
chmod +x scripts/setup.sh
./scripts/setup.sh

# Edit credentials
nano backend/.env

# Start servers
./scripts/start.sh
```

**Windows:**
```cmd
scripts\setup.bat
# Edit backend\.env
scripts\start.bat
```

**Docker:**
```bash
cp backend/.env.example backend/.env
# Edit backend/.env with your credentials
docker-compose up --build
```

### Access
- **Dashboard:** http://localhost:3000 (dev) or http://localhost:8000 (production)
- **API Docs:** http://localhost:8000/docs
- **Default Login:** admin / changeme123

## Ticket Types

The system supports differentiated ticket types, each with automatic expiry rules:

| Type | Expiry Rule | Re-entry |
|------|-------------|----------|
| **DAILY** | End of calendar day (23:59) | Yes |
| **WEEKEND** | End of Sunday (23:59) | Yes |
| **SEASON** | Configurable season end date | Yes |
| **VIP** | Event expiry hours setting | Yes |
| **STAFF** | Never expires | Yes |
| **COACH** | Never expires | Yes |
| **GENERAL** | Event expiry hours setting | No (single entry) |
| **COMPLIMENTARY** | When event ends | No (single entry) |

## Patron Flow

1. **Payment** — Patron pays at Square Terminal
2. **QR Delivery** — System generates QR code, sends via screen/SMS/email
3. **First Entry** — Patron scans QR at F8881 device; face is enrolled
4. **Re-entry** — Face recognition grants access without QR code
5. **All entries logged** with timestamps, gate IDs, and confidence scores

## Admin Dashboard (6 Pages)

1. **Live Monitor** — Real-time stats, access feed, device status
2. **Tickets** — Search, filter, view QR, refund, manual override
3. **Access Log** — Full searchable log with CSV export
4. **Events** — Create, activate, end events
5. **Devices** — Register F8881 units, test connections, sync faces
6. **Settings** — SMS/email toggles, Square credentials, admission rules

## API Endpoints

### Access Control (called by F8881 devices)
```
POST /api/validate_ticket   — QR code scan validation
POST /api/enroll_complete    — Face enrollment callback
POST /api/face_entry         — Face recognition entry
```

### Webhooks
```
POST /api/webhooks/square    — Square payment webhook
```

### Events
```
GET    /api/events           — List all events
POST   /api/events           — Create event
POST   /api/events/:id/activate — Activate event
POST   /api/events/:id/end   — End event
```

### Tickets
```
GET    /api/tickets           — List tickets (filterable)
POST   /api/tickets           — Create manual ticket
POST   /api/tickets/:id/refund — Refund ticket
POST   /api/tickets/:id/override — Manual access override
GET    /api/tickets/:id/qr   — Get QR code image
```

### Devices
```
GET    /api/devices           — List F8881 devices
POST   /api/devices           — Register device
POST   /api/devices/:id/test — Test connection
POST   /api/devices/:id/clear-faces — Clear device faces
POST   /api/devices/:id/sync-faces  — Re-push enrolled faces
```

### Dashboard
```
GET    /api/dashboard/stats   — Live statistics
GET    /api/dashboard/recent-access — Last 20 events
GET    /api/dashboard/access-log — Full paginated log
```

### Settings & Health
```
GET    /api/settings          — Get all settings
PUT    /api/settings          — Update setting
POST   /api/settings/test-sms — Send test SMS
POST   /api/settings/test-email — Send test email
GET    /health                — Server health check
```

## Network Setup

Recommended static IP assignments on dedicated LAN:

| Device | IP Address |
|--------|-----------|
| Local Server | 192.168.1.10 |
| F8881 Device #1 | 192.168.1.21 |
| F8881 Device #2 | 192.168.1.22 |
| Square Terminal | DHCP on same LAN |

## Environment Variables

See `backend/.env.example` for all configuration options including:
- Square API credentials
- Twilio SMS credentials
- SendGrid email credentials
- Database connection string
- Encryption keys
- Cloud sync settings

## Background Jobs

| Job | Interval | Purpose |
|-----|----------|---------|
| Device Health | 30s | Ping F8881 devices, update status |
| Cloud Sync | 60s | Push unsynced records to cloud |
| Event Expiry | 5min | End events past their end_time |
| Delivery Retry | 2min | Retry failed SMS/email (up to 3x) |

## Security

- JWT authentication for admin dashboard
- API key verification for device endpoints
- Device IP whitelisting
- QR tokens hashed with SHA-256 in database
- PII (phone, email) encrypted with AES-256 at rest
- Square webhook signature verification
- CORS configuration
- Rotating log files (10MB, 7-day retention)

## Project Structure

```
eventshield-pro/
├── backend/
│   ├── app/
│   │   ├── api/          # FastAPI route handlers
│   │   ├── models/       # SQLAlchemy database models
│   │   ├── services/     # F8881, Square, delivery services
│   │   ├── jobs/         # Background job scheduler
│   │   ├── utils/        # Crypto, QR, auth helpers
│   │   ├── config.py     # Environment configuration
│   │   ├── database.py   # Database engine setup
│   │   └── main.py       # FastAPI application entry
│   ├── requirements.txt
│   ├── run.py
│   └── .env.example
├── frontend/
│   ├── src/
│   │   ├── pages/        # 6 dashboard pages
│   │   ├── components/   # Layout, sidebar
│   │   ├── hooks/        # usePolling
│   │   └── utils/        # API client
│   ├── package.json
│   └── vite.config.js
├── scripts/              # Setup and start scripts
├── docker-compose.yml
├── Dockerfile
└── README.md
```
