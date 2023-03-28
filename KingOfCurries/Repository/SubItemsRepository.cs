using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using System.Data;
using System.Data.SqlClient;


namespace HarrysRestro.Repository
{
    public class SubItemsRepository : ISubItemsRepository
    {
        public SubItemsRepository()
        {
        }

        public Boolean InsertSubItems(SubItems subItems)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("@SubItemTitle", subItems.SubItemTitle),
                    new SqlParameter("@SubItemDetail", subItems.SubItemDetails),
                    new SqlParameter("@SubItemImage", subItems.SubItemImage),
                    new SqlParameter("@SubItemThumbnail", subItems.SubItemThumbnail),
                    new SqlParameter("@Priority", subItems.Priority),
                    new SqlParameter("@ItemId",subItems.ItemId),
                    new SqlParameter("@Slug",subItems.Slug),
                    new SqlParameter("@SubItemPrice",subItems.SubItemPrice),

          new SqlParameter("@UserId", subItems.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spSubItems_Insert", param);




            return data;


        }

        public IEnumerable<SubItems> GetAllSubItems()
        {
            List<SubItems> Balobj = new List<SubItems>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_SubItems_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                SubItems subItems = new SubItems();
                subItems.SubItemId = dataRow["SubItemId"].ToInt32();
                subItems.ItemTitle = dataRow["ItemTitle"].ToString();
                subItems.SubItemTitle = dataRow["SubItemTitle"].ToString();
                subItems.SubItemDetails = dataRow["SubItemDetail"].ToString();
                subItems.SubItemImage = dataRow["SubItemImage"].ToString();
                subItems.SubItemThumbnail = dataRow["SubItemThumbnail"].ToString();
                subItems.ItemId = dataRow["ItemId"].ToInt32();
                //subItems.Slug = dataRow["slug"].ToString();
                subItems.Priority = dataRow["Priority"].ToInt32();
                subItems.Slug = dataRow["Slug"].ToString();
                subItems.SubItemPrice = dataRow["SubItemPrice"].ToDouble();



                Balobj.Add(subItems);
            }
            return Balobj;

            //return datatable;


        }
        public SubItems GetAllSubItemsById(int id)
        {
            SubItems subItems = new SubItems();
            SqlParameter[] param =
             {
                new SqlParameter("@SubItemId",id)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_SubItems_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                subItems.SubItemId = dataRow["SubItemId"].ToInt32();
                subItems.SubItemTitle = dataRow["SubItemTitle"].ToString();
                subItems.SubItemDetails = dataRow["SubItemDetail"].ToString();
                subItems.SubItemImage = dataRow["SubItemImage"].ToString();
                subItems.SubItemThumbnail = dataRow["SubItemThumbnail"].ToString(); 
                subItems.Slug = dataRow["Slug"].ToString();
                subItems.SubItemPrice = dataRow["SubItemPrice"].ToDouble();
                subItems.Priority = dataRow["Priority"].ToInt32();
                subItems.ItemId = dataRow["ItemId"].ToInt32();





            }
            return subItems;

            //return datatable;


        }
        public Boolean UpdateSubItems(SubItems subItems)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                    new SqlParameter("SubItemId",subItems.SubItemId),
                    new SqlParameter("SubItemTitle", subItems.SubItemTitle),
                    new SqlParameter("SubItemDetail", subItems.SubItemDetails),
                    new SqlParameter("SubItemImage", subItems.SubItemImage),
                    new SqlParameter("SubItemThumbnail", subItems.SubItemThumbnail),
                    new SqlParameter("Priority", subItems.Priority),
                    new SqlParameter("SubItemPrice", subItems.SubItemPrice),
                    new SqlParameter("ItemId",subItems.ItemId),
                    new SqlParameter("UserId", subItems.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_SubItems_Update", param);

            return data;

        }
        public Boolean DeleteSubItem(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@SubItemId",id),
          new SqlParameter("@UserId",UserId),

       };

            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_SubItems_Delete", param);
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
