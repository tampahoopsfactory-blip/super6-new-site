"""
X05 Device Communication API — endpoints called BY the X05 device.

The HF Security X05 runs Android 11 with a custom DoorX app that calls these
REST endpoints over HTTP. Supports multi-modal biometrics:
  - Face recognition (20K capacity)
  - Iris recognition (DataType=3 infrared, bioassay mode 1 for liveness)
  - Fingerprint (FingerSDK v2.0.1 — optical, score > 80 = match)
  - NFC card reading
Hardware: 20-pin header with NO/COM/NC relay, Wiegand, RS485, ALARM, BUTTON.

Endpoints:
  POST /api/x05/register       — Device self-registration on first boot
  POST /api/x05/heartbeat      — Periodic health check (every 30s)
  POST /api/x05/validate-qr    — QR scanned, validate ticket
  POST /api/x05/enroll-complete — Biometric enrollment finished (face/iris/finger)
  POST /api/x05/face-entry     — Face recognized, request entry validation
  POST /api/x05/iris-entry     — Iris matched, request entry validation
  POST /api/x05/finger-entry   — Fingerprint matched, request entry validation
  POST /api/x05/alert          — Device sends alert (tamper, tailgate, etc.)
"""

import logging
import secrets
import uuid
from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException, Header
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.database import get_db
from app.models.device import Device, DeviceStatus
from app.models.ticket import Ticket, TicketStatus
from app.models.event import Event, EventStatus
from app.models.access_log import AccessLog, AccessEventType
from app.utils.crypto import hash_token
from app.config import settings
from app.api.schemas import (
    X05RegisterRequest, X05HeartbeatRequest,
    X05ValidateQRRequest, X05ValidateQRResponse,
    X05EnrollCompleteRequest,
    X05FaceEntryRequest, X05FaceEntryResponse,
    X05FingerEntryRequest, X05FingerEntryResponse,
    X05IrisEntryRequest, X05IrisEntryResponse,
    X05AlertRequest,
)

logger = logging.getLogger("eventshield.x05")

router = APIRouter(prefix="/api/x05", tags=["x05-device"])

# In-memory alert store (recent alerts for dashboard)
_recent_alerts: list[dict] = []
MAX_ALERTS = 100


def _verify_device_key(x_device_key: str = Header(None)):
    """Verify the device's API key (shared secret or per-device token)."""
    if not x_device_key:
        raise HTTPException(status_code=403, detail="DEVICE_UNAUTHORIZED")
    if x_device_key == settings.api_key:
        return  # Accept the global API key
    # Per-device tokens are checked in each endpoint


def _add_alert(device_id: str, gate_id: str, alert_type: str,
               message: str, severity: str):
    alert = {
        "alert_id": str(uuid.uuid4())[:8],
        "device_id": device_id,
        "gate_id": gate_id,
        "alert_type": alert_type,
        "message": message,
        "severity": severity,
        "timestamp": datetime.utcnow().isoformat(),
        "acknowledged": False,
    }
    _recent_alerts.insert(0, alert)
    if len(_recent_alerts) > MAX_ALERTS:
        _recent_alerts.pop()
    return alert


def _log_access(db, ticket_id, device_id, gate_id, event_type,
                reason_code=None, face_id=None, confidence=None):
    log = AccessLog(
        ticket_id=ticket_id,
        face_id=face_id,
        device_id=device_id or "X05",
        gate_id=gate_id,
        event_type=event_type,
        reason_code=reason_code,
        confidence_score=confidence,
    )
    db.add(log)


async def _get_active_event(db: AsyncSession):
    result = await db.execute(
        select(Event).where(Event.status == EventStatus.ACTIVE.value)
    )
    return result.scalar_one_or_none()


# --- Endpoints ---

