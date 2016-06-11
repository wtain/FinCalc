using FCHA.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FCHA
{
    public class Deposit
    {
        private List<SimpleCashflow> cashflows;

        public double Rate { get; private set; }
        public double MonthlyRate { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime StartDate { get; private set; }
        public int Duration { get; private set; }

        public Deposit(decimal amount, double rate, DateTime startDate, int duration)
        {
            Rate = rate;
            MonthlyRate = rate / 12.0;
            Amount = amount;
            StartDate = startDate;
            Duration = duration;
            cashflows = new List<SimpleCashflow>(duration);
            DateTime date = startDate;
            decimal principal = amount;
            TotalInterest = 0;
            for (int i = 0; i < duration; ++i)
            {
                decimal interest = (decimal)((double) principal * MonthlyRate);
                TotalInterest = TotalInterest + interest; // monthly compounding
                principal += interest;
                date = date.Add(TimeSpan.FromDays(30.0));
                cashflows.Add(new SimpleCashflow(date, interest));
            }                                  
        }

        public decimal TotalInterest { get; private set; }

        public decimal PrincipalAt(DateTime date)
        {
            return Amount + InterestToDate(date);
        }

        public decimal PrincipalAtMaturity
        {
            get { return Amount + TotalInterest; }
        }

        public decimal InterestToDate(DateTime date)
        {
            int idx = cashflows.FindLastIndex(cf => cf.paymentDate <= date);
            return cashflows.Take(idx + 1).Sum(cf => cf.payment);
        }

        public IEnumerable<SimpleCashflow> Cashflows
        {
            get { return cashflows; }
        }
    }
}
