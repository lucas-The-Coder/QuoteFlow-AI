import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from './contexts/AuthContext';
import Login from './components/Login';
import Register from './components/Register';
import Dashboard from './components/Dashboard';
import Layout from './components/Layout';
import QuotesList from './components/QuotesList';
import CreateQuote from './components/CreateQuote';
import CustomersList from './components/CustomersList';

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { user, loading } = useAuth();
  if (loading) return <div className="flex items-center justify-center h-screen">Loading...</div>;
  return user ? <>{children}</> : <Navigate to="/login" />;
};

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
        <Route index element={<Dashboard />} />
        <Route path="quotes" element={<QuotesList />} />
        <Route path="quotes/new" element={<CreateQuote />} />
        <Route path="customers" element={<CustomersList />} />
      </Route>
    </Routes>
  );
}

export default App;