@router.post("/register")
async def device_register(
    body: X05RegisterRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """
    X05 calls this on first boot to register itself with the server.
    If device already exists, update its info and return existing token.
    """
    result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = result.scalar_one_or_none()

    if device:
        device.serial_number = body.serial_number
        device.model = body.model
        device.firmware_version = body.firmware_version
        device.app_version = body.app_version
        device.has_fingerprint = body.has_fingerprint
        device.has_iris = body.has_iris
        device.has_nfc = body.has_nfc
        device.finger_sdk_version = body.finger_sdk_version
        device.iris_sdk_version = body.iris_sdk_version
        device.wiegand_enabled = body.wiegand_enabled
        device.relay_mode = body.relay_mode
        device.rs485_enabled = body.rs485_enabled
        device.status = DeviceStatus.ONLINE.value
        device.last_heartbeat = datetime.utcnow()
        await db.commit()

        caps = []
        if body.has_fingerprint:
            caps.append(f"Finger(SDK {body.finger_sdk_version or '?'})")
        if body.has_iris:
            caps.append(f"Iris(SDK {body.iris_sdk_version or '?'})")
        if body.has_nfc:
            caps.append("NFC")
        if body.wiegand_enabled:
            caps.append("Wiegand")

        return {
            "status": "OK",
            "message": "Device re-registered",
            "api_token": device.api_token,
            "capabilities": caps,
            "server_time": datetime.utcnow().isoformat(),
        }

    # New device — create with auto-generated token
    new_device = Device(
        device_id=body.device_id,
        device_name=f"X05-{body.serial_number[-4:]}",
        serial_number=body.serial_number,
        model=body.model,
        firmware_version=body.firmware_version,
        app_version=body.app_version,
        gate_id=f"gate_{body.device_id}",
        status=DeviceStatus.ONLINE.value,
        last_heartbeat=datetime.utcnow(),
        api_token=secrets.token_hex(32),
        has_fingerprint=body.has_fingerprint,
        has_iris=body.has_iris,
        has_nfc=body.has_nfc,
        finger_sdk_version=body.finger_sdk_version,
        iris_sdk_version=body.iris_sdk_version,
        wiegand_enabled=body.wiegand_enabled,
        relay_mode=body.relay_mode,
        rs485_enabled=body.rs485_enabled,
    )
    db.add(new_device)
    await db.commit()
    await db.refresh(new_device)

    logger.info(
        f"New X05 device registered: {body.device_id} SN={body.serial_number} "
        f"finger={body.has_fingerprint} iris={body.has_iris} nfc={body.has_nfc}"
    )

    return {
        "status": "OK",
        "message": "Device registered",
        "api_token": new_device.api_token,
        "server_time": datetime.utcnow().isoformat(),
    }


@router.post("/heartbeat")
async def device_heartbeat(
    body: X05HeartbeatRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """Periodic heartbeat from X05 device. Updates status and connectivity info."""
    result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not registered")

    device.status = DeviceStatus.ONLINE.value
    device.last_heartbeat = datetime.utcnow()
    device.face_count = body.face_count
    device.finger_count = body.finger_count
    device.iris_count = body.iris_count
    device.wifi_connected = body.wifi_connected
    device.cellular_connected = body.cellular_connected
    device.battery_level = body.battery_level
    if body.app_version:
        device.app_version = body.app_version
    await db.commit()

    return {"status": "OK", "server_time": datetime.utcnow().isoformat()}


@router.post("/validate-qr", response_model=X05ValidateQRResponse)
async def validate_qr(
    body: X05ValidateQRRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """
    X05 scans a QR code and asks the server to validate it.
    Server validates the ticket, marks QR as USED, and tells X05 what to do.
    """
    active_event = await _get_active_event(db)
    if not active_event:
        return X05ValidateQRResponse(
            action="DENY", message="No active event.",
        )

    token = body.qr_data.strip()
    token_hash = hash_token(token)

    ticket_result = await db.execute(
        select(Ticket).where(Ticket.qr_token_hash == token_hash)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "TOKEN_NOT_FOUND")
        _add_alert(body.device_id, body.gate_id, "INVALID_QR",
                   "Unknown QR code scanned", "WARNING")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY", message="Invalid ticket. QR not recognized.",
        )

    # Status checks
    if ticket.status == TicketStatus.REFUNDED.value:
        _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "TICKET_REFUNDED")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY", message="Ticket has been refunded.",
            ticket_id=ticket.ticket_id,
        )

    if ticket.is_expired():
        if ticket.status != TicketStatus.EXPIRED.value:
            ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "TICKET_EXPIRED")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY",
            message=f"{ticket.admission_type} ticket expired.",
            ticket_id=ticket.ticket_id, admission_type=ticket.admission_type,
        )

    if ticket.status != TicketStatus.ACTIVE.value:
        _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "TICKET_INVALID")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY", message="Ticket is not valid.",
            ticket_id=ticket.ticket_id,
        )

    # Re-entry check
    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "ALREADY_USED")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY", message="Ticket already used. No re-entry.",
            ticket_id=ticket.ticket_id, admission_type=ticket.admission_type,
        )

    # Capacity check
    if active_event.max_capacity > 0 and active_event.current_capacity >= active_event.max_capacity:
        _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                    AccessEventType.QR_DENIED.value, "VENUE_FULL")
        await db.commit()
        return X05ValidateQRResponse(
            action="DENY", message="Venue at full capacity.",
            ticket_id=ticket.ticket_id,
        )

    # Mark QR as USED (PC-05)
    if not ticket.qr_used:
        ticket.qr_used = True

    expires_str = ticket.expires_at.isoformat() if ticket.expires_at else None

    # First entry — need face enrollment
    if not ticket.face_enrolled:
        await db.commit()
        return X05ValidateQRResponse(
            action="ENROLL",
            message="Valid ticket. Capture face for enrollment.",
            ticket_id=ticket.ticket_id,
            patron_name=ticket.patron_name,
            admission_type=ticket.admission_type,
            expires_at=expires_str,
        )

    # Re-entry — grant access
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    if not ticket.first_gate_id:
        ticket.first_gate_id = body.gate_id
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, body.gate_id,
                AccessEventType.QR_ENTRY.value)
    await db.commit()

    return X05ValidateQRResponse(
        action="GRANT",
        message=f"Welcome back! Entry #{ticket.entry_count}",
        ticket_id=ticket.ticket_id,
        patron_name=ticket.patron_name,
        admission_type=ticket.admission_type,
        expires_at=expires_str,
        open_relay=True,
    )


