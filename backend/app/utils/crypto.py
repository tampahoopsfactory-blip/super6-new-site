"""Encryption utilities for PII fields and token hashing."""

import hashlib
import base64
import os
from cryptography.fernet import Fernet
from app.config import settings


def _get_fernet() -> Fernet:
    key = settings.encryption_key.encode()
    if len(key) < 32:
        key = key.ljust(32, b"0")
    key = base64.urlsafe_b64encode(key[:32])
    return Fernet(key)


def encrypt_pii(plaintext: str | None) -> str | None:
    if not plaintext:
        return None
    f = _get_fernet()
    return f.encrypt(plaintext.encode()).decode()


def decrypt_pii(ciphertext: str | None) -> str | None:
    if not ciphertext:
        return None
    try:
        f = _get_fernet()
        return f.decrypt(ciphertext.encode()).decode()
    except Exception:
        return ciphertext


def hash_token(token: str) -> str:
    return hashlib.sha256(token.encode()).hexdigest()
