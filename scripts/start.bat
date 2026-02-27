@echo off
REM =============================================================================
REM EventShield Pro — Start Script (Windows)
REM =============================================================================

echo.
echo ==========================================
echo   EventShield Pro — Starting
echo ==========================================
echo.

cd /d "%~dp0\..\backend"
call venv\Scripts\activate.bat

echo Starting backend server...
start "EventShield Backend" cmd /c "uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload"

echo Starting frontend dev server...
cd /d "%~dp0\..\frontend"
start "EventShield Frontend" cmd /c "npm run dev"

echo.
echo ==========================================
echo   Servers Running
echo ==========================================
echo.
echo   API:       http://localhost:8000
echo   Dashboard: http://localhost:3000
echo   API Docs:  http://localhost:8000/docs
echo.
pause