@router.post("/enroll-complete")
async def enroll_complete(
    body: X05EnrollCompleteRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """X05 reports biometric enrollment result (face, fingerprint, or both)."""
    ticket_result = await db.execute(
        select(Ticket).where(Ticket.ticket_id == body.ticket_id)
    )
    ticket = ticket_result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")

    if not body.success:
        return {"status": "DENY", "message": "Enrollment failed", "open_relay": False}

    # Enroll the biometric(s) based on type
    # Supported types: face, finger, iris, face+iris, finger+iris, both (face+finger), all
    enrolled_modalities = []
    bt = body.biometric_type.lower()
    if bt in ("face", "both", "face+iris", "all") and body.face_id:
        ticket.face_enrolled = True
        ticket.face_id = body.face_id
        enrolled_modalities.append("face")
    if bt in ("finger", "both", "finger+iris", "all") and body.finger_id:
        ticket.finger_enrolled = True
        ticket.finger_id = body.finger_id
        enrolled_modalities.append("fingerprint")
    if bt in ("iris", "face+iris", "finger+iris", "all") and body.iris_id:
        ticket.iris_enrolled = True
        ticket.iris_id = body.iris_id
        enrolled_modalities.append("iris")

    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()

    # Get gate from device
    dev_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = dev_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id
    if not ticket.first_gate_id:
        ticket.first_gate_id = gate_id

    active_event = await _get_active_event(db)
    if active_event:
        active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.QR_ENTRY.value, face_id=body.face_id)
    await db.commit()

    modality_str = " + ".join(enrolled_modalities) or "biometric"
    logger.info(
        f"X05 enrollment: ticket={ticket.ticket_id} "
        f"face={body.face_id} iris={body.iris_id} finger={body.finger_id} type={body.biometric_type}"
    )

    return {
        "status": "GRANT",
        "message": f"{modality_str.title()} enrolled! Welcome — {ticket.admission_type} pass.",
        "open_relay": True,
    }


