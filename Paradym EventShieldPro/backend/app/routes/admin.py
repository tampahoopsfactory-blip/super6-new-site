from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt
import structlog

from app import db
from app.models.tenant import Tenant

logger = structlog.get_logger()
admin_bp = Blueprint('admin', __name__)


def _require_superadmin():
    claims = get_jwt()
    roles = claims.get('roles', [])
    return 'superadmin' in roles


@admin_bp.route('/tenants', methods=['GET'])
@jwt_required()
def list_tenants():
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenants = Tenant.query.all()
        return jsonify({'tenants': [t.to_dict() for t in tenants], 'total': len(tenants)}), 200
    except Exception as e:
        logger.error('list_tenants error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants', methods=['POST'])
@jwt_required()
def create_tenant():
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403

        data = request.get_json() or {}
        for f in ('name', 'slug'):
            if not data.get(f):
                return jsonify({'error': f'{f} is required'}), 400

        tenant = Tenant(
            name=data['name'],
            slug=data['slug'],
            subscription_tier=data.get('plan', 'trial'),
        )
        try:
            db.session.add(tenant)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        logger.info('Tenant created', tenant_id=tenant.id, slug=tenant.slug)
        return jsonify({'tenant': tenant.to_dict()}), 201
    except Exception as e:
        logger.error('create_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<int:tenant_id>', methods=['GET'])
@jwt_required()
def get_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenant = Tenant.query.get(tenant_id)
        if not tenant:
            return jsonify({'error': 'Tenant not found'}), 404
        return jsonify({'tenant': tenant.to_dict()}), 200
    except Exception as e:
        logger.error('get_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<int:tenant_id>', methods=['PUT'])
@jwt_required()
def update_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenant = Tenant.query.get(tenant_id)
        if not tenant:
            return jsonify({'error': 'Tenant not found'}), 404

        data = request.get_json() or {}
        protected = {'id', 'created_at'}
        for key, value in data.items():
            if key not in protected and hasattr(tenant, key):
                setattr(tenant, key, value)

        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        return jsonify({'tenant': tenant.to_dict()}), 200
    except Exception as e:
        logger.error('update_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<int:tenant_id>', methods=['DELETE'])
@jwt_required()
def delete_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenant = Tenant.query.get(tenant_id)
        if not tenant:
            return jsonify({'error': 'Tenant not found'}), 404

        try:
            db.session.delete(tenant)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

        logger.info('Tenant deleted', tenant_id=tenant_id)
        return jsonify({'deleted': True}), 200
    except Exception as e:
        logger.error('delete_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/system/stats', methods=['GET'])
@jwt_required()
def system_stats():
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        return jsonify({
            'tenants_total': Tenant.query.count(),
            'tenants_active': Tenant.query.filter_by(is_active=True).count(),
            'system_version': '2.0.0',
            'uptime': 'N/A'
        }), 200
    except Exception as e:
        logger.error('system_stats error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500
