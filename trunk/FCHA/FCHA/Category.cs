
namespace FCHA
{
	public struct Category
	{
		public int categoryId;
		public int parentId;
		public string name;

		public Category(string name, int categoryId, int parentId)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = parentId;
		}

		public Category(string name, int categoryId)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = 0;
		}
	}
}