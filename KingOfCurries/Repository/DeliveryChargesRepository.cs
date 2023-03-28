using API.Utility;
using System.Data.SqlClient;
using System.Data;
using ExtensionMethods;
using KingofCurries.Models;
using Services;

namespace Repository
{
    public class DeliveryChargesRepository : IDeliveryChargesRepository
    { 
        public DeliveryChargesRepository()
        {

        }


        public Boolean Insert(DeliveryCharges deliveryCharges)
        {


            DataTable datatable = new DataTable();
            SqlParameter[] param =
            {
                    new SqlParameter("@Title",deliveryCharges.Title),
                    new SqlParameter("@Price", deliveryCharges.Price),

                    new SqlParameter("@UserId",deliveryCharges.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_DeliveryCharges_Insert", param);




            return data;
        }

        public IEnumerable<DeliveryCharges> GetAllDeliveryCharges()
        {
            List<DeliveryCharges> Balobj = new List<DeliveryCharges>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_DeliveryCharges_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                DeliveryCharges deliveryCharges = new DeliveryCharges();

                deliveryCharges.Title = dataRow["Title"].ToString();
                deliveryCharges.Price = dataRow["Price"].ToDecimal();

                deliveryCharges.DeliveryChargesId = dataRow["DeliveryChargesId"].ToInt32();
                Balobj.Add(deliveryCharges);
            }
            return Balobj;

            //return datatable;


        }

        public DeliveryCharges GetAllDeliveryChargesById(int id)
        {
            DeliveryCharges deliveryCharges = new DeliveryCharges();
            SqlParameter[] param =
             {
                new SqlParameter("@DeliveryChargesId",id)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_DeliveryCharges_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {


                deliveryCharges.Title = dataRow["Title"].ToString();
                deliveryCharges.Price = dataRow["Price"].ToDecimal();

                deliveryCharges.DeliveryChargesId = dataRow["DeliveryChargesId"].ToInt32();


            }
            return deliveryCharges;
        }



        public Boolean UpdateDeliveryCharges(DeliveryCharges deliveryCharges)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                  new SqlParameter("@Title",deliveryCharges.Title),
          new SqlParameter("@Price", deliveryCharges.Price),

          new SqlParameter("@UserId", deliveryCharges.UserId),
          new SqlParameter("@DeliveryChargesId", deliveryCharges.DeliveryChargesId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_DeliveryCharges_Update", param);




            return data;


        }
        public Boolean DeleteDeliveryCharges(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@DeliveryChargesId",id),
          new SqlParameter("@UserId",UserId),

       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_DeliveryCharges_Delete", param);




            return data;


        }

    }
}
