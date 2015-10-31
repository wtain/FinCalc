using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;
using FCHA.DataTypes;
using FCHA.Interfaces;

namespace FCHA
{
	public class AccountsManager : IAccountsManager
    {
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = 
			new string[] { "AccountId", "Currency", "OwnerPersonId", "Name", "Type" };

		private static readonly string[] ColumnsBalance =
			new string[] { "Value", "LastUpdatedDate" };

		public AccountsManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Account> EnumAllAccounts()
		{
			return Select(QueryBuilder.Select(Columns, "accounts"));
		}

		public IEnumerable<Account> EnumUserAccounts(Person person)
		{
			return Select(QueryBuilder.Select(Columns, "accounts", 
													"ownerPersonId", person.personId.ToString()));
		}

		public Account Get(long accountId)
		{
			return SelectOne(QueryBuilder.Select(Columns, "accounts", "accountId", accountId.ToString()));
		}

		public AccountBalance GetBalance(long accountId)
		{
			return SelectOneBalance(QueryBuilder.Select(ColumnsBalance, "AccountBalance", "accountId", accountId.ToString()));
		}

		private Account BuildStructure(SQLiteDataReader reader)
		{
			long accountId = reader.GetInt64(0);
			string currency = reader.GetString(1);
			long ownerPersonId = reader.GetInt64(2);
			string name = reader.GetString(3);
			AccountType type = AccountTypeHelper.AccountTypeFromString(reader.GetString(4));
			return new Account(accountId, currency, ownerPersonId, name, type);
		}

		private AccountBalance BuildBalanceStructure(SQLiteDataReader reader)
		{
			long balance = reader.GetInt64(0);
			DateTime lastUpdated = DateTime.Parse(reader.GetString(1));
			return new AccountBalance(balance, lastUpdated);
		}

		private IEnumerable<Account> Select(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
						yield return BuildStructure(reader);
				}
		}

		private AccountBalance SelectOneBalance(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
			using (SQLiteDataReader reader = select.ExecuteReader())
			{
				Debug.Assert(1 == reader.StepCount);

				while (reader.Read())
					return BuildBalanceStructure(reader);
			}
			return new AccountBalance();
		}

		private Account SelectOne(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					Debug.Assert(1 == reader.StepCount);

					while (reader.Read())
						return BuildStructure(reader);
				}
			return null;
		}

		private KeyValuePair<string, string> GetNameColumnPair(string name)
		{
			return new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(name));
		}

		private KeyValuePair<string, string> GetCurrencyColumnPair(string currency)
		{
			return new KeyValuePair<string, string>("Currency", QueryBuilder.DecorateString(currency));
		}

		private KeyValuePair<string, string> GetOwnerPersonIdColumnPair(long ownerPersonId)
		{
			return new KeyValuePair<string, string>("OwnerPersonId", ownerPersonId.ToString());
		}

		private KeyValuePair<string, string> GetTypeColumnPair(AccountType type)
		{
			return new KeyValuePair<string, string>("Type", QueryBuilder.DecorateString(AccountTypeHelper.AccountTypeToString(type)));
		}

		public long AddAccount(string name, string currency, long ownerPersonId, AccountType type)
		{
			string query = QueryBuilder.Insert("accounts", GetNameColumnPair(name),
														   GetCurrencyColumnPair(currency),
														   GetOwnerPersonIdColumnPair(ownerPersonId),
														   GetTypeColumnPair(type));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				return (long)insert.ExecuteScalar();
		}

		public void Add(ref Account account)
		{
			account.accountId = AddAccount(account.name, account.currency, account.ownerPersonId, account.type);
		}

		public void Update(Account account)
		{
			string query = QueryBuilder.Update("accounts", "AccountId", account.accountId.ToString(),
														   GetNameColumnPair(account.name),
														   GetCurrencyColumnPair(account.currency),
														   GetOwnerPersonIdColumnPair(account.ownerPersonId),
														   GetTypeColumnPair(account.type));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void Delete(Account account)
		{
			string query = QueryBuilder.Delete("accounts", "AccountId", account.accountId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}
	}
}
