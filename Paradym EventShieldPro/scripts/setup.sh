#!/bin/bash

# EventShield Pro - Complete Setup Script
# Sets up the entire development environment

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Configuration
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"
ENV_TEMPLATE="$PROJECT_ROOT/env.template"
ENV_FILE="$PROJECT_ROOT/.env"

# Default values
SKIP_DOCKER=false
SKIP_BACKEND=false
SKIP_FRONTEND=false
SKIP_MOBILE=false
SKIP_TERMINAL=false
SKIP_DATABASE=false
VERBOSE=false

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_step() {
    echo -e "${PURPLE}[STEP]${NC} $1"
}

print_header() {
    echo -e "${CYAN}================================${NC}"
    echo -e "${CYAN} $1${NC}"
    echo -e "${CYAN}================================${NC}"
}

# Function to show usage
show_usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  --skip-docker        Skip Docker setup"
    echo "  --skip-backend       Skip backend setup"
    echo "  --skip-frontend      Skip frontend setup"
    echo "  --skip-mobile        Skip mobile setup"
    echo "  --skip-terminal      Skip terminal setup"
    echo "  --skip-database      Skip database setup"
    echo "  --verbose            Enable verbose output"
    echo "  -h, --help           Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0                    # Full setup"
    echo "  $0 --skip-mobile     # Skip mobile setup"
    echo "  $0 --verbose         # Verbose output"
    echo ""
}

