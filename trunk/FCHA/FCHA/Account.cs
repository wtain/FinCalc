using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public struct Account
	{
		public int accountId;
		public string currency;
		public int ownerPersonId;
		public string name;

		public Account(int accountId, string currency, int ownerPersonId, string name)
		{
			this.accountId = accountId;
			this.currency = currency;
			this.ownerPersonId = ownerPersonId;
			this.name = name;
		}
	}
}

