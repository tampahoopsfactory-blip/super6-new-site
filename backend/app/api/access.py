"""
Access control endpoints — called by X05 devices at the gates.

These endpoints handle:
  - QR code validation (first-time entry with enrollment)
  - Face enrollment completion callback
  - Face-only re-entry validation
"""

from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException, Header
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.ticket import Ticket, TicketStatus
from app.models.event import Event, EventStatus
from app.models.access_log import AccessLog, AccessEventType
from app.models.device import Device, DeviceStatus
from app.api.schemas import (
    ValidateTicketRequest, ValidateTicketResponse,
    EnrollCompleteRequest, FaceEntryRequest, FaceEntryResponse,
)
from app.utils.crypto import hash_token
from app.config import settings

router = APIRouter(prefix="/api", tags=["access"])


def _verify_device_key(x_api_key: str = Header(None)):
    if x_api_key != settings.api_key:
        raise HTTPException(status_code=403, detail="DEVICE_UNAUTHORIZED")


@router.post("/validate_ticket", response_model=ValidateTicketResponse)
async def validate_ticket(
    body: ValidateTicketRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    # Verify device is registered
    device_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = device_result.scalar_one_or_none()
    if not device:
        return ValidateTicketResponse(
            status="DENY", reason_code="DEVICE_UNAUTHORIZED", grant_access=False
        )

    # Verify active event
    event_result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    active_event = event_result.scalar_one_or_none()
    if not active_event:
        _log_access(db, None, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "EVENT_NOT_ACTIVE")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="EVENT_NOT_ACTIVE", grant_access=False
        )

    # Look up ticket by token hash
    token_hash = hash_token(body.token)
    ticket_result = await db.execute(
        select(Ticket).where(Ticket.qr_token_hash == token_hash)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TOKEN_NOT_FOUND")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TOKEN_NOT_FOUND", grant_access=False
        )

    # Check ticket status
    if ticket.status == TicketStatus.REFUNDED.value:
        _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TICKET_REFUNDED")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TICKET_REFUNDED", grant_access=False,
            ticket_id=ticket.ticket_id,
        )

    if ticket.status == TicketStatus.EXPIRED.value:
        _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TOKEN_EXPIRED")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TOKEN_EXPIRED", grant_access=False,
            ticket_id=ticket.ticket_id,
        )

    if ticket.status != TicketStatus.ACTIVE.value:
        _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TICKET_INVALID")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TICKET_INVALID", grant_access=False,
            ticket_id=ticket.ticket_id,
        )

    # Check per-ticket-type expiry (DAILY expires end of day, WEEKEND end of Sunday, etc.)
    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TOKEN_EXPIRED")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TOKEN_EXPIRED", grant_access=False,
            ticket_id=ticket.ticket_id,
        )

    # Check re-entry rules (per ticket type, not just event-level)
    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_DENIED.value, "TICKET_ALREADY_USED")
        await db.commit()
        return ValidateTicketResponse(
            status="DENY", reason_code="TICKET_ALREADY_USED", grant_access=False,
            ticket_id=ticket.ticket_id,
        )

    # If face not enrolled, tell device to enroll
    if not ticket.face_enrolled:
        return ValidateTicketResponse(
            status="ENROLL",
            ticket_id=ticket.ticket_id,
            admission_type=ticket.admission_type,
            grant_access=False,
        )

    # Face already enrolled — grant access
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    if not ticket.first_gate_id:
        ticket.first_gate_id = device.gate_id
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, device.gate_id, AccessEventType.QR_ENTRY.value)
    await db.commit()

    return ValidateTicketResponse(
        status="GRANT",
        ticket_id=ticket.ticket_id,
        admission_type=ticket.admission_type,
        grant_access=True,
    )


@router.post("/enroll_complete")
async def enroll_complete(
    body: EnrollCompleteRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    result = await db.execute(select(Ticket).where(Ticket.ticket_id == body.ticket_id))
    ticket = result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")

    device_result = await db.execute(select(Device).where(Device.device_id == body.device_id))
    device = device_result.scalar_one_or_none()

    ticket.face_enrolled = True
    ticket.face_id = body.face_id
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    if not ticket.first_gate_id and device:
        ticket.first_gate_id = device.gate_id

    # Update event capacity
    event_result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    active_event = event_result.scalar_one_or_none()
    if active_event:
        active_event.current_capacity += 1

    gate_id = device.gate_id if device else body.device_id
    _log_access(db, ticket.ticket_id, body.device_id, gate_id, AccessEventType.QR_ENTRY.value)
    await db.commit()

    return {"grant_access": True, "message": "Face enrolled and access granted"}


@router.post("/face_entry", response_model=FaceEntryResponse)
async def face_entry(
    body: FaceEntryRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    device_result = await db.execute(select(Device).where(Device.device_id == body.device_id))
    device = device_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    # Look up ticket by face_id
    ticket_result = await db.execute(
        select(Ticket).where(Ticket.face_id == body.face_id)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, gate_id, AccessEventType.FACE_DENIED.value, "FACE_MISMATCH")
        await db.commit()
        return FaceEntryResponse(grant_access=False, reason="FACE_MISMATCH")

    # Verify active event
    event_result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    active_event = event_result.scalar_one_or_none()
    if not active_event:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id, AccessEventType.FACE_DENIED.value, "EVENT_NOT_ACTIVE")
        await db.commit()
        return FaceEntryResponse(grant_access=False, reason="EVENT_NOT_ACTIVE")

    if ticket.status != TicketStatus.ACTIVE.value:
        reason = "TICKET_EXPIRED" if ticket.status == TicketStatus.EXPIRED.value else "TICKET_REFUNDED"
        _log_access(db, ticket.ticket_id, body.device_id, gate_id, AccessEventType.FACE_DENIED.value, reason)
        await db.commit()
        return FaceEntryResponse(grant_access=False, reason=reason, ticket_id=ticket.ticket_id)

    # Check per-ticket-type expiry
    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, gate_id, AccessEventType.FACE_DENIED.value, "TICKET_EXPIRED")
        await db.commit()
        return FaceEntryResponse(grant_access=False, reason="TICKET_EXPIRED", ticket_id=ticket.ticket_id)

    # Check re-entry rules per ticket type
    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id, AccessEventType.FACE_DENIED.value, "TICKET_ALREADY_USED")
        await db.commit()
        return FaceEntryResponse(grant_access=False, reason="TICKET_ALREADY_USED", ticket_id=ticket.ticket_id)

    # Grant access
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    active_event.current_capacity += 1

    log = AccessLog(
        ticket_id=ticket.ticket_id,
        face_id=body.face_id,
        device_id=body.device_id,
        gate_id=gate_id,
        event_type=AccessEventType.FACE_ENTRY.value,
        confidence_score=body.confidence_score,
    )
    db.add(log)
    await db.commit()

    return FaceEntryResponse(grant_access=True, ticket_id=ticket.ticket_id)


def _log_access(db, ticket_id, device_id, gate_id, event_type, reason_code=None):
    log = AccessLog(
        ticket_id=ticket_id,
        device_id=device_id,
        gate_id=gate_id,
        event_type=event_type,
        reason_code=reason_code,
    )
    db.add(log)
