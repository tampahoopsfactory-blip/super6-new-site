#!/usr/bin/env python3
"""
EventShield Pro - Main Flask Application
Integrated with DS-F8881 Device Control and Biometric Operations
"""

from flask import Flask, jsonify, request, render_template
from flask_cors import CORS
import json
import logging
from datetime import datetime, timedelta
import os

# Import our custom APIs
from device_api import device_api
from biometric_api import biometric_api
from sms_routes import sms_bp
from google_voice_routes import google_voice_bp

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Create Flask app
app = Flask(__name__)

# CORS setup
CORS(app)

# Register blueprints
app.register_blueprint(device_api)
app.register_blueprint(biometric_api)
app.register_blueprint(sms_bp)
app.register_blueprint(google_voice_bp)

# Mock data for existing EventShield functionality
mock_events = [
    {
        "id": 1,
        "name": "Tech Conference 2024",
        "start_datetime": "2024-12-15T09:00:00",
        "end_datetime": "2024-12-15T17:00:00",
        "location": "Convention Center",
        "current_registrations": 150,
        "max_capacity": 200,
        "ticket_price": 299.99,
        "status": "active"
    },
    {
        "id": 2,
        "name": "Annual Gala Dinner",
        "start_datetime": "2024-12-20T18:00:00",
        "end_datetime": "2024-12-20T23:00:00",
        "location": "Grand Hotel",
        "current_registrations": 80,
        "max_capacity": 120,
        "ticket_price": 150.00,
        "status": "active"
    },
    {
        "id": 3,
        "name": "Workshop Series",
        "start_datetime": "2024-12-25T10:00:00",
        "end_datetime": "2024-12-25T16:00:00",
        "location": "Training Center",
        "current_registrations": 45,
        "max_capacity": 60,
        "ticket_price": 89.99,
        "status": "active"
    }
]

mock_tickets = [
    {
        "id": 1,
        "event_id": 1,
        "ticket_number": "TC2024-001",
        "attendee_name": "John Smith",
        "attendee_email": "john.smith@email.com",
        "attendee_phone": "555-0123",
        "ticket_type": "General Admission",
        "purchase_date": "2024-11-15T10:30:00",
        "status": "confirmed",
        "check_in_time": None
    },
    {
        "id": 2,
        "event_id": 1,
        "ticket_number": "TC2024-002",
        "attendee_name": "Sarah Johnson",
        "attendee_email": "sarah.j@email.com",
        "attendee_phone": "555-0124",
        "ticket_type": "VIP Pass",
        "purchase_date": "2024-11-16T14:20:00",
        "status": "confirmed",
        "check_in_time": None
    },
    {
        "id": 3,
        "event_id": 2,
        "ticket_number": "AGD2024-001",
        "attendee_name": "Michael Brown",
        "attendee_email": "michael.b@email.com",
        "attendee_phone": "555-0125",
        "ticket_type": "Standard",
        "purchase_date": "2024-11-17T09:15:00",
        "status": "confirmed",
        "check_in_time": None
    }
]

mock_guests = [
    {
        "id": 1,
        "name": "John Smith",
        "email": "john.smith@email.com",
        "phone": "555-0123",
        "company": "Tech Corp",
        "last_visit": "2024-11-15T10:30:00",
        "total_visits": 5,
        "status": "active"
    },
    {
        "id": 2,
        "name": "Sarah Johnson",
        "email": "sarah.j@email.com",
        "phone": "555-0124",
        "company": "Innovation Inc",
        "last_visit": "2024-11-16T14:20:00",
        "total_visits": 3,
        "status": "active"
    },
    {
        "id": 3,
        "name": "Michael Brown",
        "email": "michael.b@email.com",
        "phone": "555-0125",
        "company": "Future Systems",
        "last_visit": "2024-11-17T09:15:00",
        "total_visits": 7,
        "status": "active"
    }
]

