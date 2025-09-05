import sys
from PyQt5.QtWidgets import QApplication, QMainWindow, QLabel, QVBoxLayout, QWidget
from PyQt5.QtCore import Qt

class EventShieldPro(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle('EventShield Pro - Full Version')
        self.setGeometry(100, 100, 800, 600)
        self.setStyleSheet("background-color: #2b2b2b; color: #00ff00;")
        
        widget = QWidget()
        self.setCentralWidget(widget)
        layout = QVBoxLayout(widget)
        
        label = QLabel("🛡️ EventShield Pro - Access Control System\n✅ FCLASS V3.21 Replacement Active!")
        label.setAlignment(Qt.AlignCenter)
        label.setStyleSheet("font-size: 20px; font-weight: bold; margin: 50px;")
        layout.addWidget(label)

if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = EventShieldPro()
    window.show()
    sys.exit(app.exec_())

