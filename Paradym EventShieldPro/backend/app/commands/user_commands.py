"""
EventShield Pro - User CLI Commands
"""

import click
from flask.cli import AppGroup
import structlog

logger = structlog.get_logger()

user_group = AppGroup('user', help='User management commands')


@user_group.command('create')
@click.option('--email', prompt='Email', help='User email address')
@click.option('--password', prompt='Password', hide_input=True,
              confirmation_prompt=True, help='User password')
@click.option('--first-name', prompt='First name', help='User first name')
@click.option('--last-name', prompt='Last name', help='User last name')
@click.option('--tenant-id', prompt='Tenant ID', type=int, help='Tenant ID')
def create_user(email, password, first_name, last_name, tenant_id):
    """Create a new superadmin user"""
    from app.services.auth_service import AuthService
    from app.models import UserRole, db

    try:
        auth_service = AuthService()
        user = auth_service.create_user(
            email=email,
            password=password,
            first_name=first_name,
            last_name=last_name,
            tenant_id=tenant_id,
        )

        # Assign super_admin role
        admin_role = UserRole.query.filter_by(name='super_admin').first()
        if admin_role:
            user.roles.append(admin_role)
            db.session.commit()

        click.echo(f'User created: {user.email} (id={user.id})')
        logger.info("Superadmin user created via CLI", user_id=user.id)
    except Exception as e:
        click.echo(f'Error creating user: {e}', err=True)
        raise


@user_group.command('list')
def list_users():
    """List all users"""
    from app.models import User

    try:
        users = User.query.order_by(User.id).all()
        if not users:
            click.echo('No users found.')
            return

        click.echo(f'{"ID":<6} {"Email":<40} {"Name":<30} {"Active":<8} {"Tenant ID":<10}')
        click.echo('-' * 100)
        for u in users:
            click.echo(
                f'{u.id:<6} {u.email:<40} {u.full_name:<30} {str(u.is_active):<8} {u.tenant_id:<10}'
            )
    except Exception as e:
        click.echo(f'Error listing users: {e}', err=True)
        raise
