#!/bin/bash
# =============================================================================
# EventShield Pro — Setup Script (Linux/macOS)
# Run this once to install all dependencies and initialize the database.
# =============================================================================

set -e

echo ""
echo "=========================================="
echo "  EventShield Pro — Setup"
echo "=========================================="
echo ""

# Check Python
if ! command -v python3 &> /dev/null; then
    echo "ERROR: Python 3 is required. Install from https://python.org"
    exit 1
fi

PYTHON_VERSION=$(python3 -c 'import sys; print(f"{sys.version_info.major}.{sys.version_info.minor}")')
echo "  Python version: $PYTHON_VERSION"

# Check Node.js
if ! command -v node &> /dev/null; then
    echo "ERROR: Node.js is required. Install from https://nodejs.org"
    exit 1
fi

NODE_VERSION=$(node --version)
echo "  Node.js version: $NODE_VERSION"
echo ""

# Backend setup
echo "[1/4] Setting up Python virtual environment..."
cd "$(dirname "$0")/../backend"

if [ ! -d "venv" ]; then
    python3 -m venv venv
fi

source venv/bin/activate
echo "  Virtual environment activated"

echo "[2/4] Installing Python dependencies..."
pip install --upgrade pip -q
pip install -r requirements.txt -q
echo "  Dependencies installed"

# Create .env if not exists
if [ ! -f ".env" ]; then
    cp .env.example .env
    echo "  Created .env from template — EDIT THIS FILE WITH YOUR CREDENTIALS"
fi

# Initialize database
echo "[3/4] Initializing database..."
python3 -c "
import asyncio
from app.database import init_db
asyncio.run(init_db())
print('  Database initialized')
"

# Frontend setup
echo "[4/4] Setting up React frontend..."
cd ../frontend
npm install --silent 2>/dev/null || npm install
echo "  Frontend dependencies installed"

echo ""
echo "=========================================="
echo "  Setup Complete!"
echo "=========================================="
echo ""
echo "  Next steps:"
echo "  1. Edit backend/.env with your credentials"
echo "  2. Run: ./scripts/start.sh"
echo "  3. Open: http://localhost:8000"
echo ""
echo "  Default admin login:"
echo "    Username: admin"
echo "    Password: changeme123"
echo ""
