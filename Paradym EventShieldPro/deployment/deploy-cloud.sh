#!/bin/bash

# EventShield Pro - Cloud Deployment Script
# Automated deployment script for cloud-based multi-tenant SaaS

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
DEPLOYMENT_MODE="cloud"
ENVIRONMENT="production"
COMPOSE_FILE="docker-compose.cloud.yml"
ENV_FILE=".env.cloud"

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
        log_error "Environment file $ENV_FILE not found. Please create it first."
        exit 1
    fi
    
    log_success "All requirements met"
}

create_directories() {
    log_info "Creating necessary directories..."
    
    mkdir -p data/{postgres,redis,app,logs,assets,backups}
    mkdir -p monitoring/{prometheus,grafana,logstash}
    mkdir -p nginx/ssl
    mkdir -p scripts
    
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
    
    # Create Prometheus configuration
    cat > monitoring/prometheus.yml << EOF
global:
  scrape_interval: 15s
  evaluation_interval: 15s

rule_files:
  - "rules/*.yml"

scrape_configs:
  - job_name: 'eventshield-app'
    static_configs:
      - targets: ['eventshield-app:9090']
    scrape_interval: 5s
    
  - job_name: 'postgres'
    static_configs:
      - targets: ['postgres:5432']
      
  - job_name: 'redis'
    static_configs:
      - targets: ['redis:6379']
EOF

    # Create Grafana datasource
    mkdir -p monitoring/grafana/datasources
    cat > monitoring/grafana/datasources/prometheus.yml << EOF
apiVersion: 1

datasources:
  - name: Prometheus
    type: prometheus
    access: proxy
    url: http://prometheus:9090
    isDefault: true
EOF

    log_success "Monitoring configuration created"
}

deploy_application() {
    log_info "Deploying EventShield Pro application..."
    
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
    
    # Wait for database
    log_info "Waiting for database..."
    timeout 60 bash -c 'until docker-compose -f $COMPOSE_FILE exec postgres pg_isready -U eventshield -d eventshield_cloud; do sleep 2; done'
    
    # Wait for Redis
    log_info "Waiting for Redis..."
    timeout 30 bash -c 'until docker-compose -f $COMPOSE_FILE exec redis redis-cli ping; do sleep 2; done'
    
    # Wait for application
    log_info "Waiting for application..."
    timeout 60 bash -c 'until curl -f http://localhost/health; do sleep 5; done'
    
    log_success "All services are ready"
}

run_database_migrations() {
    log_info "Running database migrations..."
    
    # Wait for database to be ready
    sleep 10
    
    # Run migrations
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py migrate
    
    log_success "Database migrations completed"
}

setup_initial_data() {
    log_info "Setting up initial data..."
    
    # Create default tenant
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py create_default_tenant
    
    # Create admin user
    docker-compose -f $COMPOSE_FILE exec eventshield-app python manage.py create_admin_user
    
    log_success "Initial data setup completed"
}

show_deployment_info() {
    log_success "EventShield Pro Cloud Deployment Completed!"
    echo
    echo "Access URLs:"
    echo "  Application: https://localhost"
    echo "  Grafana:     http://localhost:3001"
    echo "  Prometheus:  http://localhost:9091"
    echo "  Kibana:      http://localhost:5601"
    echo
    echo "Default Credentials:"
    echo "  Admin User: admin@eventshield.com"
    echo "  Password:   Check your .env file"
    echo
    echo "Useful Commands:"
    echo "  View logs:    docker-compose -f $COMPOSE_FILE logs -f"
    echo "  Stop:         docker-compose -f $COMPOSE_FILE down"
    echo "  Restart:      docker-compose -f $COMPOSE_FILE restart"
    echo "  Update:       ./deploy-cloud.sh update"
    echo
}

update_application() {
    log_info "Updating EventShield Pro application..."
    
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
            log_info "Starting EventShield Pro Cloud Deployment..."
            check_requirements
            create_directories
            generate_ssl_certificates
            setup_monitoring
            deploy_application
            wait_for_services
            run_database_migrations
            setup_initial_data
            show_deployment_info
            ;;
        "update")
            log_info "Updating EventShield Pro Cloud Deployment..."
            check_requirements
            update_application
            cleanup_old_images
            log_success "Update completed"
            ;;
        "stop")
            log_info "Stopping EventShield Pro Cloud Deployment..."
            docker-compose -f $COMPOSE_FILE down
            log_success "Deployment stopped"
            ;;
        "restart")
            log_info "Restarting EventShield Pro Cloud Deployment..."
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
            echo "Usage: $0 {deploy|update|stop|restart|logs|status|cleanup}"
            exit 1
            ;;
    esac
}

# Run main function
main "$@"
