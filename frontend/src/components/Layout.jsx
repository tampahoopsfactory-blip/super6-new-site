import { NavLink, Outlet } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { api } from '../utils/api';
import {
  LayoutDashboard, Ticket, ScrollText, Calendar,
  Radio, Settings, LogOut, WifiOff
} from 'lucide-react';

const navItems = [
  { to: '/', icon: LayoutDashboard, label: 'Live Monitor' },
  { to: '/tickets', icon: Ticket, label: 'Tickets' },
  { to: '/access-log', icon: ScrollText, label: 'Access Log' },
  { to: '/events', icon: Calendar, label: 'Events' },
  { to: '/devices', icon: Radio, label: 'Devices' },
  { to: '/settings', icon: Settings, label: 'Settings' },
];

export default function Layout() {
  const [health, setHealth] = useState(null);

  useEffect(() => {
    const check = async () => {
      try { setHealth(await api.getHealth()); } catch {}
    };
    check();
    const id = setInterval(check, 30000);
    return () => clearInterval(id);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    window.location.href = '/login';
  };

  return (
    <div className="app-layout">
      <aside className="sidebar">
        <div className="sidebar-logo">
          <h1>EventShield</h1>
          <span>Access Control</span>
        </div>
        <nav className="sidebar-nav">
          {navItems.map(({ to, icon: Icon, label }) => (
            <NavLink
              key={to}
              to={to}
              end={to === '/'}
              className={({ isActive }) => isActive ? 'active' : ''}
            >
              <Icon size={18} />
              {label}
            </NavLink>
          ))}
          <a href="#" onClick={handleLogout} style={{ marginTop: 'auto' }}>
            <LogOut size={18} />
            Sign Out
          </a>
        </nav>
        <div className="sidebar-status">
          <div className="flex items-center gap-2">
            <span
              className="status-dot"
              style={{
                background: health?.status === 'ok' ? 'var(--color-success)' : 'var(--color-danger)',
              }}
            />
            Server {health?.status === 'ok' ? 'Online' : 'Offline'}
          </div>
        </div>
      </aside>

      <main className="main-content">
        {health && !health.internet && (
          <div className="offline-banner">
            <WifiOff size={14} />
            OFFLINE MODE — Cloud sync paused
          </div>
        )}
        <Outlet />
      </main>
    </div>
  );
}
