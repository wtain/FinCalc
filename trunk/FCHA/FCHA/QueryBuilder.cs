
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace FCHA
{
	public static class QueryBuilder
	{
		public static string Select(string[] fields, string tableName, params string[] filters)
		{
			Debug.Assert(filters.Length % 2 == 0);
			StringBuilder sb = new StringBuilder("SELECT ");
			if (0 == fields.Length)
				sb.Append("*");
			else
				sb.Append(string.Join<string>(", ", fields));
			sb.AppendFormat(" FROM {0}", tableName);
			if (0 != filters.Length)
				sb.AppendFormat(" WHERE {0}", FiltersCondition(filters));
			return sb.ToString();
		}

		public static string Insert(string tableName, params KeyValuePair<string, string>[] keysAndValues)
		{
			StringBuilder sb = new StringBuilder("INSERT INTO ");
			sb.AppendFormat("{0} ({1}) VALUES ({2}); ", tableName, string.Join<string>(", ", keysAndValues.Select(x => x.Key)),
																 string.Join<string>(", ", keysAndValues.Select(x => x.Value)));
			sb.Append("SELECT last_insert_rowid()");
			return sb.ToString();
		}

		public static string Update(string tableName, string keyName, string keyValue, params KeyValuePair<string, string>[] keysAndValues)
		{
			StringBuilder sb = new StringBuilder("UPDATE ");
			sb.AppendFormat("{0} SET {1} WHERE {2}", tableName,
				string.Join<string>(", ", keysAndValues.Select(x => string.Format("{0}={1}", x.Key, x.Value))),
				FiltersCondition(keyName, keyValue));
			return sb.ToString();
		}

		public static string Delete(string tableName, string keyName, string keyValue)
		{
			StringBuilder sb = new StringBuilder("DELETE FROM ");
			sb.AppendFormat("{0} WHERE {1}", tableName, FiltersCondition(keyName, keyValue));
			return sb.ToString();
		}

		public static string DecorateString(string value)
		{
			return string.Format("'{0}'", value.Replace("'", "''"));
		}

		public static string FiltersCondition(params string[] filters)
		{
			if (0 == filters.Length)
				return string.Empty;
			Debug.Assert(filters.Length % 2 == 0);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < filters.Length; i += 2)
			{
				string name = filters[i];
				string value = filters[i + 1];
				if (0 != i)
					sb.Append(" AND ");
				sb.AppendFormat("{0}={1}", name, value);
			}
			return sb.ToString();
		}
	}
}