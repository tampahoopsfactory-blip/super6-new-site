"""Settings management endpoints."""

from datetime import datetime
from fastapi import APIRouter, Depends
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.app_settings import AppSettings
from app.api.schemas import SettingsUpdate
from app.utils.auth import get_current_admin

router = APIRouter(prefix="/api/settings", tags=["settings"])

DEFAULT_SETTINGS = {
    "sms_enabled": "true",
    "email_enabled": "true",
    "twilio_account_sid": "",
    "twilio_auth_token": "",
    "twilio_from_number": "",
    "sendgrid_api_key": "",
    "sendgrid_from_email": "",
    "square_application_id": "",
    "square_access_token": "",
    "square_webhook_key": "",
    "multi_entry": "true",
    "ticket_expiry_hours": "24",
    "admin_phone": "",
    "admin_email": "",
    # Default ticket pricing — 5 types
    "price_DAILY": "15.00",
    "price_WEEKEND": "25.00",
    "price_KIDS": "10.00",
    "price_KIDS_WEEKEND": "18.00",
    "price_STAFF": "0.00",
}


@router.get("")
async def get_all_settings(
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(AppSettings))
    existing = {s.key: s.value for s in result.scalars().all()}
    merged = {**DEFAULT_SETTINGS, **existing}
    return merged


@router.put("")
async def update_setting(
    body: SettingsUpdate,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(AppSettings).where(AppSettings.key == body.key))
    setting = result.scalar_one_or_none()
    if setting:
        setting.value = body.value
        setting.updated_at = datetime.utcnow()
    else:
        db.add(AppSettings(key=body.key, value=body.value))
    await db.commit()
    return {"key": body.key, "value": body.value}


@router.get("/pricing")
async def get_pricing(
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Return ticket pricing dict for all ticket types."""
    result = await db.execute(select(AppSettings))
    existing = {s.key: s.value for s in result.scalars().all()}
    pricing = {}
    for key in ["DAILY", "WEEKEND", "KIDS", "KIDS_WEEKEND", "STAFF"]:
        setting_key = f"price_{key}"
        pricing[key] = float(existing.get(setting_key, DEFAULT_SETTINGS.get(setting_key, "0")))
    return pricing


@router.put("/pricing")
async def update_pricing(
    body: dict,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Update all ticket prices at once. Admin only."""
    for ticket_type, price in body.items():
        setting_key = f"price_{ticket_type}"
        result = await db.execute(select(AppSettings).where(AppSettings.key == setting_key))
        setting = result.scalar_one_or_none()
        if setting:
            setting.value = str(price)
            setting.updated_at = datetime.utcnow()
        else:
            db.add(AppSettings(key=setting_key, value=str(price)))
    await db.commit()
    return {"status": "ok", "updated": list(body.keys())}


@router.post("/test-sms")
async def test_sms(
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    from app.services.delivery_service import delivery_service
    result = await db.execute(select(AppSettings).where(AppSettings.key == "admin_phone"))
    setting = result.scalar_one_or_none()
    phone = setting.value if setting else ""
    if not phone:
        return {"success": False, "error": "No admin phone configured"}
    success = await delivery_service.send_sms(phone, "EventShield Pro test message")
    return {"success": success}


@router.post("/test-email")
async def test_email(
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    from app.services.delivery_service import delivery_service
    result = await db.execute(select(AppSettings).where(AppSettings.key == "admin_email"))
    setting = result.scalar_one_or_none()
    email = setting.value if setting else ""
    if not email:
        return {"success": False, "error": "No admin email configured"}
    success = await delivery_service.send_email(
        email, "EventShield Pro Test", "<h1>Test email from EventShield Pro</h1>"
    )
    return {"success": success}
