"""Pydantic request/response schemas for all API endpoints."""

from pydantic import BaseModel, Field
from typing import Optional
from datetime import datetime


# --- Auth ---
class LoginRequest(BaseModel):
    username: str
    password: str


class TokenResponse(BaseModel):
    access_token: str
    token_type: str = "bearer"


# --- Events ---
class EventCreate(BaseModel):
    name: str
    event_date: str
    start_time: datetime
    end_time: datetime
    admission_price: float = 0.00
    max_capacity: int = 0
    multi_entry: bool = True
    ticket_expiry_hours: int = 24


class EventResponse(BaseModel):
    event_id: str
    name: str
    event_date: str
    start_time: datetime
    end_time: datetime
    admission_price: float
    vip_price: Optional[float] = None
    max_capacity: int
    current_capacity: int
    status: str
    multi_entry: bool
    ticket_expiry_hours: int
    created_at: datetime

    class Config:
        from_attributes = True


# --- Tickets ---
class TicketResponse(BaseModel):
    ticket_id: str
    event_id: str
    square_payment_id: Optional[str] = None
    patron_name: Optional[str] = None
    patron_phone: Optional[str] = None
    patron_email: Optional[str] = None
    admission_type: str
    amount_paid: float
    qr_used: bool = False
    face_enrolled: bool
    finger_enrolled: bool = False
    iris_enrolled: bool = False
    status: str
    entry_count: int
    expires_at: Optional[datetime] = None
    created_at: datetime
    last_entry_at: Optional[datetime] = None
    first_gate_id: Optional[str] = None
    group_id: Optional[str] = None

    class Config:
        from_attributes = True


class TicketCreateManual(BaseModel):
    event_id: str
    patron_name: Optional[str] = None
    patron_phone: Optional[str] = None
    patron_email: Optional[str] = None
    admission_type: str = "DAILY"
    amount_paid: float = 0.00


class TicketLineItem(BaseModel):
    admission_type: str
    quantity: int = 1
    unit_price: float = 0.00
    patron_names: Optional[list[str]] = None


class GroupOrderCreate(BaseModel):
    event_id: str
    patron_name: Optional[str] = None
    patron_phone: Optional[str] = None
    patron_email: Optional[str] = None
    items: list[TicketLineItem]
    total_paid: float = 0.00


class GroupOrderResponse(BaseModel):
    group_id: str
    tickets: list[TicketResponse]
    total_tickets: int
    total_paid: float


# --- Access Control ---
class ValidateTicketRequest(BaseModel):
    token: str
    device_id: str
    timestamp: Optional[str] = None


class ValidateTicketResponse(BaseModel):
    status: str
    ticket_id: Optional[str] = None
    admission_type: Optional[str] = None
    reason_code: Optional[str] = None
    grant_access: bool = False


class EnrollCompleteRequest(BaseModel):
    ticket_id: str
    face_id: str
    device_id: str


class FaceEntryRequest(BaseModel):
    face_id: str
    device_id: str
    confidence_score: float = 0.0
    timestamp: Optional[str] = None


class FaceEntryResponse(BaseModel):
    grant_access: bool
    reason: Optional[str] = None
    ticket_id: Optional[str] = None


# --- Devices ---
class DeviceCreate(BaseModel):
    device_id: str
    device_name: str
    gate_id: str
    gate_name: Optional[str] = None
    ip_address: Optional[str] = None
    serial_number: Optional[str] = None
    model: str = "X05"


class DeviceResponse(BaseModel):
    device_id: str
    device_name: str
    ip_address: Optional[str] = None
    gate_id: str
    gate_name: Optional[str] = None
    status: str
    last_heartbeat: Optional[datetime] = None
    firmware_version: Optional[str] = None
    app_version: Optional[str] = None
    face_count: int
    finger_count: int = 0
    iris_count: int = 0
    serial_number: Optional[str] = None
    model: str = "X05"
    api_token: Optional[str] = None
    wifi_connected: bool = False
    cellular_connected: bool = False
    battery_level: Optional[int] = None
    has_fingerprint: bool = False
    has_iris: bool = False
    has_nfc: bool = False
    finger_sdk_version: Optional[str] = None
    iris_sdk_version: Optional[str] = None
    wiegand_enabled: bool = False
    relay_mode: str = "NO"
    rs485_enabled: bool = False
    created_at: datetime

    class Config:
        from_attributes = True


