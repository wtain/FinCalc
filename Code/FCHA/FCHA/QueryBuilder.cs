
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace FCHA
{
	public static class QueryBuilder
	{
		public static readonly string DateFormat = "yyyy-MM-dd";

		private static long tempTableCount = 0;

		public static string GenerateTempTableName()
		{
			return string.Format("temptable{0}", tempTableCount++);
		}

		private static string SelectStatement(string[] fields, string tableName)
		{
			StringBuilder sb = new StringBuilder("SELECT ");
			if (0 == fields.Length)
				sb.Append("*");
			else
				sb.Append(string.Join<string>(", ", fields));
			sb.AppendFormat(" FROM {0}", tableName);
			return sb.ToString();
		}

        public static string Ordered(string selectQuery, params string[] columns)
        {
            return string.Format("{0} ORDER BY {1}", selectQuery, string.Join(", ", columns.AsEnumerable()));
        }
		
		public static string Select(string[] fields, string tableName, params string[] filters)
		{
			StringBuilder sb = new StringBuilder(SelectStatement(fields, tableName));
			Debug.Assert(filters.Length % 2 == 0);
			if (0 != filters.Length)
				sb.AppendFormat(" WHERE {0}", FiltersCondition(filters));
			return sb.ToString();
		}

		public static string Select(string[] fields, string tableName, IEnumerable<KeyValuePair<string, string>> filters)
		{
			StringBuilder sb = new StringBuilder(SelectStatement(fields, tableName));
			sb.Append(WhereClause(filters));
			return sb.ToString();
		}

		private static string InsertStatement(string tableName, KeyValuePair<string, string>[] keysAndValues)
		{
			return string.Format("{0} ({1}) VALUES ({2}); ", tableName, 
									string.Join<string>(", ", keysAndValues.Select(x => x.Key)),
									string.Join<string>(", ", keysAndValues.Select(x => x.Value)));
		}

		public static string Insert(string tableName, params KeyValuePair<string, string>[] keysAndValues)
		{
			StringBuilder sb = new StringBuilder("INSERT INTO ");
			sb.Append(InsertStatement(tableName, keysAndValues));
			sb.Append("SELECT last_insert_rowid()");
			return sb.ToString();
		}

		public static string InsertView(string viewName, string underlyingTableName, string keyName, params KeyValuePair<string, string>[] keysAndValues)
		{
			StringBuilder sb = new StringBuilder("INSERT INTO ");
			sb.Append(InsertStatement(viewName, keysAndValues));
			sb.AppendFormat("SELECT MAX({1}) FROM {0}", underlyingTableName, keyName);
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

		public static string WhereClause(IEnumerable<KeyValuePair<string, string>> filters)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (var filter in filters)
			{
				if (0 != i)
					sb.Append(" AND ");
				else
					sb.Append(" WHERE ");
				sb.AppendFormat("{0}={1}", filter.Key, filter.Value);
				++i;
			}
			return sb.ToString();
		}

		public static string BuildOlapStage(string tableName, OlapStage stage)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Select(stage.aggregations.Union(stage.left).Union(stage.top).ToArray(), tableName));
			if (stage.left.Length > 0 || stage.top.Length > 0)
				sb.AppendFormat(" GROUP BY {0}", string.Join(", ", stage.left.Union(stage.top)));
			return sb.ToString();
		}

		public static string BuildOlapDimensions(string tableName, string column, LinkedList<KeyValuePair<string, string>> filters)
		{
			return string.Format("SELECT DISTINCT {0} FROM {1} {2}", column, tableName, WhereClause(filters.ToArray()));
		}

		public static string CreateTempTableAsSelect(string tableName, string select)
		{
			return string.Format("CREATE TEMP TABLE {0} AS {1}", tableName, select);
		}

		public static string DropTable(string tableName)
		{
			return string.Format("DROP TABLE {0}", tableName);
		}

		public static string SelectJoin(string table1, string table2, string key1, string key2, string[] columns1, string[] columns2)
		{
			StringBuilder sb = new StringBuilder("SELECT ");
			
			sb.AppendFormat("{0}, {1}", 
					string.Join(", ", columns1.Select(c => string.Format("t1.{0}", c))),
					string.Join(", ", columns2.Select(c => string.Format("t2.{0}", c)))
				);

			sb.AppendFormat(" FROM {0} t1 LEFT JOIN {1} t2 ON t1.{2}=t2.{3}", table1, table2, key1, key2);

			return sb.ToString();
		}
	}
}
