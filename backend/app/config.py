"""Application configuration loaded from environment variables."""

from pydantic_settings import BaseSettings
from typing import Optional


class Settings(BaseSettings):
    # Application
    app_env: str = "development"
    app_secret_key: str = "change-this-to-a-random-secret-key-min-32-chars"
    api_key: str = "default-api-key"
    admin_username: str = "admin"
    admin_password: str = "changeme123"

    # Database
    database_url: str = "sqlite+aiosqlite:///./eventshield.db"

    # Square
    square_application_id: str = ""
    square_access_token: str = ""
    square_webhook_signature_key: str = ""
    square_environment: str = "sandbox"

    # Twilio
    twilio_account_sid: str = ""
    twilio_auth_token: str = ""
    twilio_from_number: str = ""
    sms_enabled: bool = False

    # SendGrid
    sendgrid_api_key: str = ""
    sendgrid_from_email: str = "tickets@eventshield.local"
    sendgrid_from_name: str = "EventShield Pro"
    email_enabled: bool = False

    # Network
    server_host: str = "0.0.0.0"
    server_port: int = 8000
    local_server_ip: str = "192.168.1.10"

    # X05 Device Settings
    device_heartbeat_timeout: int = 120  # Seconds before marking device offline
    device_health_interval: int = 30

    # Cloud Sync
    cloud_sync_enabled: bool = False
    cloud_sync_url: str = ""
    cloud_sync_interval: int = 60
    cloud_sync_api_key: str = ""

    # Encryption
    encryption_key: str = "default-encryption-key-change-me!"

    # JWT
    jwt_algorithm: str = "HS256"
    jwt_expiration_minutes: int = 480

    model_config = {"env_file": ".env", "env_file_encoding": "utf-8"}


settings = Settings()
