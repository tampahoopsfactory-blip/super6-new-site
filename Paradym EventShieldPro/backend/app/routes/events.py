from flask import Blueprint, request, jsonify, current_app
from flask_jwt_extended import jwt_required, get_jwt_identity
from sqlalchemy import and_, or_, desc, asc
import structlog

from app.models import Event, EventCategory, EventLocation, EventSchedule, User, Tenant, db
from app.services.event_service import EventService
from app.services.license_service import LicenseService
from app.utils.decorators import validate_json, rate_limit, require_permission
from app.utils.validators import event_schemas
from app.utils.pagination import paginate_results

logger = structlog.get_logger()
events_bp = Blueprint('events', __name__, url_prefix='/api/events')

@events_bp.route('/', methods=['GET'])
@jwt_required()
def get_events():
    """Get list of events with filtering and pagination"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        # Get query parameters
        page = request.args.get('page', 1, type=int)
        per_page = min(request.args.get('per_page', 20, type=int), 100)
        search = request.args.get('search', '').strip()
        category_id = request.args.get('category_id', type=int)
        status = request.args.get('status')
        start_date = request.args.get('start_date')
        end_date = request.args.get('end_date')
        sort_by = request.args.get('sort_by', 'start_datetime')
        sort_order = request.args.get('sort_order', 'asc')
        
        # Build query
        query = Event.query.filter_by(tenant_id=user.tenant_id)
        
        # Apply filters
        if search:
            search_filter = or_(
                Event.title.ilike(f'%{search}%'),
                Event.description.ilike(f'%{search}%'),
                Event.location.ilike(f'%{search}%')
            )
            query = query.filter(search_filter)
        
        if category_id:
            query = query.filter_by(category_id=category_id)
        
        if status:
            query = query.filter_by(status=status)
        
        if start_date:
            query = query.filter(Event.start_datetime >= start_date)
        
        if end_date:
            query = query.filter(Event.end_datetime <= end_date)
        
        # Apply sorting
        if hasattr(Event, sort_by):
            sort_column = getattr(Event, sort_by)
            if sort_order.lower() == 'desc':
                query = query.order_by(desc(sort_column))
            else:
                query = query.order_by(asc(sort_column))
        else:
            query = query.order_by(asc(Event.start_datetime))
        
        # Paginate results
        paginated_events = paginate_results(query, page, per_page)
        
        # Format response
        events_data = []
        for event in paginated_events.items:
            event_data = {
                'id': event.id,
                'title': event.title,
                'slug': event.slug,
                'description': event.description,
                'status': event.status.value,
                'start_datetime': event.start_datetime.isoformat(),
                'end_datetime': event.end_datetime.isoformat(),
                'location': event.location,
                'max_capacity': event.max_capacity,
                'current_registrations': event.current_registrations,
                'is_registration_open': event.is_registration_open,
                'is_full': event.is_full,
                'created_at': event.created_at.isoformat(),
                'category': {
                    'id': event.category.id,
                    'name': event.category.name,
                    'color': event.category.color
                } if event.category else None,
                'organizer': {
                    'id': event.organizer.id,
                    'name': f"{event.organizer.first_name} {event.organizer.last_name}",
                    'email': event.organizer.email
                } if event.organizer else None
            }
            events_data.append(event_data)
        
        return jsonify({
            'events': events_data,
            'pagination': {
                'page': page,
                'per_page': per_page,
                'total': paginated_events.total,
                'pages': paginated_events.pages,
                'has_next': paginated_events.has_next,
                'has_prev': paginated_events.has_prev
            }
        }), 200
        
    except Exception as e:
        logger.error("Get events error", error=str(e), user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>', methods=['GET'])
@jwt_required()
def get_event(event_id):
    """Get single event by ID"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Get event schedules
        schedules = []
        for schedule in event.schedules:
            schedule_data = {
                'id': schedule.id,
                'start_datetime': schedule.start_datetime.isoformat(),
                'end_datetime': schedule.end_datetime.isoformat(),
                'timezone': schedule.timezone,
                'is_recurring': schedule.is_recurring,
                'recurrence_pattern': schedule.recurrence_pattern
            }
            schedules.append(schedule_data)
        
        # Get event location details
        location_data = None
        if event.location_model:
            location_data = {
                'id': event.location_model.id,
                'name': event.location_model.name,
                'address': event.location_model.address,
                'city': event.location_model.city,
                'state': event.location_model.state,
                'country': event.location_model.country,
                'postal_code': event.location_model.postal_code,
                'coordinates': event.location_model.coordinates,
                'venue_type': event.location_model.venue_type
            }
        
        event_data = {
            'id': event.id,
            'title': event.title,
            'slug': event.slug,
            'description': event.description,
            'short_description': event.short_description,
            'status': event.status.value,
            'visibility': event.visibility.value,
            'event_type': event.event_type.value,
            'start_datetime': event.start_datetime.isoformat(),
            'end_datetime': event.end_datetime.isoformat(),
            'location': event.location,
            'location_details': location_data,
            'max_capacity': event.max_capacity,
            'current_registrations': event.current_registrations,
            'is_registration_open': event.is_registration_open,
            'is_full': event.is_full,
            'registration_deadline': event.registration_deadline.isoformat() if event.registration_deadline else None,
            'pricing': event.pricing,
            'currency': event.currency,
            'includes_meal': event.includes_meal,
            'includes_drinks': event.includes_drinks,
            'includes_swag': event.includes_swag,
            'dress_code': event.dress_code,
            'age_restriction': event.age_restriction,
            'accessibility_features': event.accessibility_features,
            'media': event.media,
            'metadata': event.metadata,
            'created_at': event.created_at.isoformat(),
            'updated_at': event.updated_at.isoformat(),
            'category': {
                'id': event.category.id,
                'name': event.category.name,
                'description': event.category.description,
                'color': event.category.color,
                'icon': event.category.icon
            } if event.category else None,
            'organizer': {
                'id': event.organizer.id,
                'name': f"{event.organizer.first_name} {event.organizer.last_name}",
                'email': event.organizer.email
            } if event.organizer else None,
            'schedules': schedules
        }
        
        return jsonify({'event': event_data}), 200
        
    except Exception as e:
        logger.error("Get event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/', methods=['POST'])
@jwt_required()
@require_permission('event:create')
@validate_json(event_schemas['create'])
def create_event():
    """Create a new event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        # Check license limits
        license_service = LicenseService()
        if not license_service.check_event_limit(user.tenant_id):
            return jsonify({'error': 'Event limit reached for your subscription tier'}), 403
        
        data = request.get_json()
        
        # Create event
        event_service = EventService()
        event = event_service.create_event(
            title=data['title'],
            description=data['description'],
            start_datetime=data['start_datetime'],
            end_datetime=data['end_datetime'],
            location=data.get('location'),
            max_capacity=data.get('max_capacity'),
            organizer_id=current_user_id,
            tenant_id=user.tenant_id,
            **data
        )
        
        # Increment license usage
        license_service.increment_event_usage(user.tenant_id)
        
        logger.info("Event created successfully", 
                   event_id=event.id, user_id=current_user_id, tenant_id=user.tenant_id)
        
        return jsonify({
            'message': 'Event created successfully',
            'event_id': event.id,
            'event_slug': event.slug
        }), 201
        
    except ValueError as e:
        return jsonify({'error': str(e)}), 400
    except Exception as e:
        logger.error("Create event error", error=str(e), user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>', methods=['PUT'])
@jwt_required()
@require_permission('event:update')
@validate_json(event_schemas['update'])
def update_event(event_id):
    """Update an existing event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Check if user can edit this event
        if not user.has_permission('event:update_all') and event.organizer_id != current_user_id:
            return jsonify({'error': 'Permission denied'}), 403
        
        data = request.get_json()
        
        # Update event
        event_service = EventService()
        updated_event = event_service.update_event(event, data)
        
        logger.info("Event updated successfully", 
                   event_id=event_id, user_id=current_user_id)
        
        return jsonify({
            'message': 'Event updated successfully',
            'event_id': event_id
        }), 200
        
    except ValueError as e:
        return jsonify({'error': str(e)}), 400
    except Exception as e:
        logger.error("Update event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>', methods=['DELETE'])
@jwt_required()
@require_permission('event:delete')
def delete_event(event_id):
    """Delete an event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Check if user can delete this event
        if not user.has_permission('event:delete_all') and event.organizer_id != current_user_id:
            return jsonify({'error': 'Permission denied'}), 403
        
        # Check if event has tickets
        if event.tickets and len(event.tickets) > 0:
            return jsonify({'error': 'Cannot delete event with existing tickets'}), 400
        
        # Delete event
        event_service = EventService()
        event_service.delete_event(event)
        
        # Decrement license usage
        license_service = LicenseService()
        license_service.decrement_event_usage(user.tenant_id)
        
        logger.info("Event deleted successfully", 
                   event_id=event_id, user_id=current_user_id)
        
        return jsonify({'message': 'Event deleted successfully'}), 200
        
    except Exception as e:
        logger.error("Delete event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>/publish', methods=['POST'])
@jwt_required()
@require_permission('event:publish')
def publish_event(event_id):
    """Publish an event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Check if user can publish this event
        if not user.has_permission('event:publish_all') and event.organizer_id != current_user_id:
            return jsonify({'error': 'Permission denied'}), 403
        
        # Publish event
        event_service = EventService()
        event_service.publish_event(event)
        
        logger.info("Event published successfully", 
                   event_id=event_id, user_id=current_user_id)
        
        return jsonify({'message': 'Event published successfully'}), 200
        
    except ValueError as e:
        return jsonify({'error': str(e)}), 400
    except Exception as e:
        logger.error("Publish event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>/unpublish', methods=['POST'])
@jwt_required()
@require_permission('event:publish')
def unpublish_event(event_id):
    """Unpublish an event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Check if user can unpublish this event
        if not user.has_permission('event:publish_all') and event.organizer_id != current_user_id:
            return jsonify({'error': 'Permission denied'}), 403
        
        # Unpublish event
        event_service = EventService()
        event_service.unpublish_event(event)
        
        logger.info("Event unpublished successfully", 
                   event_id=event_id, user_id=current_user_id)
        
        return jsonify({'message': 'Event unpublished successfully'}), 200
        
    except Exception as e:
        logger.error("Unpublish event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/categories', methods=['GET'])
@jwt_required()
def get_event_categories():
    """Get list of event categories"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        categories = EventCategory.query.filter_by(tenant_id=user.tenant_id, is_active=True).all()
        
        categories_data = []
        for category in categories:
            category_data = {
                'id': category.id,
                'name': category.name,
                'slug': category.slug,
                'description': category.description,
                'icon': category.icon,
                'color': category.color,
                'event_count': len(category.events)
            }
            categories_data.append(category_data)
        
        return jsonify({'categories': categories_data}), 200
        
    except Exception as e:
        logger.error("Get categories error", error=str(e), user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/categories', methods=['POST'])
@jwt_required()
@require_permission('event:manage_categories')
@validate_json(event_schemas['create_category'])
def create_event_category():
    """Create a new event category"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        data = request.get_json()
        
        # Check if category already exists
        existing_category = EventCategory.query.filter_by(
            name=data['name'], 
            tenant_id=user.tenant_id
        ).first()
        
        if existing_category:
            return jsonify({'error': 'Category already exists'}), 409
        
        # Create category
        category = EventCategory(
            name=data['name'],
            slug=data.get('slug') or data['name'].lower().replace(' ', '-'),
            description=data.get('description'),
            icon=data.get('icon'),
            color=data.get('color', '#000000'),
            tenant_id=user.tenant_id
        )
        
        db.session.add(category)
        db.session.commit()
        
        logger.info("Event category created successfully", 
                   category_id=category.id, user_id=current_user_id)
        
        return jsonify({
            'message': 'Category created successfully',
            'category_id': category.id
        }), 201
        
    except Exception as e:
        logger.error("Create category error", error=str(e), user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>/stats', methods=['GET'])
@jwt_required()
@require_permission('event:read_stats')
def get_event_stats(event_id):
    """Get event statistics"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Get event statistics
        event_service = EventService()
        stats = event_service.get_event_statistics(event)
        
        return jsonify({'stats': stats}), 200
        
    except Exception as e:
        logger.error("Get event stats error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

@events_bp.route('/<int:event_id>/duplicate', methods=['POST'])
@jwt_required()
@require_permission('event:create')
def duplicate_event(event_id):
    """Duplicate an existing event"""
    try:
        current_user_id = get_jwt_identity()
        user = User.query.get(current_user_id)
        
        event = Event.query.filter_by(
            id=event_id, 
            tenant_id=user.tenant_id
        ).first()
        
        if not event:
            return jsonify({'error': 'Event not found'}), 404
        
        # Check license limits
        license_service = LicenseService()
        if not license_service.check_event_limit(user.tenant_id):
            return jsonify({'error': 'Event limit reached for your subscription tier'}), 403
        
        # Duplicate event
        event_service = EventService()
        duplicated_event = event_service.duplicate_event(event, user)
        
        # Increment license usage
        license_service.increment_event_usage(user.tenant_id)
        
        logger.info("Event duplicated successfully", 
                   original_event_id=event_id, new_event_id=duplicated_event.id, user_id=current_user_id)
        
        return jsonify({
            'message': 'Event duplicated successfully',
            'new_event_id': duplicated_event.id,
            'new_event_slug': duplicated_event.slug
        }), 201
        
    except Exception as e:
        logger.error("Duplicate event error", error=str(e), event_id=event_id, user_id=current_user_id)
        return jsonify({'error': 'Internal server error'}), 500