mock_blacklist = [
    {
        "id": 1,
        "name": "Robert Wilson",
        "email": "robert.w@email.com",
        "phone": "555-9999",
        "reason": "Security violation",
        "blacklisted_date": "2024-10-15T16:00:00",
        "status": "active"
    },
    {
        "id": 2,
        "name": "Lisa Davis",
        "email": "lisa.d@email.com",
        "phone": "555-9998",
        "reason": "Policy violation",
        "blacklisted_date": "2024-10-20T11:30:00",
        "status": "active"
    }
]

# Basic routes for existing functionality
@app.route('/')
def home():
    """Home endpoint"""
    return jsonify({
        "message": "EventShield Pro API",
        "version": "2.0.0",
        "status": "running",
        "features": [
            "Event Management",
            "Ticket Management",
            "Guest Management",
            "DS-F8881 Device Control",
            "Biometric Operations",
            "Face Recognition",
            "Palm Scanning",
            "Access Control"
        ],
        "timestamp": datetime.now().isoformat()
    })

@app.route('/api/health')
def health():
    """Health check endpoint"""
    return jsonify({
        "status": "healthy",
        "service": "EventShield Pro API",
        "version": "2.0.0",
        "timestamp": datetime.now().isoformat(),
        "modules": {
            "device_api": "active",
            "biometric_api": "active",
            "core_api": "active"
        }
    })

@app.route('/api/events')
def get_events():
    """Get all events"""
    return jsonify({
        "events": mock_events,
        "total": len(mock_events),
        "timestamp": datetime.now().isoformat()
    })

@app.route('/api/tickets')
def get_tickets():
    """Get all tickets"""
    return jsonify({
        "tickets": mock_tickets,
        "total": len(mock_tickets),
        "timestamp": datetime.now().isoformat()
    })

@app.route('/api/guests')
def get_guests():
    """Get all guests"""
    return jsonify({
        "guests": mock_guests,
        "total": len(mock_guests),
        "timestamp": datetime.now().isoformat()
    })

@app.route('/api/blacklist')
def get_blacklist():
    """Get blacklist entries"""
    return jsonify({
        "blacklist": mock_blacklist,
        "total": len(mock_blacklist),
        "timestamp": datetime.now().isoformat()
    })

@app.route('/api/dashboard/stats')
def dashboard_stats():
    """Get dashboard statistics"""
    total_revenue = sum(e["ticket_price"] * e["current_registrations"] for e in mock_events)
    events_this_month = len([e for e in mock_events if "12" in e["start_datetime"]])
    revenue_this_month = sum(e["ticket_price"] * e["current_registrations"] for e in mock_events if "12" in e["start_datetime"])
    
    return jsonify({
        "total_events": len(mock_events),
        "total_tickets": len(mock_tickets),
        "total_revenue": total_revenue,
        "total_attendees": sum(e["current_registrations"] for e in mock_events),
        "events_this_month": events_this_month,
        "revenue_this_month": revenue_this_month,
        "growth_rate": 15.5,
        "timestamp": datetime.now().isoformat()
    })

# DS-F8881 Device Status Integration
@app.route('/api/dashboard/device-status')
def device_status_dashboard():
    """Get device status for dashboard"""
    try:
        # Import device manager to get status
        from device_api import device_manager
        
        # Get device status summary
        devices = device_manager.get_all_devices()
        device_status = device_manager.get_all_status()
        
        # Calculate summary
        total_devices = len(devices)
        connected_devices = len([d for d in device_status.values() if d.connection_status.value == 1])
        disconnected_devices = total_devices - connected_devices
        
        # Get recent activity (last 24 hours)
        recent_activity = []
        for device_id, status in device_status.items():
            if status.last_keepalive:
                last_keepalive = datetime.fromtimestamp(status.last_keepalive)
                if datetime.now() - last_keepalive < timedelta(hours=24):
                    recent_activity.append({
                        "device_id": device_id,
                        "last_activity": last_keepalive.isoformat(),
                        "status": status.connection_status.name
                    })
        
        return jsonify({
            "success": True,
            "device_summary": {
                "total_devices": total_devices,
                "connected_devices": connected_devices,
                "disconnected_devices": disconnected_devices,
                "connection_rate": (connected_devices / total_devices * 100) if total_devices > 0 else 0
            },
            "recent_activity": recent_activity,
            "timestamp": datetime.now().isoformat()
        })
        
    except Exception as e:
        logger.error(f"Error getting device status: {e}")
        return jsonify({
            "success": False,
            "error": str(e),
            "device_summary": {
                "total_devices": 0,
                "connected_devices": 0,
                "disconnected_devices": 0,
                "connection_rate": 0
            },
            "recent_activity": [],
            "timestamp": datetime.now().isoformat()
        })

