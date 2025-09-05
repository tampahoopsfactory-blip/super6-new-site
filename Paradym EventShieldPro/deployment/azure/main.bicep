// EventShield Pro - Azure Infrastructure as Code
// Bicep template for complete deployment

@description('Name of the EventShield Pro deployment')
param deploymentName string = 'eventshield-pro'

@description('Environment (dev, staging, prod)')
@allowed(['dev', 'staging', 'prod'])
param environment string = 'dev'

@description('Location for all resources')
param location string = resourceGroup().location

@description('Azure SQL Server admin username')
param sqlAdminUsername string

@description('Azure SQL Server admin password')
@secure()
param sqlAdminPassword string

@description('Application Insights connection string')
param appInsightsConnectionString string

@description('Stripe publishable key')
param stripePublishableKey string

@description('Stripe secret key')
@secure()
param stripeSecretKey string

@description('SendGrid API key')
@secure()
param sendGridApiKey string

// Variables
var resourcePrefix = '${deploymentName}-${environment}'
var appServicePlanSku = environment == 'prod' ? 'P1v3' : environment == 'staging' ? 'P1v2' : 'B1'
var sqlSku = environment == 'prod' ? 'Standard' : 'Basic'
var redisSku = environment == 'prod' ? 'Standard' : 'Basic'
var redisCapacity = environment == 'prod' ? 2 : 0

// Resource Group
resource rg 'Microsoft.Resources/resourceGroups' existing = {
  name: resourceGroup().name
}

// Application Insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${resourcePrefix}-ai'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// Log Analytics Workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${resourcePrefix}-logs'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: environment == 'prod' ? 90 : 30
  }
}

// Azure SQL Server
resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: '${resourcePrefix}-sql'
  location: location
  properties: {
    administratorLogin: sqlAdminUsername
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
  }
}

// Azure SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  parent: sqlServer
  name: 'eventshield_pro'
  location: location
  sku: {
    name: sqlSku
    tier: sqlSku
    capacity: environment == 'prod' ? 100 : 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: environment == 'prod' ? 34359738368 : 2147483648 // 32GB or 2GB
    zoneRedundant: environment == 'prod'
  }
}

// Redis Cache
resource redisCache 'Microsoft.Cache/Redis@2023-04-01' = {
  name: '${resourcePrefix}-redis'
  location: location
  sku: {
    name: redisSku
    family: 'C'
    capacity: redisCapacity
  }
  properties: {
    enableNonSslPort: false
    minimumTlsVersion: '1.2'
    redisConfiguration: {
      maxmemoryPolicy: 'allkeys-lru'
    }
  }
}

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: '${resourcePrefix}storage'
  location: location
  sku: {
    name: environment == 'prod' ? 'Standard_GRS' : 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        blob: {
          enabled: true
        }
        file: {
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
  }
}

// Storage Container
resource storageContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-09-01' = {
  parent: storageAccount
  name: 'default/eventshield-pro'
  properties: {
    publicAccess: 'None'
  }
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: '${resourcePrefix}-plan'
  location: location
  sku: {
    name: appServicePlanSku
    tier: appServicePlanSku == 'B1' ? 'Basic' : 'PremiumV3'
  }
  properties: {
    reserved: false
  }
}

// Backend API App Service
resource backendApp 'Microsoft.Web/sites@2021-02-01' = {
  name: '${resourcePrefix}-api'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'PYTHON|3.11'
      appSettings: [
        {
          name: 'APP_ENV'
          value: environment
        }
        {
          name: 'DB_HOST'
          value: '${sqlServer.properties.fullyQualifiedDomainName}'
        }
        {
          name: 'DB_NAME'
          value: sqlDatabase.name
        }
        {
          name: 'DB_USER'
          value: sqlAdminUsername
        }
        {
          name: 'DB_PASSWORD'
          value: sqlAdminPassword
        }
        {
          name: 'REDIS_HOST'
          value: redisCache.properties.hostName
        }
        {
          name: 'REDIS_PASSWORD'
          value: redisCache.listKeys().primaryKey
        }
        {
          name: 'AZURE_STORAGE_CONNECTION_STRING'
          value: storageAccount.listKeys().keys[0].value
        }
        {
          name: 'AZURE_APP_INSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'STRIPE_PUBLISHABLE_KEY'
          value: stripePublishableKey
        }
        {
          name: 'STRIPE_SECRET_KEY'
          value: stripeSecretKey
        }
        {
          name: 'SENDGRID_API_KEY'
          value: sendGridApiKey
        }
        {
          name: 'WEBSITES_PORT'
          value: '5000'
        }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          type: 'SQLAzure'
          connectionString: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Persist Security Info=False;User ID=${sqlAdminUsername};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
      ]
    }
    httpsOnly: true
  }
}

