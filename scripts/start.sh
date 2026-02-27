#!/bin/bash
# =============================================================================
# EventShield Pro — Start Script
# Starts both the backend API server and frontend dev server.
# =============================================================================

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

echo ""
echo "=========================================="
echo "  EventShield Pro — Starting"
echo "=========================================="
echo ""

# Start backend
echo "Starting backend server..."
cd "$PROJECT_DIR/backend"
source venv/bin/activate 2>/dev/null || true

# Start backend in background
uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload &
BACKEND_PID=$!
echo "  Backend PID: $BACKEND_PID (http://localhost:8000)"

# Start frontend
echo "Starting frontend dev server..."
cd "$PROJECT_DIR/frontend"
npm run dev &
FRONTEND_PID=$!
echo "  Frontend PID: $FRONTEND_PID (http://localhost:3000)"

echo ""
echo "=========================================="
echo "  Servers Running"
echo "=========================================="
echo ""
echo "  API:       http://localhost:8000"
echo "  Dashboard: http://localhost:3000"
echo "  API Docs:  http://localhost:8000/docs"
echo ""
echo "  Press Ctrl+C to stop all servers"
echo ""

# Trap Ctrl+C to kill both processes
trap "echo ''; echo 'Stopping servers...'; kill $BACKEND_PID $FRONTEND_PID 2>/dev/null; exit 0" INT TERM

# Wait for processes
wait
