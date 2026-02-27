"""
Square Terminal integration service.

Handles payment webhook processing, Terminal API interactions,
and QR code delivery workflow.
"""

import hashlib
import hmac
import uuid
import logging
from datetime import datetime
from typing import Optional
import httpx
from app.config import settings

logger = logging.getLogger("eventshield.square")

SQUARE_BASE_URL = {
    "sandbox": "https://connect.squareupsandbox.com/v2",
    "production": "https://connect.squareup.com/v2",
}


class SquareService:
    def __init__(self):
        self.base_url = SQUARE_BASE_URL.get(settings.square_environment, SQUARE_BASE_URL["sandbox"])
        self.headers = {
            "Authorization": f"Bearer {settings.square_access_token}",
            "Content-Type": "application/json",
            "Square-Version": "2024-01-18",
        }

    def verify_webhook_signature(self, body: bytes, signature: str, url: str) -> bool:
        if not settings.square_webhook_signature_key:
            logger.warning("Webhook signature key not configured, skipping verification")
            return True
        key = settings.square_webhook_signature_key.encode()
        payload = url.encode() + body
        expected = hmac.new(key, payload, hashlib.sha256).base64digest()
        return hmac.compare_digest(expected, signature)

    async def get_payment_details(self, payment_id: str) -> Optional[dict]:
        async with httpx.AsyncClient() as client:
            try:
                resp = await client.get(
                    f"{self.base_url}/payments/{payment_id}",
                    headers=self.headers,
                    timeout=10.0,
                )
                if resp.status_code == 200:
                    return resp.json().get("payment")
                logger.error(f"Square API error: {resp.status_code} {resp.text}")
            except Exception as e:
                logger.error(f"Square API request failed: {e}")
        return None

    async def create_terminal_checkout(self, amount_cents: int, device_id: str,
                                        note: str = "") -> Optional[dict]:
        async with httpx.AsyncClient() as client:
            try:
                body = {
                    "idempotency_key": str(uuid.uuid4()),
                    "checkout": {
                        "amount_money": {
                            "amount": amount_cents,
                            "currency": "USD",
                        },
                        "device_options": {
                            "device_id": device_id,
                        },
                        "note": note,
                    },
                }
                resp = await client.post(
                    f"{self.base_url}/terminals/checkouts",
                    headers=self.headers,
                    json=body,
                    timeout=10.0,
                )
                if resp.status_code == 200:
                    return resp.json().get("checkout")
                logger.error(f"Terminal checkout error: {resp.status_code} {resp.text}")
            except Exception as e:
                logger.error(f"Terminal checkout failed: {e}")
        return None

    async def refund_payment(self, payment_id: str, amount_cents: int,
                              reason: str = "Refund") -> Optional[dict]:
        async with httpx.AsyncClient() as client:
            try:
                body = {
                    "idempotency_key": str(uuid.uuid4()),
                    "payment_id": payment_id,
                    "amount_money": {
                        "amount": amount_cents,
                        "currency": "USD",
                    },
                    "reason": reason,
                }
                resp = await client.post(
                    f"{self.base_url}/refunds",
                    headers=self.headers,
                    json=body,
                    timeout=10.0,
                )
                if resp.status_code == 200:
                    return resp.json().get("refund")
                logger.error(f"Refund error: {resp.status_code} {resp.text}")
            except Exception as e:
                logger.error(f"Refund request failed: {e}")
        return None


square_service = SquareService()
