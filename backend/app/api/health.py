"""Health check endpoint."""

import socket
from fastapi import APIRouter
from app.database import async_session
from app.models.device import Device
from app.api.schemas import HealthResponse
from sqlalchemy import select, text

router = APIRouter(tags=["health"])


def _check_internet() -> bool:
    try:
        socket.create_connection(("8.8.8.8", 53), timeout=3)
        return True
    except OSError:
        return False


@router.get("/health", response_model=HealthResponse)
async def health_check():
    db_status = "ok"
    devices = {}

    try:
        async with async_session() as db:
            await db.execute(text("SELECT 1"))
            result = await db.execute(select(Device))
            for d in result.scalars().all():
                devices[d.device_id] = {
                    "name": d.device_name,
                    "status": d.status,
                    "ip": d.ip_address,
                }
    except Exception as e:
        db_status = f"error: {e}"

    return HealthResponse(
        status="ok",
        database=db_status,
        devices=devices,
        internet=_check_internet(),
    )
