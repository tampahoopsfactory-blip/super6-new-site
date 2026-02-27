"""
Scan Station API — endpoints for QR scanning at gates.

This module orchestrates the full entry flow with the HF Security X05:
  1. X05 scans QR code → POST /api/scan/validate
  2. Server validates ticket, marks QR as USED (PC-05)
  3. Response tells X05 to enroll face (first entry) or grant access (re-entry)
  4. X05 captures face locally → POST /api/scan/enrollment-complete
  5. Server records enrollment, tells X05 to open relay
  6. X05 opens turnstile relay directly (Wiegand/RS485)

For re-entry (face only, no QR):
  - X05 recognizes face locally
  - X05 sends POST /api/scan/face-entry
  - Server verifies ticket still valid, responds with grant/deny
  - X05 opens relay if granted
"""

import logging
from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException, Header
from sqlalchemy import select, func
from sqlalchemy.ext.asyncio import AsyncSession
from pydantic import BaseModel
from typing import Optional

from app.database import get_db
from app.models.ticket import Ticket, TicketStatus, compute_expiry
from app.models.event import Event, EventStatus
from app.models.access_log import AccessLog, AccessEventType
from app.models.device import Device, DeviceStatus
from app.utils.crypto import hash_token
from app.config import settings

logger = logging.getLogger("eventshield.scan_station")

router = APIRouter(prefix="/api/scan", tags=["scan-station"])


# --- Schemas ---

class ScanRequest(BaseModel):
    qr_data: str
    gate_id: str


class ScanResponse(BaseModel):
    status: str  # ENROLL, GRANT, DENY
    message: str
    ticket_id: Optional[str] = None
    patron_name: Optional[str] = None
    admission_type: Optional[str] = None
    expires_at: Optional[str] = None
    device_id: Optional[str] = None
    enrollment_session: Optional[str] = None


class EnrollmentCompleteRequest(BaseModel):
    enrollment_session: str
    ticket_id: str
    device_id: str
    face_id: str
    success: bool


class EnrollmentCompleteResponse(BaseModel):
    status: str  # GRANT, DENY
    message: str
    open_relay: bool


class FaceEntryRequest(BaseModel):
    device_id: str
    face_id: str
    confidence_score: float = 0.0
    timestamp: Optional[str] = None


class FaceEntryResponse(BaseModel):
    grant_access: bool
    open_relay: bool
    reason: Optional[str] = None


class GateStatusResponse(BaseModel):
    gate_id: str
    device_id: Optional[str] = None
    device_online: bool = False
    active_event: Optional[str] = None
    entries_today: int = 0
    last_entry: Optional[str] = None


# --- Helpers ---

def _verify_station_key(x_api_key: str = Header(None)):
    if x_api_key != settings.api_key:
        raise HTTPException(status_code=403, detail="STATION_UNAUTHORIZED")


async def _get_active_event(db: AsyncSession) -> Optional[Event]:
    result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    return result.scalar_one_or_none()


async def _get_gate_device(db: AsyncSession, gate_id: str) -> Optional[Device]:
    result = await db.execute(
        select(Device).where(Device.gate_id == gate_id)
    )
    return result.scalar_one_or_none()


def _log_access(db, ticket_id, device_id, gate_id, event_type, reason_code=None, face_id=None, confidence=None):
    log = AccessLog(
        ticket_id=ticket_id,
        face_id=face_id,
        device_id=device_id or "SCAN_STATION",
        gate_id=gate_id,
        event_type=event_type,
        reason_code=reason_code,
        confidence_score=confidence,
    )
    db.add(log)


# --- Endpoints ---

