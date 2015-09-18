using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;

namespace FCHA
{
	public class TempTable : IDisposable
	{
		private string m_name;
		private SQLiteConnection m_conn;

		public string Name
		{
			get { return m_name; }
		}

		public TempTable(SQLiteConnection conn, string select)
		{
			m_conn = conn;
			m_name = QueryBuilder.GenerateTempTableName();
			string query = QueryBuilder.CreateTempTableAsSelect(m_name, select);
			using (SQLiteCommand cmd = new SQLiteCommand(query, m_conn))
				cmd.ExecuteNonQuery();
		}

		public void Dispose()
		{
			string query = QueryBuilder.DropTable(m_name);
			using (SQLiteCommand cmd = new SQLiteCommand(query, m_conn))
				cmd.ExecuteNonQuery();
		}

		public IEnumerable<string> GetColumnValues(string columnName, LinkedList<KeyValuePair<string, string>> filters)
		{
			string query = QueryBuilder.BuildOlapDimensions(Name, columnName, filters);
			SQLiteCommand cmd = new SQLiteCommand(query, m_conn);
			using (SQLiteDataReader reader = cmd.ExecuteReader())
				while (reader.Read())
					yield return reader.GetString(0);
		}

		public long SelectData(LinkedList<KeyValuePair<string, string>> leftFilters, LinkedList<KeyValuePair<string, string>> topFilters, string aggregation)
		{
			string query = QueryBuilder.Select(new string[] { aggregation }, m_name, leftFilters.Concat(topFilters));
			SQLiteCommand cmd = new SQLiteCommand(query, m_conn);
			object o = cmd.ExecuteScalar();
			if (o is DBNull)
				return 0;
			return (long)o;
		}
	}
}
