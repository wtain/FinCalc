using System.Collections.Generic;

namespace FCHA.Interfaces
{
    public interface ICategoriesManager
    {
        long AddCategory(string name, bool isIncome);
        long AddCategory(string name, long parentId, bool isIncome);
        void DeleteCategory(Category cat);
        IEnumerable<Category> EnumAllCategories();
        IEnumerable<Category> EnumCategoriesByParent(long parentId);
        void UpdateCategory(Category cat);
    }
}