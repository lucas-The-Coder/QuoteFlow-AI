import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/client';
import { Customer } from '../types';

const CreateQuote: React.FC = () => {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [form, setForm] = useState({ customerId: '', amount: '', validUntil: '' });
  const navigate = useNavigate();

  useEffect(() => {
    api.get('/customers').then(res => setCustomers(res.data)).catch(() => {
      setCustomers([{ id: 1, name: 'Acme Corp', email: 'info@acme.com' }]);
    });
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post('/quotes', {
        CustomerId: parseInt(form.customerId),
        Amount: parseFloat(form.amount),
        ValidUntil: form.validUntil,
      });
      navigate('/quotes');
    } catch (error) {
      alert('Failed to create quote - check backend connection');
    }
  };

  return (
    <div className="max-w-2xl">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Create New Quotation</h1>
      <div className="bg-white rounded-xl shadow-sm p-6">
        <form onSubmit={handleSubmit} className="space-y-5">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Customer</label>
            <select required value={form.customerId} onChange={(e) => setForm({...form, customerId: e.target.value})} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-royalblue">
              <option value="">Select customer</option>
              {customers.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Amount (ZAR)</label>
            <input type="number" required value={form.amount} onChange={(e) => setForm({...form, amount: e.target.value})} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-royalblue" placeholder="15000" />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Valid Until</label>
            <input type="date" value={form.validUntil} onChange={(e) => setForm({...form, validUntil: e.target.value})} className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-royalblue" />
          </div>
          <div className="flex gap-3">
            <button type="submit" className="bg-gradient-main text-white px-6 py-2 rounded-lg font-medium">Create & Send</button>
            <button type="button" onClick={() => navigate('/quotes')} className="px-6 py-2 border border-gray-300 rounded-lg">Cancel</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateQuote;
