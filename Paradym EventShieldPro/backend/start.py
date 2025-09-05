import os
from app import create_app, db

# Use SQLite for development
os.environ['DATABASE_URL'] = 'sqlite:///eventshield.db'

app = create_app()

if __name__ == '__main__':
    with app.app_context():
        db.create_all()  # Create tables
    app.run(debug=True, host='0.0.0.0', port=5000)
