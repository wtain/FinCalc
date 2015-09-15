using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;

namespace FCHA
{
	public class UsersManager
	{
		private SQLiteConnection m_conn;

		public UsersManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Person> EnumAllUsers()
		{
			return SelectUsers(QueryBuilder.Select(new string[] { "Name", "FullName", "PersonId" }, "persons"));
		}

		public Person GetUser(long personId)
		{
			return SelectOne(QueryBuilder.Select(new string[] { "Name", "FullName", "PersonId" }, "persons", "personId", personId.ToString()));
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
			return new Person();
		}

		public long AddUser(string name, string fullName)
		{
			string query = QueryBuilder.Insert("persons", new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(name)),
														  new KeyValuePair<string, string>("FullName", QueryBuilder.DecorateString(fullName)));
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
				new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(person.name)),
				new KeyValuePair<string, string>("FullName", QueryBuilder.DecorateString(person.fullName)));
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
