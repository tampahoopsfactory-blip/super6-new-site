#!/bin/bash

# EventShield Pro - Standalone Deployment Script
# Automated deployment script for standalone/on-premise deployment

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
DEPLOYMENT_MODE="standalone"
ENVIRONMENT="production"
COMPOSE_FILE="docker-compose.standalone.yml"
ENV_FILE=".env.standalone"

# Functions
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

check_requirements() {
    log_info "Checking deployment requirements..."
    
    # Check if Docker is installed
    if ! command -v docker &> /dev/null; then
        log_error "Docker is not installed. Please install Docker first."
        exit 1
    fi
    
    # Check if Docker Compose is installed
    if ! command -v docker-compose &> /dev/null; then
        log_error "Docker Compose is not installed. Please install Docker Compose first."
        exit 1
    fi
    
    # Check if .env file exists
    if [ ! -f "$ENV_FILE" ]; then
        log_warning "Environment file $ENV_FILE not found. Creating from template..."
        create_env_file
    fi
    
    # Check if user has permission to access devices
    if ! groups $USER | grep -q docker; then
        log_warning "User $USER is not in docker group. You may need to run with sudo for device access."
    fi
    
    log_success "All requirements met"
}

create_env_file() {
    log_info "Creating environment file from template..."
    
    cat > $ENV_FILE << EOF
# EventShield Pro - Standalone Environment Configuration

# Security
SECRET_KEY=$(openssl rand -hex 32)
ENCRYPTION_KEY=$(openssl rand -hex 32)

# Tenant Configuration
DEFAULT_TENANT_ID=standalone-tenant
TENANT_NAME=EventShield Pro Standalone

# Database (SQLite - no external database required)
DATABASE_URL=sqlite:///app/data/eventshield.db

# Redis
REDIS_PASSWORD=

# SMTP Configuration (Optional)
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=
SMTP_PASSWORD=

# Monitoring
GRAFANA_PASSWORD=$(openssl rand -base64 12)

# Device Configuration
DEVICE_DISCOVERY_ENABLED=true
DEVICE_NETWORK_RANGE=192.168.1.0/24
MAX_DEVICES=50

# Backup Configuration
BACKUP_ENABLED=true
BACKUP_SCHEDULE=0 2 * * *
BACKUP_RETENTION_DAYS=30

# Update Configuration
AUTO_UPDATE=false
UPDATE_CHECK_INTERVAL=86400
EOF

    log_success "Environment file created: $ENV_FILE"
    log_warning "Please review and update the configuration in $ENV_FILE before proceeding"
}

create_directories() {
    log_info "Creating necessary directories..."
    
    mkdir -p data/{app,logs,assets,backups,updates}
    mkdir -p monitoring/{prometheus,grafana}
    mkdir -p nginx/ssl
    mkdir -p scripts
    mkdir -p devices
    
    # Set permissions for device access
    chmod 755 devices/
    
    log_success "Directories created"
}

generate_ssl_certificates() {
    log_info "Generating SSL certificates..."
    
    if [ ! -f "nginx/ssl/cert.pem" ] || [ ! -f "nginx/ssl/key.pem" ]; then
        openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
            -keyout nginx/ssl/key.pem \
            -out nginx/ssl/cert.pem \
            -subj "/C=US/ST=State/L=City/O=Organization/CN=eventshield.local"
        
        log_success "SSL certificates generated"
    else
        log_info "SSL certificates already exist"
    fi
}

