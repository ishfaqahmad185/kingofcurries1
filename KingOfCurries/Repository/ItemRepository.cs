using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {

        }








        public Boolean InsertItems(Item item)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("@ItemTitle", item.ItemTitle),
                    new SqlParameter("@ItemDetail", item.ItemDetail),
                    new SqlParameter("@ItemImage", item.ItemImage),
                    new SqlParameter("@ThumbnailImage", item.ThumbnailImage),
                    new SqlParameter("@CategoryId", item.CategoryId),
                    new SqlParameter("@SubCategoryId", item.SubCategoryId),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Priority", item.Priority),
                    new SqlParameter("@IsDiscount", item.IsDiscount),
                    new SqlParameter("@DiscountAmount", item.DiscountAmount),
                    new SqlParameter("@Slug",item.Slug),
                    new SqlParameter("@IsFeatureItem", item.IsFeatureItem),
                    new SqlParameter("@IsFlatDiscount", item.IsFlatDiscount),
                    new SqlParameter("@UserId", item.UserId),
                    //new SqlParameter("@ItemAllergen", item.ItemAllergen),


                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spItem_Insert", param);




            return data;


        }

           
        public Boolean UpdateItem(Item item)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("@ItemId", item.ItemId),
                    new SqlParameter("@ItemTitle", item.ItemTitle),
                    new SqlParameter("@ItemDetail", item.ItemDetail),
                    new SqlParameter("@ItemImage", item.ItemImage),
                    new SqlParameter("@ThumbnailImage", item.ThumbnailImage),
                    new SqlParameter("@CategoryId", item.CategoryId),
                    new SqlParameter("@SubCategoryId", item.SubCategoryId),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Priority", item.Priority),
                    new SqlParameter("@IsDiscount", item.IsDiscount),
                    new SqlParameter("@DiscountAmount", item.DiscountAmount),
                  
                    new SqlParameter("@IsFeatureItem", item.IsFeatureItem),
                    new SqlParameter("@IsFlatDiscount", item.IsFlatDiscount),
                    new SqlParameter("@UserId", item.UserId),
                    //new SqlParameter("@ItemAllergen", item.ItemAllergen),


                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spItem_Update", param);




            return data;
        }


        public IEnumerable<Item> GetAllItems()
        {
            List<Item> Balobj = new List<Item>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spItem_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Item item = new Item();
             

                Balobj.Add(BindItem(dataRow));
            }
            return Balobj;

            //return datatable;


        }

        public IEnumerable<Item> GetAllFilterItems(string name)
        {
            List<Item> Balobj = new List<Item>();
            SqlParameter[] param =
             {
                new SqlParameter("@Name", name),


                };


            DataTable datatable = SqlHelper.GetTableFromSP("GetAllFilterItems", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Item item = new Item();


                Balobj.Add(BindItem(dataRow));
            }
            return Balobj;

            //return datatable;


        }


        private Item BindItem(DataRow dataRow)
        {
            Item item = new Item();
            item.ItemId = dataRow["ItemId"].ToInt32();
            item.CategoryId = dataRow["CategoryId"].ToInt32();
            item.SubCategoryId = dataRow["SubCategoryId"].ToInt32();
            item.ItemId = dataRow["ItemId"].ToInt32();
            item.ItemTitle = dataRow["ItemTitle"].ToString();
            item.ItemAllergen = dataRow["ItemAllergen"].ToString();
            item.ItemDetail = dataRow["ItemDetail"].ToString();
            item.ItemImage = dataRow["ItemImage"].ToString();
            item.ThumbnailImage = dataRow["ThumbnailImage"].ToString();
            item.SubCategoryName = dataRow["subCategoryName"].ToString();
            item.CategoryName = dataRow["CategoryName"].ToString();
            item.Price = dataRow["Price"].ToDouble();
            item.Priority = dataRow["Priority"].ToInt32();
            item.IsDiscount = dataRow["IsDiscount"].ToBool();
            item.DiscountAmount = dataRow["DiscountAmount"].ToDouble();
            item.IsFeatureItem = dataRow["IsFeatureItem"].ToBool();
            item.IsFlatDiscount = dataRow["IsFlatDiscount"].ToBool();


            return item;    
        }



        public IEnumerable<Item> GetAllSubCategories()
        {
            List<Item> Balobj = new List<Item>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spddlSubcategory_Get", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Item item = new Item();
                item.SubCategoryId = dataRow["SubCategoryId"].ToInt32();
                item.SubCategoryName = dataRow["SubCategoryName"].ToString();
               


                Balobj.Add(item);
            }
            return Balobj;

            //return datatable;


        }

        public Item GetAllItemsById(int id)
        {
            Item item = new Item();
            SqlParameter[] param =
             {
                new SqlParameter("@ItemId",id)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("spItem_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {

                item.ItemId = dataRow["ItemId"].ToInt32();
                item.CategoryId = dataRow["CategoryId"].ToInt32();
                item.SubCategoryId = dataRow["SubCategoryId"].ToInt32();
                item.ItemId = dataRow["ItemId"].ToInt32();
                item.ItemTitle = dataRow["ItemTitle"].ToString();
                item.ItemAllergen = dataRow["ItemAllergen"].ToString();
                item.ItemDetail = dataRow["ItemDetail"].ToString();
                item.ItemImage = dataRow["ItemImage"].ToString();
                item.ThumbnailImage = dataRow["ThumbnailImage"].ToString();
                //item.SubCategoryName = dataRow["subCategoryName"].ToString();
               //item.CategoryName = dataRow["CategoryName"].ToString();
                item.Price = dataRow["Price"].ToDouble();
                item.Priority = dataRow["Priority"].ToInt32();
                item.IsDiscount = dataRow["IsDiscount"].ToBool();
                item.DiscountAmount = dataRow["DiscountAmount"].ToDouble();
                item.IsFeatureItem = dataRow["IsFeatureItem"].ToBool();
                item.IsFlatDiscount = dataRow["IsFlatDiscount"].ToBool();



            }
            return item;
        }
        public bool DeleteItems(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@ItemId",id),
          new SqlParameter("@UserId",UserId),

       };


            var data = SqlHelper.ExecuteProcedureNonQuery("spItem_Delete", param);




            return data;


        }
		public IEnumerable<Allergens> GetAllAllergens()
		{
			List<Allergens> Balobj = new List<Allergens>();
			SqlParameter[] param =
			 {


				};


			DataTable datatable = SqlHelper.GetTableFromSP("SPAllergen_GetAll", param);

			foreach (DataRow dataRow in datatable.Rows)
			{
				Allergens allergens = new Allergens();
				allergens.AllergenId = dataRow["AllergenId"].ToInt32();
				allergens.AllergenName = dataRow["AllergenName"].ToString();
				allergens.Detail = dataRow["Detail"].ToString();







				Balobj.Add(allergens);
			}
			return Balobj;

		}


	}

}