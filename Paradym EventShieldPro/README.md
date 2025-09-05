# EventShield Pro

**Complete Enterprise SaaS Event Ticketing & Access Control Platform**

EventShield Pro is a comprehensive, multi-tenant SaaS platform that combines advanced event ticketing, facial recognition access control, and turnstile management with a built-in licensing system for commercial sales.

## 🚀 Features

### Core System
- **Flask Backend**: RESTful API with Azure SQL integration
- **React Frontend**: Modern web dashboard with real-time updates
- **React Native Mobile Apps**: iOS and Android applications
- **Electron Terminal Software**: Desktop access control interface
- **Azure SQL Database**: Multi-tenant, scalable database architecture
- **Hardware Integration**: DS-F881 facial recognition, DSN-50P turnstiles

### Licensing Platform
- **Customer Management**: Multi-tenant architecture
- **Subscription Billing**: Stripe integration with webhooks
- **License Validation**: Real-time license checking
- **Usage Tracking**: Comprehensive analytics and reporting
- **White-Label Options**: Customizable branding

### Business Tiers
- **Trial**: Free tier with limited features
- **Standard**: $99/month - Core features
- **Premium**: $299/month - Advanced features + priority support
- **Enterprise**: Custom pricing - Full customization + dedicated support

## 🏗️ Architecture

```
EventShield Pro/
├── backend/                 # Flask API server
├── frontend/                # React web application
├── mobile/                  # React Native apps
├── terminal/                # Electron desktop app
├── database/                # Database schemas and migrations
├── docs/                    # API documentation
├── deployment/              # Docker, Azure, deployment configs
├── licensing/               # License management system
└── hardware/                # Hardware integration modules
```

## 🛠️ Technology Stack

### Backend
- **Framework**: Flask 2.3+
- **Database**: Azure SQL Database
- **Authentication**: JWT + OAuth2
- **API**: RESTful with OpenAPI/Swagger
- **Queue**: Redis + Celery
- **Cache**: Redis

### Frontend
- **Framework**: React 18 + TypeScript
- **State Management**: Redux Toolkit + RTK Query
- **UI Library**: Material-UI (MUI) v5
- **Styling**: Emotion + CSS-in-JS
- **Build Tool**: Vite

### Mobile
- **Framework**: React Native 0.72+
- **Navigation**: React Navigation 6
- **State Management**: Zustand
- **UI Components**: React Native Elements

### Terminal
- **Framework**: Electron 25+
- **Frontend**: React + TypeScript
- **Hardware**: Serial/USB communication

## 🚀 Quick Start

### Prerequisites
- Python 3.11+
- Node.js 18+
- Docker & Docker Compose
- Azure CLI (for deployment)
- Xcode (for iOS development)
- Android Studio (for Android development)

### Local Development Setup

1. **Clone and Setup**
   ```bash
   git clone <repository-url>
   cd EventShieldPro
   ```

2. **Backend Setup**
   ```bash
   cd backend
   python -m venv venv
   source venv/bin/activate  # On Windows: venv\Scripts\activate
   pip install -r requirements.txt
   python manage.py run
   ```

3. **Frontend Setup**
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

4. **Database Setup**
   ```bash
   cd database
   docker-compose up -d
   ```

5. **Mobile Setup**
   ```bash
   cd mobile
   npm install
   npx react-native run-ios     # For iOS
   npx react-native run-android # For Android
   ```

6. **Terminal Setup**
   ```bash
   cd terminal
   npm install
   npm run electron-dev
   ```

## 📱 Mobile Apps

### iOS
- Minimum iOS version: 13.0
- Supports iPhone and iPad
- Face ID integration
- Push notifications

### Android
- Minimum API level: 21 (Android 5.0)
- Supports phones and tablets
- Biometric authentication
- FCM notifications

## 🔧 Hardware Integration

### DS-F881 Facial Recognition
- Real-time face detection
- Multi-face tracking
- Access control integration
- Event logging

### DSN-50P Turnstiles
- Serial communication
- Access validation
- Emergency override
- Maintenance monitoring

## 💳 Licensing System

### Subscription Management
- Stripe integration for payments
- Webhook handling for subscription updates
- Usage-based billing
- Automatic license renewal

### Multi-Tenant Architecture
- Isolated customer environments
- Custom branding options
- Resource allocation per tier
- Usage analytics

## 🚀 Deployment

### Azure Deployment
- Azure App Service for backend
- Azure Static Web Apps for frontend
- Azure SQL Database
- Azure Redis Cache
- Azure Container Registry

### Docker Support
- Multi-stage builds
- Production-optimized images
- Health checks
- Environment-specific configs

## 📊 Monitoring & Analytics

- Real-time system health
- Performance metrics
- Usage analytics
- Error tracking
- Audit logs

## 🔒 Security Features

- JWT authentication
- Role-based access control (RBAC)
- API rate limiting
- Data encryption at rest
- HTTPS enforcement
- CORS configuration

## 📚 API Documentation

Comprehensive API documentation available at `/docs` endpoint when running the backend server.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is proprietary software. All rights reserved.

## 🆘 Support

- **Documentation**: [docs.eventshieldpro.com](https://docs.eventshieldpro.com)
- **Support Portal**: [support.eventshieldpro.com](https://support.eventshieldpro.com)
- **Email**: support@eventshieldpro.com
- **Phone**: +1 (555) 123-4567

---

**EventShield Pro** - Secure, Scalable, Enterprise-Grade Event Management
