"""
EventShield Pro - Database CLI Commands
"""

import click
from flask.cli import AppGroup
import structlog

logger = structlog.get_logger()

db_group = AppGroup('db', help='Database management commands')


@db_group.command('init')
def db_init():
    """Create all database tables"""
    from app import db
    try:
        db.create_all()
        click.echo('Database tables created successfully.')
        logger.info("Database initialized via CLI")
    except Exception as e:
        click.echo(f'Error creating tables: {e}', err=True)
        raise


@db_group.command('drop')
@click.confirmation_option(prompt='Are you sure you want to drop all tables?')
def db_drop():
    """Drop all database tables"""
    from app import db
    try:
        db.drop_all()
        click.echo('All database tables dropped.')
        logger.warning("Database dropped via CLI")
    except Exception as e:
        click.echo(f'Error dropping tables: {e}', err=True)
        raise


@db_group.command('seed')
def db_seed():
    """Insert sample data into the database"""
    from app import db
    from app.models import Tenant, UserRole, UserPermission

    try:
        # Create system roles
        UserRole.create_system_roles()
        click.echo('System roles created.')

        # Create system permissions
        UserPermission.create_system_permissions()
        click.echo('System permissions created.')

        # Create a sample tenant if none exist
        if not Tenant.query.first():
            sample_tenant = Tenant(
                name='Demo Tenant',
                slug='demo',
                is_active=True,
            )
            db.session.add(sample_tenant)
            db.session.commit()
            click.echo(f'Sample tenant created: {sample_tenant.slug}')

        click.echo('Database seeded successfully.')
        logger.info("Database seeded via CLI")
    except Exception as e:
        db.session.rollback()
        click.echo(f'Error seeding database: {e}', err=True)
        raise
