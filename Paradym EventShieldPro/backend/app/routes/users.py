"""
EventShield Pro - Users Route Blueprint
CRUD operations for users and user profiles
"""

from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt_identity
import structlog

from app.models import User, UserProfile, db
from app.services.auth_service import AuthService
from app.utils.decorators import validate_json
from app.utils.validators import user_schemas

logger = structlog.get_logger()
users_bp = Blueprint('users', __name__, url_prefix='/api/v1/users')


# ---------------------------------------------------------------------------
# Users collection
# ---------------------------------------------------------------------------

@users_bp.route('/', methods=['GET'])
@jwt_required()
def list_users():
    """List all users (filtered by caller's tenant)"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        if not current_user:
            return jsonify({'error': 'User not found'}), 404

        page = request.args.get('page', 1, type=int)
        per_page = min(request.args.get('per_page', 20, type=int), 100)
        is_active = request.args.get('is_active')

        query = User.query.filter_by(tenant_id=current_user.tenant_id)
        if is_active is not None:
            query = query.filter_by(is_active=is_active.lower() == 'true')

        pagination = query.order_by(User.created_at.desc()).paginate(
            page=page, per_page=per_page, error_out=False
        )

        return jsonify({
            'users': [u.to_dict() for u in pagination.items],
            'total': pagination.total,
            'page': page,
            'per_page': per_page,
            'pages': pagination.pages,
        }), 200

    except Exception as e:
        logger.error("List users error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@users_bp.route('/', methods=['POST'])
@jwt_required()
@validate_json(user_schemas['create'])
def create_user():
    """Create a new user"""
    try:
        data = request.get_json()
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        if not current_user:
            return jsonify({'error': 'Unauthorized'}), 401

        # Check for duplicate email within tenant
        tenant_id = data.get('tenant_id', current_user.tenant_id)
        existing = User.query.filter_by(email=data['email'], tenant_id=tenant_id).first()
        if existing:
            return jsonify({'error': 'User with that email already exists'}), 409

        auth_service = AuthService()
        user = auth_service.create_user(
            email=data['email'],
            password=data['password'],
            first_name=data['first_name'],
            last_name=data['last_name'],
            tenant_id=tenant_id,
        )

        logger.info("User created via API", user_id=user.id, created_by=current_user_id)
        return jsonify({'message': 'User created', 'user': user.to_dict()}), 201

    except Exception as e:
        logger.error("Create user error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


# ---------------------------------------------------------------------------
# Single user
# ---------------------------------------------------------------------------

@users_bp.route('/<int:user_id>', methods=['GET'])
@jwt_required()
def get_user(user_id):
    """Get a user by ID"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        user = User.query.get(user_id)

        if not user:
            return jsonify({'error': 'User not found'}), 404

        # Restrict cross-tenant access
        if user.tenant_id != current_user.tenant_id:
            return jsonify({'error': 'Not found'}), 404

        return jsonify({'user': user.to_dict()}), 200

    except Exception as e:
        logger.error("Get user error", error=str(e), user_id=user_id)
        return jsonify({'error': 'Internal server error'}), 500


@users_bp.route('/<int:user_id>', methods=['PUT'])
@jwt_required()
def update_user(user_id):
    """Update a user's account fields"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        user = User.query.get(user_id)

        if not user:
            return jsonify({'error': 'User not found'}), 404
        if user.tenant_id != current_user.tenant_id:
            return jsonify({'error': 'Not found'}), 404

        data = request.get_json(silent=True) or {}

        for field in ('first_name', 'last_name', 'phone'):
            if field in data:
                setattr(user, field, data[field])

        if 'is_active' in data:
            user.is_active = bool(data['is_active'])

        db.session.commit()
        logger.info("User updated", user_id=user_id, updated_by=current_user_id)
        return jsonify({'message': 'User updated', 'user': user.to_dict()}), 200

    except Exception as e:
        logger.error("Update user error", error=str(e), user_id=user_id)
        return jsonify({'error': 'Internal server error'}), 500


@users_bp.route('/<int:user_id>', methods=['DELETE'])
@jwt_required()
def delete_user(user_id):
    """Soft-delete (deactivate) a user"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        user = User.query.get(user_id)

        if not user:
            return jsonify({'error': 'User not found'}), 404
        if user.tenant_id != current_user.tenant_id:
            return jsonify({'error': 'Not found'}), 404
        if user.id == current_user_id:
            return jsonify({'error': 'Cannot delete your own account'}), 400

        user.is_active = False
        db.session.commit()
        logger.info("User deactivated", user_id=user_id, deleted_by=current_user_id)
        return jsonify({'message': 'User deactivated'}), 200

    except Exception as e:
        logger.error("Delete user error", error=str(e), user_id=user_id)
        return jsonify({'error': 'Internal server error'}), 500


# ---------------------------------------------------------------------------
# User profile
# ---------------------------------------------------------------------------

@users_bp.route('/<int:user_id>/profile', methods=['GET'])
@jwt_required()
def get_user_profile(user_id):
    """Get a user's profile"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        user = User.query.get(user_id)

        if not user:
            return jsonify({'error': 'User not found'}), 404
        if user.tenant_id != current_user.tenant_id:
            return jsonify({'error': 'Not found'}), 404

        profile = UserProfile.query.filter_by(user_id=user_id).first()
        return jsonify({
            'profile': profile.to_dict() if profile else {}
        }), 200

    except Exception as e:
        logger.error("Get profile error", error=str(e), user_id=user_id)
        return jsonify({'error': 'Internal server error'}), 500


@users_bp.route('/<int:user_id>/profile', methods=['PUT'])
@jwt_required()
def update_user_profile(user_id):
    """Create or update a user's profile"""
    try:
        current_user_id = get_jwt_identity()
        current_user = User.query.get(current_user_id)
        user = User.query.get(user_id)

        if not user:
            return jsonify({'error': 'User not found'}), 404
        if user.tenant_id != current_user.tenant_id:
            return jsonify({'error': 'Not found'}), 404

        data = request.get_json(silent=True) or {}

        profile = UserProfile.query.filter_by(user_id=user_id).first()
        if not profile:
            profile = UserProfile(user_id=user_id)
            db.session.add(profile)

        updatable = (
            'avatar_url', 'bio', 'date_of_birth', 'gender',
            'address', 'city', 'state', 'country', 'postal_code',
            'language', 'timezone', 'notification_preferences',
            'website', 'linkedin', 'twitter', 'facebook',
        )
        for field in updatable:
            if field in data:
                setattr(profile, field, data[field])

        db.session.commit()
        logger.info("User profile updated", user_id=user_id, updated_by=current_user_id)
        return jsonify({'message': 'Profile updated', 'profile': profile.to_dict()}), 200

    except Exception as e:
        logger.error("Update profile error", error=str(e), user_id=user_id)
        return jsonify({'error': 'Internal server error'}), 500
