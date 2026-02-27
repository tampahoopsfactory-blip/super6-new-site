"""
F8881 Facial Recognition Device Communication Service.

Implements the DS-F8881 protocol over TCP/UDP based on the DoorAccessIO SDK.
The protocol uses a binary packet format with the following structure:

  [Header 0x7E][Length 2B][SN 16B][Command 2B][Password 8B][Data NB][Checksum 1B]

Communication modes:
  - TCP Client: Server connects to device on port 8000
  - UDP: Server sends to device on port 8101, listens on local port
  - TCP Server: Device connects to server as client (for WAN setups)

This module provides a pure-Python async implementation for:
  - Device discovery and health monitoring
  - Face enrollment and deletion
  - Door/turnstile relay control
  - Real-time event monitoring (watch mode)
  - Transaction record retrieval
"""

import asyncio
import struct
import logging
import time
from typing import Optional
from dataclasses import dataclass, field
from datetime import datetime

logger = logging.getLogger("eventshield.f8881")


@dataclass
class DeviceConnection:
    ip: str
    tcp_port: int = 8000
    udp_port: int = 8101
    sn: str = "0000000000000000"
    password: str = "FFFFFFFF"
    timeout: float = 5.0
    reader: Optional[asyncio.StreamReader] = field(default=None, repr=False)
    writer: Optional[asyncio.StreamWriter] = field(default=None, repr=False)
    connected: bool = False
    last_heartbeat: Optional[datetime] = None


class F8881Protocol:
    """Binary protocol encoder/decoder for the F8881 device family."""

    HEADER = 0x7E
    CMD_READ_SN = 0x0101
    CMD_OPEN_DOOR = 0x0108
    CMD_CLOSE_DOOR = 0x0109
    CMD_HOLD_DOOR = 0x010A
    CMD_BEGIN_WATCH = 0x0114
    CMD_CLOSE_WATCH = 0x0115
    CMD_READ_TCP = 0x0103
    CMD_WRITE_TCP = 0x0104
    CMD_ADD_PERSON = 0x0B01
    CMD_DELETE_PERSON = 0x0B02
    CMD_CLEAR_ALL_PERSONS = 0x0B03
    CMD_READ_PERSON = 0x0B04
    CMD_READ_TRANSACTION = 0x0501
    CMD_HEARTBEAT_RESPONSE = 0x0122

    @staticmethod
    def _checksum(data: bytes) -> int:
        return sum(data) & 0xFF

    @classmethod
    def build_packet(cls, sn: str, password: str, cmd: int, data: bytes = b"") -> bytes:
        sn_bytes = sn.encode("ascii")[:16].ljust(16, b"0")
        pwd_bytes = bytes.fromhex(password.ljust(8, "F")[:8])
        payload = sn_bytes + struct.pack(">H", cmd) + pwd_bytes + data
        length = len(payload) + 1  # +1 for checksum
        packet = struct.pack(">BH", cls.HEADER, length) + payload
        chk = cls._checksum(packet[1:])
        return packet + struct.pack("B", chk)

    @classmethod
    def parse_response(cls, raw: bytes) -> dict:
        if len(raw) < 29 or raw[0] != cls.HEADER:
            return {"valid": False, "error": "Invalid packet"}
        length = struct.unpack(">H", raw[1:3])[0]
        sn = raw[3:19].decode("ascii", errors="replace").strip("\x00")
        cmd = struct.unpack(">H", raw[19:21])[0]
        data = raw[29 : 3 + length - 1] if length > 27 else b""
        return {"valid": True, "sn": sn, "cmd": cmd, "data": data, "raw": raw}


