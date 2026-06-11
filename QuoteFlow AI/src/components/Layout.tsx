import React from 'react';
import { Link, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Layout: React.FC = () => {
  const { user, logout } = useAuth();
  const location = useLocation();

  const navItems = [
    { name: 'Dashboard', path: '/' },
    { name: 'Quotes', path: '/quotes' },
    { name: 'Customers', path: '/customers' },
  ];

  return (
    <div className="min-h-screen bg-gray-50 flex">
      <aside className="w-64 bg-white shadow-lg">
        <div className="p-6 bg-gradient-main">
          <h1 className="text-white text-xl font-bold">QuoteFlow AI</h1>
          <p className="text-white/80 text-sm mt-1">Automated Follow-ups</p>
        </div>
        <nav className="mt-6">
          {navItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              className={`block px-6 py-3 text-sm font-medium transition ${
                location.pathname === item.path
                  ? 'bg-royalblue/10 text-royalblue border-r-4 border-royalblue'
                  : 'text-gray-600 hover:bg-gray-50'
              }`}
            >
              {item.name}
            </Link>
          ))}
        </nav>
      </aside>
      <div className="flex-1 flex flex-col">
        <header className="bg-white shadow-sm px-6 py-4 flex justify-between items-center">
          <h2 className="text-lg font-semibold text-gray-800">
            Welcome, {user?.firstName}
          </h2>
          <button
            onClick={logout}
            className="text-sm text-gray-600 hover:text-royalblue"
          >
            Logout
          </button>
        </header>
        <main className="flex-1 p-6 overflow-auto">
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default Layout;
