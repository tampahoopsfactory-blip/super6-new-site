from flask import Blueprint, request, jsonify, current_app
from flask_jwt_extended import (
    create_access_token, create_refresh_token, jwt_required,
    get_jwt_identity, get_jwt
)
from werkzeug.security import check_password_hash
from datetime import timedelta, datetime
import structlog

from app.models import User, UserProfile, Tenant, db
from app.services.auth_service import AuthService
from app.utils.decorators import validate_json, rate_limit
from app.utils.validators import auth_schemas

logger = structlog.get_logger()
auth_bp = Blueprint('auth', __name__, url_prefix='/api/auth')

@auth_bp.route('/login', methods=['POST'])
@validate_json(auth_schemas['login'])
@rate_limit(5, 300)  # 5 attempts per 5 minutes
def login():
    """User login endpoint"""
    try:
        data = request.get_json()
        email = data.get('email')
        password = data.get('password')
        tenant_slug = data.get('tenant_slug')
        
        # Get tenant
        tenant = Tenant.query.filter_by(slug=tenant_slug, is_active=True).first()
        if not tenant:
            return jsonify({'error': 'Invalid tenant'}), 400
        
        # Authenticate user
        user = User.query.filter_by(email=email, tenant_id=tenant.id, is_active=True).first()
        if not user or not check_password_hash(user.password_hash, password):
            logger.warning("Failed login attempt", email=email, tenant=tenant_slug)
            return jsonify({'error': 'Invalid credentials'}), 401
        
        # Check if user account is locked
        if user.is_locked:
            return jsonify({'error': 'Account is locked. Please contact support.'}), 423
        
        # Check if user account is expired
        if user.is_expired:
            return jsonify({'error': 'Account has expired. Please contact support.'}), 423
        
        # Generate tokens
        access_token = create_access_token(
            identity=user.id,
            additional_claims={
                'tenant_id': user.tenant_id,
                'user_id': user.id,
                'email': user.email,
                'roles': [role.name for role in user.roles]
            },
            expires_delta=timedelta(hours=1)
        )
        
        refresh_token = create_refresh_token(
            identity=user.id,
            additional_claims={
                'tenant_id': user.tenant_id,
                'user_id': user.id
            },
            expires_delta=timedelta(days=30)
        )
        
        # Update last login
        user.last_login = datetime.utcnow()
        user.login_attempts = 0
        user.is_locked = False
        
        # Log successful login
        logger.info("User logged in successfully", 
                   user_id=user.id, email=email, tenant=tenant_slug)
        
        return jsonify({
            'access_token': access_token,
            'refresh_token': refresh_token,
            'token_type': 'bearer',
            'expires_in': 3600,
            'user': {
                'id': user.id,
                'email': user.email,
                'first_name': user.first_name,
                'last_name': user.last_name,
                'roles': [role.name for role in user.roles],
                'tenant_id': user.tenant_id,
                'tenant_name': tenant.name
            }
        }), 200
        
    except Exception as e:
        logger.error("Login error", error=str(e), email=email)
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/register', methods=['POST'])
@validate_json(auth_schemas['register'])
@rate_limit(3, 3600)  # 3 attempts per hour
def register():
    """User registration endpoint"""
    try:
        data = request.get_json()
        email = data.get('email')
        password = data.get('password')
        first_name = data.get('first_name')
        last_name = data.get('last_name')
        tenant_slug = data.get('tenant_slug')
        
        # Get tenant
        tenant = Tenant.query.filter_by(slug=tenant_slug, is_active=True).first()
        if not tenant:
            return jsonify({'error': 'Invalid tenant'}), 400
        
        # Check if user already exists
        existing_user = User.query.filter_by(email=email, tenant_id=tenant.id).first()
        if existing_user:
            return jsonify({'error': 'User already exists'}), 409
        
        # Create user
        auth_service = AuthService()
        user = auth_service.create_user(
            email=email,
            password=password,
            first_name=first_name,
            last_name=last_name,
            tenant_id=tenant.id
        )
        
        # Create user profile
        profile = UserProfile(
            user_id=user.id,
            bio=data.get('bio'),
            phone=data.get('phone'),
            address=data.get('address'),
            preferences=data.get('preferences', {})
        )
        
        # Save to database
        db.session.add(profile)
        db.session.commit()
        
        logger.info("User registered successfully", 
                   user_id=user.id, email=email, tenant=tenant_slug)
        
        return jsonify({
            'message': 'User registered successfully',
            'user_id': user.id
        }), 201
        
    except Exception as e:
        logger.error("Registration error", error=str(e), email=email)
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/refresh', methods=['POST'])
@jwt_required(refresh=True)
def refresh():
    """Refresh access token"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        if not user or not user.is_active:
            return jsonify({'error': 'Invalid user'}), 401
        
        # Create new access token
        access_token = create_access_token(
            identity=current_user_id,
            additional_claims={
                'tenant_id': user.tenant_id,
                'user_id': user.id,
                'email': user.email,
                'roles': [role.name for role in user.roles]
            },
            expires_delta=timedelta(hours=1)
        )
        
        return jsonify({
            'access_token': access_token,
            'token_type': 'bearer',
            'expires_in': 3600
        }), 200
        
    except Exception as e:
        logger.error("Token refresh error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/logout', methods=['POST'])
@jwt_required()
def logout():
    """User logout endpoint"""
    try:
        current_user_id = get_jwt_identity()
        
        # Add token to blacklist (if using Redis)
        jti = get_jwt()['jti']
        if current_app.config.get('REDIS_URL'):
            from app.services.redis_service import RedisService
            redis_service = RedisService()
            redis_service.blacklist_token(jti, 3600)  # Blacklist for 1 hour
        
        logger.info("User logged out", user_id=current_user_id)
        
        return jsonify({'message': 'Logged out successfully'}), 200
        
    except Exception as e:
        logger.error("Logout error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/me', methods=['GET'])
@jwt_required()
def get_current_user():
    """Get current user information"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        if not user:
            return jsonify({'error': 'User not found'}), 404
        
        # Get user profile
        profile = UserProfile.query.filter_by(user_id=user.id).first()
        
        # Get tenant
        tenant = Tenant.query.get(user.tenant_id)
        
        return jsonify({
            'user': {
                'id': user.id,
                'email': user.email,
                'first_name': user.first_name,
                'last_name': user.last_name,
                'roles': [role.name for role in user.roles],
                'permissions': [perm.name for perm in user.permissions],
                'is_active': user.is_active,
                'last_login': user.last_login.isoformat() if user.last_login else None,
                'created_at': user.created_at.isoformat(),
                'profile': {
                    'bio': profile.bio if profile else None,
                    'phone': profile.phone if profile else None,
                    'address': profile.address if profile else None,
                    'preferences': profile.preferences if profile else {}
                } if profile else {},
                'tenant': {
                    'id': tenant.id,
                    'name': tenant.name,
                    'slug': tenant.slug
                } if tenant else None
            }
        }), 200
        
    except Exception as e:
        logger.error("Get current user error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/change-password', methods=['POST'])
@jwt_required()
@validate_json(auth_schemas['change_password'])
def change_password():
    """Change user password"""
    try:
        current_user_id = get_jwt_identity()
        data = request.get_json()
        
        current_password = data.get('current_password')
        new_password = data.get('new_password')
        
        user = User.query.get(current_user_id)
        if not user:
            return jsonify({'error': 'User not found'}), 404
        
        # Verify current password
        if not check_password_hash(user.password_hash, current_password):
            return jsonify({'error': 'Current password is incorrect'}), 400
        
        # Update password
        auth_service = AuthService()
        auth_service.update_password(user, new_password)
        
        logger.info("Password changed successfully", user_id=current_user_id)
        
        return jsonify({'message': 'Password changed successfully'}), 200
        
    except Exception as e:
        logger.error("Change password error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/forgot-password', methods=['POST'])
@validate_json(auth_schemas['forgot_password'])
@rate_limit(3, 3600)  # 3 attempts per hour
def forgot_password():
    """Send password reset email"""
    try:
        data = request.get_json()
        email = data.get('email')
        tenant_slug = data.get('tenant_slug')
        
        # Get tenant
        tenant = Tenant.query.filter_by(slug=tenant_slug, is_active=True).first()
        if not tenant:
            return jsonify({'error': 'Invalid tenant'}), 400
        
        # Get user
        user = User.query.filter_by(email=email, tenant_id=tenant.id, is_active=True).first()
        if not user:
            # Don't reveal if user exists
            return jsonify({'message': 'If the email exists, a reset link has been sent'}), 200
        
        # Generate reset token
        auth_service = AuthService()
        reset_token = auth_service.generate_password_reset_token(user)
        
        # Send reset email
        auth_service.send_password_reset_email(user, reset_token, tenant)
        
        logger.info("Password reset email sent", user_id=user.id, email=email)
        
        return jsonify({'message': 'If the email exists, a reset link has been sent'}), 200
        
    except Exception as e:
        logger.error("Forgot password error", error=str(e), email=email)
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/reset-password', methods=['POST'])
@validate_json(auth_schemas['reset_password'])
@rate_limit(3, 3600)  # 3 attempts per hour
def reset_password():
    """Reset password using reset token"""
    try:
        data = request.get_json()
        token = data.get('token')
        new_password = data.get('new_password')
        
        # Verify and use reset token
        auth_service = AuthService()
        user = auth_service.verify_password_reset_token(token)
        
        if not user:
            return jsonify({'error': 'Invalid or expired reset token'}), 400
        
        # Update password
        auth_service.update_password(user, new_password)
        
        # Invalidate reset token
        auth_service.invalidate_password_reset_token(token)
        
        logger.info("Password reset successfully", user_id=user.id)
        
        return jsonify({'message': 'Password reset successfully'}), 200
        
    except Exception as e:
        logger.error("Reset password error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/verify-email', methods=['POST'])
@validate_json(auth_schemas['verify_email'])
def verify_email():
    """Verify email address using verification token"""
    try:
        data = request.get_json()
        token = data.get('token')
        
        # Verify email token
        auth_service = AuthService()
        user = auth_service.verify_email_token(token)
        
        if not user:
            return jsonify({'error': 'Invalid or expired verification token'}), 400
        
        # Mark email as verified
        user.email_verified = True
        user.email_verified_at = datetime.utcnow()
        db.session.commit()
        
        logger.info("Email verified successfully", user_id=user.id)
        
        return jsonify({'message': 'Email verified successfully'}), 200
        
    except Exception as e:
        logger.error("Email verification error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/resend-verification', methods=['POST'])
@validate_json(auth_schemas['resend_verification'])
@rate_limit(3, 3600)  # 3 attempts per hour
def resend_verification():
    """Resend email verification"""
    try:
        data = request.get_json()
        email = data.get('email')
        tenant_slug = data.get('tenant_slug')
        
        # Get tenant
        tenant = Tenant.query.filter_by(slug=tenant_slug, is_active=True).first()
        if not tenant:
            return jsonify({'error': 'Invalid tenant'}), 400
        
        # Get user
        user = User.query.filter_by(email=email, tenant_id=tenant.id, is_active=True).first()
        if not user:
            return jsonify({'error': 'User not found'}), 404
        
        if user.email_verified:
            return jsonify({'error': 'Email already verified'}), 400
        
        # Generate new verification token
        auth_service = AuthService()
        verification_token = auth_service.generate_email_verification_token(user)
        
        # Send verification email
        auth_service.send_email_verification_email(user, verification_token, tenant)
        
        logger.info("Verification email resent", user_id=user.id, email=email)
        
        return jsonify({'message': 'Verification email sent'}), 200
        
    except Exception as e:
        logger.error("Resend verification error", error=str(e), email=email)
        return jsonify({'error': 'Internal server error'}), 500

@auth_bp.route('/validate-token', methods=['POST'])
@jwt_required()
def validate_token():
    """Validate current JWT token"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        if not user or not user.is_active:
            return jsonify({'valid': False, 'error': 'Invalid user'}), 401
        
        return jsonify({
            'valid': True,
            'user_id': user.id,
            'tenant_id': user.tenant_id
        }), 200
        
    except Exception as e:
        logger.error("Token validation error", error=str(e))
        return jsonify({'valid': False, 'error': 'Token validation failed'}), 401
