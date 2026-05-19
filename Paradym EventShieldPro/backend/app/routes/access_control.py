"""
EventShield Pro - Access Control Route Blueprint
Access log management, grant/deny entry, and statistics
"""

from datetime import datetime, timedelta

from flask import Blueprint, request, jsonify
from flask_jwt_extended import jwt_required, get_jwt_identity
from sqlalchemy import func
import structlog

from app.models import AccessLog, AccessPoint, User, db

logger = structlog.get_logger()
access_control_bp = Blueprint('access_control', __name__, url_prefix='/api/v1/access-control')


@access_control_bp.route('/logs', methods=['GET'])
@jwt_required()
def list_logs():
    """List access logs with optional filters"""
    try:
        page = request.args.get('page', 1, type=int)
        per_page = min(request.args.get('per_page', 50, type=int), 200)
        event_id = request.args.get('event_id', type=int)
        user_id = request.args.get('user_id', type=int)
        device_id = request.args.get('device_id')
        status = request.args.get('status')
        date_from = request.args.get('date_from')
        date_to = request.args.get('date_to')

        query = AccessLog.query

        if event_id:
            query = query.filter_by(event_id=event_id)
        if user_id:
            query = query.filter_by(user_id=user_id)
        if device_id:
            query = query.filter_by(device_id=device_id)
        if status:
            query = query.filter(AccessLog.access_status.ilike(status))
        if date_from:
            try:
                dt_from = datetime.fromisoformat(date_from)
                query = query.filter(AccessLog.access_time >= dt_from)
            except ValueError:
                return jsonify({'error': 'Invalid date_from format'}), 400
        if date_to:
            try:
                dt_to = datetime.fromisoformat(date_to)
                query = query.filter(AccessLog.access_time <= dt_to)
            except ValueError:
                return jsonify({'error': 'Invalid date_to format'}), 400

        pagination = query.order_by(AccessLog.access_time.desc()).paginate(
            page=page, per_page=per_page, error_out=False
        )

        logs = []
        for log in pagination.items:
            logs.append({
                'id': log.id,
                'event_id': log.event_id,
                'access_point_id': log.access_point_id,
                'user_id': log.user_id,
                'ticket_id': log.ticket_id,
                'access_type': log.access_type.value if log.access_type else None,
                'access_status': log.access_status.value if log.access_status else None,
                'validation_method': log.validation_method.value if log.validation_method else None,
                'access_time': log.access_time.isoformat() if log.access_time else None,
                'device_id': log.device_id,
                'location': log.location,
            })

        return jsonify({
            'logs': logs,
            'total': pagination.total,
            'page': page,
            'per_page': per_page,
            'pages': pagination.pages,
        }), 200

    except Exception as e:
        logger.error("List access logs error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@access_control_bp.route('/grant', methods=['POST'])
@jwt_required()
def grant_access():
    """Record a granted access entry"""
    try:
        current_user_id = get_jwt_identity()
        data = request.get_json(silent=True) or {}

        event_id = data.get('event_id')
        ticket_id = data.get('ticket_id')
        access_point_id = data.get('access_point_id')

        if not event_id:
            return jsonify({'error': 'event_id is required'}), 400
        if not access_point_id:
            return jsonify({'error': 'access_point_id is required'}), 400

        from app.models.access_control import AccessType, AccessStatus, ValidationMethod

        log = AccessLog(
            event_id=event_id,
            access_point_id=access_point_id,
            user_id=data.get('user_id', current_user_id),
            ticket_id=ticket_id,
            access_type=AccessType.ENTRY,
            access_status=AccessStatus.GRANTED,
            validation_method=ValidationMethod(data.get('validation_method', 'manual')),
            device_id=data.get('device_id'),
            location=data.get('location'),
            validation_notes=data.get('notes'),
        )
        db.session.add(log)
        db.session.commit()

        logger.info("Access granted", log_id=log.id, event_id=event_id, ticket_id=ticket_id)
        return jsonify({'message': 'Access granted', 'log_id': log.id}), 201

    except Exception as e:
        logger.error("Grant access error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@access_control_bp.route('/deny', methods=['POST'])
@jwt_required()
def deny_access():
    """Record a denied access attempt"""
    try:
        current_user_id = get_jwt_identity()
        data = request.get_json(silent=True) or {}

        event_id = data.get('event_id')
        access_point_id = data.get('access_point_id')
        reason = data.get('reason', 'Access denied')

        if not event_id:
            return jsonify({'error': 'event_id is required'}), 400
        if not access_point_id:
            return jsonify({'error': 'access_point_id is required'}), 400

        from app.models.access_control import AccessType, AccessStatus, ValidationMethod

        log = AccessLog(
            event_id=event_id,
            access_point_id=access_point_id,
            user_id=data.get('user_id'),
            ticket_id=data.get('ticket_id'),
            access_type=AccessType.ENTRY,
            access_status=AccessStatus.DENIED,
            validation_method=ValidationMethod(data.get('validation_method', 'manual')),
            device_id=data.get('device_id'),
            location=data.get('location'),
            validation_notes=reason,
            error_message=reason,
        )
        db.session.add(log)
        db.session.commit()

        logger.info("Access denied", log_id=log.id, event_id=event_id, reason=reason)
        return jsonify({'message': 'Access denied recorded', 'log_id': log.id}), 201

    except Exception as e:
        logger.error("Deny access error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500


@access_control_bp.route('/stats', methods=['GET'])
@jwt_required()
def access_stats():
    """Summary counts of access events by date and device"""
    try:
        event_id = request.args.get('event_id', type=int)
        days = request.args.get('days', 7, type=int)
        since = datetime.utcnow() - timedelta(days=days)

        query = AccessLog.query.filter(AccessLog.access_time >= since)
        if event_id:
            query = query.filter_by(event_id=event_id)

        logs = query.all()

        total = len(logs)
        granted = sum(1 for l in logs if l.access_status and l.access_status.value == 'granted')
        denied = sum(1 for l in logs if l.access_status and l.access_status.value == 'denied')

        # Group by date
        by_date = {}
        for log in logs:
            if log.access_time:
                date_key = log.access_time.date().isoformat()
                by_date[date_key] = by_date.get(date_key, 0) + 1

        # Group by device
        by_device = {}
        for log in logs:
            if log.device_id:
                by_device[log.device_id] = by_device.get(log.device_id, 0) + 1

        return jsonify({
            'total': total,
            'granted': granted,
            'denied': denied,
            'by_date': by_date,
            'by_device': by_device,
            'period_days': days,
        }), 200

    except Exception as e:
        logger.error("Access stats error", error=str(e))
        return jsonify({'error': 'Internal server error'}), 500
