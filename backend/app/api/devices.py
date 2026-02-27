"""Device management endpoints for HF Security X05 units."""

import secrets
from datetime import datetime
from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.device import Device
from app.api.schemas import DeviceCreate, DeviceResponse
from app.utils.auth import get_current_admin

router = APIRouter(prefix="/api/devices", tags=["devices"])


@router.get("", response_model=list[DeviceResponse])
async def list_devices(db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Device).order_by(Device.created_at.desc()))
    return result.scalars().all()


@router.get("/{device_id}", response_model=DeviceResponse)
async def get_device(device_id: str, db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(Device).where(Device.device_id == device_id))
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not found")
    return device


@router.post("", response_model=DeviceResponse)
async def register_device(
    body: DeviceCreate,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    existing = await db.execute(select(Device).where(Device.device_id == body.device_id))
    if existing.scalar_one_or_none():
        raise HTTPException(status_code=409, detail="Device already registered")

    device = Device(
        device_id=body.device_id,
        device_name=body.device_name,
        ip_address=body.ip_address,
        gate_id=body.gate_id,
        gate_name=body.gate_name,
        serial_number=body.serial_number,
        model=body.model,
        api_token=secrets.token_hex(32),
    )
    db.add(device)
    await db.commit()
    await db.refresh(device)
    return device


@router.put("/{device_id}", response_model=DeviceResponse)
async def update_device(
    device_id: str,
    body: DeviceCreate,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(Device).where(Device.device_id == device_id))
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not found")

    device.device_name = body.device_name
    device.ip_address = body.ip_address
    device.gate_id = body.gate_id
    device.gate_name = body.gate_name
    device.serial_number = body.serial_number
    device.model = body.model
    await db.commit()
    await db.refresh(device)
    return device


@router.delete("/{device_id}")
async def delete_device(
    device_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(Device).where(Device.device_id == device_id))
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not found")
    await db.delete(device)
    await db.commit()
    return {"message": f"Device {device_id} deleted"}


@router.post("/{device_id}/ping")
async def ping_device(
    device_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Check if device is online by looking at last heartbeat."""
    result = await db.execute(select(Device).where(Device.device_id == device_id))
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not found")

    if not device.last_heartbeat:
        return {"status": "UNKNOWN", "message": "Device has never connected"}

    seconds_ago = (datetime.utcnow() - device.last_heartbeat).total_seconds()
    if seconds_ago < 120:
        return {
            "status": "ONLINE",
            "last_heartbeat": device.last_heartbeat.isoformat(),
            "seconds_ago": int(seconds_ago),
        }
    return {
        "status": "OFFLINE",
        "last_heartbeat": device.last_heartbeat.isoformat(),
        "seconds_ago": int(seconds_ago),
    }


@router.post("/{device_id}/regenerate-token")
async def regenerate_token(
    device_id: str,
    admin=Depends(get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    """Regenerate the API token for a device."""
    result = await db.execute(select(Device).where(Device.device_id == device_id))
    device = result.scalar_one_or_none()
    if not device:
        raise HTTPException(status_code=404, detail="Device not found")

    device.api_token = secrets.token_hex(32)
    await db.commit()
    return {"device_id": device_id, "api_token": device.api_token}
