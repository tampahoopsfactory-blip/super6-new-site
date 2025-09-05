#!/usr/bin/env python3
"""
EventShield Pro - DS-F8881 Biometric API Endpoints
Flask API for biometric operations and management
"""

from flask import Flask, jsonify, request, Blueprint, Response
from flask_cors import CORS
import json
import logging
import base64
import cv2
import numpy as np
from typing import Dict, List, Any, Optional
from datetime import datetime
import io
from PIL import Image

# Import our biometric operations
from biometric_operations import (
    BiometricManager,
    BiometricData,
    PersonRecord,
    RecognitionResult,
    CaptureSource,
    BiometricType,
    CaptureQuality
)

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Create Flask blueprint for biometric API
biometric_api = Blueprint('biometric_api', __name__, url_prefix='/api/biometric')

# Global biometric manager instance
biometric_manager = BiometricManager()

# CORS setup
CORS(biometric_api)

# Error handling
class BiometricAPIError(Exception):
    """Custom exception for biometric API errors"""
    def __init__(self, message: str, status_code: int = 400):
        self.message = message
        self.status_code = status_code
        super().__init__(self.message)

@biometric_api.errorhandler(BiometricAPIError)
def handle_biometric_api_error(error):
    """Handle biometric API errors"""
    return jsonify({
        'error': True,
        'message': error.message,
        'timestamp': datetime.now().isoformat()
    }), error.status_code

@biometric_api.errorhandler(Exception)
def handle_generic_error(error):
    """Handle generic errors"""
    logger.error(f"Unexpected error: {error}")
    return jsonify({
        'error': True,
        'message': 'Internal server error',
        'timestamp': datetime.now().isoformat()
    }), 500

# Utility functions
def image_to_base64(image: np.ndarray) -> str:
    """Convert numpy image to base64 string"""
    try:
        # Convert BGR to RGB
        rgb_image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
        
        # Convert to PIL Image
        pil_image = Image.fromarray(rgb_image)
        
        # Convert to base64
        buffer = io.BytesIO()
        pil_image.save(buffer, format='JPEG')
        img_str = base64.b64encode(buffer.getvalue()).decode()
        
        return img_str
    except Exception as e:
        logger.error(f"Failed to convert image to base64: {e}")
        return ""

def base64_to_image(base64_str: str) -> Optional[np.ndarray]:
    """Convert base64 string to numpy image"""
    try:
        # Decode base64
        img_data = base64.b64decode(base64_str)
        
        # Convert to numpy array
        nparr = np.frombuffer(img_data, np.uint8)
        
        # Decode image
        image = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
        return image
    except Exception as e:
        logger.error(f"Failed to convert base64 to image: {e}")
        return None

def biometric_data_to_dict(bio_data: BiometricData) -> Dict[str, Any]:
    """Convert biometric data to dictionary for JSON response"""
    return {
        'id': bio_data.id,
        'type': bio_data.type.value,
        'hash': bio_data.hash,
        'quality_score': bio_data.quality_score,
        'quality_level': bio_data.quality_level.value,
        'capture_source': bio_data.capture_source.value,
        'timestamp': bio_data.timestamp.isoformat(),
        'metadata': bio_data.metadata
    }

def person_to_dict(person: PersonRecord) -> Dict[str, Any]:
    """Convert person record to dictionary for JSON response"""
    return {
        'id': person.id,
        'name': person.name,
        'email': person.email,
        'phone': person.phone,
        'created_at': person.created_at.isoformat(),
        'updated_at': person.updated_at.isoformat(),
        'is_active': person.is_active,
        'face_data': biometric_data_to_dict(person.face_data) if person.face_data else None,
        'palm_data': biometric_data_to_dict(person.palm_data) if person.palm_data else None,
        'fingerprint_data': biometric_data_to_dict(person.fingerprint_data) if person.fingerprint_data else None,
        'metadata': person.metadata
    }

def recognition_result_to_dict(result: RecognitionResult) -> Dict[str, Any]:
    """Convert recognition result to dictionary for JSON response"""
    return {
        'person_id': result.person_id,
        'confidence': result.confidence,
        'match_type': result.match_type.value,
        'timestamp': result.timestamp.isoformat(),
        'device_id': result.device_id,
        'location': result.location,
        'metadata': result.metadata
    }

# Person Management Endpoints

