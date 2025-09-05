# EventShield Pro - Makefile
# Comprehensive build and management commands

.PHONY: help setup build run stop clean test deploy docs

# Default target
help:
	@echo "EventShield Pro - Available Commands:"
	@echo ""
	@echo "Setup & Installation:"
	@echo "  setup          - Initial project setup and dependencies"
	@echo "  setup-backend  - Setup Python backend environment"
	@echo "  setup-frontend - Setup Node.js frontend environment"
	@echo "  setup-mobile   - Setup React Native mobile environment"
	@echo "  setup-terminal - Setup Electron terminal environment"
	@echo ""
	@echo "Development:"
	@echo "  dev            - Start all services in development mode"
	@echo "  dev-backend    - Start backend API server"
	@echo "  dev-frontend   - Start frontend development server"
	@echo "  dev-mobile     - Start mobile development server"
	@echo "  dev-terminal   - Start terminal application"
	@echo ""
	@echo "Building:"
	@echo "  build          - Build all components"
	@echo "  build-backend  - Build backend Docker image"
	@echo "  build-frontend - Build frontend production bundle"
	@echo "  build-mobile   - Build mobile apps"
	@echo "  build-terminal - Build terminal application"
	@echo ""
	@echo "Database:"
	@echo "  db-up          - Start database services"
	@echo "  db-down        - Stop database services"
	@echo "  db-reset       - Reset database with sample data"
	@echo "  db-migrate     - Run database migrations"
	@echo ""
	@echo "Testing:"
	@echo "  test           - Run all tests"
	@echo "  test-backend   - Run backend tests"
	@echo "  test-frontend  - Run frontend tests"
	@echo "  test-e2e       - Run end-to-end tests"
	@echo ""
	@echo "Deployment:"
	@echo "  deploy         - Deploy to production"
	@echo "  deploy-staging - Deploy to staging"
	@echo "  deploy-azure   - Deploy to Azure"
	@echo ""
	@echo "Maintenance:"
	@echo "  clean          - Clean build artifacts and containers"
	@echo "  logs           - View application logs"
	@echo "  monitor        - Open monitoring dashboards"
	@echo "  backup         - Create database backup"
	@echo ""
	@echo "Documentation:"
	@echo "  docs           - Generate API documentation"
	@echo "  docs-serve     - Serve documentation locally"

# =============================================================================
# SETUP & INSTALLATION
# =============================================================================

setup: setup-backend setup-frontend setup-mobile setup-terminal
	@echo "✅ EventShield Pro setup complete!"

setup-backend:
	@echo "🐍 Setting up Python backend environment..."
	cd backend && python -m venv venv
	cd backend && source venv/bin/activate && pip install -r requirements.txt
	@echo "✅ Backend setup complete!"

setup-frontend:
	@echo "⚛️  Setting up Node.js frontend environment..."
	cd frontend && npm install
	@echo "✅ Frontend setup complete!"

setup-mobile:
	@echo "📱 Setting up React Native mobile environment..."
	cd mobile && npm install
	@echo "✅ Mobile setup complete!"

setup-terminal:
	@echo "🖥️  Setting up Electron terminal environment..."
	cd terminal && npm install
	@echo "✅ Terminal setup complete!"

# =============================================================================
# DEVELOPMENT
# =============================================================================

dev: db-up
	@echo "🚀 Starting EventShield Pro development environment..."
	docker-compose up -d backend frontend mobile-dev terminal hardware-simulator
	@echo "✅ Development environment started!"
	@echo "🌐 Frontend: http://localhost:3000"
	@echo "🔧 Backend API: http://localhost:5000"
	@echo "📱 Mobile Dev: http://localhost:8081"
	@echo "📊 Grafana: http://localhost:3001 (admin/admin)"
	@echo "📧 MailHog: http://localhost:8025"

dev-backend: db-up
	@echo "🔧 Starting backend API server..."
	docker-compose up -d backend
	@echo "✅ Backend running at http://localhost:5000"

dev-frontend:
	@echo "⚛️  Starting frontend development server..."
	docker-compose up -d frontend
	@echo "✅ Frontend running at http://localhost:3000"

dev-mobile:
	@echo "📱 Starting mobile development server..."
	docker-compose up -d mobile-dev
	@echo "✅ Mobile dev server running at http://localhost:8081"

dev-terminal:
	@echo "🖥️  Starting terminal application..."
	docker-compose up -d terminal
	@echo "✅ Terminal application started"

# =============================================================================
# BUILDING
# =============================================================================

build: build-backend build-frontend build-mobile build-terminal
	@echo "✅ All components built successfully!"

build-backend:
	@echo "🔧 Building backend Docker image..."
	docker-compose build backend
	@echo "✅ Backend built!"

build-frontend:
	@echo "⚛️  Building frontend production bundle..."
	cd frontend && npm run build
	@echo "✅ Frontend built!"

build-mobile:
	@echo "📱 Building mobile applications..."
	cd mobile && npm run build:android && npm run build:ios
	@echo "✅ Mobile apps built!"

build-terminal:
	@echo "🖥️  Building terminal application..."
	cd terminal && npm run build
	@echo "✅ Terminal built!"

# =============================================================================
# DATABASE MANAGEMENT
# =============================================================================

db-up:
	@echo "🗄️  Starting database services..."
	docker-compose up -d sqlserver redis azurite
	@echo "⏳ Waiting for database services to be ready..."
	@until docker-compose exec -T sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P EventShieldPro2024! -Q "SELECT 1" > /dev/null 2>&1; do sleep 5; done
	@echo "✅ Database services ready!"

db-down:
	@echo "🛑 Stopping database services..."
	docker-compose stop sqlserver redis azurite
	@echo "✅ Database services stopped!"

