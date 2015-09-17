﻿using System;
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
			new string[] { "AccountId", "Currency", "OwnerPersonId", "Name", "Type" };

		private static readonly string[] ColumnsBalance =
			new string[] { "Value", "LastUpdatedDate" };

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

		public Account GetAccount(long accountId)
		{
			return SelectOne(QueryBuilder.Select(Columns, "accounts", "accountId", accountId.ToString()));
		}

		public AccountBalance GetAccountBalance(long accountId)
		{
			return SelectOneBalance(QueryBuilder.Select(ColumnsBalance, "AccountBalance", "accountId", accountId.ToString()));
		}

		private Account BuildAccountStructure(SQLiteDataReader reader)
		{
			long accountId = reader.GetInt64(0);
			string currency = reader.GetString(1);
			long ownerPersonId = reader.GetInt64(2);
			string name = reader.GetString(3);
			AccountType type = Account.AccountTypeFromString(reader.GetString(4));
			return new Account(accountId, currency, ownerPersonId, name, type);
		}

		private AccountBalance BuildAccountBalanceStructure(SQLiteDataReader reader)
		{
			long balance = reader.GetInt64(0);
			DateTime lastUpdated = DateTime.Parse(reader.GetString(1));
			return new AccountBalance(balance, lastUpdated);
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

		private AccountBalance SelectOneBalance(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
			using (SQLiteDataReader reader = select.ExecuteReader())
			{
				Debug.Assert(1 == reader.StepCount);

				while (reader.Read())
					return BuildAccountBalanceStructure(reader);
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
						return BuildAccountStructure(reader);
				}
			return new Account();
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
			return new KeyValuePair<string, string>("Type", QueryBuilder.DecorateString(Account.AccountTypeToString(type)));
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

		public void AddAccount(ref Account account)
		{
			account.accountId = AddAccount(account.name, account.currency, account.ownerPersonId, account.type);
		}

		public void UpdateAccount(Account account)
		{
			string query = QueryBuilder.Update("accounts", "AccountId", account.accountId.ToString(),
														   GetNameColumnPair(account.name),
														   GetCurrencyColumnPair(account.currency),
														   GetOwnerPersonIdColumnPair(account.ownerPersonId),
														   GetTypeColumnPair(account.type));
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
