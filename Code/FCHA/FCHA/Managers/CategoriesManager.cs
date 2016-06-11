using System.Data.SQLite;
using System.Collections.Generic;
using FCHA.Interfaces;
using FCHA.DataTypes;

namespace FCHA
{
	public class CategoriesManager : ICategoriesManager
    {
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = new string[] { "name", "categoryId", "parentId", "SeqNo", "Type" };

		public CategoriesManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Category> EnumAllCategories()
		{
			return Select(QueryBuilder.Ordered(QueryBuilder.Select(Columns, "categories"), "SeqNo"));
		}

		public IEnumerable<Category> EnumByParent(long parentId)
		{
			return Select(QueryBuilder.Ordered(QueryBuilder.Select(Columns, "categories", "parentId", parentId.ToString()), "SeqNo"));
		}

		public long Add(string name, CategoryType type)
		{
			return Add(name, 0, type);
		}

		private KeyValuePair<string, string> GetNameColumnPair(string name)
		{
			return new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(name));
		}

		private KeyValuePair<string, string> GetParentIdColumnPair(long parentId)
		{
			return new KeyValuePair<string, string>("parentId", parentId.ToString());
		}

        private KeyValuePair<string, string> GetTypeColumnPair(CategoryType type)
        {
            return new KeyValuePair<string, string>("Type", CategoryTypeHelper.CategoryTypeToString(type));
        }

        public long Add(string name, long parentId, CategoryType type)
		{
            string query = QueryBuilder.Insert("categories", GetNameColumnPair(name),
															      GetParentIdColumnPair(parentId),
                                                                  GetTypeColumnPair(type));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				return (long) insert.ExecuteScalar();
		}

		public void Update(Category cat)
		{
			string query = QueryBuilder.Update("categories", "categoryId", cat.categoryId.ToString(),
														          GetNameColumnPair(cat.name),
																  GetParentIdColumnPair(cat.parentId),
                                                                  GetTypeColumnPair(cat.type));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void Delete(Category cat)
		{
			string query = QueryBuilder.Delete("categories", "categoryId", cat.categoryId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}

		private IEnumerable<Category> Select(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
					{
						string name = reader.GetString(0);
						long categoryId = reader.GetInt64(1);
						long parentId = reader.GetInt64(2);
                        double seqNo = reader.GetDouble(3);
                        CategoryType type = CategoryTypeHelper.CategoryTypeFromString(reader.GetString(4));
						yield return new Category(name, categoryId, parentId, type, seqNo);
					}
				}
		}
	}
}