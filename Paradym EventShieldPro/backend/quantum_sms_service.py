"""
EventShield Pro - Quantum SMS Gateway Service
Integrates with an Android device (Moto G Play, AT&T Prepaid) acting as a
physical SMS gateway via the Android SMS Gateway app REST API.

Device:  Moto G Play · Android 16 · AT&T Prepaid
Phone:   +1 (407) 866-9451
IMEI:    350173621027037
SIM ID:  110028197535958
"""

import json
import logging
import os
from datetime import datetime
from typing import Dict, List, Optional

import requests

logger = logging.getLogger(__name__)

DEVICE_CONFIG_PATH = os.path.join(os.path.dirname(__file__), "quantum_sms_device.json")


def _load_device_config() -> Dict:
    with open(DEVICE_CONFIG_PATH, "r") as f:
        return json.load(f)


class QuantumSMSDevice:
    """Manages the Moto G Play Android SMS gateway device."""

    def __init__(self):
        cfg = _load_device_config()
        self.phone_number: str = cfg["phone_number"]
        self.imei: str = cfg["imei"]
        self.sim_id: str = cfg["sim_id"]
        self.model: str = cfg["model"]
        self.os: str = cfg["os"]
        self.carrier: str = cfg["carrier"]

        gw = cfg["gateway"]
        host = os.getenv("QUANTUM_SMS_HOST", gw["host"])
        port = os.getenv("QUANTUM_SMS_PORT", str(gw["port"]))
        version = os.getenv("QUANTUM_SMS_API_VERSION", gw["api_version"])
        protocol = os.getenv("QUANTUM_SMS_PROTOCOL", gw["protocol"])
        self.base_url: str = f"{protocol}://{host}:{port}/{version}"

        sms = cfg["sms_settings"]
        self.max_length: int = sms["max_message_length"]
        self.split_long: bool = sms["split_long_messages"]
        self.delivery_report: bool = sms["delivery_report"]
        self.retry_attempts: int = sms["retry_attempts"]
        self.retry_delay: int = sms["retry_delay_seconds"]

        hc = cfg["health_check"]
        self.health_interval: int = hc["interval_seconds"]
        self.health_timeout: int = hc["timeout_seconds"]

    # ------------------------------------------------------------------
    # Core send
    # ------------------------------------------------------------------

    def send_sms(
        self,
        to: str,
        message: str,
        priority: str = "normal",
    ) -> Dict:
        """
        Send an SMS through the Android gateway.

        Args:
            to:       E.164 recipient number, e.g. "+18135551234"
            message:  Text body (split automatically if longer than 160 chars)
            priority: "low" | "normal" | "high" | "critical"

        Returns:
            Dict with success, message_id, and timestamp.
        """
        to = self._normalise_number(to)
        parts = self._split_message(message) if self.split_long else [message]
        results = []

        for idx, part in enumerate(parts):
            payload = {
                "phoneNumber": to,
                "message": part,
                "deliveryReport": self.delivery_report,
                "simNumber": 1,
            }
            result = self._post_with_retry("/message", payload)
            result["part"] = idx + 1
            result["total_parts"] = len(parts)
            results.append(result)

        all_ok = all(r.get("success") for r in results)
        return {
            "success": all_ok,
            "to": to,
            "parts": results,
            "priority": priority,
            "timestamp": datetime.now().isoformat(),
        }

    # ------------------------------------------------------------------
    # Convenience alert senders (mirror sms_service.py interface)
    # ------------------------------------------------------------------

    def send_criminal_alert(self, person_name: str, match_details: str) -> Dict:
        msg = (
            f"CRIMINAL DB MATCH — {person_name}\n"
            f"Details: {match_details}\n"
            f"Action: Deny access and notify security."
        )
        return self.send_sms(self.phone_number, msg, priority="critical")

    def send_payment_alert(self, amount: float, method: str, status: str) -> Dict:
        msg = (
            f"PAYMENT ALERT\n"
            f"Amount: ${amount:.2f} | Method: {method} | Status: {status}\n"
            f"Time: {datetime.now().strftime('%H:%M:%S')}"
        )
        priority = "high" if status.lower() == "failed" else "normal"
        return self.send_sms(self.phone_number, msg, priority=priority)

    def send_access_alert(
        self, person_name: str, access_type: str, status: str
    ) -> Dict:
        msg = (
            f"ACCESS ALERT\n"
            f"Person: {person_name} | Type: {access_type} | Status: {status}\n"
            f"Time: {datetime.now().strftime('%H:%M:%S')}"
        )
        priority = "high" if status.lower() == "denied" else "normal"
        return self.send_sms(self.phone_number, msg, priority=priority)

    def send_device_alert(
        self, device_name: str, status: str, issue: Optional[str] = None
    ) -> Dict:
        msg = f"DEVICE ALERT\nDevice: {device_name} | Status: {status}"
        if issue:
            msg += f"\nIssue: {issue}"
        msg += f"\nTime: {datetime.now().strftime('%H:%M:%S')}"
        priority = "high" if status.lower() == "offline" else "normal"
        return self.send_sms(self.phone_number, msg, priority=priority)

    def send_bulk(self, recipients: List[str], message: str) -> Dict:
        """Send the same message to multiple recipients."""
        results = []
        for number in recipients:
            result = self.send_sms(number, message)
            result["recipient"] = number
            results.append(result)
        success_count = sum(1 for r in results if r.get("success"))
        return {
            "success": success_count > 0,
            "total": len(recipients),
            "sent": success_count,
            "failed": len(recipients) - success_count,
            "results": results,
            "timestamp": datetime.now().isoformat(),
        }

    # ------------------------------------------------------------------
    # Health & diagnostics
    # ------------------------------------------------------------------

    def health_check(self) -> Dict:
        try:
            resp = requests.get(
                f"{self.base_url}/health",
                timeout=self.health_timeout,
            )
            if resp.status_code == 200:
                data = resp.json()
                return {
                    "success": True,
                    "online": True,
                    "device": self.model,
                    "carrier": self.carrier,
                    "phone": self.phone_number,
                    "gateway_response": data,
                    "timestamp": datetime.now().isoformat(),
                }
            return {
                "success": False,
                "online": False,
                "error": f"HTTP {resp.status_code}",
                "timestamp": datetime.now().isoformat(),
            }
        except requests.exceptions.ConnectionError:
            return {
                "success": False,
                "online": False,
                "error": "Gateway unreachable — ensure the Android SMS Gateway app is running",
                "timestamp": datetime.now().isoformat(),
            }
        except Exception as exc:
            return {
                "success": False,
                "online": False,
                "error": str(exc),
                "timestamp": datetime.now().isoformat(),
            }

    def device_info(self) -> Dict:
        return {
            "phone_number": self.phone_number,
            "imei": self.imei,
            "sim_id": self.sim_id,
            "model": self.model,
            "os": self.os,
            "carrier": self.carrier,
            "gateway_url": self.base_url,
        }

    # ------------------------------------------------------------------
    # Internal helpers
    # ------------------------------------------------------------------

    def _post_with_retry(self, path: str, payload: Dict) -> Dict:
        import time

        url = f"{self.base_url}{path}"
        last_error = ""
        for attempt in range(1, self.retry_attempts + 1):
            try:
                resp = requests.post(url, json=payload, timeout=15)
                if resp.status_code in (200, 201, 202):
                    data = resp.json()
                    return {
                        "success": True,
                        "message_id": data.get("id") or data.get("messageId"),
                        "status": data.get("status", "sent"),
                    }
                last_error = f"HTTP {resp.status_code}: {resp.text[:200]}"
                logger.warning("Quantum SMS attempt %d failed: %s", attempt, last_error)
            except requests.exceptions.RequestException as exc:
                last_error = str(exc)
                logger.warning("Quantum SMS attempt %d error: %s", attempt, last_error)

            if attempt < self.retry_attempts:
                time.sleep(self.retry_delay)

        return {"success": False, "error": last_error}

    @staticmethod
    def _normalise_number(number: str) -> str:
        digits = "".join(c for c in number if c.isdigit())
        if len(digits) == 10:
            return f"+1{digits}"
        if len(digits) == 11 and digits.startswith("1"):
            return f"+{digits}"
        return f"+{digits}" if not number.startswith("+") else number

    def _split_message(self, message: str) -> List[str]:
        if len(message) <= self.max_length:
            return [message]
        parts = []
        while message:
            parts.append(message[: self.max_length])
            message = message[self.max_length :]
        return parts


# ---------------------------------------------------------------------------
# Module-level singleton + convenience functions
# ---------------------------------------------------------------------------

quantum_sms = QuantumSMSDevice()


def send_sms(to: str, message: str, priority: str = "normal") -> Dict:
    return quantum_sms.send_sms(to, message, priority)


def send_criminal_alert(person_name: str, match_details: str) -> Dict:
    return quantum_sms.send_criminal_alert(person_name, match_details)


def send_payment_alert(amount: float, method: str, status: str) -> Dict:
    return quantum_sms.send_payment_alert(amount, method, status)


def send_access_alert(person_name: str, access_type: str, status: str) -> Dict:
    return quantum_sms.send_access_alert(person_name, access_type, status)


def send_device_alert(device_name: str, status: str, issue: Optional[str] = None) -> Dict:
    return quantum_sms.send_device_alert(device_name, status, issue)


def health_check() -> Dict:
    return quantum_sms.health_check()


def device_info() -> Dict:
    return quantum_sms.device_info()
