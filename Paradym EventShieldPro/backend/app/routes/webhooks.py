from flask import Blueprint, request, jsonify, current_app
from datetime import datetime
import structlog

logger = structlog.get_logger()
webhooks_bp = Blueprint('webhooks', __name__)


@webhooks_bp.route('/stripe', methods=['POST'])
def stripe_webhook():
    """Handle Stripe webhook events."""
    try:
        payload = request.get_data(as_text=True)
        sig_header = request.headers.get('Stripe-Signature', '')
        webhook_secret = current_app.config.get('STRIPE_WEBHOOK_SECRET', '')

        # Signature verification (requires stripe package)
        if webhook_secret:
            try:
                import stripe
                stripe.api_key = current_app.config.get('STRIPE_SECRET_KEY', '')
                event = stripe.Webhook.construct_event(payload, sig_header, webhook_secret)
            except Exception as e:
                logger.warning('Stripe webhook signature verification failed', error=str(e))
                return jsonify({'error': 'Invalid signature'}), 400
        else:
            import json
            event = json.loads(payload)

        event_type = event.get('type', '')
        logger.info('Stripe webhook received', event_type=event_type)

        if event_type == 'customer.subscription.updated':
            _handle_subscription_updated(event['data']['object'])
        elif event_type == 'customer.subscription.deleted':
            _handle_subscription_canceled(event['data']['object'])
        elif event_type == 'invoice.payment_succeeded':
            _handle_payment_succeeded(event['data']['object'])
        elif event_type == 'invoice.payment_failed':
            _handle_payment_failed(event['data']['object'])

        return jsonify({'received': True}), 200

    except Exception as e:
        logger.error('Stripe webhook error', error=str(e))
        return jsonify({'error': 'Webhook processing failed'}), 500


@webhooks_bp.route('/hardware', methods=['POST'])
def hardware_webhook():
    """Handle inbound hardware device events."""
    try:
        data = request.get_json() or {}
        device_id = data.get('device_id')
        event_type = data.get('event_type')
        payload = data.get('payload', {})

        logger.info('Hardware event received', device_id=device_id, event_type=event_type)

        if event_type == 'face_recognized':
            _handle_face_recognized(device_id, payload)
        elif event_type == 'access_denied':
            _handle_access_denied(device_id, payload)
        elif event_type == 'device_online':
            logger.info('Device came online', device_id=device_id)
        elif event_type == 'device_offline':
            logger.warning('Device went offline', device_id=device_id)

        return jsonify({'received': True}), 200

    except Exception as e:
        logger.error('Hardware webhook error', error=str(e))
        return jsonify({'error': 'Webhook processing failed'}), 500


def _handle_subscription_updated(subscription):
    logger.info('Subscription updated', subscription_id=subscription.get('id'))


def _handle_subscription_canceled(subscription):
    logger.info('Subscription canceled', subscription_id=subscription.get('id'))


def _handle_payment_succeeded(invoice):
    logger.info('Payment succeeded', invoice_id=invoice.get('id'))


def _handle_payment_failed(invoice):
    logger.warning('Payment failed', invoice_id=invoice.get('id'))


def _handle_face_recognized(device_id, payload):
    logger.info('Face recognized', device_id=device_id, person_id=payload.get('person_id'))


def _handle_access_denied(device_id, payload):
    logger.warning('Access denied', device_id=device_id, reason=payload.get('reason'))
