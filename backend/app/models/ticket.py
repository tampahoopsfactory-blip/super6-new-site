"""Ticket model — a single admission ticket linked to a patron and event.

Supports three ticket types per EventShield Pro requirements:
  DAILY    — expires at end of calendar day (23:59:59 local)
  WEEKEND  — expires at end of Sunday night (23:59:59)
  STAFF    — no expiry, unlimited access for workers/volunteers/officials
"""

import uuid
from datetime import datetime, timedelta
from sqlalchemy import String, Integer, DateTime, Numeric, Boolean, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.database import Base
import enum


class TicketStatus(str, enum.Enum):
    ACTIVE = "ACTIVE"
    EXPIRED = "EXPIRED"
    REFUNDED = "REFUNDED"
    INVALID = "INVALID"


class AdmissionType(str, enum.Enum):
    DAILY = "DAILY"
    WEEKEND = "WEEKEND"
    KIDS = "KIDS"
    STAFF = "STAFF"


# Expiry rules per ticket type
TICKET_EXPIRY_RULES = {
    "DAILY": "end_of_day",
    "WEEKEND": "end_of_weekend",
    "KIDS": "end_of_day",
    "STAFF": "never",
}

# Multi-entry rules per ticket type
TICKET_MULTIENTRY_RULES = {
    "DAILY": True,      # re-entry allowed within day
    "WEEKEND": True,    # re-entry allowed Sat & Sun
    "KIDS": True,       # re-entry allowed within day (child admission)
    "STAFF": True,      # unlimited access
}


def compute_expiry(admission_type: str, created_at: datetime,
                   event_end: datetime = None, expiry_hours: int = 24,
                   season_end_date: datetime = None) -> datetime | None:
    """Calculate the expiry datetime for a ticket based on its type."""
    rule = TICKET_EXPIRY_RULES.get(admission_type, "end_of_day")

    if rule == "end_of_day":
        return created_at.replace(hour=23, minute=59, second=59, microsecond=0)

    elif rule == "end_of_weekend":
        # Find next Sunday 23:59:59
        days_until_sunday = (6 - created_at.weekday()) % 7
        if days_until_sunday == 0 and created_at.weekday() != 6:
            days_until_sunday = 7
        if created_at.weekday() == 6:
            days_until_sunday = 0
        sunday = created_at + timedelta(days=days_until_sunday)
        return sunday.replace(hour=23, minute=59, second=59, microsecond=0)

    elif rule == "never":
        return None

    return created_at + timedelta(hours=expiry_hours)


class Ticket(Base):
    __tablename__ = "tickets"

    ticket_id: Mapped[str] = mapped_column(
        String(36), primary_key=True, default=lambda: str(uuid.uuid4())
    )
    event_id: Mapped[str] = mapped_column(
        String(36), ForeignKey("events.event_id"), nullable=False
    )
    square_payment_id: Mapped[str] = mapped_column(
        String(255), unique=True, nullable=True
    )
    patron_name: Mapped[str] = mapped_column(String(255), nullable=True)
    patron_phone: Mapped[str] = mapped_column(String(255), nullable=True)
    patron_email: Mapped[str] = mapped_column(String(255), nullable=True)
    admission_type: Mapped[str] = mapped_column(
        String(20), default=AdmissionType.DAILY.value
    )
    amount_paid: Mapped[float] = mapped_column(Numeric(10, 2), default=0.00)
    qr_token_hash: Mapped[str] = mapped_column(String(64), unique=True, index=True)
    qr_used: Mapped[bool] = mapped_column(Boolean, default=False)
    face_enrolled: Mapped[bool] = mapped_column(Boolean, default=False)
    face_id: Mapped[str] = mapped_column(String(255), nullable=True)
    finger_enrolled: Mapped[bool] = mapped_column(Boolean, default=False)
    finger_id: Mapped[str] = mapped_column(String(255), nullable=True)
    iris_enrolled: Mapped[bool] = mapped_column(Boolean, default=False)
    iris_id: Mapped[str] = mapped_column(String(255), nullable=True)
    status: Mapped[str] = mapped_column(
        String(20), default=TicketStatus.ACTIVE.value
    )
    entry_count: Mapped[int] = mapped_column(Integer, default=0)
    expires_at: Mapped[datetime] = mapped_column(DateTime, nullable=True)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)
    last_entry_at: Mapped[datetime] = mapped_column(DateTime, nullable=True)
    first_gate_id: Mapped[str] = mapped_column(String(50), nullable=True)
    group_id: Mapped[str] = mapped_column(String(36), nullable=True, index=True)
    synced_to_cloud: Mapped[bool] = mapped_column(Boolean, default=False)

    event = relationship("Event", back_populates="tickets")

    def is_expired(self) -> bool:
        """Check if this ticket has expired based on its type-specific rules."""
        if self.status != TicketStatus.ACTIVE.value:
            return True
        if self.expires_at is None:
            return False  # STAFF — never expires
        return datetime.utcnow() > self.expires_at

    def allows_reentry(self) -> bool:
        """Check if this ticket type allows multiple entries."""
        return TICKET_MULTIENTRY_RULES.get(self.admission_type, False)
