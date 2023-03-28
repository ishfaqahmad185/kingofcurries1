
using KingofCurries.Models;

namespace Services
{
    public interface ISubCategoryRepository
    {
		bool InsertSubCategory(SubCategory subCategory);
		IEnumerable<SubCategory> GetAllSubCategory();

		bool UpdateSubCategory(SubCategory subCategory);
        IEnumerable<SubCategory> GetAllSubCategoryDDL(int SubCatid);
		SubCategory GetSubCategoryById(int SubCatid);
		bool DeleteSubCategory(int SubCatid, int UserId);


	}
}