db-reset:
	@echo "🔄 Resetting database with sample data..."
	docker-compose exec backend python manage.py db-reset
	@echo "✅ Database reset complete!"

db-migrate:
	@echo "🔄 Running database migrations..."
	docker-compose exec backend python manage.py db-migrate
	@echo "✅ Migrations complete!"

# =============================================================================
# TESTING
# =============================================================================

test: test-backend test-frontend test-e2e
	@echo "✅ All tests completed!"

test-backend:
	@echo "🧪 Running backend tests..."
	docker-compose exec backend python -m pytest tests/ -v
	@echo "✅ Backend tests complete!"

test-frontend:
	@echo "🧪 Running frontend tests..."
	cd frontend && npm test -- --watchAll=false
	@echo "✅ Frontend tests complete!"

test-e2e:
	@echo "🧪 Running end-to-end tests..."
	cd tests/e2e && npm run test
	@echo "✅ E2E tests complete!"

# =============================================================================
# DEPLOYMENT
# =============================================================================

deploy: deploy-azure
	@echo "✅ Deployment complete!"

deploy-staging:
	@echo "🚀 Deploying to staging environment..."
	cd deployment && ./deploy-staging.sh
	@echo "✅ Staging deployment complete!"

deploy-azure:
	@echo "☁️  Deploying to Azure..."
	cd deployment/azure && ./deploy.sh
	@echo "✅ Azure deployment complete!"

# =============================================================================
# MAINTENANCE
# =============================================================================

clean:
	@echo "🧹 Cleaning build artifacts and containers..."
	docker-compose down -v --remove-orphans
	docker system prune -f
	@echo "✅ Cleanup complete!"

logs:
	@echo "📋 Viewing application logs..."
	docker-compose logs -f

monitor:
	@echo "📊 Opening monitoring dashboards..."
	@echo "📈 Prometheus: http://localhost:9090"
	@echo "📊 Grafana: http://localhost:3001 (admin/admin)"
	@echo "📧 MailHog: http://localhost:8025"
	@if command -v open >/dev/null 2>&1; then \
		open http://localhost:9090; \
		open http://localhost:3001; \
		open http://localhost:8025; \
	fi

backup:
	@echo "💾 Creating database backup..."
	docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P EventShieldPro2024! -Q "BACKUP DATABASE eventshield_pro TO DISK = '/var/opt/mssql/backup/eventshield_pro_$(shell date +%Y%m%d_%H%M%S).bak'"
	@echo "✅ Backup created!"

# =============================================================================
# DOCUMENTATION
# =============================================================================

docs:
	@echo "📚 Generating API documentation..."
	docker-compose exec backend python manage.py generate-docs
	@echo "✅ Documentation generated!"

docs-serve:
	@echo "📖 Serving documentation locally..."
	cd docs && python -m http.server 8000
	@echo "📚 Documentation available at http://localhost:8000"

# =============================================================================
# UTILITY COMMANDS
# =============================================================================

status:
	@echo "📊 EventShield Pro Status:"
	docker-compose ps

restart:
	@echo "🔄 Restarting all services..."
	docker-compose restart
	@echo "✅ Services restarted!"

update:
	@echo "🔄 Updating all dependencies..."
	git pull origin main
	docker-compose pull
	docker-compose build --no-cache
	@echo "✅ Update complete!"

shell-backend:
	@echo "🐍 Opening backend shell..."
	docker-compose exec backend bash

shell-frontend:
	@echo "⚛️  Opening frontend shell..."
	docker-compose exec frontend sh

# =============================================================================
# PRODUCTION COMMANDS
# =============================================================================

prod-build:
	@echo "🏭 Building production images..."
	docker-compose -f docker-compose.prod.yml build
	@echo "✅ Production build complete!"

prod-deploy:
	@echo "🚀 Deploying production environment..."
	docker-compose -f docker-compose.prod.yml up -d
	@echo "✅ Production deployment complete!"

prod-logs:
	@echo "📋 Viewing production logs..."
	docker-compose -f docker-compose.prod.yml logs -f

# =============================================================================
# HARDWARE INTEGRATION
# =============================================================================

hardware-test:
	@echo "🔧 Testing hardware integration..."
	docker-compose exec backend python -m hardware.test_all
	@echo "✅ Hardware tests complete!"

hardware-sim:
	@echo "🎭 Starting hardware simulators..."
	docker-compose up -d hardware-simulator
	@echo "✅ Hardware simulators running at ports 8000-8001"

# =============================================================================
# LICENSING SYSTEM
# =============================================================================

license-test:
	@echo "🔑 Testing licensing system..."
	docker-compose exec backend python -m licensing.test_licenses
	@echo "✅ License tests complete!"

stripe-webhook:
	@echo "💳 Starting Stripe webhook listener..."
	docker-compose up -d stripe-cli
	@echo "✅ Stripe webhook listener running!"

# =============================================================================
# HELPERS
# =============================================================================

check-deps:
	@echo "🔍 Checking system dependencies..."
	@command -v docker >/dev/null 2>&1 || { echo "❌ Docker is required but not installed."; exit 1; }
	@command -v docker-compose >/dev/null 2>&1 || { echo "❌ Docker Compose is required but not installed."; exit 1; }
	@command -v node >/dev/null 2>&1 || { echo "❌ Node.js is required but not installed."; exit 1; }
	@command -v python3 >/dev/null 2>&1 || { echo "❌ Python 3 is required but not installed."; exit 1; }
	@echo "✅ All dependencies are available!"

version:
	@echo "EventShield Pro v1.0.0"
	@echo "Built with ❤️  for enterprise event management"
