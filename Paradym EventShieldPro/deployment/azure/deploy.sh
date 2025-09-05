#!/bin/bash

# EventShield Pro - Azure Deployment Script
# Deploys the complete infrastructure using Azure Bicep

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(dirname "$(dirname "$SCRIPT_DIR")")"
BICEP_FILE="$SCRIPT_DIR/main.bicep"
PARAMS_FILE="$SCRIPT_DIR/parameters.json"

# Default values
DEPLOYMENT_NAME="eventshield-pro"
ENVIRONMENT="dev"
LOCATION="East US"
RESOURCE_GROUP=""
SUBSCRIPTION_ID=""

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

# Function to show usage
show_usage() {
    echo "Usage: $0 [OPTIONS]"
    echo ""
    echo "Options:"
    echo "  -n, --name NAME          Deployment name (default: eventshield-pro)"
    echo "  -e, --environment ENV    Environment: dev, staging, prod (default: dev)"
    echo "  -l, --location LOC       Azure location (default: East US)"
    echo "  -g, --resource-group RG  Resource group name (required)"
    echo "  -s, --subscription SUB   Azure subscription ID"
    echo "  -h, --help               Show this help message"
    echo ""
    echo "Examples:"
    echo "  $0 -g my-rg -e dev"
    echo "  $0 -g my-rg -e prod -l 'West US 2'"
    echo ""
}

# Function to check prerequisites
check_prerequisites() {
    print_status "Checking prerequisites..."
    
    # Check if Azure CLI is installed
    if ! command -v az &> /dev/null; then
        print_error "Azure CLI is not installed. Please install it first."
        exit 1
    fi
    
    # Check if user is logged in
    if ! az account show &> /dev/null; then
        print_error "You are not logged in to Azure. Please run 'az login' first."
        exit 1
    fi
    
    # Check if Bicep is available
    if ! az bicep version &> /dev/null; then
        print_warning "Bicep is not available. Installing..."
        az bicep install
    fi
    
    print_success "Prerequisites check passed"
}

# Function to validate parameters
validate_parameters() {
    print_status "Validating parameters..."
    
    if [[ -z "$RESOURCE_GROUP" ]]; then
        print_error "Resource group name is required. Use -g or --resource-group"
        exit 1
    fi
    
    if [[ ! "$ENVIRONMENT" =~ ^(dev|staging|prod)$ ]]; then
        print_error "Environment must be one of: dev, staging, prod"
        exit 1
    fi
    
    # Validate location
    if ! az account list-locations --query "[?name=='$LOCATION'].name" --output tsv | grep -q "$LOCATION"; then
        print_error "Invalid Azure location: $LOCATION"
        exit 1
    fi
    
    print_success "Parameters validation passed"
}

# Function to create resource group if it doesn't exist
create_resource_group() {
    print_status "Checking resource group..."
    
    if ! az group show --name "$RESOURCE_GROUP" &> /dev/null; then
        print_status "Creating resource group: $RESOURCE_GROUP"
        az group create \
            --name "$RESOURCE_GROUP" \
            --location "$LOCATION" \
            --tags \
                Environment="$ENVIRONMENT" \
                Project="EventShield Pro" \
                DeployedBy="$(whoami)" \
                DeployedAt="$(date -u +%Y-%m-%dT%H:%M:%SZ)"
        print_success "Resource group created"
    else
        print_success "Resource group already exists"
    fi
}

# Function to create parameters file
create_parameters_file() {
    print_status "Creating parameters file..."
    
    # Check if parameters file exists
    if [[ ! -f "$PARAMS_FILE" ]]; then
        print_warning "Parameters file not found. Creating template..."
        
        cat > "$PARAMS_FILE" << EOF
{
  "\$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "deploymentName": {
      "value": "$DEPLOYMENT_NAME"
    },
    "environment": {
      "value": "$ENVIRONMENT"
    },
    "location": {
      "value": "$LOCATION"
    },
    "sqlAdminUsername": {
      "value": "eventshield_admin"
    },
    "sqlAdminPassword": {
      "reference": {
        "keyVault": {
          "id": "/subscriptions/{subscription-id}/resourceGroups/{resource-group}/providers/Microsoft.KeyVault/vaults/{vault-name}"
        },
        "secretName": "sql-admin-password"
      }
    },
    "appInsightsConnectionString": {
      "value": "YOUR_APP_INSIGHTS_CONNECTION_STRING"
    },
    "stripePublishableKey": {
      "value": "YOUR_STRIPE_PUBLISHABLE_KEY"
    },
    "stripeSecretKey": {
      "reference": {
        "keyVault": {
          "id": "/subscriptions/{subscription-id}/resourceGroups/{resource-group}/providers/Microsoft.KeyVault/vaults/{vault-name}"
        },
        "secretName": "stripe-secret-key"
      }
    },
    "sendGridApiKey": {
      "reference": {
        "keyVault": {
          "id": "/subscriptions/{subscription-id}/resourceGroups/{resource-group}/providers/Microsoft.KeyVault/vaults/{vault-name}"
        },
        "secretName": "sendgrid-api-key"
      }
    }
  }
}
EOF
        
        print_warning "Please update the parameters file with your actual values before deployment"
        print_warning "Parameters file created at: $PARAMS_FILE"
        exit 1
    fi
    
    print_success "Parameters file found"
}

