@echo off
REM =============================================================================
REM EventShield Pro — Setup Script (Windows)
REM =============================================================================

echo.
echo ==========================================
echo   EventShield Pro — Setup
echo ==========================================
echo.

REM Check Python
python --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: Python 3 is required. Install from https://python.org
    exit /b 1
)

REM Backend setup
echo [1/4] Setting up Python virtual environment...
cd /d "%~dp0\..\backend"

if not exist "venv" (
    python -m venv venv
)

call venv\Scripts\activate.bat
echo   Virtual environment activated

echo [2/4] Installing Python dependencies...
pip install --upgrade pip -q
pip install -r requirements.txt -q
echo   Dependencies installed

REM Create .env
if not exist ".env" (
    copy .env.example .env
    echo   Created .env from template
)

echo [3/4] Initializing database...
python -c "import asyncio; from app.database import init_db; asyncio.run(init_db()); print('  Database initialized')"

echo [4/4] Setting up React frontend...
cd /d "%~dp0\..\frontend"
call npm install
echo   Frontend dependencies installed

echo.
echo ==========================================
echo   Setup Complete!
echo ==========================================
echo.
echo   1. Edit backend\.env with your credentials
echo   2. Run: scripts\start.bat
echo   3. Open: http://localhost:8000
echo.
pause