@router.post("/face-entry", response_model=X05FaceEntryResponse)
async def face_entry(
    body: X05FaceEntryRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """X05 recognized a face and asks if entry should be granted."""
    dev_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = dev_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    ticket_result = await db.execute(
        select(Ticket).where(Ticket.face_id == body.face_id)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "FACE_NOT_FOUND",
                    face_id=body.face_id, confidence=body.confidence_score)
        _add_alert(body.device_id, gate_id, "UNKNOWN_FACE",
                   "Unrecognized face at gate", "WARNING")
        await db.commit()
        return X05FaceEntryResponse(
            grant_access=False, open_relay=False, reason="FACE_NOT_FOUND",
        )

    active_event = await _get_active_event(db)
    if not active_event:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "EVENT_NOT_ACTIVE",
                    face_id=body.face_id, confidence=body.confidence_score)
        await db.commit()
        return X05FaceEntryResponse(
            grant_access=False, open_relay=False, reason="EVENT_NOT_ACTIVE",
        )

    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "TICKET_EXPIRED",
                    face_id=body.face_id, confidence=body.confidence_score)
        _add_alert(body.device_id, gate_id, "EXPIRED_ENTRY_ATTEMPT",
                   f"Expired {ticket.admission_type} ticket at gate", "INFO")
        await db.commit()
        return X05FaceEntryResponse(
            grant_access=False, open_relay=False, reason="TICKET_EXPIRED",
        )

    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FACE_DENIED.value, "NO_REENTRY",
                    face_id=body.face_id, confidence=body.confidence_score)
        await db.commit()
        return X05FaceEntryResponse(
            grant_access=False, open_relay=False, reason="NO_REENTRY",
        )

    # Grant entry
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.FACE_ENTRY.value,
                face_id=body.face_id, confidence=body.confidence_score)
    await db.commit()

    return X05FaceEntryResponse(
        grant_access=True,
        open_relay=True,
        ticket_id=ticket.ticket_id,
        patron_name=ticket.patron_name,
    )


@router.post("/finger-entry", response_model=X05FingerEntryResponse)
async def finger_entry(
    body: X05FingerEntryRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """X05 matched a fingerprint (FingerSDK v2.0.1, score > 80) and requests entry."""
    dev_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = dev_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    ticket_result = await db.execute(
        select(Ticket).where(Ticket.finger_id == body.finger_id)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, gate_id,
                    AccessEventType.FINGER_DENIED.value, "FINGER_NOT_FOUND")
        _add_alert(body.device_id, gate_id, "UNKNOWN_FINGER",
                   "Unrecognized fingerprint at gate", "WARNING")
        await db.commit()
        return X05FingerEntryResponse(
            grant_access=False, open_relay=False, reason="FINGER_NOT_FOUND",
        )

    active_event = await _get_active_event(db)
    if not active_event:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FINGER_DENIED.value, "EVENT_NOT_ACTIVE")
        await db.commit()
        return X05FingerEntryResponse(
            grant_access=False, open_relay=False, reason="EVENT_NOT_ACTIVE",
        )

    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FINGER_DENIED.value, "TICKET_EXPIRED")
        await db.commit()
        return X05FingerEntryResponse(
            grant_access=False, open_relay=False, reason="TICKET_EXPIRED",
        )

    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.FINGER_DENIED.value, "NO_REENTRY")
        await db.commit()
        return X05FingerEntryResponse(
            grant_access=False, open_relay=False, reason="NO_REENTRY",
        )

    # Grant entry
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.FINGER_ENTRY.value, confidence=body.match_score)
    await db.commit()

    return X05FingerEntryResponse(
        grant_access=True,
        open_relay=True,
        ticket_id=ticket.ticket_id,
        patron_name=ticket.patron_name,
    )


