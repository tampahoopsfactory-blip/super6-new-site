#!/usr/bin/env python3
"""
EventShield Pro - DS-F8881 Biometric Operations
Face recognition, palm scanning, and biometric data management
"""

import cv2
import numpy as np
import base64
import json
import time
import threading
from typing import Dict, List, Optional, Tuple, Any, Callable
from dataclasses import dataclass, field
from datetime import datetime
import logging
from enum import Enum
import hashlib
import sqlite3
import os

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

class BiometricType(Enum):
    """Types of biometric data"""
    FACE = "face"
    PALM = "palm"
    FINGERPRINT = "fingerprint"

class CaptureSource(Enum):
    """Biometric capture sources"""
    DEVICE_CAMERA = "device_camera"
    WEBCAM = "webcam"
    DEVICE_SCANNER = "device_scanner"
    TERMINAL_SCANNER = "terminal_scanner"

class CaptureQuality(Enum):
    """Biometric capture quality levels"""
    EXCELLENT = "excellent"
    GOOD = "good"
    FAIR = "fair"
    POOR = "poor"

@dataclass
class BiometricData:
    """Biometric data structure"""
    id: str
    type: BiometricType
    data: bytes
    hash: str
    quality_score: float
    quality_level: CaptureQuality
    capture_source: CaptureSource
    timestamp: datetime
    metadata: Dict[str, Any] = field(default_factory=dict)
    
    def __post_init__(self):
        if not self.hash:
            self.hash = self._calculate_hash()
    
    def _calculate_hash(self) -> str:
        """Calculate hash of biometric data"""
        return hashlib.sha256(self.data).hexdigest()

@dataclass
class PersonRecord:
    """Person record with biometric data"""
    id: str
    name: str
    email: str
    phone: str
    face_data: Optional[BiometricData] = None
    palm_data: Optional[BiometricData] = None
    fingerprint_data: Optional[BiometricData] = None
    created_at: datetime = field(default_factory=datetime.now)
    updated_at: datetime = field(default_factory=datetime.now)
    is_active: bool = True
    metadata: Dict[str, Any] = field(default_factory=dict)

@dataclass
class RecognitionResult:
    """Biometric recognition result"""
    person_id: str
    confidence: float
    match_type: BiometricType
    timestamp: datetime
    device_id: str
    location: str
    metadata: Dict[str, Any] = field(default_factory=dict)

