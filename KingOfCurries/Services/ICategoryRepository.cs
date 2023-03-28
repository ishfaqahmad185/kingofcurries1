using KingofCurries.Models;
using System.Collections.Generic;

namespace Services
{
    public interface ICategoryRepository
    {
        bool Addcategory(Category category);
        IEnumerable<Category> GetAllCategory();
        Category GetCategoryById(int CatId);
        bool  Updatecategory(Category category);
        bool DeleteCategory(int id, int UserId);
        
    }
}