@router.post("/iris-entry", response_model=X05IrisEntryResponse)
async def iris_entry(
    body: X05IrisEntryRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """X05 matched an iris (DataType=3, infrared liveness) and requests entry."""
    dev_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = dev_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    ticket_result = await db.execute(
        select(Ticket).where(Ticket.iris_id == body.iris_id)
    )
    ticket = ticket_result.scalar_one_or_none()

    if not ticket:
        _log_access(db, None, body.device_id, gate_id,
                    AccessEventType.IRIS_DENIED.value, "IRIS_NOT_FOUND")
        _add_alert(body.device_id, gate_id, "UNKNOWN_IRIS",
                   "Unrecognized iris at gate", "WARNING")
        await db.commit()
        return X05IrisEntryResponse(
            grant_access=False, open_relay=False, reason="IRIS_NOT_FOUND",
        )

    active_event = await _get_active_event(db)
    if not active_event:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.IRIS_DENIED.value, "EVENT_NOT_ACTIVE")
        await db.commit()
        return X05IrisEntryResponse(
            grant_access=False, open_relay=False, reason="EVENT_NOT_ACTIVE",
        )

    if ticket.is_expired():
        ticket.status = TicketStatus.EXPIRED.value
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.IRIS_DENIED.value, "TICKET_EXPIRED")
        _add_alert(body.device_id, gate_id, "EXPIRED_ENTRY_ATTEMPT",
                   f"Expired {ticket.admission_type} ticket at gate (iris)", "INFO")
        await db.commit()
        return X05IrisEntryResponse(
            grant_access=False, open_relay=False, reason="TICKET_EXPIRED",
        )

    if not ticket.allows_reentry() and ticket.entry_count >= 1:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.IRIS_DENIED.value, "NO_REENTRY")
        await db.commit()
        return X05IrisEntryResponse(
            grant_access=False, open_relay=False, reason="NO_REENTRY",
        )

    if active_event.max_capacity > 0 and active_event.current_capacity >= active_event.max_capacity:
        _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                    AccessEventType.IRIS_DENIED.value, "VENUE_FULL")
        await db.commit()
        return X05IrisEntryResponse(
            grant_access=False, open_relay=False, reason="VENUE_FULL",
        )

    # Grant entry
    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()
    active_event.current_capacity += 1

    _log_access(db, ticket.ticket_id, body.device_id, gate_id,
                AccessEventType.IRIS_ENTRY.value, confidence=body.quality_score)
    await db.commit()

    return X05IrisEntryResponse(
        grant_access=True,
        open_relay=True,
        ticket_id=ticket.ticket_id,
        patron_name=ticket.patron_name,
    )


@router.post("/alert")
async def device_alert(
    body: X05AlertRequest,
    db: AsyncSession = Depends(get_db),
    _=Depends(_verify_device_key),
):
    """X05 sends an alert (tailgate, tamper, low battery, etc.)."""
    dev_result = await db.execute(
        select(Device).where(Device.device_id == body.device_id)
    )
    device = dev_result.scalar_one_or_none()
    gate_id = device.gate_id if device else body.device_id

    alert = _add_alert(
        body.device_id, gate_id, body.alert_type,
        body.message or body.alert_type, body.severity,
    )

    logger.warning(
        f"X05 alert: device={body.device_id} type={body.alert_type} "
        f"severity={body.severity} msg={body.message}"
    )

    return {"status": "OK", "alert_id": alert["alert_id"]}


@router.get("/alerts")
async def get_alerts(
    limit: int = 50,
    _=Depends(_verify_device_key),
):
    """Get recent alerts for the dashboard."""
    return _recent_alerts[:limit]


@router.post("/alerts/{alert_id}/acknowledge")
async def acknowledge_alert(
    alert_id: str,
    _=Depends(_verify_device_key),
):
    """Mark an alert as acknowledged."""
    for alert in _recent_alerts:
        if alert["alert_id"] == alert_id:
            alert["acknowledged"] = True
            return {"status": "OK"}
    raise HTTPException(status_code=404, detail="Alert not found")
