"""Event management endpoints."""

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy import select, update
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.event import Event, EventStatus
from app.models.ticket import Ticket, TicketStatus
from app.api.schemas import EventCreate, EventResponse
from app.utils.auth import get_current_admin

router = APIRouter(prefix="/api/events", tags=["events"])


@router.get("", response_model=list[EventResponse])
async def list_events(db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Event).order_by(Event.created_at.desc()))
    return result.scalars().all()


@router.get("/active", response_model=EventResponse | None)
async def get_active_event(db: AsyncSession = Depends(get_db)):
    result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    return result.scalar_one_or_none()


@router.get("/{event_id}", response_model=EventResponse)
async def get_event(event_id: str, db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Event).where(Event.event_id == event_id))
    event = result.scalar_one_or_none()
    if not event:
        raise HTTPException(status_code=404, detail="Event not found")
    return event


@router.post("", response_model=EventResponse)
async def create_event(
    body: EventCreate,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    event = Event(
        name=body.name,
        event_date=body.event_date,
        start_time=body.start_time,
        end_time=body.end_time,
        admission_price=body.admission_price,
        vip_price=body.vip_price,
        max_capacity=body.max_capacity,
        multi_entry=body.multi_entry,
        ticket_expiry_hours=body.ticket_expiry_hours,
    )
    db.add(event)
    await db.commit()
    await db.refresh(event)
    return event


@router.post("/{event_id}/activate", response_model=EventResponse)
async def activate_event(
    event_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    # Deactivate any current active event first
    await db.execute(
        update(Event)
        .where(Event.status == EventStatus.ACTIVE.value)
        .values(status=EventStatus.ENDED.value)
    )
    await db.execute(
        update(Event).where(Event.event_id == event_id).values(status=EventStatus.ACTIVE.value)
    )
    await db.commit()
    result = await db.execute(select(Event).where(Event.event_id == event_id))
    return result.scalar_one()


@router.post("/{event_id}/end", response_model=EventResponse)
async def end_event(
    event_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    await db.execute(
        update(Event).where(Event.event_id == event_id).values(status=EventStatus.ENDED.value)
    )
    await db.execute(
        update(Ticket)
        .where(Ticket.event_id == event_id, Ticket.status == TicketStatus.ACTIVE.value)
        .values(status=TicketStatus.EXPIRED.value)
    )
    await db.commit()
    result = await db.execute(select(Event).where(Event.event_id == event_id))
    return result.scalar_one()
