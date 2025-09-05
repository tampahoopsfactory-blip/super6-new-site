#!/usr/bin/env python3
"""
EventShield Pro - Flask Management Script
Database operations and administrative tasks
"""

import os
import sys
import click
from datetime import datetime, timedelta
from flask.cli import FlaskGroup
from app import create_app, db
from app.models import *

# Create Flask app
app = create_app()

# Create CLI group
cli = FlaskGroup(app)

@cli.command()
def run():
    """Run the Flask development server"""
    app.run(debug=True, host='0.0.0.0', port=5000)

@cli.command()
def init_db():
    """Initialize the database with all tables"""
    click.echo("Creating database tables...")
    db.create_all()
    click.echo("✅ Database tables created successfully!")

@cli.command()
def drop_db():
    """Drop all database tables"""
    if click.confirm("Are you sure you want to drop all tables? This will delete all data!"):
        click.echo("Dropping database tables...")
        db.drop_all()
        click.echo("✅ Database tables dropped successfully!")

@cli.command()
def reset_db():
    """Reset the database (drop and recreate all tables)"""
    if click.confirm("Are you sure you want to reset the database? This will delete all data!"):
        click.echo("Resetting database...")
        db.drop_all()
        db.create_all()
        click.echo("✅ Database reset successfully!")

@cli.command()
def create_super_admin():
    """Create a super administrator user"""
    click.echo("Creating super administrator...")
    
    # Check if super admin already exists
    existing_admin = User.query.filter_by(email='admin@eventshieldpro.com').first()
    if existing_admin:
        click.echo("❌ Super administrator already exists!")
        return
    
    # Create default tenant
    tenant = Tenant(
        name='EventShield Pro',
        slug='eventshield-pro',
        domain='eventshieldpro.com',
        company_name='EventShield Pro',
        subscription_tier='enterprise',
        subscription_status='active',
        is_verified=True
    )
    db.session.add(tenant)
    db.session.flush()  # Get the tenant ID
    
    # Create super admin user
    admin = User(
        email='admin@eventshieldpro.com',
        username='admin',
        first_name='Super',
        last_name='Administrator',
        is_verified=True,
        tenant_id=tenant.id
    )
    admin.password = 'Admin123!'  # Change this in production!
    db.session.add(admin)
    
    # Create system roles and permissions
    UserRole.create_system_roles()
    UserPermission.create_system_permissions()
    
    # Assign super admin role
    super_admin_role = UserRole.query.filter_by(name='super_admin').first()
    if super_admin_role:
        admin.roles.append(super_admin_role)
    
    db.session.commit()
    click.echo("✅ Super administrator created successfully!")
    click.echo(f"Email: {admin.email}")
    click.echo(f"Password: Admin123!")

@cli.command()
def create_sample_data():
    """Create sample data for development"""
    click.echo("Creating sample data...")
    
    # Create sample tenant
    tenant = Tenant(
        name='Sample Company',
        slug='sample-company',
        domain='sample.com',
        company_name='Sample Company Inc.',
        subscription_tier='premium',
        subscription_status='active',
        is_verified=True
    )
    db.session.add(tenant)
    db.session.flush()
    
    # Create sample user
    user = User(
        email='user@sample.com',
        username='sampleuser',
        first_name='John',
        last_name='Doe',
        is_verified=True,
        tenant_id=tenant.id
    )
    user.password = 'Password123!'
    db.session.add(user)
    
    # Create sample event categories
    categories = [
        EventCategory(name='Technology', slug='technology', description='Tech events and conferences'),
        EventCategory(name='Business', slug='business', description='Business and corporate events'),
        EventCategory(name='Entertainment', slug='entertainment', description='Entertainment and cultural events'),
        EventCategory(name='Education', slug='education', description='Educational workshops and seminars')
    ]
    for category in categories:
        db.session.add(category)
    
    # Create sample event location
    location = EventLocation(
        name='Sample Conference Center',
        address='123 Main Street',
        city='Sample City',
        state='Sample State',
        country='United States',
        postal_code='12345',
        venue_type='conference_center',
        capacity=500
    )
    db.session.add(location)
    db.session.flush()
    
    # Create sample event
    event = Event(
        title='Sample Tech Conference 2024',
        slug='sample-tech-conference-2024',
        description='A sample technology conference for demonstration purposes',
        short_description='Learn about the latest in technology',
        event_type='conference',
        category_id=categories[0].id,
        location_id=location.id,
        status=EventStatus.PUBLISHED,
        visibility=EventVisibility.PUBLIC,
        is_approved=True,
        max_capacity=200,
        is_free=True,
        tenant_id=tenant.id,
        organizer_id=user.id
    )
    db.session.add(event)
    db.session.flush()
    
    # Create sample event schedule
    schedule = EventSchedule(
        event_id=event.id,
        start_datetime=datetime.utcnow() + timedelta(days=30),
        end_datetime=datetime.utcnow() + timedelta(days=30, hours=8),
        timezone='UTC'
    )
    db.session.add(schedule)
    
    db.session.commit()
    click.echo("✅ Sample data created successfully!")

@cli.command()
def create_tenant():
    """Create a new tenant"""
    name = click.prompt("Tenant name")
    slug = click.prompt("Tenant slug", default=name.lower().replace(' ', '-'))
    domain = click.prompt("Domain (optional)")
    company_name = click.prompt("Company name")
    contact_email = click.prompt("Contact email")
    
    tenant = Tenant(
        name=name,
        slug=slug,
        domain=domain,
        company_name=company_name,
        contact_email=contact_email,
        subscription_tier='trial',
        subscription_status='active',
        trial_ends_at=datetime.utcnow() + timedelta(days=14)
    )
    
    db.session.add(tenant)
    db.session.commit()
    click.echo(f"✅ Tenant '{name}' created successfully!")

