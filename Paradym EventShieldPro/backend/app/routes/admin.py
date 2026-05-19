from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt
from datetime import datetime
import structlog

logger = structlog.get_logger()
admin_bp = Blueprint('admin', __name__)

# In-memory tenant store (replace with DB via app.models.Tenant)
_tenants: dict = {
    '1': {
        'id': '1',
        'name': 'Demo Tenant',
        'slug': 'demo',
        'plan': 'standard',
        'is_active': True,
        'created_at': '2026-01-01T00:00:00Z'
    }
}


def _require_superadmin():
    claims = get_jwt()
    roles = claims.get('roles', [])
    if 'superadmin' not in roles:
        return False
    return True


@admin_bp.route('/tenants', methods=['GET'])
@jwt_required()
def list_tenants():
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        return jsonify({'tenants': list(_tenants.values()), 'total': len(_tenants)}), 200
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
        required = ['name', 'slug']
        for f in required:
            if not data.get(f):
                return jsonify({'error': f'{f} is required'}), 400

        tenant_id = str(len(_tenants) + 1)
        tenant = {
            'id': tenant_id,
            'name': data['name'],
            'slug': data['slug'],
            'plan': data.get('plan', 'trial'),
            'is_active': True,
            'created_at': datetime.utcnow().isoformat()
        }
        _tenants[tenant_id] = tenant
        logger.info('Tenant created', tenant_id=tenant_id, slug=data['slug'])
        return jsonify({'tenant': tenant}), 201
    except Exception as e:
        logger.error('create_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<tenant_id>', methods=['GET'])
@jwt_required()
def get_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenant = _tenants.get(tenant_id)
        if not tenant:
            return jsonify({'error': 'Tenant not found'}), 404
        return jsonify({'tenant': tenant}), 200
    except Exception as e:
        logger.error('get_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<tenant_id>', methods=['PUT'])
@jwt_required()
def update_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        tenant = _tenants.get(tenant_id)
        if not tenant:
            return jsonify({'error': 'Tenant not found'}), 404
        data = request.get_json() or {}
        tenant.update({k: v for k, v in data.items() if k not in ('id', 'created_at')})
        return jsonify({'tenant': tenant}), 200
    except Exception as e:
        logger.error('update_tenant error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@admin_bp.route('/tenants/<tenant_id>', methods=['DELETE'])
@jwt_required()
def delete_tenant(tenant_id):
    try:
        if not _require_superadmin():
            return jsonify({'error': 'Superadmin access required'}), 403
        if tenant_id not in _tenants:
            return jsonify({'error': 'Tenant not found'}), 404
        del _tenants[tenant_id]
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
            'tenants_total': len(_tenants),
            'tenants_active': sum(1 for t in _tenants.values() if t.get('is_active')),
            'system_version': '2.0.0',
            'uptime': 'N/A'
        }), 200
    except Exception as e:
        logger.error('system_stats error', error=str(e))
        return jsonify({'error': 'Internal server error'}), 500
