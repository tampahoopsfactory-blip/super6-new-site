"""
EventShield Pro - Custom Decorators
Request validation and rate limiting decorators
"""

import time
import functools
from collections import defaultdict
from flask import request, jsonify
import structlog

logger = structlog.get_logger()

# In-memory store for rate limiting: {key: [(timestamp, count)]}
_rate_limit_store = defaultdict(list)


def validate_json(schema):
    """
    Decorator that validates request JSON against a schema dict.
    Schema format: {'required': ['field1', 'field2', ...]}
    Returns 400 if required fields are missing or body is not JSON.
    """
    def decorator(f):
        @functools.wraps(f)
        def wrapper(*args, **kwargs):
            data = request.get_json(silent=True)
            if data is None:
                return jsonify({'error': 'Request body must be valid JSON'}), 400

            required_fields = schema.get('required', [])
            missing = [field for field in required_fields if field not in data]
            if missing:
                return jsonify({
                    'error': 'Missing required fields',
                    'missing_fields': missing
                }), 400

            return f(*args, **kwargs)
        return wrapper
    return decorator


def rate_limit(max_calls, period_seconds):
    """
    Simple in-memory rate limiter decorator.
    Limits to max_calls per period_seconds per remote address.
    Passes through on error to avoid blocking legitimate traffic.
    """
    def decorator(f):
        @functools.wraps(f)
        def wrapper(*args, **kwargs):
            try:
                client_key = f"{request.remote_addr}:{f.__name__}"
                now = time.time()
                window_start = now - period_seconds

                # Prune old entries
                _rate_limit_store[client_key] = [
                    ts for ts in _rate_limit_store[client_key]
                    if ts > window_start
                ]

                call_count = len(_rate_limit_store[client_key])
                if call_count >= max_calls:
                    logger.warning(
                        "Rate limit exceeded",
                        client=request.remote_addr,
                        endpoint=f.__name__,
                        max_calls=max_calls,
                        period_seconds=period_seconds
                    )
                    return jsonify({'error': 'Rate limit exceeded. Please try again later.'}), 429

                _rate_limit_store[client_key].append(now)
            except Exception as e:
                logger.warning("Rate limiter error (passing through)", error=str(e))

            return f(*args, **kwargs)
        return wrapper
    return decorator


def require_permission(permission_name):
    """
    Decorator that checks if the current user has a specific permission.
    Must be used after @jwt_required().
    """
    def decorator(f):
        @functools.wraps(f)
        def wrapper(*args, **kwargs):
            from flask_jwt_extended import get_jwt_identity
            from app.models import User

            try:
                current_user_id = get_jwt_identity()
                user = User.query.get(current_user_id)
                if not user:
                    return jsonify({'error': 'User not found'}), 404
                if not user.has_permission(permission_name):
                    return jsonify({'error': 'Insufficient permissions'}), 403
            except Exception as e:
                logger.error("Permission check error", error=str(e))
                return jsonify({'error': 'Permission check failed'}), 500

            return f(*args, **kwargs)
        return wrapper
    return decorator
