"""QR code generation utilities."""

import io
import base64
import qrcode
from qrcode.constants import ERROR_CORRECT_H


def generate_qr_code(data: str, size: int = 300) -> bytes:
    qr = qrcode.QRCode(
        version=None,
        error_correction=ERROR_CORRECT_H,
        box_size=10,
        border=4,
    )
    qr.add_data(data)
    qr.make(fit=True)
    img = qr.make_image(fill_color="black", back_color="white")
    img = img.resize((size, size))
    buf = io.BytesIO()
    img.save(buf, format="PNG")
    return buf.getvalue()


def generate_qr_base64(data: str, size: int = 300) -> str:
    raw = generate_qr_code(data, size)
    return base64.b64encode(raw).decode()
