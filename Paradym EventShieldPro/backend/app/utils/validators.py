"""
EventShield Pro - Request Validators
Schema definitions for request validation
"""

# Auth schemas: each value is a set of required field names
auth_schemas = {
    'login': {
        'required': ['email', 'password', 'tenant_slug']
    },
    'register': {
        'required': ['email', 'password', 'first_name', 'last_name', 'tenant_slug']
    },
    'change_password': {
        'required': ['current_password', 'new_password']
    },
    'forgot_password': {
        'required': ['email', 'tenant_slug']
    },
    'reset_password': {
        'required': ['token', 'new_password']
    },
    'verify_email': {
        'required': ['token']
    },
    'resend_verification': {
        'required': ['email', 'tenant_slug']
    },
}

# User schemas
user_schemas = {
    'create': {
        'required': ['email', 'password', 'first_name', 'last_name', 'tenant_id']
    },
    'update': {
        'required': []
    },
    'update_profile': {
        'required': []
    },
}

# Ticket schemas
ticket_schemas = {
    'create': {
        'required': ['event_id', 'ticket_type_id', 'original_price', 'final_price']
    },
    'validate': {
        'required': []
    },
}

# Access control schemas
access_schemas = {
    'grant': {
        'required': ['event_id', 'ticket_id']
    },
    'deny': {
        'required': ['event_id', 'ticket_id', 'reason']
    },
}

# Event schemas
event_schemas = {
    'create': {
        'required': ['title', 'start_datetime', 'end_datetime']
    },
    'update': {
        'required': []
    },
    'create_category': {
        'required': ['name']
    },
}
