
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace FCHA
{
	public static class QueryBuilder
	{
		public static readonly string DateFormat = "dd-MMM-yyyy";
		
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

		public static string BuildOlapStage(string tableName, OlapStage stage)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Select(stage.aggregations.Union(stage.left).Union(stage.top).ToArray(), tableName));
			if (stage.left.Length > 0 || stage.top.Length > 0)
				sb.AppendFormat("GROUP BY {0}", string.Join(", ", stage.left.Union(stage.top)));
			return sb.ToString();
		}

		public static string BuildOlapLeftDimensions(string tableName, OlapStage stage)
		{
			return string.Format("SELECT DISTINCT {0} FROM {1}", string.Join(", ", stage.left), tableName);
		}

		public static string BuildOlapTopDimensions(string tableName, OlapStage stage)
		{
			return string.Format("SELECT DISTINCT {0} FROM {1}", string.Join(", ", stage.top), tableName);
		}
	}
}
