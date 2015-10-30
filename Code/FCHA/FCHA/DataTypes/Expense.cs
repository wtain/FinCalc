using System;
using System.Diagnostics;

namespace FCHA
{
    [DebuggerDisplay("{amount}, accountId={accountId}")]
	public struct Expense
	{
		public long expenseId;
		public long accountId;
		public long amount;
		public long categoryId;
		public DateTime date;
		public string description;

		public Expense(long expenseId, long accountId, long amount, long categoryId, DateTime date, string description)
		{
			this.expenseId = expenseId;
			this.accountId = accountId;
			this.amount = amount;
			this.categoryId = categoryId;
			this.date = date;
			this.description = description;
		}
	}
}
