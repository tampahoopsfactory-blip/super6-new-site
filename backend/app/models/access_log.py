"""Access log model — records every entry attempt at every gate."""

import uuid
from datetime import datetime
from sqlalchemy import String, Float, DateTime, Boolean, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column
from app.database import Base
import enum


class AccessEventType(str, enum.Enum):
    QR_ENTRY = "QR_ENTRY"
    FACE_ENTRY = "FACE_ENTRY"
    FINGER_ENTRY = "FINGER_ENTRY"
    NFC_ENTRY = "NFC_ENTRY"
    QR_DENIED = "QR_DENIED"
    FACE_DENIED = "FACE_DENIED"
    FINGER_DENIED = "FINGER_DENIED"
    NFC_DENIED = "NFC_DENIED"
    IRIS_ENTRY = "IRIS_ENTRY"
    IRIS_DENIED = "IRIS_DENIED"
    MANUAL_OVERRIDE = "MANUAL_OVERRIDE"


class AccessLog(Base):
    __tablename__ = "access_log"

    log_id: Mapped[str] = mapped_column(
        String(36), primary_key=True, default=lambda: str(uuid.uuid4())
    )
    ticket_id: Mapped[str] = mapped_column(
        String(36), ForeignKey("tickets.ticket_id"), nullable=True
    )
    face_id: Mapped[str] = mapped_column(String(255), nullable=True)
    device_id: Mapped[str] = mapped_column(String(50), nullable=False)
    gate_id: Mapped[str] = mapped_column(String(50), nullable=False)
    event_type: Mapped[str] = mapped_column(String(30), nullable=False)
    reason_code: Mapped[str] = mapped_column(String(50), nullable=True)
    confidence_score: Mapped[float] = mapped_column(Float, nullable=True)
    timestamp: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    synced_to_cloud: Mapped[bool] = mapped_column(Boolean, default=False)
