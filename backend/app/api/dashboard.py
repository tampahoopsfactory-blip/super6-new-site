"""Dashboard statistics, real-time data, and alert endpoints."""

from fastapi import APIRouter, Depends, Query
from sqlalchemy import select, func
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.event import Event, EventStatus
from app.models.ticket import Ticket, TicketStatus
from app.models.access_log import AccessLog, AccessEventType
from app.models.device import Device, DeviceStatus
from app.api.schemas import DashboardStats, EventResponse, AccessLogResponse

router = APIRouter(prefix="/api/dashboard", tags=["dashboard"])


@router.get("/stats", response_model=DashboardStats)
async def get_stats(db: AsyncSession = Depends(get_db)):
    # Active event
    event_result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    active_event = event_result.scalar_one_or_none()
    active_event_resp = EventResponse.model_validate(active_event) if active_event else None

    # Ticket stats for active event
    total_sold = 0
    revenue = 0.0
    if active_event:
        sold_result = await db.execute(
            select(func.count(Ticket.ticket_id)).where(
                Ticket.event_id == active_event.event_id,
                Ticket.status != TicketStatus.REFUNDED.value,
            )
        )
        total_sold = sold_result.scalar() or 0

        rev_result = await db.execute(
            select(func.sum(Ticket.amount_paid)).where(
                Ticket.event_id == active_event.event_id,
                Ticket.status != TicketStatus.REFUNDED.value,
            )
        )
        revenue = float(rev_result.scalar() or 0.0)

    # Entry stats
    total_entries = 0
    denied_count = 0
    if active_event:
        entries_result = await db.execute(
            select(func.count(AccessLog.log_id)).where(
                AccessLog.event_type.in_([
                    AccessEventType.QR_ENTRY.value,
                    AccessEventType.FACE_ENTRY.value,
                    AccessEventType.MANUAL_OVERRIDE.value,
                ])
            )
        )
        total_entries = entries_result.scalar() or 0

        denied_result = await db.execute(
            select(func.count(AccessLog.log_id)).where(
                AccessLog.event_type.in_([
                    AccessEventType.QR_DENIED.value,
                    AccessEventType.FACE_DENIED.value,
                ])
            )
        )
        denied_count = denied_result.scalar() or 0

    # Device stats
    devices_result = await db.execute(select(func.count(Device.device_id)))
    devices_total = devices_result.scalar() or 0

    online_result = await db.execute(
        select(func.count(Device.device_id)).where(
            Device.status == DeviceStatus.ONLINE.value
        )
    )
    devices_online = online_result.scalar() or 0

    patrons_inside = active_event.current_capacity if active_event else 0

    return DashboardStats(
        total_tickets_sold=total_sold,
        total_entries=total_entries,
        patrons_inside=patrons_inside,
        revenue=revenue,
        denied_count=denied_count,
        active_event=active_event_resp,
        devices_online=devices_online,
        devices_total=devices_total,
    )


@router.get("/recent-access", response_model=list[AccessLogResponse])
async def recent_access(
    limit: int = Query(20, ge=1, le=100),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(
        select(AccessLog).order_by(AccessLog.timestamp.desc()).limit(limit)
    )
    return result.scalars().all()


@router.get("/access-log", response_model=list[AccessLogResponse])
async def access_log(
    gate: str | None = None,
    event_type: str | None = None,
    page: int = Query(1, ge=1),
    per_page: int = Query(50, ge=1, le=200),
    db: AsyncSession = Depends(get_db),
):
    query = select(AccessLog).order_by(AccessLog.timestamp.desc())
    if gate:
        query = query.where(AccessLog.gate_id == gate)
    if event_type:
        query = query.where(AccessLog.event_type == event_type)
    query = query.offset((page - 1) * per_page).limit(per_page)
    result = await db.execute(query)
    return result.scalars().all()


@router.get("/alerts")
async def get_dashboard_alerts(limit: int = Query(50, ge=1, le=200)):
    """Get recent alerts from X05 devices for dashboard display."""
    from app.api.x05_api import _recent_alerts
    return _recent_alerts[:limit]
