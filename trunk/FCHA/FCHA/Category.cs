
namespace FCHA
{
	public struct Category
	{
		public long categoryId;
		public long parentId;
		public string name;

		public Category(string name, long categoryId, long parentId)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = parentId;
		}

		public Category(string name, long categoryId)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = 0;
		}
	}
}