# Biometric Status Integration
@app.route('/api/dashboard/biometric-status')
def biometric_status_dashboard():
    """Get biometric system status for dashboard"""
    try:
        # Import biometric manager to get status
        from biometric_api import biometric_manager
        
        # Get biometric statistics
        all_persons = biometric_manager.search_persons("", limit=10000)
        
        # Calculate statistics
        total_persons = len(all_persons)
        persons_with_face = len([p for p in all_persons if p.face_data])
        persons_with_palm = len([p for p in all_persons if p.palm_data])
        active_persons = len([p for p in all_persons if p.is_active])
        
        # Get recent biometric activity (last 24 hours)
        recent_activity = []
        for person in all_persons:
            for bio_data in [person.face_data, person.palm_data, person.fingerprint_data]:
                if bio_data and bio_data.timestamp:
                    if datetime.now() - bio_data.timestamp < timedelta(hours=24):
                        recent_activity.append({
                            "person_name": person.name,
                            "biometric_type": bio_data.type.value,
                            "capture_time": bio_data.timestamp.isoformat(),
                            "quality": bio_data.quality_level.value
                        })
        
        return jsonify({
            "success": True,
            "biometric_summary": {
                "total_persons": total_persons,
                "active_persons": active_persons,
                "persons_with_face": persons_with_face,
                "persons_with_palm": persons_with_palm,
                "face_coverage_rate": (persons_with_face / total_persons * 100) if total_persons > 0 else 0,
                "palm_coverage_rate": (persons_with_palm / total_persons * 100) if total_persons > 0 else 0
            },
            "recent_activity": recent_activity[:10],  # Limit to 10 most recent
            "timestamp": datetime.now().isoformat()
        })
        
    except Exception as e:
        logger.error(f"Error getting biometric status: {e}")
        return jsonify({
            "success": False,
            "error": str(e),
            "biometric_summary": {
                "total_persons": 0,
                "active_persons": 0,
                "persons_with_face": 0,
                "persons_with_palm": 0,
                "face_coverage_rate": 0,
                "palm_coverage_rate": 0
            },
            "recent_activity": [],
            "timestamp": datetime.now().isoformat()
        })

# System Information Endpoint
@app.route('/api/system/info')
def system_info():
    """Get system information"""
    try:
        # Get device count
        from device_api import device_manager
        device_count = len(device_manager.get_all_devices())
        
        # Get biometric count
        from biometric_api import biometric_manager
        person_count = len(biometric_manager.search_persons("", limit=10000))
        
        return jsonify({
            "success": True,
            "system_info": {
                "service_name": "EventShield Pro",
                "version": "2.0.0",
                "uptime": "running",
                "modules": {
                    "device_control": {
                        "status": "active",
                        "device_count": device_count,
                        "supported_devices": ["DS-F8881"]
                    },
                    "biometric_system": {
                        "status": "active",
                        "person_count": person_count,
                        "supported_types": ["face", "palm", "fingerprint"]
                    },
                    "event_management": {
                        "status": "active",
                        "event_count": len(mock_events),
                        "ticket_count": len(mock_tickets)
                    }
                },
                "api_endpoints": {
                    "device_control": "/api/devices/*",
                    "biometric_operations": "/api/biometric/*",
                    "event_management": "/api/events, /api/tickets, /api/guests",
                    "dashboard": "/api/dashboard/*"
                }
            },
            "timestamp": datetime.now().isoformat()
        })
        
    except Exception as e:
        logger.error(f"Error getting system info: {e}")
        return jsonify({
            "success": False,
            "error": str(e),
            "timestamp": datetime.now().isoformat()
        })

