"""Entry point to run the EventShield Pro server."""

import uvicorn
from app.config import settings

if __name__ == "__main__":
    uvicorn.run(
        "app.main:app",
        host=settings.server_host,
        port=settings.server_port,
        reload=settings.app_env == "development",
        log_level="info",
    )
