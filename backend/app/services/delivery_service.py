"""
Notification delivery service for SMS (Twilio) and Email (SendGrid).

Handles QR code delivery to patrons after ticket purchase.
Supports retry logic via the delivery_queue table.
"""

import logging
from datetime import datetime
from typing import Optional
from app.config import settings
from app.utils.qr import generate_qr_code, generate_qr_base64

logger = logging.getLogger("eventshield.delivery")


class DeliveryService:

    async def send_sms(self, to_number: str, message: str,
                       media_url: Optional[str] = None) -> bool:
        if not settings.sms_enabled:
            logger.info(f"SMS disabled, would send to {to_number}")
            return True
        try:
            from twilio.rest import Client
            client = Client(settings.twilio_account_sid, settings.twilio_auth_token)
            kwargs = {
                "body": message,
                "from_": settings.twilio_from_number,
                "to": to_number,
            }
            if media_url:
                kwargs["media_url"] = [media_url]
            msg = client.messages.create(**kwargs)
            logger.info(f"SMS sent to {to_number}: {msg.sid}")
            return True
        except Exception as e:
            logger.error(f"SMS send failed to {to_number}: {e}")
            return False

    async def send_email(self, to_email: str, subject: str,
                          html_content: str, attachments: list = None) -> bool:
        if not settings.email_enabled:
            logger.info(f"Email disabled, would send to {to_email}")
            return True
        try:
            from sendgrid import SendGridAPIClient
            from sendgrid.helpers.mail import (
                Mail, Attachment, FileContent, FileName, FileType, Disposition,
            )
            message = Mail(
                from_email=(settings.sendgrid_from_email, settings.sendgrid_from_name),
                to_emails=to_email,
                subject=subject,
                html_content=html_content,
            )
            if attachments:
                for att in attachments:
                    attachment = Attachment(
                        FileContent(att["content"]),
                        FileName(att["filename"]),
                        FileType(att.get("type", "image/png")),
                        Disposition("attachment"),
                    )
                    message.attachment = attachment
            sg = SendGridAPIClient(settings.sendgrid_api_key)
            response = sg.send(message)
            logger.info(f"Email sent to {to_email}: {response.status_code}")
            return response.status_code in (200, 201, 202)
        except Exception as e:
            logger.error(f"Email send failed to {to_email}: {e}")
            return False

    async def send_ticket_sms(self, to_number: str, event_name: str,
                               event_date: str, qr_token: str,
                               admission_type: str) -> bool:
        message = (
            f"🎟 EventShield Pro\n\n"
            f"Your ticket for {event_name} on {event_date} is confirmed!\n"
            f"Admission: {admission_type}\n\n"
            f"Show this QR code at the gate for entry.\n"
            f"Token: {qr_token[:8]}..."
        )
        return await self.send_sms(to_number, message)

    async def send_ticket_email(self, to_email: str, event_name: str,
                                 event_date: str, qr_token: str,
                                 admission_type: str, patron_name: str = "Guest") -> bool:
        qr_b64 = generate_qr_base64(qr_token, 300)
        html = f"""
        <div style="font-family: -apple-system, BlinkMacSystemFont, 'SF Pro Display', 'Segoe UI', sans-serif; max-width: 480px; margin: 0 auto; padding: 40px 20px; color: #1d1d1f;">
            <div style="text-align: center; margin-bottom: 32px;">
                <h1 style="font-size: 28px; font-weight: 600; letter-spacing: -0.02em; margin: 0;">EventShield Pro</h1>
            </div>
            <div style="background: #f5f5f7; border-radius: 16px; padding: 32px; text-align: center;">
                <p style="font-size: 15px; color: #86868b; margin: 0 0 4px;">Your ticket for</p>
                <h2 style="font-size: 22px; font-weight: 600; margin: 0 0 4px;">{event_name}</h2>
                <p style="font-size: 15px; color: #86868b; margin: 0 0 24px;">{event_date}</p>
                <div style="background: white; border-radius: 12px; padding: 24px; display: inline-block;">
                    <img src="data:image/png;base64,{qr_b64}" alt="QR Code" style="width: 200px; height: 200px;" />
                </div>
                <div style="margin-top: 20px;">
                    <span style="display: inline-block; background: #1d1d1f; color: white; font-size: 13px; font-weight: 500; padding: 6px 16px; border-radius: 20px;">{admission_type}</span>
                </div>
            </div>
            <div style="margin-top: 24px; padding: 0 8px;">
                <p style="font-size: 14px; color: #86868b; line-height: 1.5;">
                    Hi {patron_name}, present this QR code at the entrance gate.
                    After your first scan, facial recognition will be enrolled for faster re-entry.
                </p>
            </div>
        </div>
        """
        qr_raw = generate_qr_code(qr_token, 300)
        import base64
        attachments = [{
            "content": base64.b64encode(qr_raw).decode(),
            "filename": "ticket-qr.png",
            "type": "image/png",
        }]
        return await self.send_email(
            to_email, f"Your ticket for {event_name}", html, attachments
        )


delivery_service = DeliveryService()
