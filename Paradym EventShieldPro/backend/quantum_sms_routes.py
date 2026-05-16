"""
EventShield Pro - Quantum SMS API Routes
Flask blueprint exposing the Android SMS gateway (Moto G Play, AT&T Prepaid).
"""

from flask import Blueprint, jsonify, request
from datetime import datetime
import logging

from quantum_sms_service import quantum_sms

logger = logging.getLogger(__name__)

quantum_sms_api = Blueprint("quantum_sms_api", __name__, url_prefix="/api/quantum-sms")


# ---------------------------------------------------------------------------
# Device info & health
# ---------------------------------------------------------------------------

@quantum_sms_api.route("/device", methods=["GET"])
def get_device_info():
    return jsonify({"success": True, "device": quantum_sms.device_info()})


@quantum_sms_api.route("/health", methods=["GET"])
def health():
    result = quantum_sms.health_check()
    status_code = 200 if result.get("online") else 503
    return jsonify(result), status_code


# ---------------------------------------------------------------------------
# Send endpoints
# ---------------------------------------------------------------------------

@quantum_sms_api.route("/send", methods=["POST"])
def send_sms():
    data = request.get_json(silent=True) or {}
    to = data.get("to", "").strip()
    message = data.get("message", "").strip()

    if not to or not message:
        return jsonify({"success": False, "error": "to and message are required"}), 400

    priority = data.get("priority", "normal")
    result = quantum_sms.send_sms(to, message, priority)
    return jsonify(result), 200 if result["success"] else 502


@quantum_sms_api.route("/send-bulk", methods=["POST"])
def send_bulk():
    data = request.get_json(silent=True) or {}
    recipients = data.get("recipients", [])
    message = data.get("message", "").strip()

    if not recipients or not message:
        return jsonify({"success": False, "error": "recipients and message are required"}), 400

    result = quantum_sms.send_bulk(recipients, message)
    return jsonify(result), 200 if result["success"] else 502


# ---------------------------------------------------------------------------
# Alert shortcuts
# ---------------------------------------------------------------------------

@quantum_sms_api.route("/alert/criminal", methods=["POST"])
def criminal_alert():
    data = request.get_json(silent=True) or {}
    person_name = data.get("person_name", "Unknown")
    match_details = data.get("match_details", "")
    result = quantum_sms.send_criminal_alert(person_name, match_details)
    return jsonify(result)


@quantum_sms_api.route("/alert/payment", methods=["POST"])
def payment_alert():
    data = request.get_json(silent=True) or {}
    try:
        amount = float(data.get("amount", 0))
    except (TypeError, ValueError):
        return jsonify({"success": False, "error": "amount must be a number"}), 400
    method = data.get("method", "Unknown")
    status = data.get("status", "Unknown")
    result = quantum_sms.send_payment_alert(amount, method, status)
    return jsonify(result)


@quantum_sms_api.route("/alert/access", methods=["POST"])
def access_alert():
    data = request.get_json(silent=True) or {}
    person_name = data.get("person_name", "Unknown")
    access_type = data.get("access_type", "General")
    status = data.get("status", "Unknown")
    result = quantum_sms.send_access_alert(person_name, access_type, status)
    return jsonify(result)


@quantum_sms_api.route("/alert/device", methods=["POST"])
def device_alert():
    data = request.get_json(silent=True) or {}
    device_name = data.get("device_name", "Unknown Device")
    status = data.get("status", "Unknown")
    issue = data.get("issue")
    result = quantum_sms.send_device_alert(device_name, status, issue)
    return jsonify(result)
