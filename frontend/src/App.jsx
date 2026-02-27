import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Layout from './components/Layout';
import Login from './pages/Login';
import LiveMonitor from './pages/LiveMonitor';
import Tickets from './pages/Tickets';
import AccessLogPage from './pages/AccessLog';
import Events from './pages/Events';
import Devices from './pages/Devices';
import Settings from './pages/Settings';

function PrivateRoute({ children }) {
  const token = localStorage.getItem('token');
  return token ? children : <Navigate to="/login" />;
}

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/*"
          element={
            <PrivateRoute>
              <Layout />
            </PrivateRoute>
          }
        >
          <Route index element={<LiveMonitor />} />
          <Route path="tickets" element={<Tickets />} />
          <Route path="access-log" element={<AccessLogPage />} />
          <Route path="events" element={<Events />} />
          <Route path="devices" element={<Devices />} />
          <Route path="settings" element={<Settings />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
