using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace FCHA
{
	public class UsersManager
	{
		private SQLiteConnection m_conn;

		public UsersManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		// todo: uniquify all managers

		public void AddUser(string name, string fullName)
		{
			string query = QueryBuilder.Insert("persons", new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(name)),
														  new KeyValuePair<string, string>("FullName", QueryBuilder.DecorateString(fullName)));
			SQLiteCommand insert = new SQLiteCommand(query, m_conn);
			insert.ExecuteNonQuery();
		}

		public void UpdateUser(Person person)
		{
			string query = QueryBuilder.Update("persons", "PersonId", person.personId.ToString(),
				new KeyValuePair<string, string>("Name", QueryBuilder.DecorateString(person.name)),
				new KeyValuePair<string, string>("FullName", QueryBuilder.DecorateString(person.fullName)));
			SQLiteCommand update = new SQLiteCommand(query, m_conn);
			update.ExecuteNonQuery();
		}

		public void DeleteUser(Person person)
		{
			string query = QueryBuilder.Delete("persons", "PersonId", person.personId.ToString());
			SQLiteCommand delete = new SQLiteCommand(query, m_conn);
			delete.ExecuteNonQuery();
		}
	}
}
