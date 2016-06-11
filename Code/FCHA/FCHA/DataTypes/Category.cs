
using FCHA.DataTypes;

namespace FCHA
{
	public class Category
	{
		public long categoryId;
		public long parentId;
		public string name;
        public CategoryType type;
        public double seqNo;

		public Category(string name, long categoryId, long parentId, CategoryType type, double seqNo)
		{
			this.categoryId = categoryId;
			this.name = name;
			this.parentId = parentId;
            this.type = type;
            this.seqNo = seqNo;
        }

		public Category(string name, long categoryId, CategoryType type, double seqNo)
            : this(name, categoryId, 0, type, seqNo)
		{
        }
	}
}