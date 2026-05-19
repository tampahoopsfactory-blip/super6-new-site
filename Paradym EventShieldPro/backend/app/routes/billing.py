from flask import Blueprint, request, jsonify, current_app
from flask_jwt_extended import jwt_required, get_jwt
import structlog

from app.models.tenant import Tenant

logger = structlog.get_logger()
billing_bp = Blueprint('billing', __name__)

_NOT_CONFIGURED = {'error': 'Stripe not configured — set STRIPE_SECRET_KEY'}


def _stripe():
    try:
        import stripe as _stripe_mod
    except ImportError:
        return None, _NOT_CONFIGURED
    key = current_app.config.get('STRIPE_SECRET_KEY')
    if not key:
        return None, _NOT_CONFIGURED
    _stripe_mod.api_key = key
    return _stripe_mod, None


def _get_tenant_id():
    return get_jwt().get('tenant_id')


def _customer_id(tenant_id):
    tenant = Tenant.query.get(tenant_id)
    if tenant and tenant.subscription and tenant.subscription.stripe_customer_id:
        return tenant.subscription.stripe_customer_id
    return f'placeholder_tenant_{tenant_id}'


@billing_bp.route('/subscriptions', methods=['GET'])
@jwt_required()
def get_subscription():
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        customer_id = _customer_id(_get_tenant_id())
        subs = stripe.Subscription.list(customer=customer_id, limit=1)
        data = subs['data'][0] if subs['data'] else None
        return jsonify({'subscription': data}), 200
    except Exception as e:
        logger.error('get_subscription error', error=str(e))
        return jsonify({'error': str(e)}), 500


@billing_bp.route('/subscriptions', methods=['POST'])
@jwt_required()
def create_subscription():
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        data = request.get_json() or {}
        price_id = data.get('price_id')
        if not price_id:
            return jsonify({'error': 'price_id is required'}), 400

        customer_id = _customer_id(_get_tenant_id())
        sub = stripe.Subscription.create(
            customer=customer_id,
            items=[{'price': price_id}],
        )
        return jsonify({'subscription': sub}), 201
    except Exception as e:
        logger.error('create_subscription error', error=str(e))
        return jsonify({'error': str(e)}), 500


@billing_bp.route('/subscriptions/<sub_id>', methods=['PUT'])
@jwt_required()
def update_subscription(sub_id):
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        data = request.get_json() or {}
        sub = stripe.Subscription.modify(sub_id, **data)
        return jsonify({'subscription': sub}), 200
    except Exception as e:
        logger.error('update_subscription error', error=str(e))
        return jsonify({'error': str(e)}), 500


@billing_bp.route('/subscriptions/<sub_id>', methods=['DELETE'])
@jwt_required()
def cancel_subscription(sub_id):
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        sub = stripe.Subscription.cancel(sub_id)
        logger.info('Subscription cancelled', sub_id=sub_id)
        return jsonify({'subscription': sub}), 200
    except Exception as e:
        logger.error('cancel_subscription error', error=str(e))
        return jsonify({'error': str(e)}), 500


@billing_bp.route('/invoices', methods=['GET'])
@jwt_required()
def list_invoices():
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        customer_id = _customer_id(_get_tenant_id())
        invoices = stripe.Invoice.list(customer=customer_id, limit=10)
        return jsonify({'invoices': invoices['data'], 'total': len(invoices['data'])}), 200
    except Exception as e:
        logger.error('list_invoices error', error=str(e))
        return jsonify({'error': str(e)}), 500


@billing_bp.route('/invoices/<invoice_id>', methods=['GET'])
@jwt_required()
def get_invoice(invoice_id):
    try:
        stripe, err = _stripe()
        if err:
            return jsonify(err), 503

        invoice = stripe.Invoice.retrieve(invoice_id)
        return jsonify({'invoice': invoice}), 200
    except Exception as e:
        logger.error('get_invoice error', error=str(e))
        return jsonify({'error': str(e)}), 500
