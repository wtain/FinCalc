using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;
using FCHA.Interfaces;

namespace FCHA
{
	public class UsersManager : IUsersManager
    {
		private SQLiteConnection m_conn;

		public static readonly string[] Columns = new string[] { "Name", "FullName", "PersonId" };

		public UsersManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Person> EnumAllUsers()
		{
			return SelectUsers(QueryBuilder.Select(Columns, "persons"));
		}

		public Person GetUser(long personId)
		{
			return SelectOne(QueryBuilder.Select(Columns, "persons", "personId", personId.ToString()));
		}

		private Person BuildUserStructure(SQLiteDataReader reader)
		{
			string name = reader.GetString(0);
			string fullName = reader.GetString(1);
			long personId = reader.GetInt64(2);
			return new Person(name, fullName, personId);
		}

		private IEnumerable<Person> SelectUsers(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
						yield return BuildUserStructure(reader);
				}
		}

		private Person SelectOne(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					Debug.Assert(1 == reader.StepCount);

					while (reader.Read())
						return BuildUserStructure(reader);
				}
			return null;
		}

		private KeyValuePair<string, string> GetNameColumnPair(string name)
		{
			return new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(name));
		}

		private KeyValuePair<string, string> GetFullNameColumnPair(string fullName)
		{
			return new KeyValuePair<string, string>("FullName", QueryBuilder.DecorateString(fullName));
		}

		public long AddUser(string name, string fullName)
		{
			string query = QueryBuilder.Insert("persons", GetNameColumnPair(name),
														  GetFullNameColumnPair(fullName));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				return (long)insert.ExecuteScalar();
		}

		public void AddUser(ref Person person)
		{
			person.personId = AddUser(person.name, person.fullName);
		}

		public void UpdateUser(Person person)
		{
			string query = QueryBuilder.Update("persons", "PersonId", person.personId.ToString(),
														  GetNameColumnPair(person.name),
														  GetFullNameColumnPair(person.fullName));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void DeleteUser(Person person)
		{
			string query = QueryBuilder.Delete("persons", "PersonId", person.personId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}
	}
}
