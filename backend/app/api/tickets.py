"""Ticket management endpoints."""

import uuid
from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException, Query
from sqlalchemy import select, func
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.ticket import Ticket, TicketStatus, compute_expiry
from app.models.event import Event, EventStatus
from app.models.delivery_queue import DeliveryQueue, DeliveryChannel, DeliveryStatus
from app.api.schemas import TicketResponse, TicketCreateManual, GroupOrderCreate, GroupOrderResponse
from app.utils.auth import get_current_admin
from app.utils.crypto import encrypt_pii, decrypt_pii, hash_token
from app.utils.qr import generate_qr_base64
from app.services.delivery_service import delivery_service

router = APIRouter(prefix="/api/tickets", tags=["tickets"])


@router.get("", response_model=list[TicketResponse])
async def list_tickets(
    event_id: str | None = None,
    status: str | None = None,
    admission_type: str | None = None,
    face_enrolled: bool | None = None,
    page: int = Query(1, ge=1),
    per_page: int = Query(50, ge=1, le=200),
    db: AsyncSession = Depends(get_db),
):
    query = select(Ticket).order_by(Ticket.created_at.desc())
    if event_id:
        query = query.where(Ticket.event_id == event_id)
    if status:
        query = query.where(Ticket.status == status)
    if admission_type:
        query = query.where(Ticket.admission_type == admission_type)
    if face_enrolled is not None:
        query = query.where(Ticket.face_enrolled == face_enrolled)

    query = query.offset((page - 1) * per_page).limit(per_page)
    result = await db.execute(query)
    tickets = result.scalars().all()

    # Decrypt PII for response
    responses = []
    for t in tickets:
        resp = TicketResponse.model_validate(t)
        resp.patron_phone = decrypt_pii(t.patron_phone)
        resp.patron_email = decrypt_pii(t.patron_email)
        responses.append(resp)
    return responses


@router.get("/{ticket_id}", response_model=TicketResponse)
async def get_ticket(ticket_id: str, db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Ticket).where(Ticket.ticket_id == ticket_id))
    ticket = result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")
    resp = TicketResponse.model_validate(ticket)
    resp.patron_phone = decrypt_pii(ticket.patron_phone)
    resp.patron_email = decrypt_pii(ticket.patron_email)
    return resp


@router.get("/{ticket_id}/qr")
async def get_ticket_qr(ticket_id: str, db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Ticket).where(Ticket.ticket_id == ticket_id))
    ticket = result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")
    qr_b64 = generate_qr_base64(ticket_id, 300)
    return {"ticket_id": ticket_id, "qr_base64": qr_b64}


