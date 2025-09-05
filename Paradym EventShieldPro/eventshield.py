import sys
import sqlite3
from PyQt5.QtWidgets import (
    QApplication, QWidget, QLabel, QLineEdit, QPushButton, QVBoxLayout,
    QHBoxLayout, QCheckBox, QMessageBox, QDateEdit, QTimeEdit
)
from PyQt5.QtCore import Qt, QDateTime, QTime

class EventShield(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("EventShield Pro — Checkbox Ticket Types")
        self.setGeometry(200, 120, 800, 520)
        self.setup_db()
        self.init_ui()

    # ---------------- DB ---------------- #
    def setup_db(self):
        self.conn = sqlite3.connect("eventshield_guests.db")
        cur = self.conn.cursor()
        cur.execute(
            '''CREATE TABLE IF NOT EXISTS guests (
                   id INTEGER PRIMARY KEY AUTOINCREMENT,
                   phone TEXT NOT NULL,
                   name TEXT,
                   email TEXT,
                   ticket_type TEXT,
                   expiration_date TEXT,
                   expiration_time TEXT
               )'''
        )
        self.conn.commit()

    # ---------------- UI ---------------- #
    def init_ui(self):
        v = QVBoxLayout(self)

        # Phone (required)
        row = QHBoxLayout()
        row.addWidget(QLabel("Phone (required):"))
        self.phone = QLineEdit()
        self.phone.setPlaceholderText("(123) 456-7890 x123")
        # mask for (###) ###-#### x### with blanks as _
        self.phone.setInputMask("(000) 000-0000 x999;_")
        row.addWidget(self.phone)
        v.addLayout(row)

        # Name, Email (optional)
        row = QHBoxLayout(); row.addWidget(QLabel("Name:")); self.name = QLineEdit(); self.name.setPlaceholderText("Full name (optional)"); row.addWidget(self.name); v.addLayout(row)
        row = QHBoxLayout(); row.addWidget(QLabel("Email:")); self.email = QLineEdit(); self.email.setPlaceholderText("Email (optional)"); row.addWidget(self.email); v.addLayout(row)

        # Ticket type — horizontal checkboxes (single-select)
        v.addWidget(QLabel("Ticket Type:"))
        self.ticket_checks = {
            "Daily": QCheckBox("Daily"),
            "Weekend": QCheckBox("Weekend"),
            "Coach Pass": QCheckBox("Coach Pass"),
            "Staff": QCheckBox("Staff"),
        }
        row = QHBoxLayout()
        for cb in self.ticket_checks.values():
            row.addWidget(cb)
        v.addLayout(row)

        # Auto-expiration label created BEFORE signals/initial check
        self.exp_label = QLabel("Auto Expiration: ")
        v.addWidget(self.exp_label)

        # Set default selection WITHOUT emitting signals, then connect
        self.ticket_checks["Daily"].blockSignals(True)
        self.ticket_checks["Daily"].setChecked(True)
        self.ticket_checks["Daily"].blockSignals(False)
        for cb in self.ticket_checks.values():
            cb.stateChanged.connect(self._ticket_changed)
        self._recalc_expiration()

        # Optional override widgets (placeholders)
        row = QHBoxLayout(); self.exp_date = QDateEdit(); self.exp_time = QTimeEdit(); row.addWidget(self.exp_date); row.addWidget(self.exp_time); v.addLayout(row)

        # Register button
        btn = QPushButton("Register Guest"); btn.clicked.connect(self._register); v.addWidget(btn)

    # ------------- Helpers ------------- #
    def _ticket_changed(self, _):
        # make them single-select
        snd = self.sender()
        for lbl, cb in self.ticket_checks.items():
            if cb is not snd:
                cb.blockSignals(True); cb.setChecked(False); cb.blockSignals(False)
        self._recalc_expiration()

    def _current_ticket(self) -> str:
        for lbl, cb in self.ticket_checks.items():
            if cb.isChecked():
                return lbl
        return "Daily"

    def _recalc_expiration(self):
        t = self._current_ticket(); now = QDateTime.currentDateTime()
        if t == "Daily":
            exp = QDateTime(now.date(), QTime(23, 59, 59))
        elif t == "Weekend":
            # Mon=1..Sun=7
            dow = now.date().dayOfWeek()
            days_to_sun = (7 - dow) % 7
            exp = QDateTime(now).addDays(days_to_sun); exp.setTime(QTime(23, 59, 59))
        elif t == "Coach Pass":
            exp = QDateTime(now).addDays(3)
        elif t == "Staff":
            exp = QDateTime(now).addYears(10)
        else:
            exp = QDateTime(now).addDays(1)
        self._last_exp = exp
        self.exp_label.setText(f"Auto Expiration: {exp.toString('yyyy-MM-dd HH:mm:ss')}")

    def _register(self):
        phone = self.phone.text().strip()
        if not phone:
            QMessageBox.warning(self, "Validation Error", "Phone number is required.")
            return
        name = self.name.text().strip(); email = self.email.text().strip(); ticket = self._current_ticket()
        exp_str = self._last_exp.toString("yyyy-MM-dd HH:mm:ss"); d, t = exp_str.split(" ")
        cur = self.conn.cursor()
        cur.execute(
            "INSERT INTO guests (phone, name, email, ticket_type, expiration_date, expiration_time) VALUES (?,?,?,?,?,?)",
            (phone, name, email, ticket, d, t),
        )
        self.conn.commit()
        QMessageBox.information(self, "Success", "Guest registered successfully!")
        # reset
        self.phone.clear(); self.name.clear(); self.email.clear()
        for cb in self.ticket_checks.values(): cb.setChecked(False)
        self.ticket_checks["Daily"].setChecked(True); self._recalc_expiration()

if __name__ == '__main__':
    app = QApplication(sys.argv)
    w = EventShield(); w.show()
    sys.exit(app.exec_())