class F8881Service:
    """High-level async service for managing F8881 device communication."""

    def __init__(self):
        self._connections: dict[str, DeviceConnection] = {}

    async def connect(self, device_id: str, ip: str, port: int = 8000,
                      sn: str = "0000000000000000", password: str = "FFFFFFFF") -> bool:
        try:
            reader, writer = await asyncio.wait_for(
                asyncio.open_connection(ip, port), timeout=5.0
            )
            conn = DeviceConnection(
                ip=ip, tcp_port=port, sn=sn, password=password,
                reader=reader, writer=writer, connected=True,
                last_heartbeat=datetime.utcnow()
            )
            self._connections[device_id] = conn
            logger.info(f"Connected to F8881 device {device_id} at {ip}:{port}")
            return True
        except Exception as e:
            logger.error(f"Failed to connect to {device_id} at {ip}:{port}: {e}")
            return False

    async def disconnect(self, device_id: str):
        conn = self._connections.get(device_id)
        if conn and conn.writer:
            conn.writer.close()
            try:
                await conn.writer.wait_closed()
            except Exception:
                pass
            conn.connected = False
            logger.info(f"Disconnected from device {device_id}")

    async def _send_command(self, device_id: str, cmd: int, data: bytes = b"",
                            timeout: float = 5.0) -> Optional[dict]:
        conn = self._connections.get(device_id)
        if not conn or not conn.connected:
            logger.warning(f"Device {device_id} not connected")
            return None

        packet = F8881Protocol.build_packet(conn.sn, conn.password, cmd, data)
        try:
            conn.writer.write(packet)
            await conn.writer.drain()
            response = await asyncio.wait_for(conn.reader.read(4096), timeout=timeout)
            return F8881Protocol.parse_response(response)
        except asyncio.TimeoutError:
            logger.warning(f"Command timeout for device {device_id}")
            return None
        except Exception as e:
            logger.error(f"Command error for device {device_id}: {e}")
            conn.connected = False
            return None

    async def ping(self, device_id: str) -> bool:
        conn = self._connections.get(device_id)
        if not conn:
            return False
        try:
            reader, writer = await asyncio.wait_for(
                asyncio.open_connection(conn.ip, conn.tcp_port), timeout=3.0
            )
            writer.close()
            await writer.wait_closed()
            conn.last_heartbeat = datetime.utcnow()
            return True
        except Exception:
            return False

    async def read_sn(self, device_id: str) -> Optional[str]:
        result = await self._send_command(device_id, F8881Protocol.CMD_READ_SN)
        if result and result.get("valid"):
            return result.get("sn")
        return None

    async def open_door(self, device_id: str, door_number: int = 1) -> bool:
        door_data = struct.pack("BB", door_number, 1)
        result = await self._send_command(device_id, F8881Protocol.CMD_OPEN_DOOR, door_data)
        return result is not None and result.get("valid", False)

    async def close_door(self, device_id: str, door_number: int = 1) -> bool:
        door_data = struct.pack("BB", door_number, 1)
        result = await self._send_command(device_id, F8881Protocol.CMD_CLOSE_DOOR, door_data)
        return result is not None and result.get("valid", False)

    async def hold_door(self, device_id: str, door_number: int = 1) -> bool:
        door_data = struct.pack("BB", door_number, 1)
        result = await self._send_command(device_id, F8881Protocol.CMD_HOLD_DOOR, door_data)
        return result is not None and result.get("valid", False)

    async def begin_watch(self, device_id: str) -> bool:
        result = await self._send_command(device_id, F8881Protocol.CMD_BEGIN_WATCH)
        return result is not None and result.get("valid", False)

    async def close_watch(self, device_id: str) -> bool:
        result = await self._send_command(device_id, F8881Protocol.CMD_CLOSE_WATCH)
        return result is not None and result.get("valid", False)

    async def add_person_with_face(self, device_id: str, user_code: int,
                                    name: str, face_image: bytes) -> bool:
        name_bytes = name.encode("utf-8")[:32].ljust(32, b"\x00")
        code_bytes = struct.pack(">I", user_code)
        img_len = struct.pack(">I", len(face_image))
        data = code_bytes + name_bytes + img_len + face_image
        result = await self._send_command(
            device_id, F8881Protocol.CMD_ADD_PERSON, data, timeout=15.0
        )
        return result is not None and result.get("valid", False)

    async def delete_person(self, device_id: str, user_codes: list[int]) -> bool:
        count = struct.pack(">H", len(user_codes))
        codes = b"".join(struct.pack(">I", c) for c in user_codes)
        data = count + codes
        result = await self._send_command(device_id, F8881Protocol.CMD_DELETE_PERSON, data)
        return result is not None and result.get("valid", False)

    async def clear_all_faces(self, device_id: str) -> bool:
        result = await self._send_command(device_id, F8881Protocol.CMD_CLEAR_ALL_PERSONS)
        return result is not None and result.get("valid", False)

    async def push_face_to_all(self, exclude_device: str, user_code: int,
                                name: str, face_image: bytes) -> dict[str, bool]:
        results = {}
        tasks = []
        for did, conn in self._connections.items():
            if did != exclude_device and conn.connected:
                tasks.append((did, self.add_person_with_face(did, user_code, name, face_image)))
        for did, task in tasks:
            results[did] = await task
        return results

    def get_connection(self, device_id: str) -> Optional[DeviceConnection]:
        return self._connections.get(device_id)

    def list_connections(self) -> dict[str, DeviceConnection]:
        return dict(self._connections)


# Global singleton instance
f8881_service = F8881Service()
