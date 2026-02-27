"""
APScheduler-based background job runner.

Runs four periodic jobs:
  1. Device health monitor — checks X05 heartbeat freshness every 30s
  2. Cloud sync — pushes unsynced records every 60s
  3. Event expiry check — expires ended events every 5min
  4. Delivery retry — retries failed SMS/email every 2min
"""

import asyncio
import logging
from datetime import datetime
from apscheduler.schedulers.asyncio import AsyncIOScheduler
from apscheduler.triggers.interval import IntervalTrigger
from sqlalchemy import select, update
from app.database import async_session
from app.config import settings

logger = logging.getLogger("eventshield.jobs")

scheduler = AsyncIOScheduler()


async def device_health_check():
    """Check X05 device heartbeats and mark stale devices as offline."""
    from app.models.device import Device, DeviceStatus

    async with async_session() as db:
        result = await db.execute(select(Device))
        devices = result.scalars().all()

        for device in devices:
            if device.status == DeviceStatus.MAINTENANCE.value:
                continue

            # X05 devices send heartbeats themselves — we just check freshness
            if device.last_heartbeat:
                seconds_since = (datetime.utcnow() - device.last_heartbeat).total_seconds()
                is_online = seconds_since < settings.device_heartbeat_timeout
            else:
                is_online = False

            new_status = DeviceStatus.ONLINE.value if is_online else DeviceStatus.OFFLINE.value

            if device.status != new_status:
                await db.execute(
                    update(Device)
                    .where(Device.device_id == device.device_id)
                    .values(status=new_status)
                )
                if not is_online:
                    logger.warning(f"Device {device.device_id} ({device.device_name}) went OFFLINE")

        await db.commit()


async def cloud_sync():
    """Push unsynced tickets and access logs to the cloud endpoint."""
    if not settings.cloud_sync_enabled or not settings.cloud_sync_url:
        return

    import httpx
    from app.models.ticket import Ticket
    from app.models.access_log import AccessLog

    async with async_session() as db:
        # Sync tickets
        result = await db.execute(
            select(Ticket).where(Ticket.synced_to_cloud == False).limit(100)
        )
        tickets = result.scalars().all()

        result = await db.execute(
            select(AccessLog).where(AccessLog.synced_to_cloud == False).limit(100)
        )
        logs = result.scalars().all()

        if not tickets and not logs:
            return

        payload = {
            "tickets": [
                {
                    "ticket_id": t.ticket_id,
                    "event_id": t.event_id,
                    "admission_type": t.admission_type,
                    "status": t.status,
                    "entry_count": t.entry_count,
                    "created_at": t.created_at.isoformat() if t.created_at else None,
                }
                for t in tickets
            ],
            "access_logs": [
                {
                    "log_id": l.log_id,
                    "ticket_id": l.ticket_id,
                    "device_id": l.device_id,
                    "event_type": l.event_type,
                    "timestamp": l.timestamp.isoformat() if l.timestamp else None,
                }
                for l in logs
            ],
        }

        try:
            async with httpx.AsyncClient() as client:
                resp = await client.post(
                    settings.cloud_sync_url,
                    json=payload,
                    headers={"X-API-Key": settings.cloud_sync_api_key},
                    timeout=15.0,
                )
                if resp.status_code in (200, 201):
                    for t in tickets:
                        await db.execute(
                            update(Ticket).where(Ticket.ticket_id == t.ticket_id)
                            .values(synced_to_cloud=True)
                        )
                    for l in logs:
                        await db.execute(
                            update(AccessLog).where(AccessLog.log_id == l.log_id)
                            .values(synced_to_cloud=True)
                        )
                    await db.commit()
                    logger.info(f"Cloud sync: {len(tickets)} tickets, {len(logs)} logs")
                else:
                    logger.error(f"Cloud sync failed: {resp.status_code}")
        except Exception as e:
            logger.error(f"Cloud sync error: {e}")


async def event_expiry_check():
    """Mark events past their end_time as ENDED and expire their tickets."""
    from app.models.event import Event, EventStatus
    from app.models.ticket import Ticket, TicketStatus

    async with async_session() as db:
        now = datetime.utcnow()
        result = await db.execute(
            select(Event).where(
                Event.status == EventStatus.ACTIVE.value,
                Event.end_time < now,
            )
        )
        expired_events = result.scalars().all()

        for event in expired_events:
            await db.execute(
                update(Event)
                .where(Event.event_id == event.event_id)
                .values(status=EventStatus.ENDED.value)
            )
            await db.execute(
                update(Ticket)
                .where(
                    Ticket.event_id == event.event_id,
                    Ticket.status == TicketStatus.ACTIVE.value,
                )
                .values(status=TicketStatus.EXPIRED.value)
            )
            logger.info(f"Event '{event.name}' ended and tickets expired")

        if expired_events:
            await db.commit()


async def delivery_retry():
    """Retry failed SMS and email deliveries (up to 3 attempts)."""
    from app.models.delivery_queue import DeliveryQueue, DeliveryStatus, DeliveryChannel
    from app.services.delivery_service import delivery_service

    async with async_session() as db:
        result = await db.execute(
            select(DeliveryQueue).where(
                DeliveryQueue.status.in_([
                    DeliveryStatus.PENDING.value,
                    DeliveryStatus.FAILED.value,
                ]),
                DeliveryQueue.attempts < 3,
            ).limit(10)
        )
        items = result.scalars().all()

        for item in items:
            success = False
            if item.channel == DeliveryChannel.SMS.value:
                success = await delivery_service.send_sms(item.destination, "Ticket QR delivery retry")
            elif item.channel == DeliveryChannel.EMAIL.value:
                success = await delivery_service.send_email(
                    item.destination, "Your EventShield Ticket", "Ticket delivery retry"
                )

            item.attempts += 1
            item.last_attempt_at = datetime.utcnow()
            if success:
                item.status = DeliveryStatus.SENT.value
            elif item.attempts >= 3:
                item.status = DeliveryStatus.FAILED.value
                item.error_message = "Max retry attempts reached"

        await db.commit()


def start_scheduler():
    scheduler.add_job(
        device_health_check,
        IntervalTrigger(seconds=settings.device_health_interval),
        id="device_health",
        replace_existing=True,
    )
    scheduler.add_job(
        cloud_sync,
        IntervalTrigger(seconds=settings.cloud_sync_interval),
        id="cloud_sync",
        replace_existing=True,
    )
    scheduler.add_job(
        event_expiry_check,
        IntervalTrigger(minutes=5),
        id="event_expiry",
        replace_existing=True,
    )
    scheduler.add_job(
        delivery_retry,
        IntervalTrigger(minutes=2),
        id="delivery_retry",
        replace_existing=True,
    )
    scheduler.start()
    logger.info("Background scheduler started with 4 jobs")


def stop_scheduler():
    scheduler.shutdown(wait=False)
    logger.info("Background scheduler stopped")
