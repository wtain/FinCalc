using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FCHA
{
	public class Account
	{
		public long accountId;
		public string currency;
		public long ownerPersonId;
		public string name;
		public AccountType type;

		public static Account CreateDefault(long ownerId)
		{
			return new Account(0, "RUB", ownerId, "(Default)", AccountType.Cash);
		}

        // Do we need id here?
		public Account(long accountId, string currency, long ownerPersonId, string name, AccountType type)
		{
			this.accountId = accountId;
			this.currency = currency;
			this.ownerPersonId = ownerPersonId;
			this.name = name;
			this.type = type;
		}
	}
}

