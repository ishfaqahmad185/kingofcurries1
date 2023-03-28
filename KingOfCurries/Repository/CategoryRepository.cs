using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using Services;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {


        public bool Addcategory(Category category)
        {
            SqlParameter[] param =
            {

                new SqlParameter("@CategoryName", category.CategoryName),
                new SqlParameter("@CategoryImageurl", category.CategoryImageurl),

                new SqlParameter("@CategoryDescription",category.CategoryDescription),
                new SqlParameter( "@CategoryPriority",category.CategoryPriority),
                new SqlParameter( "@Slug",category.Slug),
                new SqlParameter("@isFeature",category.isFeature),
                new SqlParameter("@UserId",category.UserId),


        };
            SqlHelper.ExecuteProcedureReturnString("sp_Category_Insert", param);
            return true;
        }

        public IEnumerable<Category> GetAllCategory()
        {
            List<Category> Balobj = new List<Category>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_Category_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Category category = new Category();
                category.CategoryName = dataRow["CategoryName"].ToString();
                category.CategoryImageurl = dataRow["CategoryImageurl"].ToString();
                category.CategoryDescription = dataRow["CategoryDescription"].ToString();
                category.CategoryPriority = dataRow["CategoryPriority"].ToInt32();
                category.isFeature = dataRow["isFeature"].ToBool();
                category.Slug = dataRow["Slug"].ToString();



                category.CategoryId = dataRow["CategoryId"].ToInt32();
                Balobj.Add(category);
            }
            return Balobj;
        }  
        
        
        
        public Category GetCategoryById(int CatId)
        {
            Category category = new Category();
            SqlParameter[] param =
             {
                new SqlParameter("@Category_Id",CatId)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_Category_GetById", param);
            foreach (DataRow dataRow in datatable.Rows)
            {
                
                category.CategoryName = dataRow["CategoryName"].ToString();
                category.CategoryImageurl = dataRow["CategoryImageurl"].ToString();
                category.CategoryDescription = dataRow["CategoryDescription"].ToString();
                category.CategoryPriority = dataRow["CategoryPriority"].ToInt32();
                category.isFeature = dataRow["isFeature"].ToBool();
                category.Slug = dataRow["Slug"].ToString();


                category.CategoryId = dataRow["CategoryId"].ToInt32();
           
            }
            return category;
        }



        public bool Updatecategory(Category category)
        {
            SqlParameter[] param =
            {

                new SqlParameter("@Category_Id", category.CategoryId),
                new SqlParameter("@Category_Name", category.CategoryName),
                new SqlParameter("@Category_imageurl", category.CategoryImageurl),

                new SqlParameter("@Category_Description",category.CategoryDescription),
                new SqlParameter( "@Category_priority",category.CategoryPriority),
                //new SqlParameter( "@Slug",category.Slug),
                new SqlParameter("@Isfeature",category.isFeature),
                new SqlParameter("@UserId",category.UserId),



        };
            SqlHelper.ExecuteProcedureReturnString("sp_Category_Update", param);
            return true;
        }

        public bool DeleteCategory(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@CategoryId",id),
          new SqlParameter("@UserId",UserId),

       };

            var data = SqlHelper.ExecuteProcedureNonQuery("sp_Category_Delete", param);

            return data;

        }

	

	}
}