// Frontend Static Web App
resource frontendApp 'Microsoft.Web/staticSites@2021-02-01' = {
  name: '${resourcePrefix}-web'
  location: location
  properties: {
    branch: 'main'
    buildProperties: {
      apiLocation: '/api'
      appLocation: '/'
      outputLocation: 'dist'
    }
  }
}

// CDN Profile
resource cdnProfile 'Microsoft.Cdn/profiles@2021-06-01' = {
  name: '${resourcePrefix}-cdn'
  location: location
  sku: {
    name: 'Standard_Microsoft'
  }
}

// CDN Endpoint
resource cdnEndpoint 'Microsoft.Cdn/profiles/endpoints@2021-06-01' = {
  parent: cdnProfile
  name: '${resourcePrefix}-endpoint'
  location: location
  properties: {
    originHostHeader: frontendApp.properties.defaultHostname
    isHttpsEnabled: true
    isHttpAllowed: false
    optimizationType: 'GeneralWebDelivery'
    queryStringCachingBehavior: 'IgnoreQueryString'
    contentTypesToCompress: [
      'text/plain'
      'text/html'
      'text/css'
      'text/javascript'
      'application/javascript'
      'application/xml'
      'application/json'
    ]
  }
}

// Key Vault
resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: '${resourcePrefix}-kv'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: environment == 'prod' ? 90 : 7
    enablePurgeProtection: environment == 'prod'
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

// API Management
resource apiManagement 'Microsoft.ApiManagement/service@2021-08-01' = {
  name: '${resourcePrefix}-apim'
  location: location
  sku: {
    name: environment == 'prod' ? 'Premium' : 'Developer'
    capacity: environment == 'prod' ? 1 : 1
  }
  properties: {
    publisherName: 'EventShield Pro'
    publisherEmail: 'admin@eventshieldpro.com'
    notificationSenderEmail: 'apimgmt-noreply@azure.com'
    hostnameConfigurations: [
      {
        type: 'Proxy'
        hostName: '${resourcePrefix}-api.azure-api.net'
        defaultSslBinding: true
      }
    ]
    virtualNetworkType: 'None'
    apiVersionConstraint: {
      minApiVersion: '2021-08-01'
    }
  }
}

// Virtual Network
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: '${resourcePrefix}-vnet'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'default'
        properties: {
          addressPrefix: '10.0.0.0/24'
          networkSecurityGroup: {
            id: nsg.id
          }
        }
      }
      {
        name: 'database'
        properties: {
          addressPrefix: '10.0.1.0/24'
          serviceEndpoints: [
            {
              service: 'Microsoft.Sql'
            }
          ]
        }
      }
    ]
  }
}

// Network Security Group
resource nsg 'Microsoft.Network/networkSecurityGroups@2021-05-01' = {
  name: '${resourcePrefix}-nsg'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowHTTPS'
        properties: {
          priority: 100
          protocol: 'Tcp'
          access: 'Allow'
          direction: 'Inbound'
          sourceAddressPrefix: 'Internet'
          sourcePortRange: '*'
          destinationAddressPrefix: '*'
          destinationPortRange: '443'
        }
      }
      {
        name: 'AllowHTTP'
        properties: {
          priority: 110
          protocol: 'Tcp'
          access: 'Allow'
          direction: 'Inbound'
          sourceAddressPrefix: 'Internet'
          sourcePortRange: '*'
          destinationAddressPrefix: '*'
          destinationPortRange: '80'
        }
      }
      {
        name: 'AllowSSH'
        properties: {
          priority: 120
          protocol: 'Tcp'
          access: 'Allow'
          direction: 'Inbound'
          sourceAddressPrefix: 'Internet'
          sourcePortRange: '*'
          destinationAddressPrefix: '*'
          destinationPortRange: '22'
        }
      }
    ]
  }
}

