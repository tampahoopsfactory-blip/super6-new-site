"""Square payment webhook handler."""

import uuid
import logging
from datetime import datetime
from fastapi import APIRouter, Request, HTTPException
from sqlalchemy import select
from app.database import async_session
from app.models.event import Event, EventStatus
from app.models.ticket import Ticket, compute_expiry
from app.models.delivery_queue import DeliveryQueue, DeliveryChannel
from app.utils.crypto import encrypt_pii, hash_token
from app.services.square_service import square_service
from app.services.delivery_service import delivery_service

logger = logging.getLogger("eventshield.webhooks")

router = APIRouter(prefix="/api/webhooks", tags=["webhooks"])


@router.post("/square")
async def square_webhook(request: Request):
    body = await request.body()
    signature = request.headers.get("x-square-hmacsha256-signature", "")
    webhook_url = str(request.url)

    # Verify signature (if key configured)
    if not square_service.verify_webhook_signature(body, signature, webhook_url):
        logger.warning("Invalid Square webhook signature")
        raise HTTPException(status_code=401, detail="Invalid signature")

    data = await request.json()
    event_type = data.get("type", "")

    if event_type == "payment.completed" or event_type == "payment.updated":
        payment_data = data.get("data", {}).get("object", {}).get("payment", {})
        payment_id = payment_data.get("id")
        amount = payment_data.get("amount_money", {}).get("amount", 0)

        if not payment_id:
            return {"received": True}

        # Extract patron info from note or custom fields
        note = payment_data.get("note", "")
        patron_phone = None
        patron_email = None
        patron_name = None

        # Parse collection data from note field (format: name|phone|email)
        if "|" in note:
            parts = note.split("|")
            if len(parts) >= 1:
                patron_name = parts[0].strip() or None
            if len(parts) >= 2:
                patron_phone = parts[1].strip() or None
            if len(parts) >= 3:
                patron_email = parts[2].strip() or None

        async with async_session() as db:
            # Check for duplicate
            existing = await db.execute(
                select(Ticket).where(Ticket.square_payment_id == payment_id)
            )
            if existing.scalar_one_or_none():
                return {"received": True, "duplicate": True}

            # Get active event
            event_result = await db.execute(
                select(Event).where(Event.status == EventStatus.ACTIVE.value)
            )
            active_event = event_result.scalar_one_or_none()
            if not active_event:
                logger.error("Payment received but no active event")
                return {"received": True, "error": "No active event"}

            # Determine admission type based on price
            admission_type = "DAILY"
            if active_event.vip_price and amount >= int(active_event.vip_price * 100):
                admission_type = "VIP"

            ticket_id = str(uuid.uuid4())
            token_hash = hash_token(ticket_id)
            now = datetime.utcnow()
            expires_at = compute_expiry(
                admission_type=admission_type,
                created_at=now,
                event_end=active_event.end_time,
                event_expiry_hours=active_event.ticket_expiry_hours,
            )

            ticket = Ticket(
                ticket_id=ticket_id,
                event_id=active_event.event_id,
                square_payment_id=payment_id,
                patron_name=patron_name,
                patron_phone=encrypt_pii(patron_phone),
                patron_email=encrypt_pii(patron_email),
                admission_type=admission_type,
                amount_paid=amount / 100.0,
                qr_token_hash=token_hash,
                expires_at=expires_at,
            )
            db.add(ticket)
            active_event.current_capacity += 1

            # Queue deliveries
            if patron_phone:
                db.add(DeliveryQueue(
                    ticket_id=ticket_id,
                    channel=DeliveryChannel.SMS.value,
                    destination=patron_phone,
                ))
            if patron_email:
                db.add(DeliveryQueue(
                    ticket_id=ticket_id,
                    channel=DeliveryChannel.EMAIL.value,
                    destination=patron_email,
                ))

            await db.commit()
            logger.info(f"Ticket {ticket_id} created from Square payment {payment_id}")

            # Send deliveries immediately
            if patron_phone:
                await delivery_service.send_ticket_sms(
                    patron_phone, active_event.name, active_event.event_date,
                    ticket_id, admission_type,
                )
            if patron_email:
                await delivery_service.send_ticket_email(
                    patron_email, active_event.name, active_event.event_date,
                    ticket_id, admission_type, patron_name or "Guest",
                )

    return {"received": True}
