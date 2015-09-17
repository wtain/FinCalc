using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public struct AccountBalance
	{
		public long balance;
		public DateTime lastUpdated;

		public AccountBalance(long balance, DateTime lastUpdated)
		{
			this.balance = balance;
			this.lastUpdated = lastUpdated;	
		}
	}
}
