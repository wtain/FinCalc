using FCHA.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FCHA
{
    public class Loan
    {
        private decimal m_principal;
        private double m_rate;
        private DateTime m_startDate;
        private Calendar m_calendar;
        private decimal m_payment;
        private List<Cashflow> m_payments;

        public Loan(decimal principal, double rate, DateTime startDate, int duration, Calendar calendar)
        {
            m_principal = principal;
            m_rate = rate;
            m_startDate = startDate;
            m_calendar = calendar;
            int nPaymentsPerYear = 12; // Monthly; duration - months
            double flowRate = rate / nPaymentsPerYear;
            m_payment = (decimal) (((double) principal * flowRate) / (1 - 1 / Math.Pow(1 + flowRate, duration)));
            decimal amount = principal;
            DateTime date = startDate; // ! paying immediately, percents for the month forward; in case of full deal the principal payment would be 1 month off
            m_payments = new List<Cashflow>(duration);
            for (long i = 0; i < duration; ++i)
            {
                decimal interestPayment = (decimal) ((double) amount * flowRate);
                decimal principalPayment = m_payment - interestPayment;
                amount -= principalPayment;
                m_payments.Add(new Cashflow(date, principalPayment, interestPayment));
                date = date.AddMonths(1); // don't respect calendar
            }
            Debug.Assert(Math.Abs(amount) < 0.01m);
            Debug.Assert(m_payments.Count == duration);
        }

        public void AdditionalPayment(DateTime date, decimal amount)
        {
            // todo: + percents for partial month
            int i = 0;
            for (; i < m_payments.Count; ++i)
                if (m_payments[i].paymentDate > date)
                    break;
            Debug.Assert(i > 0 && i < m_payments.Count);
            decimal totalPaid = 0.0m;
            for (int j = 0; j < i; ++j)
                totalPaid += m_payments[j].principalPayment;
            totalPaid += amount;
            decimal newAmount = m_principal - totalPaid;
            bool bHasFuturePayments = (i < m_payments.Count);
            DateTime newStartDate = bHasFuturePayments ? m_payments[i + 1].paymentDate : date;
            int newDuration = m_payments.Count - i;
            m_payments.RemoveRange(i, newDuration);
            m_payments.Add(new Cashflow(date, amount, 0.0m)); // todo: interest payments here
            if (bHasFuturePayments)
            {
                Loan newLoan = new Loan(newAmount, m_rate, newStartDate, newDuration, m_calendar);
                m_payments.AddRange(newLoan.Schedule);
                m_payment = newLoan.Payment;
            }
        }

        public decimal Payment
        {
            get { return m_payment; }
        }

        public decimal TotalInterest
        {
            get { return m_payments.Sum(cf => cf.InterestPayment); }
        }

        public decimal TotalPayment
        {
            get { return m_payments.Sum(cf => cf.TotalPayment); }
        }

        public decimal TotalPrincipalPayment
        {
            get { return m_payments.Sum(cf => cf.principalPayment); }
        }

        public IEnumerable<Cashflow> Schedule
        {
            get { return m_payments; }
        }

        public int NumberOfPayments
        {
            get { return m_payments.Count; }
        }
    }
}
