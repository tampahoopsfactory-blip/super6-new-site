"""
EventShield Pro - Authentication Service
Handles user creation, password management, and email/verification tokens
"""

import secrets
from datetime import datetime, timedelta

from werkzeug.security import generate_password_hash
import structlog

logger = structlog.get_logger()

# In-memory token stores (replace with Redis / DB in production)
_password_reset_tokens = {}   # token -> user_id
_email_verify_tokens = {}     # token -> user_id

# Requires itsdangerous for signed tokens
try:
    from itsdangerous import URLSafeTimedSerializer, SignatureExpired, BadSignature
    _ITSDANGEROUS_AVAILABLE = True
except ImportError:
    _ITSDANGEROUS_AVAILABLE = False
    logger.warning("itsdangerous not installed; falling back to random tokens")


class AuthService:
    """Service class for authentication-related operations"""

    _SECRET_KEY = "eventshield-auth-secret"  # overridden by app config in production
    _RESET_TOKEN_MAX_AGE = 3600       # 1 hour
    _VERIFY_TOKEN_MAX_AGE = 86400     # 24 hours

    # ------------------------------------------------------------------
    # User creation
    # ------------------------------------------------------------------

    def create_user(self, email, password, first_name, last_name, tenant_id, username=None):
        """Create a new user and persist to database"""
        from app.models import User, db

        if username is None:
            base = email.split('@')[0].lower().replace('.', '_')
            username = base[:95] + '_' + secrets.token_hex(2)

        user = User(
            email=email,
            username=username,
            first_name=first_name,
            last_name=last_name,
            tenant_id=tenant_id,
        )
        user.password = password  # uses the model's setter to hash
        db.session.add(user)
        db.session.commit()

        logger.info("User created", user_id=user.id, email=email)
        return user

    # ------------------------------------------------------------------
    # Password management
    # ------------------------------------------------------------------

    def update_password(self, user, new_password):
        """Update a user's password hash"""
        from app.models import db

        user.password = new_password  # model setter hashes it
        db.session.commit()
        logger.info("Password updated", user_id=user.id)

    # ------------------------------------------------------------------
    # Password reset tokens
    # ------------------------------------------------------------------

    def generate_password_reset_token(self, user):
        """Generate a signed password reset token"""
        if _ITSDANGEROUS_AVAILABLE:
            s = URLSafeTimedSerializer(self._SECRET_KEY)
            token = s.dumps({'user_id': user.id, 'purpose': 'password_reset'})
        else:
            token = secrets.token_urlsafe(48)

        _password_reset_tokens[token] = user.id
        logger.info("Password reset token generated", user_id=user.id)
        return token

    def verify_password_reset_token(self, token):
        """Verify token and return the corresponding User or None"""
        from app.models import User

        user_id = None

        if _ITSDANGEROUS_AVAILABLE:
            try:
                s = URLSafeTimedSerializer(self._SECRET_KEY)
                data = s.loads(token, max_age=self._RESET_TOKEN_MAX_AGE)
                if data.get('purpose') != 'password_reset':
                    return None
                user_id = data.get('user_id')
            except (SignatureExpired, BadSignature):
                return None
        else:
            user_id = _password_reset_tokens.get(token)

        if user_id is None:
            return None

        return User.query.get(user_id)

    def invalidate_password_reset_token(self, token):
        """Remove a used reset token from the store"""
        _password_reset_tokens.pop(token, None)
        logger.info("Password reset token invalidated")

    def send_password_reset_email(self, user, token, tenant):
        """Send a password reset email (stub — logs only)"""
        reset_url = f"https://{tenant.slug}.eventshieldpro.com/reset-password?token={token}"
        logger.info(
            "Password reset email (stub)",
            user_id=user.id,
            email=user.email,
            tenant=tenant.slug,
            reset_url=reset_url,
        )

    # ------------------------------------------------------------------
    # Email verification tokens
    # ------------------------------------------------------------------

    def generate_email_verification_token(self, user):
        """Generate a signed email verification token"""
        if _ITSDANGEROUS_AVAILABLE:
            s = URLSafeTimedSerializer(self._SECRET_KEY)
            token = s.dumps({'user_id': user.id, 'purpose': 'email_verify'})
        else:
            token = secrets.token_urlsafe(48)

        _email_verify_tokens[token] = user.id
        logger.info("Email verification token generated", user_id=user.id)
        return token

    def verify_email_token(self, token):
        """Verify an email verification token and return the User or None"""
        from app.models import User

        user_id = None

        if _ITSDANGEROUS_AVAILABLE:
            try:
                s = URLSafeTimedSerializer(self._SECRET_KEY)
                data = s.loads(token, max_age=self._VERIFY_TOKEN_MAX_AGE)
                if data.get('purpose') != 'email_verify':
                    return None
                user_id = data.get('user_id')
            except (SignatureExpired, BadSignature):
                return None
        else:
            user_id = _email_verify_tokens.get(token)

        if user_id is None:
            return None

        return User.query.get(user_id)

    def send_email_verification_email(self, user, token, tenant):
        """Send an email verification email (stub — logs only)"""
        verify_url = f"https://{tenant.slug}.eventshieldpro.com/verify-email?token={token}"
        logger.info(
            "Email verification email (stub)",
            user_id=user.id,
            email=user.email,
            tenant=tenant.slug,
            verify_url=verify_url,
        )