@router.post("/validate", response_model=ScanResponse)
async def scan_validate(
    body: ScanRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_station_key),
):
    """
    Step 1: X05 scans a QR code. Server validates the ticket.
    Marks QR as USED (PC-05 requirement) on first scan.
    """
    active_event = await _get_active_event(db)
    if not active_event:
        return ScanResponse(
            status="DENY", message="No active event. Contact event staff.",
        )

    device = await _get_gate_device(db, body.gate_id)

    token = body.qr_data.strip()
    token_hash = hash_token(token)

    ticket_result = await db.execute(
        select(Ticket).where(Ticket.qr_token_hash == token_hash)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "TOKEN_NOT_FOUND")
        await db.commit()
        return ScanResponse(status="DENY", message="Invalid ticket. QR code not recognized.")

    # Check ticket status
    if ticket.status == TicketStatus.REFUNDED.value:
        _log_access(db, ticket.ticket_id, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "TICKET_REFUNDED")
        await db.commit()
        return ScanResponse(
            status="DENY", message="This ticket has been refunded.",
            ticket_id=ticket.ticket_id,
        )

    if ticket.status == TicketStatus.EXPIRED.value or ticket.is_expired():
        if ticket.status != TicketStatus.EXPIRED.value:
            ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "TICKET_EXPIRED")
        await db.commit()
        return ScanResponse(
            status="DENY", message=f"This {ticket.admission_type} ticket has expired.",
            ticket_id=ticket.ticket_id, admission_type=ticket.admission_type,
        )

    if ticket.status != TicketStatus.ACTIVE.value:
        _log_access(db, ticket.ticket_id, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "TICKET_INVALID")
        await db.commit()
        return ScanResponse(
            status="DENY", message="Ticket is not valid.",
            ticket_id=ticket.ticket_id,
        )

    # Check re-entry rules
    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "TICKET_ALREADY_USED")
        await db.commit()
        return ScanResponse(
            status="DENY",
            message="This ticket has already been used. No re-entry allowed.",
            ticket_id=ticket.ticket_id, admission_type=ticket.admission_type,
        )

    # Check capacity
    if active_event.max_capacity > 0 and active_event.current_capacity >= active_event.max_capacity:
        _log_access(db, ticket.ticket_id, device.device_id if device else None,
                    body.gate_id, AccessEventType.QR_DENIED.value, "VENUE_FULL")
        await db.commit()
        return ScanResponse(
            status="DENY", message="Venue is at full capacity. Please wait.",
            ticket_id=ticket.ticket_id,
        )

    # --- PC-05: Mark QR as USED on first scan ---
    if not ticket.qr_used:
        ticket.qr_used = True

    expires_str = ticket.expires_at.isoformat() if ticket.expires_at else None

    # --- FIRST ENTRY: Need face enrollment ---
    if not ticket.face_enrolled:
        import uuid
        enrollment_session = str(uuid.uuid4())[:8]

        device_id = None
        if device:
            device_id = device.device_id
            logger.info(
                f"Enrollment started: ticket={ticket.ticket_id} "
                f"type={ticket.admission_type} gate={body.gate_id} "
                f"session={enrollment_session}"
            )

        await db.commit()
        return ScanResponse(
            status="ENROLL",
            message="Valid ticket. Please look at the camera for face registration.",
            ticket_id=ticket.ticket_id,
            patron_name=ticket.patron_name,
            admission_type=ticket.admission_type,
            expires_at=expires_str,
            device_id=device_id,
            enrollment_session=enrollment_session,
        )

    # --- RE-ENTRY with face already enrolled ---
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    if not ticket.first_gate_id:
        ticket.first_gate_id = body.gate_id
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, device.device_id if device else None,
                body.gate_id, AccessEventType.QR_ENTRY.value)
    await db.commit()

    return ScanResponse(
        status="GRANT",
        message=f"Welcome back! {ticket.admission_type} pass — entry #{ticket.entry_count}",
        ticket_id=ticket.ticket_id,
        patron_name=ticket.patron_name,
        admission_type=ticket.admission_type,
        expires_at=expires_str,
    )


@router.post("/enrollment-complete", response_model=EnrollmentCompleteResponse)
async def enrollment_complete(
    body: EnrollmentCompleteRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_station_key),
):
    """
    Step 2: X05 captured the face and reports enrollment result.
    Server records it and tells X05 to open the relay.
    """
    ticket_result = await db.execute(
        select(Ticket).where(Ticket.ticket_id == body.ticket_id)
    )
    ticket = ticket_result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")

    if not body.success:
        logger.warning(f"Enrollment failed for ticket {body.ticket_id}")
        return EnrollmentCompleteResponse(
            status="DENY",
            message="Face enrollment failed. Please try again.",
            open_relay=False,
        )

    # Record enrollment
    ticket.face_enrolled = True
    ticket.face_id = body.face_id
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()

    # Find gate from device
    device_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = device_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    if not ticket.first_gate_id:
        ticket.first_gate_id = gate_id

    # Update event capacity
    active_event = await _get_active_event(db)
    if active_event:
        active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.QR_ENTRY.value, face_id=body.face_id)
    await db.commit()

    logger.info(
        f"Enrollment complete: ticket={ticket.ticket_id} "
        f"face={body.face_id} type={ticket.admission_type}"
    )

    return EnrollmentCompleteResponse(
        status="GRANT",
        message=f"Face registered! Welcome — {ticket.admission_type} pass.",
        open_relay=True,
    )


