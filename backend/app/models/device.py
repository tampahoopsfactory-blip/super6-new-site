"""Device model — registered HF Security X05 multi-modal biometric devices.

The X05 supports face recognition, iris recognition, fingerprint (FingerSDK v2.0.1),
NFC card, and has Wiegand/RS485/RS232 output for turnstile integration.
Iris uses DataType=3 (infrared), bioassay mode 1 for liveness detection.
Hardware: 20-pin header with NO/COM/NC relay, ALARM+/-, Wiegand, RS485, BUTTON.
"""

from datetime import datetime
from sqlalchemy import String, Integer, DateTime, Boolean
from sqlalchemy.orm import Mapped, mapped_column
from app.database import Base
import enum


class DeviceStatus(str, enum.Enum):
    ONLINE = "ONLINE"
    OFFLINE = "OFFLINE"
    MAINTENANCE = "MAINTENANCE"


class Device(Base):
    __tablename__ = "devices"

    device_id: Mapped[str] = mapped_column(String(50), primary_key=True)
    device_name: Mapped[str] = mapped_column(String(100), nullable=False)
    ip_address: Mapped[str] = mapped_column(String(45), nullable=True)
    gate_id: Mapped[str] = mapped_column(String(50), nullable=False)
    gate_name: Mapped[str] = mapped_column(String(100), nullable=True)
    status: Mapped[str] = mapped_column(
        String(20), default=DeviceStatus.OFFLINE.value
    )
    last_heartbeat: Mapped[datetime] = mapped_column(DateTime, nullable=True)
    firmware_version: Mapped[str] = mapped_column(String(50), nullable=True)
    app_version: Mapped[str] = mapped_column(String(50), nullable=True)
    face_count: Mapped[int] = mapped_column(Integer, default=0)
    finger_count: Mapped[int] = mapped_column(Integer, default=0)
    serial_number: Mapped[str] = mapped_column(String(64), nullable=True)
    model: Mapped[str] = mapped_column(String(50), default="X05")
    api_token: Mapped[str] = mapped_column(String(64), nullable=True)
    wifi_connected: Mapped[bool] = mapped_column(Boolean, default=False)
    cellular_connected: Mapped[bool] = mapped_column(Boolean, default=False)
    battery_level: Mapped[int] = mapped_column(Integer, nullable=True)

    # Multi-modal biometric capabilities (reported by device)
    has_fingerprint: Mapped[bool] = mapped_column(Boolean, default=False)
    has_iris: Mapped[bool] = mapped_column(Boolean, default=False)
    has_nfc: Mapped[bool] = mapped_column(Boolean, default=False)
    finger_sdk_version: Mapped[str] = mapped_column(String(30), nullable=True)
    iris_sdk_version: Mapped[str] = mapped_column(String(30), nullable=True)
    iris_count: Mapped[int] = mapped_column(Integer, default=0)

    # Hardware output configuration
    wiegand_enabled: Mapped[bool] = mapped_column(Boolean, default=False)
    relay_mode: Mapped[str] = mapped_column(String(10), default="NO")  # NO or NC
    rs485_enabled: Mapped[bool] = mapped_column(Boolean, default=False)

    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
