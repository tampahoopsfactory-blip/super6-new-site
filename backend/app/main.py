"""
EventShield Pro — Main FastAPI Application

Smart Event Access Control System integrating Square payments,
HF Security X05 facial recognition, and turnstile hardware.
"""

import os
import logging
import logging.handlers
from contextlib import asynccontextmanager
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from fastapi.responses import FileResponse
from app.config import settings
from app.database import init_db
from app.jobs.scheduler import start_scheduler, stop_scheduler
from app.api import auth, events, tickets, access, devices, dashboard, webhooks, settings_api, health, scan_station, x05_api

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s [%(name)s] %(levelname)s: %(message)s",
    handlers=[
        logging.StreamHandler(),
        logging.handlers.RotatingFileHandler(
            "eventshield.log", maxBytes=10 * 1024 * 1024, backupCount=7
        ),
    ],
)
logger = logging.getLogger("eventshield")


@asynccontextmanager
async def lifespan(app: FastAPI):
    logger.info("EventShield Pro starting up...")
    await init_db()

    # Seed admin user if not exists
    from app.database import async_session
    from app.models.admin_user import AdminUser
    from app.utils.auth import hash_password
    from sqlalchemy import select

    async with async_session() as db:
        result = await db.execute(
            select(AdminUser).where(AdminUser.username == settings.admin_username)
        )
        if not result.scalar_one_or_none():
            db.add(AdminUser(
                username=settings.admin_username,
                password_hash=hash_password(settings.admin_password),
            ))
            await db.commit()
            logger.info(f"Admin user '{settings.admin_username}' created")

    start_scheduler()
    logger.info("EventShield Pro ready")
    yield
    stop_scheduler()
    logger.info("EventShield Pro shutting down")


app = FastAPI(
    title="EventShield Pro",
    description="Smart Event Access Control System",
    version="2.0.0",
    lifespan=lifespan,
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Register API routes
app.include_router(auth.router)
app.include_router(events.router)
app.include_router(tickets.router)
app.include_router(access.router)
app.include_router(devices.router)
app.include_router(dashboard.router)
app.include_router(webhooks.router)
app.include_router(settings_api.router)
app.include_router(health.router)
app.include_router(scan_station.router)
app.include_router(x05_api.router)


# Serve scan station PWA as standalone app at /scan/
scan_station_dir = os.path.join(os.path.dirname(os.path.dirname(__file__)), "scan_station")
if os.path.isdir(scan_station_dir):
    app.mount("/scan", StaticFiles(directory=scan_station_dir, html=True), name="scan-station")


# Serve static frontend in production (SPA-aware)
static_dir = os.path.join(os.path.dirname(os.path.dirname(__file__)), "static")
if os.path.isdir(static_dir):
    # Mount /assets for JS/CSS bundles
    assets_dir = os.path.join(static_dir, "assets")
    if os.path.isdir(assets_dir):
        app.mount("/assets", StaticFiles(directory=assets_dir), name="assets")

    # Catch-all: serve index.html for any non-API route (SPA client-side routing)
    @app.get("/{full_path:path}")
    async def serve_spa(full_path: str):
        # Try to serve a real file first (favicon, etc.)
        file_path = os.path.join(static_dir, full_path)
        if full_path and os.path.isfile(file_path):
            return FileResponse(file_path)
        # Otherwise serve index.html and let React Router handle it
        return FileResponse(os.path.join(static_dir, "index.html"))
else:
    @app.get("/")
    async def root():
        return {
            "name": "EventShield Pro",
            "version": "2.0.0",
            "status": "running",
            "device": "HF Security X05",
            "dashboard": "Run frontend dev server: cd frontend && npm run dev",
        }
