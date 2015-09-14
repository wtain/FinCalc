using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;

namespace FCHA
{
	public class AccountsManager
	{
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = 
			new string[] { "AccountId", "Currency", "OwnerPersonId", "Name" };

		public AccountsManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Account> EnumAllAccounts()
		{
			return SelectAccounts(QueryBuilder.Select(Columns, "accounts"));
		}

		public IEnumerable<Account> EnumUserAccounts(Person person)
		{
			return SelectAccounts(QueryBuilder.Select(Columns, "accounts", 
													"ownerPersonId", person.personId.ToString()));
		}

		public Account GetAccount(int accountId)
		{
			return SelectOne(QueryBuilder.Select(Columns, "accounts", "accountId", accountId.ToString()));
		}

		private Account BuildAccountStructure(SQLiteDataReader reader)
		{
			int accountId = reader.GetInt32(0);
			string currency = reader.GetString(1);
			int ownerPersonId = reader.GetInt32(2);
			string name = reader.GetString(3);
			return new Account(accountId, currency, ownerPersonId, name);
		}

		private IEnumerable<Account> SelectAccounts(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
						yield return BuildAccountStructure(reader);
				}
		}

		private Account SelectOne(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					Debug.Assert(1 == reader.StepCount);

					while (reader.Read())
						return BuildAccountStructure(reader);
				}
			return new Account();
		}

		public void AddAccount(string name, string currency, int ownerPersonId)
		{
			string query = QueryBuilder.Insert("accounts", new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(name)),
														   new KeyValuePair<string, string>("Currency", QueryBuilder.DecorateString(currency)),
														   new KeyValuePair<string, string>("OwnerPersonId", ownerPersonId.ToString()));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				insert.ExecuteNonQuery();
		}

		public void AddAccount(Account account)
		{
			AddAccount(account.name, account.currency, account.ownerPersonId);
		}

		public void UpdateAccount(Account account)
		{
			string query = QueryBuilder.Update("accounts", "AccountId", account.accountId.ToString(),
														   new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(account.name)),
														   new KeyValuePair<string, string>("Currency", QueryBuilder.DecorateString(account.currency)),
														   new KeyValuePair<string, string>("OwnerPersonId", account.ownerPersonId.ToString()));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void DeleteAccount(Account account)
		{
			string query = QueryBuilder.Delete("accounts", "AccountId", account.accountId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}
	}
}
