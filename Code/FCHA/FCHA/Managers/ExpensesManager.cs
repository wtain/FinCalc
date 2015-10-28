using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using FCHA.Interfaces;

namespace FCHA
{
	public class ExpensesManager : IExpensesManager
    {
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = 
			new string[] { "ExpenseId", "AccountId", "Amount", "CategoryId", "Date", "Description" };

		private static readonly string TableName = "expenses";

		public ExpensesManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Expense> EnumAllExpenses()
		{
			return SelectExpenses(QueryBuilder.Select(Columns, TableName));
		}

		private Expense BuildExpenseStructure(SQLiteDataReader reader)
		{
			long expenseId = reader.GetInt64(0);
			long accountId = reader.GetInt64(1);
			long amount = reader.GetInt64(2);
			long categoryId = reader.GetInt64(3);
			DateTime date = DateTime.Parse(reader.GetString(4));
			string description = reader.GetString(5);
			return new Expense(expenseId, accountId, amount, categoryId, date, description);
		}

		private IEnumerable<Expense> SelectExpenses(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
						yield return BuildExpenseStructure(reader);
				}
		}

		private KeyValuePair<string, string> GetAccountIdColumnPair(long accountId)
		{
			return new KeyValuePair<string, string>("AccountId", accountId.ToString());	
		}

		private KeyValuePair<string, string> GetAmountColumnPair(long amount)
		{
			return new KeyValuePair<string, string>("Amount", amount.ToString());	
		}

		private KeyValuePair<string, string> GetCategoryIdColumnPair(long categoryId)
		{
			return new KeyValuePair<string, string>("CategoryId", categoryId.ToString());	
		}

		private KeyValuePair<string, string> GetDateColumnPair(DateTime date)
		{
			return new KeyValuePair<string, string>("Date", QueryBuilder.DecorateString(date.ToString(QueryBuilder.DateFormat)));	
		}

		private KeyValuePair<string, string> GetDescriptionColumnPair(string description)
		{
			return new KeyValuePair<string, string>("Description", QueryBuilder.DecorateString(description));	
		}

		public long AddExpense(long accountId, long amount, long categoryId, DateTime date, string description)
		{
			string query = QueryBuilder.Insert(TableName, GetAccountIdColumnPair(accountId),
														  GetAmountColumnPair(amount),
														  GetCategoryIdColumnPair(categoryId),
														  GetDateColumnPair(date),
														  GetDescriptionColumnPair(description));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				return (long)insert.ExecuteScalar();
		}

		public void AddExpense(ref Expense expense)
		{
			expense.expenseId = AddExpense(expense.accountId, expense.amount, expense.categoryId, expense.date, expense.description);
		}

		public void UpdateExpense(Expense expense)
		{
			string query = QueryBuilder.Update(TableName, "ExpenseId", expense.expenseId.ToString(),
														  GetAccountIdColumnPair(expense.accountId),
														  GetAmountColumnPair(expense.amount),
														  GetCategoryIdColumnPair(expense.categoryId),
														  GetDateColumnPair(expense.date),
														  GetDescriptionColumnPair(expense.description));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void DeleteExpense(Expense expense)
		{
			string query = QueryBuilder.Delete(TableName, "ExpenseId", expense.expenseId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}
	}
}
