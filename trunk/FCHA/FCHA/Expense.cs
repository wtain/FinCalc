using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public struct Expense
	{
		public long expenseId;
		public long accountId;
		public decimal amount;
		public long categoryId;
		public DateTime date;
		public string description;

		public Expense(long expenseId, long accountId, decimal amount, long categoryId, DateTime date, string description)
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