# Error handlers
@app.errorhandler(404)
def not_found(error):
    """Handle 404 errors"""
    return jsonify({
        "error": True,
        "message": "Endpoint not found",
        "status_code": 404,
        "timestamp": datetime.now().isoformat()
    }), 404

@app.errorhandler(500)
def internal_error(error):
    """Handle 500 errors"""
    return jsonify({
        "error": True,
        "message": "Internal server error",
        "status_code": 500,
        "timestamp": datetime.now().isoformat()
    }), 500

# Development and testing routes
@app.route('/api/test/device-connection')
def test_device_connection():
    """Test device connection functionality"""
    try:
        from device_api import device_manager
        
        # Try to discover devices
        devices = device_manager.get_all_devices()
        
        if devices:
            # Test connection to first device
            first_device_id = list(devices.keys())[0]
            device = device_manager.get_device(first_device_id)
            
            if device:
                # Try to read device SN
                sn = device.read_device_sn()
                
                return jsonify({
                    "success": True,
                    "message": "Device connection test completed",
                    "device_count": len(devices),
                    "test_device": {
                        "id": first_device_id,
                        "connected": device.is_connected(),
                        "serial_number": sn,
                        "status": device.get_status()
                    },
                    "timestamp": datetime.now().isoformat()
                })
            else:
                return jsonify({
                    "success": False,
                    "message": "No devices available for testing",
                    "timestamp": datetime.now().isoformat()
                })
        else:
            return jsonify({
                "success": False,
                "message": "No devices registered",
                "timestamp": datetime.now().isoformat()
            })
            
    except Exception as e:
        logger.error(f"Device connection test failed: {e}")
        return jsonify({
            "success": False,
            "error": str(e),
            "message": "Device connection test failed",
            "timestamp": datetime.now().isoformat()
        }), 500

@app.route('/api/test/biometric-capture')
def test_biometric_capture():
    """Test biometric capture functionality"""
    try:
        from biometric_api import biometric_manager
        
        # Test webcam availability
        webcam_available = biometric_manager.capture.webcam is not None
        
        # Test database connection
        test_person = biometric_manager.add_person("Test User", "test@example.com", "555-0000")
        
        if test_person:
            # Clean up test data
            biometric_manager.database.update_person(
                biometric_manager.get_person(test_person)
            )
            biometric_manager.get_person(test_person).is_active = False
            
            return jsonify({
                "success": True,
                "message": "Biometric system test completed",
                "webcam_available": webcam_available,
                "database_working": True,
                "test_person_created": test_person,
                "timestamp": datetime.now().isoformat()
            })
        else:
            return jsonify({
                "success": False,
                "message": "Failed to create test person",
                "webcam_available": webcam_available,
                "database_working": False,
                "timestamp": datetime.now().isoformat()
            })
            
    except Exception as e:
        logger.error(f"Biometric capture test failed: {e}")
        return jsonify({
            "success": False,
            "error": str(e),
            "message": "Biometric capture test failed",
            "timestamp": datetime.now().isoformat()
        }), 500

# Main application entry point
if __name__ == '__main__':
    print("🚀 EventShield Pro v2.0 Starting...")
    print("📍 API available at: http://localhost:5001")
    print("🌐 Frontend should connect to: http://localhost:3000")
    print("🔧 DS-F8881 Device Control: /api/devices/*")
    print("👤 Biometric Operations: /api/biometric/*")
    print("📊 Dashboard: /api/dashboard/*")
    print("📋 System Info: /api/system/info")
    print("🧪 Testing: /api/test/*")
    
    try:
        app.run(debug=True, host='0.0.0.0', port=5001)
    except KeyboardInterrupt:
        print("\n🛑 EventShield Pro stopped by user")
    except Exception as e:
        print(f"❌ EventShield Pro failed to start: {e}")

