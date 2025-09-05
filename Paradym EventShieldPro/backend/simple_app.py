#!/usr/bin/env python3
"""
EventShield Pro - Simple Flask Backend with Ticket Expiration
Quick working version for preview
"""
from flask import Flask, jsonify, request
from flask_cors import CORS
import json
from datetime import datetime, timedelta
import time

app = Flask(__name__)
CORS(app)

# Mock data for preview
mock_events = [
    {
        "id": 1,
        "name": "Tech Conference 2024",
        "start_datetime": "2024-12-15T09:00:00",
        "end_datetime": "2024-12-15T18:00:00",
        "location": "Convention Center",
        "current_registrations": 150,
        "max_capacity": 200,
        "ticket_price": 129.99,
        "status": "Active"
    },
    {
        "id": 2,
        "name": "Workshop Series",
        "start_datetime": "2024-12-16T10:00:00",
        "end_datetime": "2024-12-16T16:00:00",
        "location": "Training Room A",
        "current_registrations": 25,
        "max_capacity": 30,
        "ticket_price": 49.99,
        "status": "Active"
    }
]

mock_tickets = [
    { 
        "id": 1, 
        "number": "TC2024-001", 
        "guest": "Marcus Johnson", 
        "type": "Weekend Pass", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "$129.99", 
        "session": "AM/PM", 
        "access": "Full Event",
        "purchaseDate": "2024-12-13T09:00:00",
        "deviceAccess": "Active",
        "expirationNote": ""
    },
    { 
        "id": 2, 
        "number": "TC2024-002", 
        "guest": "Priya Patel", 
        "type": "Daily Pass", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "$49.99", 
        "session": "AM", 
        "access": "Standard",
        "purchaseDate": "2024-12-14T14:30:00",
        "deviceAccess": "Active",
        "expirationNote": ""
    },
    { 
        "id": 3, 
        "number": "TC2024-003", 
        "guest": "Carlos Rodriguez", 
        "type": "Coach Pass 🆓", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "FREE", 
        "session": "AM/PM", 
        "access": "VIP",
        "purchaseDate": "2024-12-13T08:00:00",
        "deviceAccess": "Active",
        "expirationNote": ""
    },
    { 
        "id": 4, 
        "number": "TC2024-004", 
        "guest": "Alice Brown", 
        "type": "Staff Pass 🆓", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "FREE", 
        "session": "24/7", 
        "access": "Full Access",
        "purchaseDate": "2024-12-10T07:00:00",
        "deviceAccess": "Active",
        "expirationNote": ""
    },
    # Test tickets to demonstrate expiration system
    { 
        "id": 5, 
        "number": "TC2024-005", 
        "guest": "Test Daily User", 
        "type": "Daily Pass", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "$49.99", 
        "session": "AM", 
        "access": "Standard",
        "purchaseDate": (datetime.now() - timedelta(days=1)).isoformat(), # Yesterday
        "deviceAccess": "Active",
        "expirationNote": ""
    },
    { 
        "id": 6, 
        "number": "TC2024-006", 
        "guest": "Test Weekend User", 
        "type": "Weekend Pass", 
        "status": "Active", 
        "expires": "Dec 15, 2024", 
        "price": "$129.99", 
        "session": "AM/PM", 
        "access": "Full Event",
        "purchaseDate": (datetime.now() - timedelta(days=3)).isoformat(), # 3 days ago
        "deviceAccess": "Active",
        "expirationNote": ""
    }
]

mock_guests = [
    { "id": 1, "name": "Marcus Johnson", "email": "marcus@email.com", "phone": "+1 (555) 123-4567", "status": "Registered", "ticket_id": 1 },
    { "id": 2, "name": "Priya Patel", "email": "priya@email.com", "phone": "+1 (555) 234-5678", "status": "Registered", "ticket_id": 2 },
    { "id": 3, "name": "Carlos Rodriguez", "email": "carlos@email.com", "phone": "+1 (555) 345-6789", "status": "Registered", "ticket_id": 3 },
    { "id": 4, "name": "Alice Brown", "email": "alice@email.com", "phone": "+1 (555) 456-7890", "status": "Registered", "ticket_id": 4 }
]

mock_blacklist = [
    { "id": 1, "name": "Tyrone Washington", "email": "tyrone@email.com", "phone": "+1 (555) 999-0000", "reason": "Security Violation", "date": "Dec 12, 2024", "status": "Active", "risk": "High" },
    { "id": 2, "name": "Mei Chen", "email": "mei@email.com", "phone": "+1 (555) 888-1111", "reason": "Previous Incident", "date": "Dec 10, 2024", "status": "Active", "risk": "Medium" },
    { "id": 3, "name": "Ahmed Hassan", "email": "ahmed@email.com", "phone": "+1 (555) 777-2222", "reason": "Unauthorized Access", "date": "Dec 8, 2024", "status": "Active", "risk": "High" },
    { "id": 4, "name": "Isabella Santos", "email": "isabella@email.com", "phone": "+1 (555) 666-3333", "reason": "Policy Violation", "date": "Dec 5, 2024", "status": "Inactive", "risk": "Low" }
]

