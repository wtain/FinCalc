using System.Data.SQLite;
using System.Collections.Generic;

namespace FCHA
{
	public class CategoriesManager
	{
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = new string[] { "name", "categoryId", "parentId" };

		public CategoriesManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Category> EnumAllCategories()
		{
			return SelectCategories(QueryBuilder.Select(Columns, "categories"));
		}

		public IEnumerable<Category> EnumCategoriesByParent(long parentId)
		{
			return SelectCategories(QueryBuilder.Select(Columns, "categories", "parentId", parentId.ToString()));
		}

		public void AddCategory(string name)
		{
			AddCategory(name, 0);
		}

		private KeyValuePair<string, string> GetNameColumnPair(string name)
		{
			return new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(name));
		}

		private KeyValuePair<string, string> GetParentIdColumnPair(long parentId)
		{
			return new KeyValuePair<string, string>("parentId", parentId.ToString());
		}

		public void AddCategory(string name, long parentId)
		{
			string query = QueryBuilder.Insert("categories_view", GetNameColumnPair(name),
															      GetParentIdColumnPair(parentId));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				insert.ExecuteNonQuery();
		}

		public void UpdateCategory(Category cat)
		{
			string query = QueryBuilder.Update("categories", "categoryId", cat.categoryId.ToString(),
														          GetNameColumnPair(cat.name),
																  GetParentIdColumnPair(cat.parentId));
			using (SQLiteCommand update = new SQLiteCommand(query, m_conn))
				update.ExecuteNonQuery();
		}

		public void DeleteCategory(Category cat)
		{
			string query = QueryBuilder.Delete("categories", "categoryId", cat.categoryId.ToString());
			using (SQLiteCommand delete = new SQLiteCommand(query, m_conn))
				delete.ExecuteNonQuery();
		}

		private IEnumerable<Category> SelectCategories(string query)
		{
			using (SQLiteCommand select = new SQLiteCommand(query, m_conn))
				using (SQLiteDataReader reader = select.ExecuteReader())
				{
					while (reader.Read())
					{
						string name = reader.GetString(0);
						long categoryId = reader.GetInt64(1);
						long parentId = reader.GetInt64(2);
						yield return new Category(name, categoryId, parentId);
					}
				}
		}
	}
}