using System.Data.SQLite;
using System.Collections.Generic;
using FCHA.Interfaces;

namespace FCHA
{
	public class CategoriesManager : ICategoriesManager
    {
		private SQLiteConnection m_conn;

		private static readonly string[] Columns = new string[] { "name", "categoryId", "parentId", "SeqNo", "IsIncome" };

		public CategoriesManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Category> EnumAllCategories()
		{
			return SelectCategories(QueryBuilder.Ordered(QueryBuilder.Select(Columns, "categories"), "SeqNo"));
		}

		public IEnumerable<Category> EnumCategoriesByParent(long parentId)
		{
			return SelectCategories(QueryBuilder.Ordered(QueryBuilder.Select(Columns, "categories", "parentId", parentId.ToString()), "SeqNo"));
		}

		public long AddCategory(string name, bool isIncome)
		{
			return AddCategory(name, 0, isIncome);
		}

		private KeyValuePair<string, string> GetNameColumnPair(string name)
		{
			return new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(name));
		}

		private KeyValuePair<string, string> GetParentIdColumnPair(long parentId)
		{
			return new KeyValuePair<string, string>("parentId", parentId.ToString());
		}

        private KeyValuePair<string, string> GetIsIncomeColumnPair(bool isIncome)
        {
            return new KeyValuePair<string, string>("IsIncome", isIncome ? "1" : "0");
        }

        public long AddCategory(string name, long parentId, bool isIncome)
		{
            //InsertView
            // "categories_view", "CategoryId",
            string query = QueryBuilder.Insert("categories", GetNameColumnPair(name),
															      GetParentIdColumnPair(parentId),
                                                                  GetIsIncomeColumnPair(isIncome));
			using (SQLiteCommand insert = new SQLiteCommand(query, m_conn))
				return (long) insert.ExecuteScalar();
		}

		public void UpdateCategory(Category cat)
		{
			string query = QueryBuilder.Update("categories", "categoryId", cat.categoryId.ToString(),
														          GetNameColumnPair(cat.name),
																  GetParentIdColumnPair(cat.parentId),
                                                                  GetIsIncomeColumnPair(cat.isIncome));
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
                        long seqNo = reader.GetInt64(3);
                        bool isIncome = false;
                        if (!reader.IsDBNull(4))
                            isIncome = reader.GetBoolean(4);
						yield return new Category(name, categoryId, parentId, isIncome);
					}
				}
		}
	}
}