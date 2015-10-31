using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface ICategoriesManager
    {
        long Add(string name, bool isIncome);
        long Add(string name, long parentId, bool isIncome);
        void Delete(Category cat);
        IEnumerable<Category> EnumAllCategories();
        IEnumerable<Category> EnumByParent(long parentId);
        void Update(Category cat);
    }
}