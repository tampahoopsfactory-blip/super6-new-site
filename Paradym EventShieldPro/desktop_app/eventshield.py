import sys
from PyQt5.QtWidgets import QApplication, QMainWindow, QLabel, QVBoxLayout, QWidget, QLineEdit, QPushButton, QComboBox, QHBoxLayout
from PyQt5.QtCore import Qt

class EventShieldPro(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle('🛡️ EventShield Pro - Complete System')
        self.setGeometry(100, 100, 900, 600)
        self.setStyleSheet("background-color: #1e1e1e; color: #ffffff;")
        
        widget = QWidget()
        self.setCentralWidget(widget)
        layout = QVBoxLayout(widget)
        
        # Header
        header = QLabel('🛡️ EventShield Pro - Access Control System')
        header.setAlignment(Qt.AlignCenter)
        header.setStyleSheet("font-size: 24px; font-weight: bold; color: #00aa00; margin: 20px;")
        layout.addWidget(header)
        
        # Status
        status = QLabel('✅ FCLASS V3.21 Replacement Active - Ready for Operation')
        status.setAlignment(Qt.AlignCenter)
        status.setStyleSheet("font-size: 14px; color: #00aa00; margin: 10px;")
        layout.addWidget(status)
        
        # Guest Registration
        layout.addWidget(QLabel('👤 Guest Registration:'))
        self.name_input = QLineEdit()
        self.name_input.setPlaceholderText('Enter guest full name')
        self.name_input.setStyleSheet("background-color: #2d2d2d; color: #ffffff; border: 2px solid #404040; border-radius: 5px; padding: 8px;")
        layout.addWidget(self.name_input)
        
        # Ticket Type
        layout.addWidget(QLabel('🎫 Ticket Type:'))
        self.ticket_combo = QComboBox()
        self.ticket_combo.addItems(['Daily Ticket (Saturday Midnight)', 'Weekend Ticket (Sunday Midnight)', 'Coach Pass', 'Staff Pass'])
        self.ticket_combo.setStyleSheet("background-color: #2d2d2d; color: #ffffff; border: 2px solid #404040; border-radius: 5px; padding: 5px;")
        layout.addWidget(self.ticket_combo)
        
        # Buttons
        button_layout = QHBoxLayout()
        
        capture_btn = QPushButton('📷 Capture Face')
        capture_btn.setStyleSheet("background-color: #2d2d2d; color: #ffffff; border: 2px solid #404040; border-radius: 5px; padding: 10px;")
        button_layout.addWidget(capture_btn)
        
        register_btn = QPushButton('✅ Register Guest')
        register_btn.setStyleSheet("background-color: #1a4a1a; color: #ffffff; border: 2px solid #00aa00; border-radius: 5px; padding: 10px;")
        button_layout.addWidget(register_btn)
        
        layout.addLayout(button_layout)
        
        # Device Status
        device_status = QLabel('🔌 DS-F881 Status: Ready to Connect (TCP/IP Port 4370)')
        device_status.setStyleSheet("color: #ffaa00; margin: 20px; padding: 10px; background-color: #2a2a1a; border-radius: 5px;")
        layout.addWidget(device_status)

if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = EventShieldPro()
    window.show()
    sys.exit(app.exec_())
