using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using System.Data;
using System.Data.SqlClient;

namespace HarrysRestro.Repository
{
    public class FreeItemsRepository : IFreeItemsRepository
    {
        public FreeItemsRepository()
        {
        }

        public Boolean InsertFreeItems(FreeItems freeItems)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("FreeItemTitle", freeItems.FreeItemTitle),
                    new SqlParameter("FreeItemDetail", freeItems.FreeItemDetails),
                    new SqlParameter("FreeItemImage", freeItems.FreeItemImage),
                    new SqlParameter("FreeItemThumbnail", freeItems.FreeItemThumbnail),
                    new SqlParameter("Priority", freeItems.Priority),
                    new SqlParameter("ItemId",freeItems.ItemId),
                    new SqlParameter("@Slug",freeItems.Slug),

          new SqlParameter("@UserId", freeItems.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spFreeItems_Insert", param);




            return data;


        }

        public IEnumerable<FreeItems> GetAllFreeItems()
        {
            List<FreeItems> Balobj = new List<FreeItems>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_FreeItems_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                FreeItems freeItems = new FreeItems();
                freeItems.FreeItemId = dataRow["FreeItemId"].ToInt32();
                freeItems.ItemId = dataRow["ItemId"].ToInt32();
                freeItems.ItemTitle = dataRow["ItemTitle"].ToString();
                freeItems.FreeItemTitle = dataRow["FreeItemTitle"].ToString();
                freeItems.FreeItemDetails = dataRow["FreeItemDetail"].ToString();
                freeItems.FreeItemImage = dataRow["FreeItemImage"].ToString();
                freeItems.FreeItemThumbnail = dataRow["FreeItemThumbnail"].ToString();
                freeItems.Slug = dataRow["slug"].ToString();
                freeItems.Priority = dataRow["Priority"].ToInt32();



                Balobj.Add(freeItems);
            }
            return Balobj;

            //return datatable;


        }
        public FreeItems GetAllFreeItemsById(int id)
        {
            FreeItems freeItems = new FreeItems();
            SqlParameter[] param =
             {
                new SqlParameter("@FreeItemId",id)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_FreeItems_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                freeItems.FreeItemId = dataRow["FreeItemId"].ToInt32();
                freeItems.ItemId= dataRow["ItemId"].ToInt32();
                freeItems.FreeItemTitle = dataRow["FreeItemTitle"].ToString();
                freeItems.FreeItemDetails = dataRow["FreeItemDetail"].ToString();
                freeItems.FreeItemImage = dataRow["FreeItemImage"].ToString();
                freeItems.FreeItemThumbnail = dataRow["FreeItemThumbnail"].ToString();
                freeItems.Slug = dataRow["Slug"].ToString();
                freeItems.Priority = dataRow["Priority"].ToInt32();





            }
            return freeItems;

            //return datatable;


        }
        public Boolean UpdateFreeItems(FreeItems freeItems)
         {
        DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("FreeItemId",freeItems.FreeItemId),
                    new SqlParameter("FreeItemTitle", freeItems.FreeItemTitle),
                    new SqlParameter("FreeItemDetail", freeItems.FreeItemDetails),
                    new SqlParameter("FreeItemImage", freeItems.FreeItemImage),
                    new SqlParameter("FreeItemThumbnail", freeItems.FreeItemThumbnail),
                    new SqlParameter("Priority", freeItems.Priority),
                    new SqlParameter("ItemId",freeItems.ItemId),
                    new SqlParameter("UserId", freeItems.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_FreeItems_Update", param);




            return data;


        }
        public Boolean DeleteFreeItem(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@FreeItemId",id),
          new SqlParameter("@UserId",UserId),

       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_FreeItems_Delete", param);




            return data;


        }


        public IEnumerable<Item> GetAllItems()
        {
            List<Item> Balobj = new List<Item>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("[sp_ddlItem]", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Item item = new Item();
                item.ItemId = dataRow["ItemId"].ToInt32();
                item.ItemTitle = dataRow["ItemTitle"].ToString();



                Balobj.Add(item);
            }
            return Balobj;

            //return datatable;


        }
    }
}
