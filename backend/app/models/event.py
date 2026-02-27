"""Event model — represents a single event (e.g. a basketball game)."""

import uuid
from datetime import datetime
from sqlalchemy import String, Integer, DateTime, Numeric, Enum as SAEnum
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.database import Base
import enum


class EventStatus(str, enum.Enum):
    UPCOMING = "UPCOMING"
    ACTIVE = "ACTIVE"
    ENDED = "ENDED"


class Event(Base):
    __tablename__ = "events"

    event_id: Mapped[str] = mapped_column(
        String(36), primary_key=True, default=lambda: str(uuid.uuid4())
    )
    name: Mapped[str] = mapped_column(String(255), nullable=False)
    event_date: Mapped[str] = mapped_column(String(10), nullable=False)
    start_time: Mapped[datetime] = mapped_column(DateTime, nullable=False)
    end_time: Mapped[datetime] = mapped_column(DateTime, nullable=False)
    admission_price: Mapped[float] = mapped_column(Numeric(10, 2), default=0.00)
    vip_price: Mapped[float] = mapped_column(Numeric(10, 2), nullable=True)
    max_capacity: Mapped[int] = mapped_column(Integer, default=0)
    current_capacity: Mapped[int] = mapped_column(Integer, default=0)
    status: Mapped[str] = mapped_column(
        String(20), default=EventStatus.UPCOMING.value
    )
    multi_entry: Mapped[bool] = mapped_column(default=True)
    ticket_expiry_hours: Mapped[int] = mapped_column(Integer, default=24)
    created_at: Mapped[datetime] = mapped_column(DateTime, default=datetime.utcnow)

    tickets = relationship("Ticket", back_populates="event", lazy="selectin")
