import sys
import os
from PyQt5.QtWidgets import (QApplication, QMainWindow, QVBoxLayout, 
                           QHBoxLayout, QWidget, QLabel, QLineEdit, 
                           QPushButton, QComboBox, QTextEdit, QFrame)
from PyQt5.QtCore import Qt, QTimer
from PyQt5.QtGui import QFont, QPalette, QColor
import sqlite3
from datetime import datetime, timedelta

class EventShieldPro(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initUI()
        
    def initUI(self):
        self.setWindowTitle("EventShield Pro - Ready!")
        self.setGeometry(100, 100, 600, 400)
        self.setStyleSheet("background-color: #2b2b2b; color: #00ff00;")
        
        widget = QWidget()
        self.setCentralWidget(widget)
        layout = QVBoxLayout(widget)
        
        label = QLabel("EventShield Pro - Access Control System")
        label.setAlignment(Qt.AlignCenter)
        label.setStyleSheet("font-size: 24px; font-weight: bold;")
        layout.addWidget(label)

if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = EventShieldPro()
    window.show()
    sys.exit(app.exec_())
