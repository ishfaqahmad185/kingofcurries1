using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using Services;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        public SubCategoryRepository()
        {

        }
        public bool InsertSubCategory(SubCategory subCategory)
        {
            SqlParameter[] param =
                {
              new  SqlParameter("@CategoryId",subCategory.CategoryId),
              new  SqlParameter("@SubCategoryName",subCategory.SubCategoryName),
              new  SqlParameter("@SubcategoryDescription",subCategory.SubCategoryDescription),
              new  SqlParameter("@ImageUrl",subCategory.ImageUrl),
              new  SqlParameter("@Priority",subCategory.Priority),
              new  SqlParameter("@Features",subCategory.Features),
              new  SqlParameter("@user_id",subCategory.CreatedBy),
              new  SqlParameter("@Slug",subCategory.Slug),
                };
            SqlHelper.ExecuteProcedureReturnString("sp_SubCategory_Insert", param);
            return true;

        }


        public IEnumerable<SubCategory> GetAllSubCategory()
        {
            List<SubCategory> Balobj = new List<SubCategory>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_SubCategory_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                SubCategory SubCategory = new SubCategory();

                SubCategory.SubCategoryId = dataRow["SubCategoryId"].ToInt32();

                SubCategory.ImageUrl = dataRow["ImageUrl"].ToString();
                SubCategory.SubCategoryDescription = dataRow["SubCategoryDescription"].ToString();
                SubCategory.SubCategoryName = dataRow["SubCategoryName"].ToString();
                SubCategory.CategoryName = dataRow["CategoryName"].ToString();
                SubCategory.CategoryId = dataRow["CategoryId"].ToInt32();
                SubCategory.Priority = dataRow["Priority"].ToInt32();
                SubCategory.Features = dataRow["Features"].ToBool();
                SubCategory.isTakeAway = dataRow["isTakeAway"].ToBool();




                Balobj.Add(SubCategory);
            }
            return Balobj;

            //return datatable;


        }




        public IEnumerable<SubCategory> GetAllSubCategoryDDL(int CatId)
        {
            List<SubCategory> Balobj = new List<SubCategory>();
            SqlParameter[] param =
             {

                   new  SqlParameter("@CatId",CatId),
                };


            DataTable datatable = SqlHelper.GetTableFromSP("spSubCategoryDDL_GetByCategoryId", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                SubCategory SubCategory = new SubCategory();

                SubCategory.SubCategoryId = dataRow["SubCategoryId"].ToInt32();


                SubCategory.SubCategoryName = dataRow["SubCategoryName"].ToString();




                Balobj.Add(SubCategory);
            }
            return Balobj;

            //return datatable;


        }

        public SubCategory GetSubCategoryById(int SubCatid)
        {
            SubCategory SubCategory = new SubCategory();
            SqlParameter[] param =
             {

                   new  SqlParameter("@SubCategoryId",SubCatid),
                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_SubCategory_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {


                SubCategory.SubCategoryId = dataRow["SubCategoryId"].ToInt32();
                SubCategory.ImageUrl = dataRow["ImageUrl"].ToString();
                SubCategory.SubCategoryDescription = dataRow["SubCategoryDescription"].ToString();
                SubCategory.SubCategoryName = dataRow["SubCategoryName"].ToString();
                SubCategory.CategoryName = dataRow["CategoryName"].ToString();
                SubCategory.CategoryId = dataRow["CategoryId"].ToInt32();
                SubCategory.Priority = dataRow["Priority"].ToInt32();
                SubCategory.Features = dataRow["Features"].ToBool();





            }
            return SubCategory;

            //return datatable;


        }


        public bool DeleteSubCategory(int SubCatid, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@SubCategoryId",SubCatid),
          new SqlParameter("@user_id",UserId),

       };


            var data = SqlHelper.ExecuteProcedureNonQuery("sp_SubCategory_Delete", param);




            return data;


        }


        public bool UpdateSubCategory(SubCategory subCategory)
        {
            SqlParameter[] param =
                {
              new  SqlParameter("@subcategory_id",subCategory.SubCategoryId),
              new  SqlParameter("@category_id",subCategory.CategoryId),
              new  SqlParameter("@subcategory_name",subCategory.SubCategoryName),
              new  SqlParameter("@subcategory_description",subCategory.SubCategoryDescription),
              new  SqlParameter("@image_url",subCategory.ImageUrl),
              new  SqlParameter("@priority",subCategory.Priority),
              new  SqlParameter("@features",subCategory.Features),
              new  SqlParameter("@user_id",subCategory.CreatedBy),



                };
            SqlHelper.ExecuteProcedureReturnString("sp_SubCategory_Update", param);
            return true;

        }


    }
    }