# Function to check prerequisites
check_prerequisites() {
    print_header "Checking Prerequisites"
    
    local missing_deps=()
    
    # Check Python
    if ! command -v python3 &> /dev/null; then
        missing_deps+=("Python 3.11+")
    else
        PYTHON_VERSION=$(python3 --version | cut -d' ' -f2)
        print_success "Python: $PYTHON_VERSION"
    fi
    
    # Check Node.js
    if ! command -v node &> /dev/null; then
        missing_deps+=("Node.js 18+")
    else
        NODE_VERSION=$(node --version)
        print_success "Node.js: $NODE_VERSION"
    fi
    
    # Check npm
    if ! command -v npm &> /dev/null; then
        missing_deps+=("npm")
    else
        NPM_VERSION=$(npm --version)
        print_success "npm: $NPM_VERSION"
    fi
    
    # Check Docker
    if ! command -v docker &> /dev/null; then
        missing_deps+=("Docker")
    else
        DOCKER_VERSION=$(docker --version | cut -d' ' -f3 | cut -d',' -f1)
        print_success "Docker: $DOCKER_VERSION"
    fi
    
    # Check Docker Compose
    if ! command -v docker-compose &> /dev/null; then
        missing_deps+=("Docker Compose")
    else
        COMPOSE_VERSION=$(docker-compose --version | cut -d' ' -f3 | cut -d',' -f1)
        print_success "Docker Compose: $COMPOSE_VERSION"
    fi
    
    # Check Git
    if ! command -v git &> /dev/null; then
        missing_deps+=("Git")
    else
        GIT_VERSION=$(git --version | cut -d' ' -f3)
        print_success "Git: $GIT_VERSION"
    fi
    
    # Check Azure CLI (optional)
    if command -v az &> /dev/null; then
        AZURE_VERSION=$(az version --output json | jq -r '."azure-cli"')
        print_success "Azure CLI: $AZURE_VERSION"
    else
        print_warning "Azure CLI not found (optional for deployment)"
    fi
    
    # Report missing dependencies
    if [[ ${#missing_deps[@]} -gt 0 ]]; then
        print_error "Missing dependencies:"
        for dep in "${missing_deps[@]}"; do
            echo "  - $dep"
        done
        echo ""
        echo "Please install the missing dependencies and run the setup again."
        exit 1
    fi
    
    print_success "All prerequisites are satisfied!"
}

# Function to setup environment file
setup_environment() {
    print_step "Setting up environment configuration"
    
    if [[ ! -f "$ENV_FILE" ]]; then
        if [[ -f "$ENV_TEMPLATE" ]]; then
            cp "$ENV_TEMPLATE" "$ENV_FILE"
            print_success "Environment file created from template"
            print_warning "Please update $ENV_FILE with your actual configuration values"
        else
            print_warning "Environment template not found. Creating basic .env file"
            cat > "$ENV_FILE" << EOF
# EventShield Pro - Environment Configuration
# Generated on $(date)

APP_NAME=EventShield Pro
APP_ENV=development
APP_DEBUG=true
APP_SECRET_KEY=$(openssl rand -hex 32)

# Database
DB_HOST=localhost
DB_PORT=1433
DB_NAME=eventshield_pro
DB_USER=sa
DB_PASSWORD=EventShieldPro2024!

# Redis
REDIS_HOST=localhost
REDIS_PORT=6379

# JWT
JWT_SECRET_KEY=$(openssl rand -hex 32)
EOF
            print_success "Basic environment file created"
        fi
    else
        print_success "Environment file already exists"
    fi
}

# Function to setup Docker
setup_docker() {
    if [[ "$SKIP_DOCKER" == true ]]; then
        print_warning "Skipping Docker setup"
        return
    fi
    
    print_step "Setting up Docker environment"
    
    # Check if Docker is running
    if ! docker info &> /dev/null; then
        print_error "Docker is not running. Please start Docker and try again."
        exit 1
    fi
    
    # Create necessary directories
    mkdir -p "$PROJECT_ROOT/logs"
    mkdir -p "$PROJECT_ROOT/uploads"
    mkdir -p "$PROJECT_ROOT/database/backups"
    
    print_success "Docker environment prepared"
}

# Function to setup database
setup_database() {
    if [[ "$SKIP_DATABASE" == true ]]; then
        print_warning "Skipping database setup"
        return
    fi
    
    print_step "Setting up database"
    
    # Start database services
    print_status "Starting database services..."
    cd "$PROJECT_ROOT"
    docker-compose up -d sqlserver redis azurite
    
    # Wait for database to be ready
    print_status "Waiting for database to be ready..."
    local max_attempts=30
    local attempt=1
    
    while [[ $attempt -le $max_attempts ]]; do
        if docker-compose exec -T sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P EventShieldPro2024! -Q "SELECT 1" &> /dev/null; then
            print_success "Database is ready!"
            break
        fi
        
        if [[ $attempt -eq $max_attempts ]]; then
            print_error "Database failed to start within expected time"
            exit 1
        fi
        
        print_status "Waiting for database... (attempt $attempt/$max_attempts)"
        sleep 5
        ((attempt++))
    done
    
    print_success "Database setup completed"
}

# Function to setup backend
setup_backend() {
    if [[ "$SKIP_BACKEND" == true ]]; then
        print_warning "Skipping backend setup"
        return
    fi
    
    print_step "Setting up Python backend"
    
    cd "$PROJECT_ROOT/backend"
    
    # Create virtual environment
    if [[ ! -d "venv" ]]; then
        print_status "Creating Python virtual environment..."
        python3 -m venv venv
        print_success "Virtual environment created"
    else
        print_success "Virtual environment already exists"
    fi
    
    # Activate virtual environment and install dependencies
    print_status "Installing Python dependencies..."
    source venv/bin/activate
    pip install --upgrade pip
    pip install -r requirements.txt
    
    # Create logs directory
    mkdir -p logs
    
    print_success "Backend setup completed"
}

# Function to setup frontend
setup_frontend() {
    if [[ "$SKIP_FRONTEND" == true ]]; then
        print_warning "Skipping frontend setup"
        return
    fi
    
    print_step "Setting up React frontend"
    
    cd "$PROJECT_ROOT/frontend"
    
    # Install dependencies
    print_status "Installing Node.js dependencies..."
    npm install
    
    print_success "Frontend setup completed"
}

# Function to setup mobile
setup_mobile() {
    if [[ "$SKIP_MOBILE" == true ]]; then
        print_warning "Skipping mobile setup"
        return
    fi
    
    print_step "Setting up React Native mobile"
    
    cd "$PROJECT_ROOT/mobile"
    
    # Install dependencies
    print_status "Installing Node.js dependencies..."
    npm install
    
    # Check for React Native CLI
    if ! command -v npx &> /dev/null; then
        print_error "npx not found. Please install Node.js and npm."
        exit 1
    fi
    
    print_success "Mobile setup completed"
}

# Function to setup terminal
setup_terminal() {
    if [[ "$SKIP_TERMINAL" == true ]]; then
        print_warning "Skipping terminal setup"
        return
    fi
    
    print_step "Setting up Electron terminal"
    
    cd "$PROJECT_ROOT/terminal"
    
    # Install dependencies
    print_status "Installing Node.js dependencies..."
    npm install
    
    print_success "Terminal setup completed"
}

# Function to initialize database
initialize_database() {
    if [[ "$SKIP_DATABASE" == true ]]; then
        print_warning "Skipping database initialization"
        return
    fi
    
    print_step "Initializing database"
    
    cd "$PROJECT_ROOT/backend"
    
    # Activate virtual environment
    source venv/bin/activate
    
    # Initialize database
    print_status "Creating database tables..."
    python manage.py init_db
    
    # Create super admin
    print_status "Creating super administrator..."
    python manage.py create_super_admin
    
    # Create sample data
    print_status "Creating sample data..."
    python manage.py create_sample_data
    
    print_success "Database initialization completed"
}

# Function to start development environment
start_development() {
    print_step "Starting development environment"
    
    cd "$PROJECT_ROOT"
    
    # Start all services
    print_status "Starting all services..."
    docker-compose up -d
    
    print_success "Development environment started!"
    echo ""
    echo "Services available at:"
    echo "  🌐 Frontend: http://localhost:3000"
    echo "  🔧 Backend API: http://localhost:5000"
    echo "  📱 Mobile Dev: http://localhost:8081"
    echo "  📊 Grafana: http://localhost:3001 (admin/admin)"
    echo "  📧 MailHog: http://localhost:8025"
    echo "  📈 Prometheus: http://localhost:9090"
    echo ""
    echo "To stop all services, run: docker-compose down"
    echo "To view logs, run: docker-compose logs -f"
}

# Function to show next steps
show_next_steps() {
    print_header "Setup Complete!"
    echo ""
    echo "🎉 EventShield Pro has been set up successfully!"
    echo ""
    echo "Next steps:"
    echo ""
    echo "1. 📝 Update configuration:"
    echo "   - Edit .env file with your actual values"
    echo "   - Configure Stripe, SendGrid, and other services"
    echo ""
    echo "2. 🚀 Start development:"
    echo "   - Run: make dev"
    echo "   - Or manually: docker-compose up -d"
    echo ""
    echo "3. 🔐 Access the system:"
    echo "   - Frontend: http://localhost:3000"
    echo "   - Backend API: http://localhost:5000"
    echo "   - Default admin: admin@eventshieldpro.com / Admin123!"
    echo ""
    echo "4. 📚 Read documentation:"
    echo "   - README.md for project overview"
    echo "   - docs/ for detailed documentation"
    echo ""
    echo "5. 🧪 Run tests:"
    echo "   - make test"
    echo ""
    echo "6. 🚀 Deploy to production:"
    echo "   - cd deployment/azure"
    echo "   - ./deploy.sh -g your-resource-group -e prod"
    echo ""
    echo "For help and support:"
    echo "  - GitHub Issues: https://github.com/your-org/eventshield-pro/issues"
    echo "  - Documentation: https://docs.eventshieldpro.com"
    echo "  - Support: support@eventshieldpro.com"
}

# Function to cleanup on error
cleanup() {
    print_error "Setup failed. Cleaning up..."
    
    # Stop any running containers
    if command -v docker-compose &> /dev/null; then
        cd "$PROJECT_ROOT"
        docker-compose down &> /dev/null || true
    fi
    
    exit 1
}

# Set trap for cleanup
trap cleanup ERR

# Main execution
main() {
    print_header "EventShield Pro - Complete Setup"
    echo ""
    echo "This script will set up the complete EventShield Pro development environment."
    echo "It includes:"
    echo "  • Python backend (Flask)"
    echo "  • React frontend"
    echo "  • React Native mobile apps"
    echo "  • Electron terminal application"
    echo "  • Azure SQL Database (local)"
    echo "  • Redis cache"
    echo "  • Monitoring and logging"
    echo ""
    
    # Parse command line arguments
    while [[ $# -gt 0 ]]; do
        case $1 in
            --skip-docker)
                SKIP_DOCKER=true
                shift
                ;;
            --skip-backend)
                SKIP_BACKEND=true
                shift
                ;;
            --skip-frontend)
                SKIP_FRONTEND=true
                shift
                ;;
            --skip-mobile)
                SKIP_MOBILE=true
                shift
                ;;
            --skip-terminal)
                SKIP_TERMINAL=true
                shift
                ;;
            --skip-database)
                SKIP_DATABASE=true
                shift
                ;;
            --verbose)
                VERBOSE=true
                shift
                ;;
            -h|--help)
                show_usage
                exit 0
                ;;
            *)
                print_error "Unknown option: $1"
                show_usage
                exit 1
                ;;
        esac
    done
    
    # Set verbose mode
    if [[ "$VERBOSE" == true ]]; then
        set -x
    fi
    
    # Execute setup steps
    check_prerequisites
    setup_environment
    setup_docker
    setup_database
    setup_backend
    setup_frontend
    setup_mobile
    setup_terminal
    initialize_database
    start_development
    show_next_steps
    
    print_success "EventShield Pro setup completed successfully!"
}

# Run main function with all arguments
main "$@"