@biometric_api.route('/persons', methods=['GET'])
def get_all_persons():
    """Get all persons in the system"""
    try:
        # Get search query
        query = request.args.get('query', '')
        limit = int(request.args.get('limit', 50))
        
        # Search persons
        persons = biometric_manager.search_persons(query, limit)
        
        # Convert to dictionaries
        person_list = [person_to_dict(person) for person in persons]
        
        return jsonify({
            'success': True,
            'persons': person_list,
            'total': len(person_list),
            'query': query,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting persons: {e}")
        raise BiometricAPIError(f"Failed to get persons: {str(e)}")

@biometric_api.route('/persons', methods=['POST'])
def add_person():
    """Add a new person to the system"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Validate required fields
        if 'name' not in data:
            raise BiometricAPIError("Name is required")
        
        # Add person
        person_id = biometric_manager.add_person(
            name=data['name'],
            email=data.get('email', ''),
            phone=data.get('phone', '')
        )
        
        if person_id:
            # Get the added person
            person = biometric_manager.get_person(person_id)
            
            return jsonify({
                'success': True,
                'message': 'Person added successfully',
                'person_id': person_id,
                'person': person_to_dict(person) if person else None,
                'timestamp': datetime.now().isoformat()
            }), 201
        else:
            raise BiometricAPIError("Failed to add person")
    
    except Exception as e:
        logger.error(f"Error adding person: {e}")
        raise BiometricAPIError(f"Failed to add person: {str(e)}")

@biometric_api.route('/persons/<person_id>', methods=['GET'])
def get_person(person_id: str):
    """Get person by ID"""
    try:
        person = biometric_manager.get_person(person_id)
        if not person:
            raise BiometricAPIError("Person not found", 404)
        
        return jsonify({
            'success': True,
            'person': person_to_dict(person),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting person {person_id}: {e}")
        raise BiometricAPIError(f"Failed to get person: {str(e)}")

@biometric_api.route('/persons/<person_id>', methods=['PUT'])
def update_person(person_id: str):
    """Update person record"""
    try:
        person = biometric_manager.get_person(person_id)
        if not person:
            raise BiometricAPIError("Person not found", 404)
        
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Update person fields
        if 'name' in data:
            person.name = data['name']
        if 'email' in data:
            person.email = data['email']
        if 'phone' in data:
            person.phone = data['phone']
        if 'is_active' in data:
            person.is_active = data['is_active']
        if 'metadata' in data:
            person.metadata.update(data['metadata'])
        
        person.updated_at = datetime.now()
        
        # Update in database
        if biometric_manager.database.update_person(person):
            return jsonify({
                'success': True,
                'message': 'Person updated successfully',
                'person': person_to_dict(person),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to update person")
    
    except Exception as e:
        logger.error(f"Error updating person {person_id}: {e}")
        raise BiometricAPIError(f"Failed to update person: {str(e)}")

@biometric_api.route('/persons/<person_id>', methods=['DELETE'])
def delete_person(person_id: str):
    """Delete person (soft delete by setting inactive)"""
    try:
        person = biometric_manager.get_person(person_id)
        if not person:
            raise BiometricAPIError("Person not found", 404)
        
        # Soft delete
        person.is_active = False
        person.updated_at = datetime.now()
        
        if biometric_manager.database.update_person(person):
            return jsonify({
                'success': True,
                'message': 'Person deleted successfully',
                'person_id': person_id,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to delete person")
    
    except Exception as e:
        logger.error(f"Error deleting person {person_id}: {e}")
        raise BiometricAPIError(f"Failed to delete person: {str(e)}")

# Biometric Capture Endpoints

@biometric_api.route('/capture/face', methods=['POST'])
def capture_face():
    """Capture face biometric data"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Get capture source
        source_str = data.get('source', 'webcam')
        try:
            source = CaptureSource(source_str)
        except ValueError:
            raise BiometricAPIError(f"Invalid capture source: {source_str}")
        
        # Capture face
        face_data = biometric_manager.capture_face(source)
        
        if face_data:
            return jsonify({
                'success': True,
                'message': 'Face captured successfully',
                'face_data': biometric_data_to_dict(face_data),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to capture face")
    
    except Exception as e:
        logger.error(f"Error capturing face: {e}")
        raise BiometricAPIError(f"Failed to capture face: {str(e)}")

@biometric_api.route('/capture/palm', methods=['POST'])
def capture_palm():
    """Capture palm biometric data"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Get capture source
        source_str = data.get('source', 'webcam')
        try:
            source = CaptureSource(source_str)
        except ValueError:
            raise BiometricAPIError(f"Invalid capture source: {source_str}")
        
        # Capture palm
        palm_data = biometric_manager.capture_palm(source)
        
        if palm_data:
            return jsonify({
                'success': True,
                'message': 'Palm captured successfully',
                'palm_data': biometric_data_to_dict(palm_data),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to capture palm")
    
    except Exception as e:
        logger.error(f"Error capturing palm: {e}")
        raise BiometricAPIError(f"Failed to capture palm: {str(e)}")

@biometric_api.route('/capture/image', methods=['POST'])
def capture_from_image():
    """Capture biometric data from uploaded image"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Get image data and type
        if 'image' not in data:
            raise BiometricAPIError("Image data is required")
        
        if 'type' not in data:
            raise BiometricAPIError("Biometric type is required")
        
        # Parse biometric type
        try:
            bio_type = BiometricType(data['type'])
        except ValueError:
            raise BiometricAPIError(f"Invalid biometric type: {data['type']}")
        
        # Convert base64 to image
        image = base64_to_image(data['image'])
        if image is None:
            raise BiometricAPIError("Invalid image data")
        
        # Process image based on type
        if bio_type == BiometricType.FACE:
            bio_data = biometric_manager.capture.process_face_image(image)
        elif bio_type == BiometricType.PALM:
            bio_data = biometric_manager.capture.process_palm_image(image)
        else:
            raise BiometricAPIError(f"Unsupported biometric type: {bio_type.value}")
        
        if bio_data:
            return jsonify({
                'success': True,
                'message': f'{bio_type.value.title()} processed successfully',
                'biometric_data': biometric_data_to_dict(bio_data),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError(f"Failed to process {bio_type.value}")
    
    except Exception as e:
        logger.error(f"Error processing image: {e}")
        raise BiometricAPIError(f"Failed to process image: {str(e)}")

# Biometric Data Management Endpoints

@biometric_api.route('/persons/<person_id>/biometric/<bio_type>', methods=['POST'])
def add_biometric_data(person_id: str, bio_type: str):
    """Add biometric data for a person"""
    try:
        # Validate person exists
        person = biometric_manager.get_person(person_id)
        if not person:
            raise BiometricAPIError("Person not found", 404)
        
        # Parse biometric type
        try:
            bio_type_enum = BiometricType(bio_type)
        except ValueError:
            raise BiometricAPIError(f"Invalid biometric type: {bio_type}")
        
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No biometric data provided")
        
        # Create biometric data object
        bio_data = BiometricData(
            id=data.get('id', f"{bio_type}_{int(datetime.now().timestamp())}"),
            type=bio_type_enum,
            data=base64.b64decode(data['data']) if 'data' in data else b'',
            hash=data.get('hash', ''),
            quality_score=data.get('quality_score', 0.0),
            quality_level=CaptureQuality(data.get('quality_level', 'fair')),
            capture_source=CaptureSource(data.get('capture_source', 'webcam')),
            timestamp=datetime.fromisoformat(data.get('timestamp', datetime.now().isoformat())),
            metadata=data.get('metadata', {})
        )
        
        # Add to database
        if biometric_manager.add_biometric_data(person_id, bio_data):
            return jsonify({
                'success': True,
                'message': f'{bio_type.title()} data added successfully',
                'person_id': person_id,
                'biometric_data': biometric_data_to_dict(bio_data),
                'timestamp': datetime.now().isoformat()
            }), 201
        else:
            raise BiometricAPIError(f"Failed to add {bio_type} data")
    
    except Exception as e:
        logger.error(f"Error adding biometric data: {e}")
        raise BiometricAPIError(f"Failed to add biometric data: {str(e)}")

@biometric_api.route('/persons/<person_id>/biometric/<bio_type>', methods=['GET'])
def get_biometric_data(person_id: str, bio_type: str):
    """Get biometric data for a person"""
    try:
        # Validate person exists
        person = biometric_manager.get_person(person_id)
        if not person:
            raise BiometricAPIError("Person not found", 404)
        
        # Parse biometric type
        try:
            bio_type_enum = BiometricType(bio_type)
        except ValueError:
            raise BiometricAPIError(f"Invalid biometric type: {bio_type}")
        
        # Get biometric data
        bio_data = None
        if bio_type_enum == BiometricType.FACE:
            bio_data = person.face_data
        elif bio_type_enum == BiometricType.PALM:
            bio_data = person.palm_data
        elif bio_type_enum == BiometricType.FINGERPRINT:
            bio_data = person.fingerprint_data
        
        if bio_data:
            return jsonify({
                'success': True,
                'person_id': person_id,
                'biometric_type': bio_type,
                'biometric_data': biometric_data_to_dict(bio_data),
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError(f"No {bio_type} data found for person")
    
    except Exception as e:
        logger.error(f"Error getting biometric data: {e}")
        raise BiometricAPIError(f"Failed to get biometric data: {str(e)}")

# Recognition Endpoints

@biometric_api.route('/recognize/face', methods=['POST'])
def recognize_face():
    """Recognize face from image"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No image data provided")
        
        # Get image data
        if 'image' not in data:
            raise BiometricAPIError("Image data is required")
        
        # Convert base64 to image
        image = base64_to_image(data['image'])
        if image is None:
            raise BiometricAPIError("Invalid image data")
        
        # Get threshold
        threshold = data.get('threshold', 0.6)
        
        # Perform recognition
        result = biometric_manager.recognize_face(image, threshold)
        
        if result:
            return jsonify({
                'success': True,
                'message': 'Face recognized successfully',
                'recognition_result': recognition_result_to_dict(result),
                'timestamp': datetime.now().isoformat()
            })
        else:
            return jsonify({
                'success': False,
                'message': 'Face not recognized',
                'timestamp': datetime.now().isoformat()
            })
    
    except Exception as e:
        logger.error(f"Error recognizing face: {e}")
        raise BiometricAPIError(f"Failed to recognize face: {str(e)}")

@biometric_api.route('/recognize/match', methods=['POST'])
def match_biometric_data():
    """Match biometric data against database"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No data provided")
        
        # Get biometric data
        if 'biometric_data' not in data:
            raise BiometricAPIError("Biometric data is required")
        
        bio_data_dict = data['biometric_data']
        
        # Create biometric data object
        bio_data = BiometricData(
            id=bio_data_dict.get('id', f"match_{int(datetime.now().timestamp())}"),
            type=BiometricType(bio_data_dict['type']),
            data=base64.b64decode(bio_data_dict['data']) if 'data' in bio_data_dict else b'',
            hash=bio_data_dict.get('hash', ''),
            quality_score=bio_data_dict.get('quality_score', 0.0),
            quality_level=CaptureQuality(bio_data_dict.get('quality_level', 'fair')),
            capture_source=CaptureSource(bio_data_dict.get('capture_source', 'webcam')),
            timestamp=datetime.fromisoformat(bio_data_dict.get('timestamp', datetime.now().isoformat())),
            metadata=bio_data_dict.get('metadata', {})
        )
        
        # Perform matching
        matches = biometric_manager.recognition.match_biometric_data(bio_data, bio_data.type)
        
        # Get person details for matches
        match_results = []
        for person_id, similarity in matches:
            person = biometric_manager.get_person(person_id)
            if person:
                match_results.append({
                    'person': person_to_dict(person),
                    'similarity': similarity
                })
        
        return jsonify({
            'success': True,
            'message': f'Found {len(match_results)} matches',
            'matches': match_results,
            'total_matches': len(match_results),
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error matching biometric data: {e}")
        raise BiometricAPIError(f"Failed to match biometric data: {str(e)}")

# Quality Assessment Endpoints

@biometric_api.route('/quality/assess', methods=['POST'])
def assess_image_quality():
    """Assess image quality for biometric processing"""
    try:
        data = request.get_json()
        if not data:
            raise BiometricAPIError("No image data provided")
        
        # Get image data
        if 'image' not in data:
            raise BiometricAPIError("Image data is required")
        
        # Convert base64 to image
        image = base64_to_image(data['image'])
        if image is None:
            raise BiometricAPIError("Invalid image data")
        
        # Assess quality
        quality_assessment = biometric_manager.get_quality_assessment(image)
        
        return jsonify({
            'success': True,
            'quality_assessment': quality_assessment,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error assessing image quality: {e}")
        raise BiometricAPIError(f"Failed to assess image quality: {str(e)}")

# Webcam Stream Endpoints

@biometric_api.route('/webcam/start', methods=['POST'])
def start_webcam_stream():
    """Start webcam video stream"""
    try:
        data = request.get_json() or {}
        
        # Start webcam stream
        success = biometric_manager.start_webcam_stream(lambda frame: None)  # No callback for now
        
        if success:
            return jsonify({
                'success': True,
                'message': 'Webcam stream started',
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to start webcam stream")
    
    except Exception as e:
        logger.error(f"Error starting webcam stream: {e}")
        raise BiometricAPIError(f"Failed to start webcam stream: {str(e)}")

@biometric_api.route('/webcam/stop', methods=['POST'])
def stop_webcam_stream():
    """Stop webcam video stream"""
    try:
        biometric_manager.stop_webcam_stream()
        
        return jsonify({
            'success': True,
            'message': 'Webcam stream stopped',
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error stopping webcam stream: {e}")
        raise BiometricAPIError(f"Failed to stop webcam stream: {str(e)}")

@biometric_api.route('/webcam/frame', methods=['GET'])
def get_webcam_frame():
    """Get current webcam frame"""
    try:
        # Capture current frame
        frame = biometric_manager.capture.capture_image(CaptureSource.WEBCAM)
        
        if frame is not None:
            # Convert to base64
            frame_base64 = image_to_base64(frame)
            
            return jsonify({
                'success': True,
                'frame': frame_base64,
                'timestamp': datetime.now().isoformat()
            })
        else:
            raise BiometricAPIError("Failed to capture webcam frame")
    
    except Exception as e:
        logger.error(f"Error getting webcam frame: {e}")
        raise BiometricAPIError(f"Failed to get webcam frame: {str(e)}")

# Statistics and Reports Endpoints

@biometric_api.route('/stats/overview', methods=['GET'])
def get_biometric_stats():
    """Get biometric system statistics"""
    try:
        # Get basic counts
        all_persons = biometric_manager.search_persons("", limit=10000)
        
        stats = {
            'total_persons': len(all_persons),
            'active_persons': len([p for p in all_persons if p.is_active]),
            'persons_with_face': len([p for p in all_persons if p.face_data]),
            'persons_with_palm': len([p for p in all_persons if p.palm_data]),
            'persons_with_fingerprint': len([p for p in all_persons if p.fingerprint_data]),
            'total_biometric_records': 0,
            'quality_distribution': {
                'excellent': 0,
                'good': 0,
                'fair': 0,
                'poor': 0
            }
        }
        
        # Count biometric records and quality distribution
        for person in all_persons:
            for bio_data in [person.face_data, person.palm_data, person.fingerprint_data]:
                if bio_data:
                    stats['total_biometric_records'] += 1
                    quality = bio_data.quality_level.value
                    if quality in stats['quality_distribution']:
                        stats['quality_distribution'][quality] += 1
        
        return jsonify({
            'success': True,
            'statistics': stats,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting biometric stats: {e}")
        raise BiometricAPIError(f"Failed to get biometric stats: {str(e)}")

@biometric_api.route('/stats/quality', methods=['GET'])
def get_quality_stats():
    """Get quality statistics for biometric data"""
    try:
        # Get all persons
        all_persons = biometric_manager.search_persons("", limit=10000)
        
        quality_stats = {
            'face': {'excellent': 0, 'good': 0, 'fair': 0, 'poor': 0},
            'palm': {'excellent': 0, 'good': 0, 'fair': 0, 'poor': 0},
            'fingerprint': {'excellent': 0, 'good': 0, 'fair': 0, 'poor': 0}
        }
        
        # Count quality distribution by type
        for person in all_persons:
            for bio_type, bio_data in [
                ('face', person.face_data),
                ('palm', person.palm_data),
                ('fingerprint', person.fingerprint_data)
            ]:
                if bio_data:
                    quality = bio_data.quality_level.value
                    if quality in quality_stats[bio_type]:
                        quality_stats[bio_type][quality] += 1
        
        return jsonify({
            'success': True,
            'quality_statistics': quality_stats,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Error getting quality stats: {e}")
        raise BiometricAPIError(f"Failed to get quality stats: {str(e)}")

# Health Check Endpoint

@biometric_api.route('/health', methods=['GET'])
def biometric_api_health():
    """Health check for biometric API"""
    try:
        # Check if biometric manager is initialized
        is_healthy = biometric_manager.is_initialized
        
        return jsonify({
            'success': True,
            'status': 'healthy' if is_healthy else 'unhealthy',
            'service': 'DS-F8881 Biometric API',
            'initialized': is_healthy,
            'timestamp': datetime.now().isoformat()
        })
    
    except Exception as e:
        logger.error(f"Health check failed: {e}")
        return jsonify({
            'success': False,
            'status': 'unhealthy',
            'service': 'DS-F8881 Biometric API',
            'error': str(e),
            'timestamp': datetime.now().isoformat()
        }), 500

# Export the blueprint
__all__ = ['biometric_api']