class BiometricDatabase:
    """SQLite database for biometric data management"""
    
    def __init__(self, db_path: str = "biometric_data.db"):
        self.db_path = db_path
        self._init_database()
    
    def _init_database(self):
        """Initialize database tables"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                # Create persons table
                cursor.execute('''
                    CREATE TABLE IF NOT EXISTS persons (
                        id TEXT PRIMARY KEY,
                        name TEXT NOT NULL,
                        email TEXT,
                        phone TEXT,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        is_active BOOLEAN DEFAULT 1,
                        metadata TEXT
                    )
                ''')
                
                # Create biometric_data table
                cursor.execute('''
                    CREATE TABLE IF NOT EXISTS biometric_data (
                        id TEXT PRIMARY KEY,
                        person_id TEXT NOT NULL,
                        type TEXT NOT NULL,
                        data BLOB NOT NULL,
                        hash TEXT NOT NULL,
                        quality_score REAL,
                        quality_level TEXT,
                        capture_source TEXT,
                        timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        metadata TEXT,
                        FOREIGN KEY (person_id) REFERENCES persons (id)
                    )
                ''')
                
                # Create recognition_logs table
                cursor.execute('''
                    CREATE TABLE IF NOT EXISTS recognition_logs (
                        id TEXT PRIMARY KEY,
                        person_id TEXT NOT NULL,
                        confidence REAL NOT NULL,
                        match_type TEXT NOT NULL,
                        timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        device_id TEXT,
                        location TEXT,
                        metadata TEXT,
                        FOREIGN KEY (person_id) REFERENCES persons (id)
                    )
                ''')
                
                # Create indexes
                cursor.execute('CREATE INDEX IF NOT EXISTS idx_persons_email ON persons(email)')
                cursor.execute('CREATE INDEX IF NOT EXISTS idx_persons_phone ON persons(phone)')
                cursor.execute('CREATE INDEX IF NOT EXISTS idx_biometric_person_type ON biometric_data(person_id, type)')
                cursor.execute('CREATE INDEX IF NOT EXISTS idx_biometric_hash ON biometric_data(hash)')
                cursor.execute('CREATE INDEX IF NOT EXISTS idx_recognition_timestamp ON recognition_logs(timestamp)')
                
                conn.commit()
                logger.info("Biometric database initialized successfully")
                
        except Exception as e:
            logger.error(f"Failed to initialize database: {e}")
            raise
    
    def add_person(self, person: PersonRecord) -> bool:
        """Add a new person to the database"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    INSERT INTO persons (id, name, email, phone, created_at, updated_at, is_active, metadata)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                ''', (
                    person.id, person.name, person.email, person.phone,
                    person.created_at.isoformat(), person.updated_at.isoformat(),
                    person.is_active, json.dumps(person.metadata)
                ))
                
                conn.commit()
                logger.info(f"Person {person.name} added to database")
                return True
                
        except Exception as e:
            logger.error(f"Failed to add person: {e}")
            return False
    
    def update_person(self, person: PersonRecord) -> bool:
        """Update person record"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    UPDATE persons 
                    SET name = ?, email = ?, phone = ?, updated_at = ?, is_active = ?, metadata = ?
                    WHERE id = ?
                ''', (
                    person.name, person.email, person.phone,
                    datetime.now().isoformat(), person.is_active,
                    json.dumps(person.metadata), person.id
                ))
                
                conn.commit()
                logger.info(f"Person {person.name} updated in database")
                return True
                
        except Exception as e:
            logger.error(f"Failed to update person: {e}")
            return False
    
    def get_person(self, person_id: str) -> Optional[PersonRecord]:
        """Get person by ID"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    SELECT id, name, email, phone, created_at, updated_at, is_active, metadata
                    FROM persons WHERE id = ?
                ''', (person_id,))
                
                row = cursor.fetchone()
                if row:
                    person = PersonRecord(
                        id=row[0],
                        name=row[1],
                        email=row[2],
                        phone=row[3],
                        created_at=datetime.fromisoformat(row[4]),
                        updated_at=datetime.fromisoformat(row[5]),
                        is_active=bool(row[6]),
                        metadata=json.loads(row[7]) if row[7] else {}
                    )
                    
                    # Load biometric data
                    person.face_data = self._get_biometric_data(person_id, BiometricType.FACE)
                    person.palm_data = self._get_biometric_data(person_id, BiometricType.PALM)
                    person.fingerprint_data = self._get_biometric_data(person_id, BiometricType.FINGERPRINT)
                    
                    return person
                
                return None
                
        except Exception as e:
            logger.error(f"Failed to get person: {e}")
            return None
    
    def _get_biometric_data(self, person_id: str, bio_type: BiometricType) -> Optional[BiometricData]:
        """Get biometric data for a person"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    SELECT id, type, data, hash, quality_score, quality_level, capture_source, timestamp, metadata
                    FROM biometric_data 
                    WHERE person_id = ? AND type = ?
                ''', (person_id, bio_type.value))
                
                row = cursor.fetchone()
                if row:
                    return BiometricData(
                        id=row[0],
                        type=BiometricType(row[1]),
                        data=row[2],
                        hash=row[3],
                        quality_score=row[4] or 0.0,
                        quality_level=CaptureQuality(row[5]) if row[5] else CaptureQuality.FAIR,
                        capture_source=CaptureSource(row[6]) if row[6] else CaptureSource.DEVICE_CAMERA,
                        timestamp=datetime.fromisoformat(row[7]),
                        metadata=json.loads(row[8]) if row[8] else {}
                    )
                
                return None
                
        except Exception as e:
            logger.error(f"Failed to get biometric data: {e}")
            return None
    
    def add_biometric_data(self, person_id: str, bio_data: BiometricData) -> bool:
        """Add biometric data to database"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    INSERT OR REPLACE INTO biometric_data 
                    (id, person_id, type, data, hash, quality_score, quality_level, capture_source, timestamp, metadata)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                ''', (
                    bio_data.id, person_id, bio_data.type.value, bio_data.data,
                    bio_data.hash, bio_data.quality_score, bio_data.quality_level.value,
                    bio_data.capture_source.value, bio_data.timestamp.isoformat(),
                    json.dumps(bio_data.metadata)
                ))
                
                conn.commit()
                logger.info(f"Biometric data added for person {person_id}")
                return True
                
        except Exception as e:
            logger.error(f"Failed to add biometric data: {e}")
            return False
    
    def add_recognition_log(self, result: RecognitionResult) -> bool:
        """Add recognition log to database"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    INSERT INTO recognition_logs 
                    (id, person_id, confidence, match_type, timestamp, device_id, location, metadata)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                ''', (
                    result.person_id, result.person_id, result.confidence,
                    result.match_type.value, result.timestamp.isoformat(),
                    result.device_id, result.location, json.dumps(result.metadata)
                ))
                
                conn.commit()
                logger.info(f"Recognition log added for person {result.person_id}")
                return True
                
        except Exception as e:
            logger.error(f"Failed to add recognition log: {e}")
            return False
    
    def search_persons(self, query: str, limit: int = 10) -> List[PersonRecord]:
        """Search persons by name, email, or phone"""
        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()
                
                cursor.execute('''
                    SELECT id, name, email, phone, created_at, updated_at, is_active, metadata
                    FROM persons 
                    WHERE (name LIKE ? OR email LIKE ? OR phone LIKE ?) AND is_active = 1
                    LIMIT ?
                ''', (f'%{query}%', f'%{query}%', f'%{query}%', limit))
                
                persons = []
                for row in cursor.fetchall():
                    person = PersonRecord(
                        id=row[0],
                        name=row[1],
                        email=row[2],
                        phone=row[3],
                        created_at=datetime.fromisoformat(row[4]),
                        updated_at=datetime.fromisoformat(row[5]),
                        is_active=bool(row[6]),
                        metadata=json.loads(row[7]) if row[7] else {}
                    )
                    
                    # Load biometric data
                    person.face_data = self._get_biometric_data(person.id, BiometricType.FACE)
                    person.palm_data = self._get_biometric_data(person.id, BiometricType.PALM)
                    person.fingerprint_data = self._get_biometric_data(person.id, BiometricType.FINGERPRINT)
                    
                    persons.append(person)
                
                return persons
                
        except Exception as e:
            logger.error(f"Failed to search persons: {e}")
            return []


class BiometricCapture:
    """Biometric data capture and processing"""
    
    def __init__(self):
        self.webcam = None
        self.capture_callbacks = {}
        self.is_capturing = False
        self.capture_thread = None
        self._lock = threading.Lock()
    
    def start_webcam_capture(self, callback: Callable[[np.ndarray], None]):
        """Start webcam capture"""
        try:
            with self._lock:
                if self.webcam is None:
                    self.webcam = cv2.VideoCapture(0)
                    if not self.webcam.isOpened():
                        raise Exception("Failed to open webcam")
                
                self.capture_callbacks['webcam'] = callback
                self.is_capturing = True
                
                if not self.capture_thread:
                    self.capture_thread = threading.Thread(target=self._webcam_worker, daemon=True)
                    self.capture_thread.start()
                
                logger.info("Webcam capture started")
                return True
                
        except Exception as e:
            logger.error(f"Failed to start webcam capture: {e}")
            return False
    
    def stop_webcam_capture(self):
        """Stop webcam capture"""
        with self._lock:
            self.is_capturing = False
            self.capture_callbacks.pop('webcam', None)
            
            if self.webcam:
                self.webcam.release()
                self.webcam = None
            
            logger.info("Webcam capture stopped")
    
    def _webcam_worker(self):
        """Webcam capture worker thread"""
        while self.is_capturing:
            try:
                if self.webcam and self.webcam.isOpened():
                    ret, frame = self.webcam.read()
                    if ret:
                        # Notify callback
                        callback = self.capture_callbacks.get('webcam')
                        if callback:
                            callback(frame)
                    
                    # Small delay to prevent excessive CPU usage
                    time.sleep(0.033)  # ~30 FPS
                else:
                    break
                    
            except Exception as e:
                logger.error(f"Webcam capture error: {e}")
                break
        
        self.is_capturing = False
    
    def capture_image(self, source: CaptureSource) -> Optional[np.ndarray]:
        """Capture image from specified source"""
        try:
            if source == CaptureSource.WEBCAM:
                return self._capture_from_webcam()
            elif source == CaptureSource.DEVICE_CAMERA:
                return self._capture_from_device()
            else:
                logger.warning(f"Capture source {source} not implemented")
                return None
                
        except Exception as e:
            logger.error(f"Failed to capture image from {source}: {e}")
            return None
    
    def _capture_from_webcam(self) -> Optional[np.ndarray]:
        """Capture image from webcam"""
        try:
            if self.webcam is None:
                self.webcam = cv2.VideoCapture(0)
                if not self.webcam.isOpened():
                    return None
            
            ret, frame = self.webcam.read()
            if ret:
                return frame
            return None
            
        except Exception as e:
            logger.error(f"Webcam capture failed: {e}")
            return None
    
    def _capture_from_device(self) -> Optional[np.ndarray]:
        """Capture image from device camera (placeholder for device integration)"""
        # This would integrate with the DS-F8881 device camera
        logger.info("Device camera capture not yet implemented")
        return None
    
    def process_face_image(self, image: np.ndarray) -> Optional[BiometricData]:
        """Process face image and extract biometric data"""
        try:
            # Convert to grayscale for face detection
            gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
            
            # Load face detection model (you can use different models)
            face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')
            
            # Detect faces
            faces = face_cascade.detectMultiScale(gray, 1.1, 4)
            
            if len(faces) == 0:
                logger.warning("No faces detected in image")
                return None
            
            if len(faces) > 1:
                logger.warning("Multiple faces detected, using first one")
            
            # Get the first face
            x, y, w, h = faces[0]
            face_roi = gray[y:y+h, x:x+w]
            
            # Resize to standard size
            face_roi = cv2.resize(face_roi, (128, 128))
            
            # Calculate quality score (simple brightness and contrast check)
            quality_score = self._calculate_image_quality(face_roi)
            quality_level = self._get_quality_level(quality_score)
            
            # Convert to bytes
            _, buffer = cv2.imencode('.jpg', face_roi)
            image_bytes = buffer.tobytes()
            
            # Create biometric data
            bio_data = BiometricData(
                id=f"face_{int(time.time())}",
                type=BiometricType.FACE,
                data=image_bytes,
                hash="",
                quality_score=quality_score,
                quality_level=quality_level,
                capture_source=CaptureSource.WEBCAM,
                timestamp=datetime.now(),
                metadata={
                    'face_count': len(faces),
                    'face_bbox': faces[0].tolist(),
                    'image_size': face_roi.shape,
                    'processing_method': 'opencv_haar'
                }
            )
            
            logger.info(f"Face image processed, quality: {quality_level.value}")
            return bio_data
            
        except Exception as e:
            logger.error(f"Failed to process face image: {e}")
            return None
    
    def process_palm_image(self, image: np.ndarray) -> Optional[BiometricData]:
        """Process palm image and extract biometric data"""
        try:
            # Convert to grayscale
            gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
            
            # Apply preprocessing for palm detection
            # This is a simplified approach - in practice you'd use specialized palm detection
            blurred = cv2.GaussianBlur(gray, (5, 5), 0)
            
            # Edge detection
            edges = cv2.Canny(blurred, 50, 150)
            
            # Find contours
            contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
            
            if not contours:
                logger.warning("No palm contours detected")
                return None
            
            # Find the largest contour (assumed to be the palm)
            largest_contour = max(contours, key=cv2.contourArea)
            
            # Get bounding rectangle
            x, y, w, h = cv2.boundingRect(largest_contour)
            
            # Extract palm region
            palm_roi = gray[y:y+h, x:x+w]
            
            # Resize to standard size
            palm_roi = cv2.resize(palm_roi, (128, 128))
            
            # Calculate quality score
            quality_score = self._calculate_image_quality(palm_roi)
            quality_level = self._get_quality_level(quality_score)
            
            # Convert to bytes
            _, buffer = cv2.imencode('.jpg', palm_roi)
            image_bytes = buffer.tobytes()
            
            # Create biometric data
            bio_data = BiometricData(
                id=f"palm_{int(time.time())}",
                type=BiometricType.PALM,
                data=image_bytes,
                hash="",
                quality_score=quality_score,
                quality_level=quality_level,
                capture_source=CaptureSource.WEBCAM,
                timestamp=datetime.now(),
                metadata={
                    'contour_count': len(contours),
                    'palm_bbox': [x, y, w, h],
                    'image_size': palm_roi.shape,
                    'processing_method': 'opencv_contour'
                }
            )
            
            logger.info(f"Palm image processed, quality: {quality_level.value}")
            return bio_data
            
        except Exception as e:
            logger.error(f"Failed to process palm image: {e}")
            return None
    
    def _calculate_image_quality(self, image: np.ndarray) -> float:
        """Calculate image quality score (0.0 to 1.0)"""
        try:
            # Calculate brightness (0-255)
            brightness = np.mean(image)
            
            # Calculate contrast (standard deviation)
            contrast = np.std(image)
            
            # Calculate sharpness (Laplacian variance)
            laplacian = cv2.Laplacian(image, cv2.CV_64F)
            sharpness = laplacian.var()
            
            # Normalize scores
            brightness_score = min(brightness / 128.0, 1.0)  # Optimal around 128
            contrast_score = min(contrast / 50.0, 1.0)  # Optimal around 50
            sharpness_score = min(sharpness / 500.0, 1.0)  # Optimal around 500
            
            # Weighted average
            quality_score = (
                brightness_score * 0.3 +
                contrast_score * 0.3 +
                sharpness_score * 0.4
            )
            
            return max(0.0, min(1.0, quality_score))
            
        except Exception as e:
            logger.error(f"Failed to calculate image quality: {e}")
            return 0.5
    
    def _get_quality_level(self, quality_score: float) -> CaptureQuality:
        """Convert quality score to quality level"""
        if quality_score >= 0.8:
            return CaptureQuality.EXCELLENT
        elif quality_score >= 0.6:
            return CaptureQuality.GOOD
        elif quality_score >= 0.4:
            return CaptureQuality.FAIR
        else:
            return CaptureQuality.POOR


class BiometricRecognition:
    """Biometric recognition and matching"""
    
    def __init__(self, database: BiometricDatabase):
        self.database = database
        self.face_recognizer = None
        self.palm_recognizer = None
        self._init_recognizers()
    
    def _init_recognizers(self):
        """Initialize recognition models"""
        try:
            # Initialize face recognition (using LBPH for simplicity)
            self.face_recognizer = cv2.face.LBPHFaceRecognizer_create()
            
            # Initialize palm recognition (placeholder)
            self.palm_recognizer = None
            
            logger.info("Biometric recognizers initialized")
            
        except Exception as e:
            logger.error(f"Failed to initialize recognizers: {e}")
    
    def train_face_recognizer(self, training_data: List[Tuple[np.ndarray, int]]):
        """Train face recognition model"""
        try:
            if not self.face_recognizer:
                logger.error("Face recognizer not initialized")
                return False
            
            if not training_data:
                logger.warning("No training data provided")
                return False
            
            # Extract images and labels
            images, labels = zip(*training_data)
            
            # Train the model
            self.face_recognizer.train(images, np.array(labels))
            
            logger.info(f"Face recognizer trained with {len(training_data)} samples")
            return True
            
        except Exception as e:
            logger.error(f"Failed to train face recognizer: {e}")
            return False
    
    def recognize_face(self, face_image: np.ndarray, threshold: float = 0.6) -> Optional[RecognitionResult]:
        """Recognize face from image"""
        try:
            if not self.face_recognizer:
                logger.error("Face recognizer not trained")
                return None
            
            # Preprocess image
            processed_image = self._preprocess_face_image(face_image)
            
            # Perform recognition
            label, confidence = self.face_recognizer.predict(processed_image)
            
            # Convert confidence to similarity score (0-1)
            similarity = 1.0 - (confidence / 100.0)
            
            if similarity >= threshold:
                # Get person details
                person = self.database.get_person(str(label))
                if person:
                    result = RecognitionResult(
                        person_id=person.id,
                        confidence=similarity,
                        match_type=BiometricType.FACE,
                        timestamp=datetime.now(),
                        device_id="webcam",
                        location="terminal",
                        metadata={
                            'recognition_method': 'LBPH',
                            'confidence_score': confidence,
                            'similarity_score': similarity,
                            'threshold': threshold
                        }
                    )
                    
                    # Log recognition
                    self.database.add_recognition_log(result)
                    
                    logger.info(f"Face recognized: {person.name} (confidence: {similarity:.2f})")
                    return result
            
            logger.info(f"Face not recognized (confidence: {similarity:.2f})")
            return None
            
        except Exception as e:
            logger.error(f"Face recognition failed: {e}")
            return None
    
    def _preprocess_face_image(self, image: np.ndarray) -> np.ndarray:
        """Preprocess face image for recognition"""
        try:
            # Convert to grayscale
            if len(image.shape) == 3:
                gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
            else:
                gray = image
            
            # Resize to standard size
            gray = cv2.resize(gray, (128, 128))
            
            # Normalize
            gray = gray.astype(np.float32) / 255.0
            
            return gray
            
        except Exception as e:
            logger.error(f"Face image preprocessing failed: {e}")
            return image
    
    def match_biometric_data(self, input_data: BiometricData, bio_type: BiometricType) -> List[Tuple[str, float]]:
        """Match biometric data against database"""
        try:
            matches = []
            
            # Get all persons with this biometric type
            persons = self.database.search_persons("")  # Get all persons
            
            for person in persons:
                stored_data = None
                
                if bio_type == BiometricType.FACE:
                    stored_data = person.face_data
                elif bio_type == BiometricType.PALM:
                    stored_data = person.palm_data
                elif bio_type == BiometricType.FINGERPRINT:
                    stored_data = person.fingerprint_data
                
                if stored_data:
                    # Calculate similarity (simple hash comparison for now)
                    similarity = self._calculate_similarity(input_data, stored_data)
                    if similarity > 0.0:
                        matches.append((person.id, similarity))
            
            # Sort by similarity (highest first)
            matches.sort(key=lambda x: x[1], reverse=True)
            
            return matches
            
        except Exception as e:
            logger.error(f"Biometric matching failed: {e}")
            return []
    
    def _calculate_similarity(self, data1: BiometricData, data2: BiometricData) -> float:
        """Calculate similarity between two biometric data samples"""
        try:
            # For now, use hash comparison
            if data1.hash == data2.hash:
                return 1.0
            
            # Convert bytes to numpy arrays for image comparison
            if data1.type in [BiometricType.FACE, BiometricType.PALM]:
                # Decode images
                img1 = cv2.imdecode(np.frombuffer(data1.data, np.uint8), cv2.IMREAD_GRAYSCALE)
                img2 = cv2.imdecode(np.frombuffer(data2.data, np.uint8), cv2.IMREAD_GRAYSCALE)
                
                if img1 is not None and img2 is not None:
                    # Resize to same size
                    img1 = cv2.resize(img1, (128, 128))
                    img2 = cv2.resize(img2, (128, 128))
                    
                    # Calculate structural similarity
                    similarity = self._calculate_structural_similarity(img1, img2)
                    return similarity
            
            return 0.0
            
        except Exception as e:
            logger.error(f"Similarity calculation failed: {e}")
            return 0.0
    
    def _calculate_structural_similarity(self, img1: np.ndarray, img2: np.ndarray) -> float:
        """Calculate structural similarity between two images"""
        try:
            # Simple mean squared error based similarity
            mse = np.mean((img1.astype(np.float32) - img2.astype(np.float32)) ** 2)
            
            # Convert MSE to similarity (0-1)
            max_mse = 255 * 255  # Maximum possible MSE
            similarity = 1.0 - (mse / max_mse)
            
            return max(0.0, min(1.0, similarity))
            
        except Exception as e:
            logger.error(f"Structural similarity calculation failed: {e}")
            return 0.0


class BiometricManager:
    """Main biometric operations manager"""
    
    def __init__(self, db_path: str = "biometric_data.db"):
        self.database = BiometricDatabase(db_path)
        self.capture = BiometricCapture()
        self.recognition = BiometricRecognition(self.database)
        self.is_initialized = True
        
        logger.info("Biometric manager initialized")
    
    def capture_face(self, source: CaptureSource = CaptureSource.WEBCAM) -> Optional[BiometricData]:
        """Capture face biometric data"""
        try:
            # Capture image
            image = self.capture.capture_image(source)
            if image is None:
                logger.error("Failed to capture image")
                return None
            
            # Process face image
            face_data = self.capture.process_face_image(image)
            if face_data is None:
                logger.error("Failed to process face image")
                return None
            
            logger.info(f"Face captured successfully from {source.value}")
            return face_data
            
        except Exception as e:
            logger.error(f"Face capture failed: {e}")
            return None
    
    def capture_palm(self, source: CaptureSource = CaptureSource.WEBCAM) -> Optional[BiometricData]:
        """Capture palm biometric data"""
        try:
            # Capture image
            image = self.capture.capture_image(source)
            if image is None:
                logger.error("Failed to capture image")
                return None
            
            # Process palm image
            palm_data = self.capture.process_palm_image(image)
            if palm_data is None:
                logger.error("Failed to process palm image")
                return None
            
            logger.info(f"Palm captured successfully from {source.value}")
            return palm_data
            
        except Exception as e:
            logger.error(f"Palm capture failed: {e}")
            return None
    
    def recognize_face(self, face_image: np.ndarray) -> Optional[RecognitionResult]:
        """Recognize face from image"""
        try:
            result = self.recognition.recognize_face(face_image)
            return result
            
        except Exception as e:
            logger.error(f"Face recognition failed: {e}")
            return None
    
    def add_person(self, name: str, email: str = "", phone: str = "") -> Optional[str]:
        """Add a new person to the system"""
        try:
            person_id = f"person_{int(time.time())}"
            
            person = PersonRecord(
                id=person_id,
                name=name,
                email=email,
                phone=phone
            )
            
            if self.database.add_person(person):
                logger.info(f"Person {name} added successfully")
                return person_id
            else:
                logger.error(f"Failed to add person {name}")
                return None
                
        except Exception as e:
            logger.error(f"Failed to add person: {e}")
            return None
    
    def add_biometric_data(self, person_id: str, bio_data: BiometricData) -> bool:
        """Add biometric data for a person"""
        try:
            if self.database.add_biometric_data(person_id, bio_data):
                logger.info(f"Biometric data added for person {person_id}")
                return True
            else:
                logger.error(f"Failed to add biometric data for person {person_id}")
                return False
                
        except Exception as e:
            logger.error(f"Failed to add biometric data: {e}")
            return False
    
    def search_persons(self, query: str) -> List[PersonRecord]:
        """Search for persons"""
        try:
            return self.database.search_persons(query)
        except Exception as e:
            logger.error(f"Person search failed: {e}")
            return []
    
    def get_person(self, person_id: str) -> Optional[PersonRecord]:
        """Get person by ID"""
        try:
            return self.database.get_person(person_id)
        except Exception as e:
            logger.error(f"Failed to get person: {e}")
            return None
    
    def start_webcam_stream(self, callback: Callable[[np.ndarray], None]):
        """Start webcam video stream"""
        try:
            return self.capture.start_webcam_capture(callback)
        except Exception as e:
            logger.error(f"Failed to start webcam stream: {e}")
            return False
    
    def stop_webcam_stream(self):
        """Stop webcam video stream"""
        try:
            self.capture.stop_webcam_capture()
        except Exception as e:
            logger.error(f"Failed to stop webcam stream: {e}")
    
    def get_quality_assessment(self, image: np.ndarray) -> Dict[str, Any]:
        """Get quality assessment of an image"""
        try:
            quality_score = self.capture._calculate_image_quality(image)
            quality_level = self.capture._get_quality_level(quality_score)
            
            return {
                'quality_score': quality_score,
                'quality_level': quality_level.value,
                'recommendation': self._get_quality_recommendation(quality_level)
            }
            
        except Exception as e:
            logger.error(f"Quality assessment failed: {e}")
            return {
                'quality_score': 0.0,
                'quality_level': 'unknown',
                'recommendation': 'Unable to assess quality'
            }
    
    def _get_quality_recommendation(self, quality_level: CaptureQuality) -> str:
        """Get recommendation based on quality level"""
        recommendations = {
            CaptureQuality.EXCELLENT: "Image quality is excellent. Ready for biometric processing.",
            CaptureQuality.GOOD: "Image quality is good. Suitable for biometric processing.",
            CaptureQuality.FAIR: "Image quality is fair. Consider recapturing for better results.",
            CaptureQuality.POOR: "Image quality is poor. Please recapture the image."
        }
        
        return recommendations.get(quality_level, "Quality assessment unavailable.")


# Example usage and testing
if __name__ == "__main__":
    # Initialize biometric manager
    manager = BiometricManager()
    
    try:
        # Add a test person
        person_id = manager.add_person("Test User", "test@example.com", "123-456-7890")
        
        if person_id:
            print(f"Test person added with ID: {person_id}")
            
            # Capture face
            print("Capturing face...")
            face_data = manager.capture_face()
            
            if face_data:
                print(f"Face captured, quality: {face_data.quality_level.value}")
                
                # Add biometric data
                if manager.add_biometric_data(person_id, face_data):
                    print("Face data added successfully")
                else:
                    print("Failed to add face data")
            else:
                print("Face capture failed")
        
        print("Biometric system test completed")
        
    except KeyboardInterrupt:
        print("\nInterrupted by user")
    
    except Exception as e:
        print(f"Test failed: {e}")
    
    finally:
        # Cleanup
        if 'manager' in locals():
            manager.stop_webcam_stream()

