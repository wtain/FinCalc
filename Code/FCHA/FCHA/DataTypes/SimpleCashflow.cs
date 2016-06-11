using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.DataTypes
{
    public class SimpleCashflow
    {
        public DateTime paymentDate;
        public decimal payment;

        public SimpleCashflow(DateTime paymentDate, decimal payment)
        {
            this.paymentDate = paymentDate;
            this.payment = payment;
        }
    }
}
