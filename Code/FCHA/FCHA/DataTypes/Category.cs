
namespace FCHA
{
	public struct Category
	{
		public long categoryId;
		public long parentId;
		public string name;
        public bool isIncome;

		public Category(string name, long categoryId, long parentId, bool isIncome)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = parentId;
            this.isIncome = isIncome;
        }

		public Category(string name, long categoryId, bool isIncome)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = 0;
            this.isIncome = isIncome;
        }
	}
}