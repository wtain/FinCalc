using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public class AccountBalance
	{
		public long balance;
		public DateTime lastUpdated;

        public AccountBalance()
        {
            balance = 0;
        }

		public AccountBalance(long balance, DateTime lastUpdated)
		{
			this.balance = balance;
			this.lastUpdated = lastUpdated;	
		}
	}
}
