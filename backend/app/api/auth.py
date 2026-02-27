"""Authentication endpoints for admin dashboard login."""

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.models.admin_user import AdminUser
from app.utils.auth import verify_password, create_access_token, hash_password
from app.api.schemas import LoginRequest, TokenResponse
from app.config import settings

router = APIRouter(prefix="/api/auth", tags=["auth"])


@router.post("/login", response_model=TokenResponse)
async def login(body: LoginRequest, db: AsyncSession = Depends(get_db)):
    result = await db.execute(select(AdminUser).where(AdminUser.username == body.username))
    user = result.scalar_one_or_none()

    if not user:
        if body.username == settings.admin_username and body.password == settings.admin_password:
            user = AdminUser(
                username=settings.admin_username,
                password_hash=hash_password(settings.admin_password),
            )
            db.add(user)
            await db.commit()
        else:
            raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid credentials")

    if user and not verify_password(body.password, user.password_hash):
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid credentials")

    token = create_access_token({"sub": user.username, "user_id": user.user_id})
    return TokenResponse(access_token=token)


@router.post("/change-password")
async def change_password(
    old_password: str,
    new_password: str,
    admin=Depends(__import__("app.utils.auth", fromlist=["get_current_admin"]).get_current_admin),
    db: AsyncSession = Depends(get_db),
):
    result = await db.execute(select(AdminUser).where(AdminUser.username == admin["sub"]))
    user = result.scalar_one_or_none()
    if not user or not verify_password(old_password, user.password_hash):
        raise HTTPException(status_code=400, detail="Invalid current password")
    user.password_hash = hash_password(new_password)
    await db.commit()
    return {"message": "Password updated"}