# --- X05 Device API Schemas ---
class X05RegisterRequest(BaseModel):
    device_id: str
    serial_number: str
    model: str = "X05"
    firmware_version: Optional[str] = None
    app_version: Optional[str] = None
    has_fingerprint: bool = False
    has_iris: bool = False
    has_nfc: bool = False
    finger_sdk_version: Optional[str] = None
    iris_sdk_version: Optional[str] = None
    wiegand_enabled: bool = False
    relay_mode: str = "NO"
    rs485_enabled: bool = False


class X05HeartbeatRequest(BaseModel):
    device_id: str
    face_count: int = 0
    finger_count: int = 0
    iris_count: int = 0
    wifi_connected: bool = False
    cellular_connected: bool = False
    battery_level: Optional[int] = None
    app_version: Optional[str] = None


class X05ValidateQRRequest(BaseModel):
    device_id: str
    qr_data: str
    gate_id: str


class X05ValidateQRResponse(BaseModel):
    action: str  # ENROLL, GRANT, DENY
    message: str
    ticket_id: Optional[str] = None
    patron_name: Optional[str] = None
    admission_type: Optional[str] = None
    expires_at: Optional[str] = None
    open_relay: bool = False


class X05EnrollCompleteRequest(BaseModel):
    device_id: str
    ticket_id: str
    face_id: Optional[str] = None
    finger_id: Optional[str] = None
    iris_id: Optional[str] = None
    biometric_type: str = "face"  # "face", "finger", "iris", "face+iris", "finger+iris", "all"
    success: bool


class X05FaceEntryRequest(BaseModel):
    device_id: str
    face_id: str
    confidence_score: float = 0.0


class X05FaceEntryResponse(BaseModel):
    grant_access: bool
    open_relay: bool = False
    reason: Optional[str] = None
    ticket_id: Optional[str] = None
    patron_name: Optional[str] = None


class X05FingerEntryRequest(BaseModel):
    """Fingerprint-based entry request (FingerSDK v2.0.1)."""
    device_id: str
    finger_id: str
    match_score: float = 0.0


class X05FingerEntryResponse(BaseModel):
    grant_access: bool
    open_relay: bool = False
    reason: Optional[str] = None
    ticket_id: Optional[str] = None
    patron_name: Optional[str] = None


class X05IrisEntryRequest(BaseModel):
    """Iris-based entry request (DataType=3 infrared, bioassay mode 1)."""
    device_id: str
    iris_id: str
    quality_score: float = 0.0


class X05IrisEntryResponse(BaseModel):
    grant_access: bool
    open_relay: bool = False
    reason: Optional[str] = None
    ticket_id: Optional[str] = None
    patron_name: Optional[str] = None


class X05AlertRequest(BaseModel):
    device_id: str
    alert_type: str
    message: Optional[str] = None
    severity: str = "WARNING"


# --- Access Log ---
class AccessLogResponse(BaseModel):
    log_id: str
    ticket_id: Optional[str] = None
    face_id: Optional[str] = None
    device_id: str
    gate_id: str
    event_type: str
    reason_code: Optional[str] = None
    confidence_score: Optional[float] = None
    timestamp: datetime

    class Config:
        from_attributes = True


# --- Dashboard ---
class DashboardStats(BaseModel):
    total_tickets_sold: int = 0
    total_entries: int = 0
    patrons_inside: int = 0
    revenue: float = 0.0
    denied_count: int = 0
    active_event: Optional[EventResponse] = None
    devices_online: int = 0
    devices_total: int = 0


class AlertItem(BaseModel):
    alert_id: str
    device_id: str
    gate_id: Optional[str] = None
    alert_type: str
    message: Optional[str] = None
    severity: str = "WARNING"
    timestamp: datetime
    acknowledged: bool = False


# --- Settings ---
class SettingsUpdate(BaseModel):
    key: str
    value: str


# --- Health ---
class HealthResponse(BaseModel):
    status: str
    database: str
    devices: dict
    uptime: Optional[str] = None
    internet: bool = False