setup_monitoring() {
    log_info "Setting up monitoring configuration..."
    
    # Create Prometheus configuration for standalone
    cat > monitoring/prometheus-standalone.yml << EOF
global:
  scrape_interval: 30s
  evaluation_interval: 30s

scrape_configs:
  - job_name: 'eventshield-app'
    static_configs:
      - targets: ['eventshield-app:9090']
    scrape_interval: 10s
    
  - job_name: 'redis'
    static_configs:
      - targets: ['redis:6379']
    
  - job_name: 'device-manager'
    static_configs:
      - targets: ['device-manager:9090']
EOF

    # Create Fluent Bit configuration
    cat > monitoring/fluent-bit.conf << EOF
[SERVICE]
    Flush         1
    Log_Level     info
    Daemon        off
    Parsers_File  parsers.conf
    HTTP_Server   On
    HTTP_Listen   0.0.0.0
    HTTP_Port     2020

[INPUT]
    Name              tail
    Path              /var/log/eventshield/*.log
    Parser            docker
    Tag               eventshield.*
    Refresh_Interval  5

[OUTPUT]
    Name  stdout
    Match *
EOF

    log_success "Monitoring configuration created"
}

deploy_application() {
    log_info "Deploying EventShield Pro standalone application..."
    
    # Pull latest images
    docker-compose -f $COMPOSE_FILE pull
    
    # Build application
    docker-compose -f $COMPOSE_FILE build --no-cache
    
    # Start services
    docker-compose -f $COMPOSE_FILE up -d
    
    log_success "Application deployed"
}

wait_for_services() {
    log_info "Waiting for services to be ready..."
    
    # Wait for Redis
    log_info "Waiting for Redis..."
    timeout 30 bash -c 'until docker-compose -f $COMPOSE_FILE exec redis redis-cli ping; do sleep 2; done'
    
    # Wait for application
    log_info "Waiting for application..."
    timeout 60 bash -c 'until curl -f http://localhost:5000/health; do sleep 5; done'
    
    log_success "All services are ready"
}

setup_initial_data() {
    log_info "Setting up initial data..."
    
    # Wait for application to be ready
    sleep 10
    
    # Create default tenant
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py create_default_tenant
    
    # Create admin user
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py create_admin_user
    
    # Initialize device types
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py init_device_types
    
    log_success "Initial data setup completed"
}

configure_device_access() {
    log_info "Configuring device access..."
    
    # Create device configuration
    cat > devices/device_config.json << EOF
{
    "discovery_enabled": true,
    "network_range": "192.168.1.0/24",
    "max_devices": 50,
    "default_timeout": 5000,
    "retry_count": 3,
    "keep_alive_interval": 30
}
EOF

    # Set permissions
    chmod 644 devices/device_config.json
    
    log_success "Device access configured"
}

show_deployment_info() {
    log_success "EventShield Pro Standalone Deployment Completed!"
    echo
    echo "Access URLs:"
    echo "  Application: http://localhost:5000"
    echo "  Monitoring:  http://localhost:9091"
    echo
    echo "Default Credentials:"
    echo "  Admin User: admin@eventshield.com"
    echo "  Password:   Check your .env file"
    echo
    echo "Device Configuration:"
    echo "  Discovery: Enabled"
    echo "  Network:   192.168.1.0/24"
    echo "  Max Devices: 50"
    echo
    echo "Useful Commands:"
    echo "  View logs:    docker-compose -f $COMPOSE_FILE logs -f"
    echo "  Stop:         docker-compose -f $COMPOSE_FILE down"
    echo "  Restart:      docker-compose -f $COMPOSE_FILE restart"
    echo "  Update:       ./deploy-standalone.sh update"
    echo "  Device logs:  docker-compose -f $COMPOSE_FILE logs -f device-manager"
    echo
    echo "Device Management:"
    echo "  Add device:   curl -X POST http://localhost:5000/api/devices -d '{\"ip\":\"192.168.1.100\",\"sn\":\"0000000000000000\"}'"
    echo "  List devices: curl http://localhost:5000/api/devices"
    echo
}

update_application() {
    log_info "Updating EventShield Pro standalone application..."
    
    # Create backup before update
    log_info "Creating backup..."
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py backup
    
    # Pull latest images
    docker-compose -f $COMPOSE_FILE pull
    
    # Rebuild application
    docker-compose -f $COMPOSE_FILE build --no-cache
    
    # Stop services
    docker-compose -f $COMPOSE_FILE down
    
    # Start services
    docker-compose -f $COMPOSE_FILE up -d
    
    # Wait for services
    wait_for_services
    
    log_success "Application updated"
}

backup_data() {
    log_info "Creating backup..."
    
    # Create backup directory with timestamp
    BACKUP_DIR="backups/backup-$(date +%Y%m%d-%H%M%S)"
    mkdir -p "$BACKUP_DIR"
    
    # Backup application data
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py backup --output "/backups/backup-$(date +%Y%m%d-%H%M%S).sql"
    
    # Copy data directory
    cp -r data/ "$BACKUP_DIR/"
    
    log_success "Backup created: $BACKUP_DIR"
}

restore_data() {
    local backup_file="$1"
    
    if [ -z "$backup_file" ]; then
        log_error "Please specify backup file to restore"
        exit 1
    fi
    
    if [ ! -f "$backup_file" ]; then
        log_error "Backup file not found: $backup_file"
        exit 1
    fi
    
    log_info "Restoring from backup: $backup_file"
    
    # Stop application
    docker-compose -f $COMPOSE_FILE down
    
    # Restore data
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py restore --input "$backup_file"
    
    # Start application
    docker-compose -f $COMPOSE_FILE up -d
    
    log_success "Data restored from: $backup_file"
}

cleanup_old_images() {
    log_info "Cleaning up old Docker images..."
    
    # Remove unused images
    docker image prune -f
    
    # Remove unused volumes
    docker volume prune -f
    
    log_success "Cleanup completed"
}

# Main execution
main() {
    case "${1:-deploy}" in
        "deploy")
            log_info "Starting EventShield Pro Standalone Deployment..."
            check_requirements
            create_directories
            generate_ssl_certificates
            setup_monitoring
            configure_device_access
            deploy_application
            wait_for_services
            setup_initial_data
            show_deployment_info
            ;;
        "update")
            log_info "Updating EventShield Pro Standalone Deployment..."
            check_requirements
            update_application
            cleanup_old_images
            log_success "Update completed"
            ;;
        "backup")
            backup_data
            ;;
        "restore")
            restore_data "$2"
            ;;
        "stop")
            log_info "Stopping EventShield Pro Standalone Deployment..."
            docker-compose -f $COMPOSE_FILE down
            log_success "Deployment stopped"
            ;;
        "restart")
            log_info "Restarting EventShield Pro Standalone Deployment..."
            docker-compose -f $COMPOSE_FILE restart
            log_success "Deployment restarted"
            ;;
        "logs")
            docker-compose -f $COMPOSE_FILE logs -f
            ;;
        "status")
            docker-compose -f $COMPOSE_FILE ps
            ;;
        "cleanup")
            cleanup_old_images
            ;;
        *)
            echo "Usage: $0 {deploy|update|backup|restore|stop|restart|logs|status|cleanup}"
            echo "  deploy  - Deploy the application"
            echo "  update  - Update the application"
            echo "  backup  - Create a backup"
            echo "  restore - Restore from backup"
            echo "  stop    - Stop the application"
            echo "  restart - Restart the application"
            echo "  logs    - View application logs"
            echo "  status  - Show service status"
            echo "  cleanup - Clean up old images"
            exit 1
            ;;
    esac
}

# Run main function
main "$@"