@router.post("/face-entry", response_model=FaceEntryResponse)
async def face_entry(
    body: FaceEntryRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_station_key),
):
    """
    X05 recognized a face locally and asks the server to validate the ticket.
    Server checks if ticket is still valid, then tells X05 to open/deny.
    """
    device_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = device_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    face_id = body.face_id
    ticket_result = await db.execute(
        select(Ticket).where(Ticket.face_id == face_id)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "FACE_NOT_FOUND",
                    face_id=face_id, confidence=body.confidence_score)
        await db.commit()
        return FaceEntryResponse(
            grant_access=False, open_relay=False, reason="FACE_NOT_FOUND"
        )

    active_event = await _get_active_event(db)
    if not active_event:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "EVENT_NOT_ACTIVE",
                    face_id=face_id, confidence=body.confidence_score)
        await db.commit()
        return FaceEntryResponse(
            grant_access=False, open_relay=False, reason="EVENT_NOT_ACTIVE"
        )

    # Server-side expiry check
    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "TICKET_EXPIRED",
                    face_id=face_id, confidence=body.confidence_score)
        await db.commit()
        return FaceEntryResponse(
            grant_access=False, open_relay=False, reason="TICKET_EXPIRED"
        )

    # Server-side re-entry check
    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "NO_REENTRY",
                    face_id=face_id, confidence=body.confidence_score)
        await db.commit()
        return FaceEntryResponse(
            grant_access=False, open_relay=False, reason="NO_REENTRY"
        )

    # All good — record entry
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.FACE_ENTRY.value,
                face_id=face_id, confidence=body.confidence_score)
    await db.commit()

    logger.info(
        f"Face re-entry: ticket={ticket.ticket_id} "
        f"type={ticket.admission_type} entry#{ticket.entry_count} "
        f"gate={gate_id}"
    )

    return FaceEntryResponse(
        grant_access=True, open_relay=True, reason=None
    )


@router.get("/gate-status/{gate_id}", response_model=GateStatusResponse)
async def gate_status(
    gate_id: str,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_station_key),
):
    """Get the current status of a gate."""
    device = await _get_gate_device(db, gate_id)
    active_event = await _get_active_event(db)

    device_online = False
    device_id = None
    if device:
        device_id = device.device_id
        device_online = device.status == DeviceStatus.ONLINE.value

    today_start = datetime.utcnow().replace(hour=0, minute=0, second=0, microsecond=0)
    count_result = await db.execute(
        select(func.count(AccessLog.log_id)).where(
            AccessLog.gate_id == gate_id,
            AccessLog.event_type.in_([
                AccessEventType.QR_ENTRY.value,
                AccessEventType.FACE_ENTRY.value,
            ]),
            AccessLog.timestamp >= today_start,
        )
    )
    entries_today = count_result.scalar() or 0

    last_result = await db.execute(
        select(AccessLog.timestamp).where(
            AccessLog.gate_id == gate_id,
            AccessLog.event_type.in_([
                AccessEventType.QR_ENTRY.value,
                AccessEventType.FACE_ENTRY.value,
            ]),
        ).order_by(AccessLog.timestamp.desc()).limit(1)
    )
    last_entry_row = last_result.scalar_one_or_none()
    last_entry = last_entry_row.isoformat() if last_entry_row else None

    return GateStatusResponse(
        gate_id=gate_id,
        device_id=device_id,
        device_online=device_online,
        active_event=active_event.name if active_event else None,
        entries_today=entries_today,
        last_entry=last_entry,
    )
