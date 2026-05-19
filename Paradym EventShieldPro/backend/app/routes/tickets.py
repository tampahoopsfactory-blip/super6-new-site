"""
EventShield Pro - Tickets Route Blueprint
Ticket management including validation and revocation
"""

from datetime import datetime

from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt_identity
import structlog

from app.models import Ticket, TicketValidation, User, db
from app.utils.decorators import validate_json
from app.utils.validators import ticket_schemas

logger = structlog.get_logger()
tickets_bp = Blueprint('tickets', __name__, url_prefix='/api/v1/tickets')


@tickets_bp.route('/', methods=['GET'])
@jwt_required()
def list_tickets():
    """List tickets, optionally filtered by event_id or status"""
    try:
        page = request.args.get('page', 1, type=int)
        per_page = min(request.args.get('per_page', 20, type=int), 100)
        event_id = request.args.get('event_id', type=int)
        status = request.args.get('status')

        query = Ticket.query
        if event_id:
            query = query.filter_by(event_id=event_id)
        if status:
            query = query.filter(Ticket.status.ilike(status))

        pagination = query.order_by(Ticket.created_at.desc()).paginate(
            page=page, per_page=per_page, error_out=False
        )

        tickets = []
        for t in pagination.items:
            tickets.append({
                'id': t.id,
                'ticket_number': t.ticket_number,
                'event_id': t.event_id,
                'status': t.status.value if t.status else None,
                'original_price': str(t.original_price),
                'final_price': str(t.final_price),
                'currency': t.currency,
                'qr_code': t.qr_code,
                'created_at': t.created_at.isoformat() if t.created_at else None,
            })

        return jsonify({
            'tickets': tickets,
            'total': pagination.total,
            'page': page,
            'per_page': per_page,
            'pages': pagination.pages,
        }), 200

    except Exception as e:
        logger.error("List tickets error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/', methods=['POST'])
@jwt_required()
@validate_json(ticket_schemas['create'])
def create_ticket():
    """Create a new ticket"""
    try:
        current_user_id = get_jwt_identity()
        data = request.get_json()

        ticket = Ticket(
            event_id=data['event_id'],
            ticket_type_id=data['ticket_type_id'],
            purchaser_id=data.get('purchaser_id', current_user_id),
            attendee_id=data.get('attendee_id', current_user_id),
            original_price=data['original_price'],
            final_price=data['final_price'],
            discount_amount=data.get('discount_amount', 0),
            tax_amount=data.get('tax_amount', 0),
            currency=data.get('currency', 'USD'),
            notes=data.get('notes'),
        )
        db.session.add(ticket)
        db.session.commit()

        logger.info("Ticket created", ticket_id=ticket.id, created_by=current_user_id)
        return jsonify({
            'message': 'Ticket created',
            'ticket': {
                'id': ticket.id,
                'ticket_number': ticket.ticket_number,
                'qr_code': ticket.qr_code,
                'access_code': ticket.access_code,
                'status': ticket.status.value if ticket.status else None,
            }
        }), 201

    except Exception as e:
        logger.error("Create ticket error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/<int:ticket_id>', methods=['GET'])
@jwt_required()
def get_ticket(ticket_id):
    """Get a ticket by ID"""
    try:
        ticket = Ticket.query.get(ticket_id)
        if not ticket:
            return jsonify({'error': 'Ticket not found'}), 404

        return jsonify({
            'id': ticket.id,
            'ticket_number': ticket.ticket_number,
            'event_id': ticket.event_id,
            'ticket_type_id': ticket.ticket_type_id,
            'purchaser_id': ticket.purchaser_id,
            'attendee_id': ticket.attendee_id,
            'status': ticket.status.value if ticket.status else None,
            'original_price': str(ticket.original_price),
            'final_price': str(ticket.final_price),
            'currency': ticket.currency,
            'qr_code': ticket.qr_code,
            'access_code': ticket.access_code,
            'check_in_time': ticket.check_in_time.isoformat() if ticket.check_in_time else None,
            'created_at': ticket.created_at.isoformat() if ticket.created_at else None,
        }), 200

    except Exception as e:
        logger.error("Get ticket error", error=str(e), ticket_id=ticket_id)
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/<int:ticket_id>', methods=['PUT'])
@jwt_required()
def update_ticket(ticket_id):
    """Update a ticket"""
    try:
        ticket = Ticket.query.get(ticket_id)
        if not ticket:
            return jsonify({'error': 'Ticket not found'}), 404

        data = request.get_json(silent=True) or {}
        for field in ('notes', 'check_in_location', 'attendee_id'):
            if field in data:
                setattr(ticket, field, data[field])

        db.session.commit()
        return jsonify({'message': 'Ticket updated'}), 200

    except Exception as e:
        logger.error("Update ticket error", error=str(e), ticket_id=ticket_id)
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/<int:ticket_id>', methods=['DELETE'])
@jwt_required()
def delete_ticket(ticket_id):
    """Cancel (soft-delete) a ticket"""
    try:
        ticket = Ticket.query.get(ticket_id)
        if not ticket:
            return jsonify({'error': 'Ticket not found'}), 404

        ticket.mark_as_cancelled()
        db.session.commit()
        logger.info("Ticket cancelled", ticket_id=ticket_id)
        return jsonify({'message': 'Ticket cancelled'}), 200

    except Exception as e:
        logger.error("Delete ticket error", error=str(e), ticket_id=ticket_id)
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/<int:ticket_id>/validate', methods=['POST'])
@jwt_required()
def validate_ticket(ticket_id):
    """Mark a ticket as used (validate entry)"""
    try:
        current_user_id = get_jwt_identity()
        ticket = Ticket.query.get(ticket_id)
        if not ticket:
            return jsonify({'error': 'Ticket not found'}), 404

        if ticket.is_used:
            return jsonify({'error': 'Ticket has already been used', 'valid': False}), 409

        if not ticket.is_valid:
            return jsonify({'error': 'Ticket is not valid for entry', 'valid': False}), 400

        data = request.get_json(silent=True) or {}
        location = data.get('location')

        ticket.mark_as_used(location=location)
        db.session.commit()

        logger.info("Ticket validated", ticket_id=ticket_id, validated_by=current_user_id)
        return jsonify({'message': 'Ticket validated successfully', 'valid': True}), 200

    except Exception as e:
        logger.error("Validate ticket error", error=str(e), ticket_id=ticket_id)
        return jsonify({'error': 'Internal server error'}), 500


@tickets_bp.route('/<int:ticket_id>/revoke', methods=['POST'])
@jwt_required()
def revoke_ticket(ticket_id):
    """Revoke a ticket"""
    try:
        current_user_id = get_jwt_identity()
        ticket = Ticket.query.get(ticket_id)
        if not ticket:
            return jsonify({'error': 'Ticket not found'}), 404

        ticket.mark_as_cancelled()
        db.session.commit()

        logger.info("Ticket revoked", ticket_id=ticket_id, revoked_by=current_user_id)
        return jsonify({'message': 'Ticket revoked'}), 200

    except Exception as e:
        logger.error("Revoke ticket error", error=str(e), ticket_id=ticket_id)
        return jsonify({'error': 'Internal server error'}), 500
