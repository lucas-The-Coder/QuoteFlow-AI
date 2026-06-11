import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import api from '../api/client';
import { Quotation } from '../types';

const QuotesList: React.FC = () => {
  const [quotes, setQuotes] = useState<Quotation[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchQuotes();
  }, []);

  const fetchQuotes = async () => {
    try {
      const res = await api.get('/quotes');
      setQuotes(res.data);
    } catch (error) {
      console.error(error);
      setQuotes([
        { id: 1, quoteNumber: 'Q-2026-001', customerId: 1, customerName: 'Acme Corp', amount: 15000, status: 'Sent', sentDate: '2026-06-01' },
        { id: 2, quoteNumber: 'Q-2026-002', customerId: 2, customerName: 'Beta Ltd', amount: 8500, status: 'Pending', sentDate: '2026-06-05' },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Quotations</h1>
        <Link to="/quotes/new" className="bg-gradient-main text-white px-4 py-2 rounded-lg font-medium hover:opacity-90">
          New Quote
        </Link>
      </div>
      <div className="bg-white rounded-xl shadow-sm overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50 border-b">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Quote #</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Customer</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Amount</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Sent</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {loading ? (
              <tr><td colSpan={5} className="px-6 py-4 text-center">Loading...</td></tr>
            ) : quotes.map((q) => (
              <tr key={q.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 font-medium text-royalblue">{q.quoteNumber}</td>
                <td className="px-6 py-4">{q.customerName}</td>
                <td className="px-6 py-4">R {q.amount.toLocaleString()}</td>
                <td className="px-6 py-4"><span className={`px-2 py-1 text-xs rounded-full ${q.status === 'Sent' ? 'bg-blue-100 text-blue-800' : 'bg-yellow-100 text-yellow-800'}`}>{q.status}</span></td>
                <td className="px-6 py-4 text-gray-500">{new Date(q.sentDate).toLocaleDateString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default QuotesList;
