using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
    public class FxRate
    {
        public string CCY { get; private set; }
        public decimal Rate { get; private set; }

        public FxRate(string ccy, decimal rate)
        {
            CCY = ccy;
            Rate = rate;
        }
    }
}
