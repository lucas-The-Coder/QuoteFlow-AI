import React, { useEffect, useState } from 'react';
import api from '../api/client';
import { DashboardMetrics } from '../types';

const Dashboard: React.FC = () => {
  const [metrics, setMetrics] = useState<DashboardMetrics | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchMetrics = async () => {
      try {
        const res = await api.get('/dashboard/metrics');
        setMetrics(res.data);
      } catch (error) {
        console.error('Failed to fetch metrics, using demo data');
        setMetrics({ totalQuotes: 24, pendingFollowUps: 8, convertedQuotes: 7, conversionRate: 29.2 });
      } finally {
        setLoading(false);
      }
    };
    fetchMetrics();
  }, []);

  const cards = [
    { label: 'Total Quotes', value: metrics?.totalQuotes, color: 'from-royalblue to-cyan' },
    { label: 'Pending Follow-ups', value: metrics?.pendingFollowUps, color: 'from-amber-500 to-orange-500' },
    { label: 'Converted', value: metrics?.convertedQuotes, color: 'from-green-500 to-emerald-500' },
    { label: 'Conversion Rate', value: `${metrics?.conversionRate}%`, color: 'from-purple-500 to-pink-500' },
  ];

  if (loading) return <div>Loading dashboard...</div>;

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Dashboard</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        {cards.map((card) => (
          <div key={card.label} className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
            <div className={`w-12 h-12 rounded-lg bg-gradient-to-r ${card.color} mb-4`}></div>
            <p className="text-sm text-gray-600">{card.label}</p>
            <p className="text-2xl font-bold text-gray-900 mt-1">{card.value}</p>
          </div>
        ))}
      </div>
      <div className="bg-white rounded-xl shadow-sm p-6">
        <h2 className="text-lg font-semibold mb-4">Follow-up Schedule</h2>
        <p className="text-gray-600">Your automated follow-ups run on days 3, 7, 14, and 21. The worker service will handle WhatsApp, Email, and SMS.</p>
        <div className="mt-4 flex gap-2">
          <span className="px-3 py-1 bg-royalblue/10 text-royalblue rounded-full text-sm">Day 3</span>
          <span className="px-3 py-1 bg-royalblue/10 text-royalblue rounded-full text-sm">Day 7</span>
          <span className="px-3 py-1 bg-royalblue/10 text-royalblue rounded-full text-sm">Day 14</span>
          <span className="px-3 py-1 bg-royalblue/10 text-royalblue rounded-full text-sm">Day 21</span>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
