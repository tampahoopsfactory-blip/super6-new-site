"""
EventShield Pro - Redis Service
Token blacklisting and caching via Redis
"""

import structlog

logger = structlog.get_logger()

# Fallback in-memory blacklist when Redis is unavailable
_memory_blacklist = set()


class RedisService:
    """Service class wrapping Redis operations for token management"""

    def __init__(self):
        self._client = None
        self._available = False
        self._try_connect()

    def _try_connect(self):
        """Attempt to connect to Redis; degrade gracefully if unavailable"""
        try:
            from flask import current_app
            redis_url = current_app.config.get('REDIS_URL')
            if not redis_url:
                logger.warning("REDIS_URL not configured; token blacklist stored in memory")
                return

            import redis
            self._client = redis.Redis.from_url(redis_url, decode_responses=True)
            self._client.ping()
            self._available = True
            logger.info("Redis connection established", url=redis_url)
        except Exception as e:
            logger.warning("Redis unavailable; using in-memory fallback", error=str(e))
            self._available = False

    # ------------------------------------------------------------------
    # Token blacklisting
    # ------------------------------------------------------------------

    def blacklist_token(self, jti: str, ttl_seconds: int = 3600) -> bool:
        """
        Add a JWT ID to the blacklist.
        Returns True on success, False on failure.
        """
        if self._available and self._client:
            try:
                self._client.setex(f"blacklist:{jti}", ttl_seconds, "1")
                logger.info("Token blacklisted in Redis", jti=jti)
                return True
            except Exception as e:
                logger.error("Redis blacklist write failed", error=str(e))

        # Fallback: memory (no TTL enforcement in the simple case)
        _memory_blacklist.add(jti)
        logger.info("Token blacklisted in memory", jti=jti)
        return True

    def is_token_blacklisted(self, jti: str) -> bool:
        """
        Check whether a JWT ID has been blacklisted.
        Returns False on any Redis error to avoid blocking legitimate traffic.
        """
        if self._available and self._client:
            try:
                result = self._client.exists(f"blacklist:{jti}")
                return bool(result)
            except Exception as e:
                logger.error("Redis blacklist read failed", error=str(e))
                return False

        return jti in _memory_blacklist

    # ------------------------------------------------------------------
    # Generic cache helpers (stub)
    # ------------------------------------------------------------------

    def get(self, key: str):
        if self._available and self._client:
            try:
                return self._client.get(key)
            except Exception:
                pass
        return None

    def set(self, key: str, value: str, ttl_seconds: int = 300) -> bool:
        if self._available and self._client:
            try:
                self._client.setex(key, ttl_seconds, value)
                return True
            except Exception:
                pass
        return False

    def delete(self, key: str) -> bool:
        if self._available and self._client:
            try:
                self._client.delete(key)
                return True
            except Exception:
                pass
        return False