# Ticket expiration service
class TicketExpirationService:
    def __init__(self):
        self.expiration_check_interval = None
        self.device_removal_queue = []
        self.is_running = False

    def is_ticket_expired(self, ticket):
        """Check if a ticket is expired based on its type and purchase date"""
        ticket_type = ticket["type"]
        purchase_date = datetime.fromisoformat(ticket["purchaseDate"])
        current_date = datetime.now()
        
        if "Daily" in ticket_type:
            # Daily tickets expire at end of purchase day (11:59 PM)
            end_of_day = purchase_date.replace(hour=23, minute=59, second=59, microsecond=999999)
            return current_date > end_of_day
        
        elif "Weekend" in ticket_type or "Coach" in ticket_type:
            # Weekend and coach passes expire at end of Sunday
            end_of_week = purchase_date
            # Find the next Sunday
            while end_of_week.weekday() != 6:  # Sunday is 6
                end_of_week += timedelta(days=1)
            end_of_week = end_of_week.replace(hour=23, minute=59, second=59, microsecond=999999)
            return current_date > end_of_week
        
        elif "Staff" in ticket_type:
            # Staff passes expire at end of event
            event_end_date = datetime(2024, 12, 15, 23, 59, 59)
            return current_date > event_end_date
        
        return False

    def get_ticket_expiration_date(self, ticket):
        """Get expiration date for a ticket"""
        ticket_type = ticket["type"]
        purchase_date = datetime.fromisoformat(ticket["purchaseDate"])
        
        if "Daily" in ticket_type:
            end_of_day = purchase_date.replace(hour=23, minute=59, second=59, microsecond=999999)
            return end_of_day
        
        elif "Weekend" in ticket_type or "Coach" in ticket_type:
            end_of_week = purchase_date
            while end_of_week.weekday() != 6:  # Sunday is 6
                end_of_week += timedelta(days=1)
            end_of_week = end_of_week.replace(hour=23, minute=59, second=59, microsecond=999999)
            return end_of_week
        
        elif "Staff" in ticket_type:
            return datetime(2024, 12, 15, 23, 59, 59)
        
        return None

    def should_remove_from_device(self, ticket):
        """Check if ticket should be removed from device"""
        if "Staff" in ticket["type"]:
            return False  # Staff passes don't get removed
        
        is_expired = self.is_ticket_expired(ticket)
        return is_expired

    def get_expiration_status(self, ticket):
        """Get expiration status for display"""
        is_expired = self.is_ticket_expired(ticket)
        expiration_date = self.get_ticket_expiration_date(ticket)
        
        time_until_expiration = ""
        if expiration_date and not is_expired:
            time_diff = expiration_date - datetime.now()
            hours = int(time_diff.total_seconds() // 3600)
            minutes = int((time_diff.total_seconds() % 3600) // 60)
            
            if hours > 24:
                days = hours // 24
                time_until_expiration = f"{days} day{'s' if days > 1 else ''}"
            elif hours > 0:
                time_until_expiration = f"{hours}h {minutes}m"
            else:
                time_until_expiration = f"{minutes}m"
        
        status = "Expired" if is_expired else "Active"
        device_access = "Removed" if (is_expired and self.should_remove_from_device(ticket)) else "Active"
        
        return {
            "isExpired": is_expired,
            "expirationDate": expiration_date.isoformat() if expiration_date else None,
            "timeUntilExpiration": time_until_expiration,
            "status": status,
            "deviceAccess": device_access
        }

    def check_all_tickets_expiration(self):
        """Check all tickets for expiration"""
        current_date = datetime.now()
        print(f"Checking ticket expiration at {current_date.strftime('%Y-%m-%d %H:%M:%S')}")
        
        for ticket in mock_tickets:
            if self.should_remove_from_device(ticket):
                expiration_date = self.get_ticket_expiration_date(ticket)
                reason = f"Expired on {expiration_date.strftime('%Y-%m-%d')}"
                
                # Queue for device removal
                self.device_removal_queue.append({
                    "ticketId": str(ticket["id"]), 
                    "reason": reason
                })
                
                # Update ticket status
                ticket["status"] = "Expired"
                ticket["deviceAccess"] = "Removed"
                ticket["expirationNote"] = reason
                
                print(f"Ticket {ticket['id']} expired and queued for device removal: {reason}")
        
        # Process removal queue
        self.process_device_removal_queue()

    def process_device_removal_queue(self):
        """Process device removal queue"""
        if not self.device_removal_queue:
            return
        
        print(f"Processing {len(self.device_removal_queue)} tickets for device removal...")
        
        for item in self.device_removal_queue:
            try:
                self.remove_ticket_from_device(item["ticketId"], item["reason"])
            except Exception as error:
                print(f"Failed to remove ticket {item['ticketId']} from device: {error}")
        
        self.device_removal_queue = []

    def remove_ticket_from_device(self, ticket_id, reason):
        """Remove ticket from device (simulated)"""
        print(f"Removing ticket {ticket_id} from device: {reason}")
        time.sleep(0.1)  # Simulate processing time
        print(f"Successfully removed ticket {ticket_id} from device")

    def start_expiration_checking(self):
        """Start automatic expiration checking"""
        if self.is_running:
            return
        
        self.is_running = True
        print("Ticket expiration checking started")
        
        # Simulate background checking
        def background_check():
            while self.is_running:
                self.check_all_tickets_expiration()
                time.sleep(30)  # Check every 30 seconds for demo
        
        import threading
        self.check_thread = threading.Thread(target=background_check, daemon=True)
        self.check_thread.start()

    def stop_expiration_checking(self):
        """Stop automatic expiration checking"""
        self.is_running = False
        print("Ticket expiration checking stopped")

# Initialize ticket expiration service
ticket_expiration_service = TicketExpirationService()

@app.route('/')
def home():
    return jsonify({
        "message": "EventShield Pro API with Ticket Expiration", 
        "version": "2.0.0", 
        "status": "running",
        "features": ["Ticket Expiration System", "Device Management", "Biometric Operations"]
    })

@app.route('/api/health')
def health():
    return jsonify({
        "status": "healthy", 
        "service": "EventShield Pro API", 
        "timestamp": datetime.now().isoformat(),
        "expiration_service": "running" if ticket_expiration_service.is_running else "stopped"
    })

@app.route('/api/events')
def get_events():
    return jsonify({"events": mock_events, "total": len(mock_events)})

@app.route('/api/tickets')
def get_tickets():
    return jsonify({"tickets": mock_tickets, "total": len(mock_tickets)})

@app.route('/api/guests')
def get_guests():
    return jsonify({"guests": mock_guests, "total": len(mock_guests)})

@app.route('/api/blacklist')
def get_blacklist():
    return jsonify({"blacklist": mock_blacklist, "total": len(mock_blacklist)})

@app.route('/api/dashboard/stats')
def dashboard_stats():
    expired_tickets = len([t for t in mock_tickets if ticket_expiration_service.is_ticket_expired(t)])
    
    return jsonify({
        "total_events": len(mock_events),
        "total_tickets": len(mock_tickets),
        "total_revenue": sum(float(e["ticket_price"]) * e["current_registrations"] for e in mock_events),
        "total_attendees": sum(e["current_registrations"] for e in mock_events),
        "events_this_month": len([e for e in mock_events if "12" in e["start_datetime"]]),
        "revenue_this_month": sum(float(e["ticket_price"]) * e["current_registrations"] for e in mock_events if "12" in e["start_datetime"]),
        "growth_rate": 15.5,
        "expired_tickets": expired_tickets
    })

@app.route('/api/tickets/expiration/check', methods=['POST'])
def check_ticket_expiration():
    """Manually check all tickets for expiration"""
    ticket_expiration_service.check_all_tickets_expiration()
    return jsonify({
        "success": True,
        "message": "Expiration check completed",
        "expired_count": len([t for t in mock_tickets if t["status"] == "Expired"])
    })

@app.route('/api/tickets/expiration/start', methods=['POST'])
def start_expiration_checking():
    """Start automatic expiration checking"""
    ticket_expiration_service.start_expiration_checking()
    return jsonify({
        "success": True,
        "message": "Expiration checking started"
    })

@app.route('/api/tickets/expiration/stop', methods=['POST'])
def stop_expiration_checking():
    """Stop automatic expiration checking"""
    ticket_expiration_service.stop_expiration_checking()
    return jsonify({
        "success": True,
        "message": "Expiration checking stopped"
    })

@app.route('/api/tickets/expiration/status')
def get_expiration_status():
    """Get expiration status for all tickets"""
    statuses = {}
    for ticket in mock_tickets:
        statuses[ticket["id"]] = ticket_expiration_service.get_expiration_status(ticket)
    
    return jsonify({
        "success": True,
        "statuses": statuses,
        "service_running": ticket_expiration_service.is_running
    })

if __name__ == '__main__':
    print("🚀 EventShield Pro v2.0 with Ticket Expiration Starting...")
    print("📍 API available at: http://localhost:5001")
    print("🌐 Frontend should connect to: http://localhost:3000")
    print("🎫 Ticket Expiration System: Active")
    print("⏰ Auto-checking: Starting in background")
    
    # Start expiration checking
    ticket_expiration_service.start_expiration_checking()
    
    try:
        app.run(debug=True, host='0.0.0.0', port=5001)
    except KeyboardInterrupt:
        print("\n🛑 EventShield Pro stopped by user")
        ticket_expiration_service.stop_expiration_checking()
    except Exception as e:
        print(f"❌ EventShield Pro failed to start: {e}")
        ticket_expiration_service.stop_expiration_checking()
