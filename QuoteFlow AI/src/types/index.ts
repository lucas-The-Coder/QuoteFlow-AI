export interface User {
  email: string;
  firstName: string;
  lastName: string;
  token: string;
}

export interface Customer {
  id: number;
  name: string;
  email: string;
  phone?: string;
  company?: string;
}

export interface Quotation {
  id: number;
  quoteNumber: string;
  customerId: number;
  customerName?: string;
  amount: number;
  status: string;
  sentDate: string;
  validUntil?: string;
}

export interface DashboardMetrics {
  totalQuotes: number;
  pendingFollowUps: number;
  convertedQuotes: number;
  conversionRate: number;
}