@cli.command()
def list_tenants():
    """List all tenants"""
    tenants = Tenant.query.all()
    if not tenants:
        click.echo("No tenants found.")
        return
    
    click.echo("Tenants:")
    click.echo("-" * 80)
    for tenant in tenants:
        click.echo(f"ID: {tenant.id}")
        click.echo(f"Name: {tenant.name}")
        click.echo(f"Slug: {tenant.slug}")
        click.echo(f"Subscription: {tenant.subscription_tier} ({tenant.subscription_status})")
        click.echo(f"Created: {tenant.created_at}")
        click.echo("-" * 80)

@cli.command()
def list_users():
    """List all users"""
    users = User.query.all()
    if not users:
        click.echo("No users found.")
        return
    
    click.echo("Users:")
    click.echo("-" * 80)
    for user in users:
        click.echo(f"ID: {user.id}")
        click.echo(f"Name: {user.full_name}")
        click.echo(f"Email: {user.email}")
        click.echo(f"Username: {user.username}")
        click.echo(f"Tenant: {user.tenant.name if user.tenant else 'N/A'}")
        click.echo(f"Status: {'Active' if user.is_active else 'Inactive'}")
        click.echo("-" * 80)

@cli.command()
def list_events():
    """List all events"""
    events = Event.query.all()
    if not events:
        click.echo("No events found.")
        return
    
    click.echo("Events:")
    click.echo("-" * 80)
    for event in events:
        click.echo(f"ID: {event.id}")
        click.echo(f"Title: {event.title}")
        click.echo(f"Slug: {event.slug}")
        click.echo(f"Status: {event.status.value if event.status else 'N/A'}")
        click.echo(f"Tenant: {event.tenant.name if event.tenant else 'N/A'}")
        click.echo(f"Organizer: {event.organizer.full_name if event.organizer else 'N/A'}")
        click.echo(f"Capacity: {event.current_registrations}/{event.max_capacity if event.max_capacity else 'Unlimited'}")
        click.echo("-" * 80)

@cli.command()
def db_migrate():
    """Run database migrations"""
    click.echo("Running database migrations...")
    try:
        from flask_migrate import upgrade
        upgrade()
        click.echo("✅ Migrations completed successfully!")
    except Exception as e:
        click.echo(f"❌ Migration failed: {e}")

@cli.command()
def db_reset():
    """Reset database with sample data"""
    if click.confirm("Are you sure you want to reset the database? This will delete all data!"):
        click.echo("Resetting database...")
        db.drop_all()
        db.create_all()
        
        # Create system roles and permissions
        UserRole.create_system_roles()
        UserPermission.create_system_permissions()
        
        # Create sample data
        create_sample_data()
        click.echo("✅ Database reset with sample data completed!")

@cli.command()
def health_check():
    """Check system health"""
    click.echo("Checking system health...")
    
    # Check database connection
    try:
        db.session.execute('SELECT 1')
        click.echo("✅ Database connection: OK")
    except Exception as e:
        click.echo(f"❌ Database connection: FAILED - {e}")
        return
    
    # Check models
    try:
        tenant_count = Tenant.query.count()
        user_count = User.query.count()
        event_count = Event.query.count()
        click.echo(f"✅ Models: OK (Tenants: {tenant_count}, Users: {user_count}, Events: {event_count})")
    except Exception as e:
        click.echo(f"❌ Models: FAILED - {e}")
    
    # Check configuration
    click.echo(f"✅ Environment: {app.config.get('APP_ENV', 'unknown')}")
    click.echo(f"✅ Debug mode: {app.config.get('DEBUG', False)}")
    
    click.echo("✅ System health check completed!")

@cli.command()
def shell():
    """Open a Python shell with app context"""
    import code
    from flask.globals import _app_ctx_stack
    
    app_context = app.app_context()
    app_context.push()
    
    # Add useful objects to the shell
    shell_context = {
        'app': app,
        'db': db,
        'User': User,
        'Tenant': Tenant,
        'Event': Event,
        'Ticket': Ticket,
        'UserRole': UserRole,
        'UserPermission': UserPermission
    }
    
    try:
        code.interact(local=shell_context)
    finally:
        app_context.pop()

@cli.command()
def test():
    """Run tests"""
    click.echo("Running tests...")
    import pytest
    import sys
    
    # Add current directory to Python path
    sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
    
    # Run pytest
    exit_code = pytest.main(['tests/', '-v'])
    sys.exit(exit_code)

@cli.command()
def generate_docs():
    """Generate API documentation"""
    click.echo("Generating API documentation...")
    
    # Create docs directory if it doesn't exist
    docs_dir = 'docs'
    os.makedirs(docs_dir, exist_ok=True)
    
    # Generate OpenAPI/Swagger spec
    try:
        from apispec import APISpec
        from apispec.ext.marshmallow import MarshmallowPlugin
        from apispec_webframeworks.flask import FlaskPlugin
        
        spec = APISpec(
            title="EventShield Pro API",
            version="1.0.0",
            openapi_version="3.0.2",
            plugins=[FlaskPlugin(), MarshmallowPlugin()],
        )
        
        # Add routes to spec
        with app.test_request_context():
            for rule in app.url_map.iter_rules():
                if rule.endpoint != 'static':
                    spec.path(view=app.view_functions[rule.endpoint])
        
        # Write spec to file
        with open(os.path.join(docs_dir, 'openapi.json'), 'w') as f:
            f.write(spec.to_json())
        
        click.echo("✅ API documentation generated successfully!")
        click.echo(f"📁 Documentation saved to: {docs_dir}/openapi.json")
        
    except Exception as e:
        click.echo(f"❌ Failed to generate documentation: {e}")

if __name__ == '__main__':
    cli()
