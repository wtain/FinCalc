using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FCHA
{
	public struct Account
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

		public Account(long accountId, string currency, long ownerPersonId, string name, AccountType type)
		{
			this.accountId = accountId;
			this.currency = currency;
			this.ownerPersonId = ownerPersonId;
			this.name = name;
			this.type = type;
		}

		public static string AccountTypeToString(AccountType type)
		{
			switch (type)
			{
				case AccountType.Cash:
					return "CASH";
				case AccountType.CreditCard:
					return "CREDITCARD";
				case AccountType.DebetCard:
					return "DEBETCARD";
				case AccountType.Deposit:
					return "DEPOSIT";
			}
			Debug.Assert(false);
			return string.Empty;
		}

		public static AccountType AccountTypeFromString(string strType)
		{
			switch (strType)
			{
				case "CASH":
					return AccountType.Cash;
				case "CREDITCARD":
					return AccountType.CreditCard;
				case "DEBETCARD":
					return AccountType.DebetCard;
				case "DEPOSIT":
					return AccountType.Deposit;
			}
			Debug.Assert(false);
			return AccountType.Cash;
		}
	}

	public enum AccountType
	{
		Cash       = 0,
		DebetCard  = 1,
		CreditCard = 2,
		Deposit    = 3
	}
}

