﻿using System.Data.SQLite;
using System.Collections.Generic;

namespace FCHA
{
	public class CategoriesManager
	{
		private SQLiteConnection m_conn;

		public CategoriesManager(SQLiteConnection conn)
		{
			m_conn = conn;
		}

		public IEnumerable<Category> EnumAllCategories()
		{
			return SelectCategories(QueryBuilder.Select(new string[] { "name", "categoryId", "parentId" }, "categories"));
		}

		public IEnumerable<Category> EnumCategoriesByParent(int parentId)
		{
			return SelectCategories(QueryBuilder.Select(new string[] { "name", "categoryId", "parentId" }, "categories", "parentId", parentId.ToString()));
		}

		public void AddCategory(string name)
		{
			string query = QueryBuilder.Insert("categories", new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(name)));
			SQLiteCommand insert = new SQLiteCommand(query, m_conn);
			insert.ExecuteNonQuery();
		}

		public void AddCategory(string name, int parentId)
		{
			string query = QueryBuilder.Insert("categories", new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(name)),
															 new KeyValuePair<string, string>("parentId", parentId.ToString()));
			SQLiteCommand insert = new SQLiteCommand(query, m_conn);
			insert.ExecuteNonQuery();
		}

		public void UpdateCategory(Category cat)
		{
			string query = QueryBuilder.Update("categories", "categoryId", cat.categoryId.ToString(),
				new KeyValuePair<string, string>("name", QueryBuilder.DecorateString(cat.name)),
				new KeyValuePair<string, string>("parentId", cat.parentId.ToString()));
			SQLiteCommand update = new SQLiteCommand(query, m_conn);
			update.ExecuteNonQuery();
		}

		public void DeleteCategory(Category cat)
		{
			string query = QueryBuilder.Delete("categories", "categoryId", cat.categoryId.ToString());
			SQLiteCommand delete = new SQLiteCommand(query, m_conn);
			delete.ExecuteNonQuery();
		}

		private IEnumerable<Category> SelectCategories(string query)
		{
			SQLiteCommand select = new SQLiteCommand(query, m_conn);
			SQLiteDataReader reader = select.ExecuteReader();

			while (reader.Read())
			{
				string name = reader.GetString(0);
				int categoryId = reader.GetInt32(1);
				int parentId = reader.GetInt32(2);
				yield return new Category(name, categoryId, parentId);
			}
		}
	}
}