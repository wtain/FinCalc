using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.DataTypes
{
    public class Cashflow 
        : SimpleCashflow
    {
        public decimal principalPayment;

        public decimal InterestPayment
        {
            get { return payment; }
        }

        public decimal TotalPayment
        {
            get { return InterestPayment + principalPayment; }
        }

        public Cashflow(DateTime paymentDate, decimal principalPayment, decimal interestPayment)
            : base(paymentDate, interestPayment)
        {
            this.principalPayment = principalPayment;
        }
    }
}
