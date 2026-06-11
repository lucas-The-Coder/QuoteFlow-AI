import React, { useEffect, useState } from 'react';
import api from '../api/client';
import { Customer } from '../types';

const CustomersList: React.FC = () => {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [form, setForm] = useState({ name: '', email: '', phone: '', company: '' });

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      const res = await api.get('/customers');
      setCustomers(res.data);
    } catch {
      setCustomers([
        { id: 1, name: 'John Smith', email: 'john@acme.com', phone: '+27 82 123 4567', company: 'Acme Corp' },
      ]);
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post('/customers', form);
      setForm({ name: '', email: '', phone: '', company: '' });
      setShowForm(false);
      fetchCustomers();
    } catch {
      alert('Add customer failed - backend not connected');
    }
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Customers</h1>
        <button onClick={() => setShowForm(!showForm)} className="bg-gradient-main text-white px-4 py-2 rounded-lg font-medium">Add Customer</button>
      </div>
      {showForm && (
        <div className="bg-white rounded-xl shadow-sm p-6 mb-6">
          <form onSubmit={handleAdd} className="grid grid-cols-2 gap-4">
            <input placeholder="Name" required value={form.name} onChange={e => setForm({...form, name: e.target.value})} className="px-4 py-2 border rounded-lg" />
            <input placeholder="Email" type="email" required value={form.email} onChange={e => setForm({...form, email: e.target.value})} className="px-4 py-2 border rounded-lg" />
            <input placeholder="Phone" value={form.phone} onChange={e => setForm({...form, phone: e.target.value})} className="px-4 py-2 border rounded-lg" />
            <input placeholder="Company" value={form.company} onChange={e => setForm({...form, company: e.target.value})} className="px-4 py-2 border rounded-lg" />
            <div className="col-span-2"><button type="submit" className="bg-royalblue text-white px-4 py-2 rounded-lg">Save</button></div>
          </form>
        </div>
      )}
      <div className="bg-white rounded-xl shadow-sm overflow-hidden">
        <table className="w-full">
          <thead className="bg-gray-50 border-b">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Name</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Company</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Email</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Phone</th>
            </tr>
          </thead>
          <tbody className="divide-y">
            {customers.map(c => (
              <tr key={c.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 font-medium">{c.name}</td>
                <td className="px-6 py-4">{c.company}</td>
                <td className="px-6 py-4 text-royalblue">{c.email}</td>
                <td className="px-6 py-4">{c.phone}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default CustomersList;