@router.post("", response_model=TicketResponse)
async def create_ticket_manual(
    body: TicketCreateManual,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    # Verify event exists
    event_result = await db.execute(select(Event).where(Event.event_id == body.event_id))
    event = event_result.scalar_one_or_none()
    if not event:
        raise HTTPException(status_code=404, detail="Event not found")

    ticket_id = str(uuid.uuid4())
    token_hash = hash_token(ticket_id)

    # Compute expiry based on ticket type
    now = datetime.utcnow()
    expires_at = compute_expiry(
        admission_type=body.admission_type,
        created_at=now,
        event_end=event.end_time,
        event_expiry_hours=event.ticket_expiry_hours,
    )

    ticket = Ticket(
        ticket_id=ticket_id,
        event_id=body.event_id,
        patron_name=body.patron_name,
        patron_phone=encrypt_pii(body.patron_phone),
        patron_email=encrypt_pii(body.patron_email),
        admission_type=body.admission_type,
        amount_paid=body.amount_paid,
        qr_token_hash=token_hash,
        expires_at=expires_at,
    )
    db.add(ticket)

    # Update event capacity
    event.current_capacity += 1

    # Queue deliveries
    if body.patron_phone:
        db.add(DeliveryQueue(
            ticket_id=ticket_id,
            channel=DeliveryChannel.SMS.value,
            destination=body.patron_phone,
        ))
    if body.patron_email:
        db.add(DeliveryQueue(
            ticket_id=ticket_id,
            channel=DeliveryChannel.EMAIL.value,
            destination=body.patron_email,
        ))

    await db.commit()
    await db.refresh(ticket)

    # Fire deliveries immediately
    if body.patron_phone:
        await delivery_service.send_ticket_sms(
            body.patron_phone, event.name, event.event_date,
            ticket_id, body.admission_type,
        )
    if body.patron_email:
        await delivery_service.send_ticket_email(
            body.patron_email, event.name, event.event_date,
            ticket_id, body.admission_type, body.patron_name or "Guest",
        )

    resp = TicketResponse.model_validate(ticket)
    resp.patron_phone = body.patron_phone
    resp.patron_email = body.patron_email
    return resp


@router.post("/group", response_model=GroupOrderResponse)
async def create_group_order(
    body: GroupOrderCreate,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Create a group order — multiple tickets under one transaction.
    Example: Father pays for family of 5, gets 5 tickets with one group_id."""
    event_result = await db.execute(select(Event).where(Event.event_id == body.event_id))
    event = event_result.scalar_one_or_none()
    if not event:
        raise HTTPException(status_code=404, detail="Event not found")

    group_id = str(uuid.uuid4())
    now = datetime.utcnow()
    created_tickets = []
    total_count = sum(item.quantity for item in body.items)

    for item in body.items:
        for i in range(item.quantity):
            ticket_id = str(uuid.uuid4())
            token_hash = hash_token(ticket_id)
            expires_at = compute_expiry(
                admission_type=item.admission_type,
                created_at=now,
                event_end=event.end_time,
                event_expiry_hours=event.ticket_expiry_hours,
            )
            ticket = Ticket(
                ticket_id=ticket_id,
                event_id=body.event_id,
                patron_name=body.patron_name,
                patron_phone=encrypt_pii(body.patron_phone),
                patron_email=encrypt_pii(body.patron_email),
                admission_type=item.admission_type,
                amount_paid=item.unit_price,
                qr_token_hash=token_hash,
                expires_at=expires_at,
                group_id=group_id,
            )
            db.add(ticket)
            created_tickets.append(ticket)

    await db.commit()

    # Build response
    ticket_responses = []
    for t in created_tickets:
        await db.refresh(t)
        resp = TicketResponse.model_validate(t)
        resp.patron_phone = body.patron_phone
        resp.patron_email = body.patron_email
        ticket_responses.append(resp)

    # Queue deliveries once for the group (to the payer)
    if body.patron_phone:
        db.add(DeliveryQueue(
            ticket_id=created_tickets[0].ticket_id,
            channel=DeliveryChannel.SMS.value,
            destination=body.patron_phone,
        ))
        await delivery_service.send_ticket_sms(
            body.patron_phone, event.name, event.event_date,
            group_id, f"GROUP ({total_count} tickets)",
        )
    if body.patron_email:
        db.add(DeliveryQueue(
            ticket_id=created_tickets[0].ticket_id,
            channel=DeliveryChannel.EMAIL.value,
            destination=body.patron_email,
        ))
        await delivery_service.send_ticket_email(
            body.patron_email, event.name, event.event_date,
            group_id, f"GROUP ({total_count} tickets)", body.patron_name or "Guest",
        )

    await db.commit()

    return GroupOrderResponse(
        group_id=group_id,
        tickets=ticket_responses,
        total_tickets=total_count,
        total_paid=body.total_paid,
    )


@router.post("/{ticket_id}/refund")
async def refund_ticket(
    ticket_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(Ticket).where(Ticket.ticket_id == ticket_id))
    ticket = result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")

    ticket.status = TicketStatus.REFUNDED.value
    await db.commit()

    # Note: X05 devices manage faces locally. Face data cleanup happens
    # when the event ends or via the admin "Clear Faces" action.

    return {"message": "Ticket refunded", "ticket_id": ticket_id}


@router.post("/{ticket_id}/override")
async def manual_override(
    ticket_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Admin manually grants access for a ticket."""
    from app.models.access_log import AccessLog, AccessEventType
    result = await db.execute(select(Ticket).where(Ticket.ticket_id == ticket_id))
    ticket = result.scalar_one_or_none()
    if not ticket:
        raise HTTPException(status_code=404, detail="Ticket not found")

    ticket.entry_count += 1
    ticket.last_entry_at = datetime.utcnow()

    log = AccessLog(
        ticket_id=ticket_id,
        device_id="ADMIN",
        gate_id="MANUAL",
        event_type=AccessEventType.MANUAL_OVERRIDE.value,
    )
    db.add(log)
    await db.commit()

    return {"message": "Access granted via manual override", "ticket_id": ticket_id}
