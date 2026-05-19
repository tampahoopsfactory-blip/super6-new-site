import re
from app import db
from app.models.event import Event, EventStatus
import structlog

logger = structlog.get_logger()


def _slugify(title):
    slug = re.sub(r'[^\w\s-]', '', title.lower())
    slug = re.sub(r'[\s_-]+', '-', slug).strip('-')
    return slug or 'event'


def _unique_slug(base_slug):
    slug = base_slug
    counter = 1
    while Event.query.filter_by(slug=slug).first():
        slug = f'{base_slug}-{counter}'
        counter += 1
    return slug


class EventService:
    def create_event(self, data, tenant_id, organizer_id):
        base_slug = _slugify(data.get('title', ''))
        slug = _unique_slug(base_slug)

        allowed = {c.key for c in Event.__table__.columns} - {'id', 'created_at', 'updated_at'}
        kwargs = {k: v for k, v in data.items() if k in allowed}
        kwargs.update({
            'slug': slug,
            'tenant_id': tenant_id,
            'organizer_id': organizer_id,
            'status': EventStatus.DRAFT,
        })

        event = Event(**kwargs)
        try:
            db.session.add(event)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise
        return event

    def get_event(self, event_id, tenant_id):
        return Event.query.filter_by(id=event_id, tenant_id=tenant_id).first()

    def update_event(self, event, data):
        protected = {'id', 'tenant_id', 'organizer_id', 'created_at', 'slug'}
        allowed = {c.key for c in Event.__table__.columns} - protected
        for key, value in data.items():
            if key in allowed:
                setattr(event, key, value)
        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise
        return event

    def delete_event(self, event):
        try:
            db.session.delete(event)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

    def publish_event(self, event):
        event.status = EventStatus.PUBLISHED
        event.is_approved = True
        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

    def unpublish_event(self, event):
        event.status = EventStatus.DRAFT
        try:
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise

    def duplicate_event(self, event, user):
        base_slug = _unique_slug(f'{event.slug}-copy')
        allowed = {c.key for c in Event.__table__.columns} - {'id', 'created_at', 'updated_at', 'slug', 'organizer_id'}
        kwargs = {k: getattr(event, k) for k in allowed if getattr(event, k, None) is not None}
        kwargs.update({
            'title': f'{event.title} (Copy)',
            'slug': base_slug,
            'organizer_id': user.id,
            'status': EventStatus.DRAFT,
            'is_approved': False,
        })
        new_event = Event(**kwargs)
        try:
            db.session.add(new_event)
            db.session.commit()
        except Exception:
            db.session.rollback()
            raise
        return new_event

    def get_event_statistics(self, event):
        ticket_count = len(event.tickets) if event.tickets else 0
        return {
            'event_id': event.id,
            'total_tickets': ticket_count,
            'current_registrations': event.current_registrations,
            'max_capacity': event.max_capacity,
            'is_full': event.is_full,
            'is_registration_open': event.is_registration_open,
        }

    def check_event_limit(self, tenant_id):
        return True

    def increment_event_usage(self, tenant_id):
        pass

    def decrement_event_usage(self, tenant_id):
        pass
