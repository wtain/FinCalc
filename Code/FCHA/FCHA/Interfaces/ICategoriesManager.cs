using FCHA.DataTypes;
using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface ICategoriesManager
    {
        long Add(string name, CategoryType type);
        long Add(string name, long parentId, CategoryType type);
        void Delete(Category cat);
        IEnumerable<Category> EnumAllCategories();
        IEnumerable<Category> EnumByParent(long parentId);
        void Update(Category cat);
    }
}