// Public IP for Load Balancer
resource publicIP 'Microsoft.Network/publicIPAddresses@2021-05-01' = {
  name: '${resourcePrefix}-pip'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
    dnsSettings: {
      domainNameLabel: '${resourcePrefix}-lb'
    }
  }
}

// Load Balancer
resource loadBalancer 'Microsoft.Network/loadBalancers@2021-05-01' = {
  name: '${resourcePrefix}-lb'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    frontendIPConfigurations: [
      {
        name: 'frontendIP'
        properties: {
          publicIPAddress: {
            id: publicIP.id
          }
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'backendPool'
      }
    ]
    loadBalancingRules: [
      {
        name: 'httpRule'
        properties: {
          frontendIPConfiguration: {
            id: loadBalancer.properties.frontendIPConfigurations[0].id
          }
          backendAddressPool: {
            id: loadBalancer.properties.backendAddressPools[0].id
          }
          protocol: 'Tcp'
          frontendPort: 80
          backendPort: 80
          enableFloatingIP: false
        }
      }
    ]
  }
}

// Application Gateway
resource appGateway 'Microsoft.Network/applicationGateways@2021-05-01' = {
  name: '${resourcePrefix}-agw'
  location: location
  properties: {
    sku: {
      name: environment == 'prod' ? 'Standard_v2' : 'Standard_Small'
      tier: environment == 'prod' ? 'Standard_v2' : 'Standard'
    }
    gatewayIPConfigurations: [
      {
        name: 'gatewayIPConfig'
        properties: {
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    frontendPorts: [
      {
        name: 'httpPort'
        properties: {
          port: 80
        }
      }
      {
        name: 'httpsPort'
        properties: {
          port: 443
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'frontendIP'
        properties: {
          publicIPAddress: {
            id: publicIP.id
          }
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'backendPool'
        properties: {
          backendAddresses: [
            {
              ipAddress: backendApp.properties.outboundIpAddresses
            }
          ]
        }
      }
    ]
    backendHttpSettingsCollection: [
      {
        name: 'httpSettings'
        properties: {
          port: 80
          protocol: 'Http'
          cookieBasedAffinity: 'Disabled'
        }
      }
    ]
    httpListeners: [
      {
        name: 'httpListener'
        properties: {
          frontendIPConfiguration: {
            id: appGateway.properties.frontendIPConfigurations[0].id
          }
          frontendPort: {
            id: appGateway.properties.frontendPorts[0].id
          }
          protocol: 'Http'
        }
      }
    ]
    requestRoutingRules: [
      {
        name: 'routingRule'
        properties: {
          ruleType: 'Basic'
          httpListener: {
            id: appGateway.properties.httpListeners[0].id
          }
          backendAddressPool: {
            id: appGateway.properties.backendAddressPools[0].id
          }
          backendHttpSettings: {
            id: appGateway.properties.backendHttpSettingsCollection[0].id
          }
        }
      }
    ]
  }
}

// Monitor Action Group
resource actionGroup 'Microsoft.Insights/actionGroups@2021-09-01' = {
  name: '${resourcePrefix}-ag'
  location: location
  properties: {
    groupShortName: 'EventShield'
    enabled: true
    emailReceivers: [
      {
        name: 'admin'
        emailAddress: 'admin@eventshieldpro.com'
        useCommonAlertSchema: true
      }
    ]
    smsReceivers: []
    webhookReceivers: []
  }
}

// Monitor Alert Rule
resource alertRule 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: '${resourcePrefix}-alert'
  location: location
  properties: {
    description: 'Alert when CPU usage is high'
    severity: 2
    enabled: true
    scopes: [
      appServicePlan.id
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'HighCPU'
          metricName: 'CpuPercentage'
          operator: 'GreaterThan'
          threshold: 80
          timeAggregation: 'Average'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroup.id
      }
    ]
  }
}

// Outputs
output backendUrl string = backendApp.properties.defaultHostName
output frontendUrl string = frontendApp.properties.defaultHostName
output cdnUrl string = cdnEndpoint.properties.hostName
output sqlServerName string = sqlServer.name
output redisHostName string = redisCache.properties.hostName
output storageAccountName string = storageAccount.name
output keyVaultName string = keyVault.name
output apiManagementName string = apiManagement.name
output publicIPAddress string = publicIP.properties.ipAddress