# Function to deploy infrastructure
deploy_infrastructure() {
    print_status "Starting infrastructure deployment..."
    
    # Set subscription if specified
    if [[ -n "$SUBSCRIPTION_ID" ]]; then
        az account set --subscription "$SUBSCRIPTION_ID"
    fi
    
    # Deploy using Bicep
    print_status "Deploying with Bicep..."
    
    deployment_output=$(az deployment group create \
        --resource-group "$RESOURCE_GROUP" \
        --template-file "$BICEP_FILE" \
        --parameters "@$PARAMS_FILE" \
        --parameters \
            deploymentName="$DEPLOYMENT_NAME" \
            environment="$ENVIRONMENT" \
            location="$LOCATION" \
        --verbose \
        --output json)
    
    if [[ $? -eq 0 ]]; then
        print_success "Infrastructure deployment completed successfully!"
        
        # Extract and display outputs
        print_status "Deployment outputs:"
        echo "$deployment_output" | jq -r '.properties.outputs | to_entries[] | "\(.key): \(.value.value)"'
        
        # Save outputs to file
        echo "$deployment_output" | jq '.properties.outputs' > "$SCRIPT_DIR/deployment-outputs.json"
        print_success "Deployment outputs saved to: $SCRIPT_DIR/deployment-outputs.json"
        
    else
        print_error "Infrastructure deployment failed"
        exit 1
    fi
}

# Function to post-deployment setup
post_deployment_setup() {
    print_status "Performing post-deployment setup..."
    
    # Get deployment outputs
    if [[ -f "$SCRIPT_DIR/deployment-outputs.json" ]]; then
        BACKEND_URL=$(jq -r '.backendUrl.value' "$SCRIPT_DIR/deployment-outputs.json")
        FRONTEND_URL=$(jq -r '.frontendUrl.value' "$SCRIPT_DIR/deployment-outputs.json")
        SQL_SERVER=$(jq -r '.sqlServerName.value' "$SCRIPT_DIR/deployment-outputs.json")
        REDIS_HOST=$(jq -r '.redisHostName.value' "$SCRIPT_DIR/deployment-outputs.json")
        STORAGE_ACCOUNT=$(jq -r '.storageAccountName.value' "$SCRIPT_DIR/deployment-outputs.json")
        KEY_VAULT=$(jq -r '.keyVaultName.value' "$SCRIPT_DIR/deployment-outputs.json")
        
        print_status "Deployed resources:"
        echo "  Backend API: $BACKEND_URL"
        echo "  Frontend Web: $FRONTEND_URL"
        echo "  SQL Server: $SQL_SERVER"
        echo "  Redis Cache: $REDIS_HOST"
        echo "  Storage Account: $STORAGE_ACCOUNT"
        echo "  Key Vault: $KEY_VAULT"
        
        # Create environment file for local development
        ENV_FILE="$PROJECT_ROOT/.env.azure"
        cat > "$ENV_FILE" << EOF
# EventShield Pro - Azure Environment Configuration
# Generated from deployment on $(date)

# Application URLs
APP_URL=https://$FRONTEND_URL
API_URL=https://$BACKEND_URL

# Database Configuration
DB_HOST=$SQL_SERVER.database.windows.net
DB_NAME=eventshield_pro
DB_USER=eventshield_admin
DB_PASSWORD=<set-in-key-vault>

# Redis Configuration
REDIS_HOST=$REDIS_HOST.redis.cache.windows.net
REDIS_SSL=true

# Azure Storage
AZURE_STORAGE_ACCOUNT=$STORAGE_ACCOUNT
AZURE_STORAGE_CONNECTION_STRING=<set-in-key-vault>

# Key Vault
AZURE_KEY_VAULT_NAME=$KEY_VAULT
AZURE_KEY_VAULT_URL=https://$KEY_VAULT.vault.azure.net/

# Update the following values in Azure Key Vault:
# - sql-admin-password
# - stripe-secret-key
# - sendgrid-api-key
# - azure-storage-connection-string
EOF
        
        print_success "Environment file created at: $ENV_FILE"
        print_warning "Please update the environment file with actual values from Azure Key Vault"
        
    else
        print_warning "Could not read deployment outputs. Please check manually."
    fi
}

# Function to show next steps
show_next_steps() {
    print_status "Next steps:"
    echo ""
    echo "1. Update Azure Key Vault with your secrets:"
    echo "   - SQL admin password"
    echo "   - Stripe API keys"
    echo "   - SendGrid API key"
    echo "   - Storage account connection string"
    echo ""
    echo "2. Deploy your application code:"
    echo "   - Backend: Deploy to Azure App Service"
    echo "   - Frontend: Deploy to Azure Static Web Apps"
    echo "   - Mobile: Build and distribute mobile apps"
    echo "   - Terminal: Build and distribute Electron app"
    echo ""
    echo "3. Configure custom domains and SSL certificates"
    echo ""
    echo "4. Set up monitoring and alerting"
    echo ""
    echo "5. Test the complete system"
    echo ""
    echo "For more information, see the documentation at:"
    echo "https://github.com/your-org/eventshield-pro/docs"
}

# Main execution
main() {
    echo "EventShield Pro - Azure Infrastructure Deployment"
    echo "================================================"
    echo ""
    
    # Parse command line arguments
    while [[ $# -gt 0 ]]; do
        case $1 in
            -n|--name)
                DEPLOYMENT_NAME="$2"
                shift 2
                ;;
            -e|--environment)
                ENVIRONMENT="$2"
                shift 2
                ;;
            -l|--location)
                LOCATION="$2"
                shift 2
                ;;
            -g|--resource-group)
                RESOURCE_GROUP="$2"
                shift 2
                ;;
            -s|--subscription)
                SUBSCRIPTION_ID="$2"
                shift 2
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
    
    # Execute deployment steps
    check_prerequisites
    validate_parameters
    create_resource_group
    create_parameters_file
    deploy_infrastructure
    post_deployment_setup
    show_next_steps
    
    print_success "Deployment process completed!"
}

# Run main function with all arguments
main "$@"
