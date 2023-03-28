using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Repository
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {

        public void InsertSubscriptions(string email, string type)
        {
            SqlParameter[] param =
         {
                new SqlParameter("@EmailAddress", email),
            new SqlParameter("@SubscriptionType", type),


                };

            SqlHelper.ExecuteProcedureNonQuery("spSubscriptions_Insert", param);

        }

        public IEnumerable<Subcriptions> GetAllSucriptions()
        {
            List<Subcriptions> Balobj = new List<Subcriptions>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_Subcriptions_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Subcriptions subcriptions = new Subcriptions();



                subcriptions.SubcriptionsId = dataRow["SubscriptionId"].ToInt32();
                subcriptions.EmailAddress = dataRow["EmailAddress"].ToString();
                subcriptions.type = dataRow["SubscriptionType"].ToString();
                subcriptions.Date = Convert.ToDateTime(dataRow["SubscriptionDate"].ToString()) ;  
                subcriptions.Status = dataRow["Status"].ToBool();

                Balobj.Add(subcriptions);
            }
            return Balobj;

            //return datatable;


        }

        public Boolean DeleteSubscriptions(int id)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@SubscriptionId",id),
          

       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Subscriptions_Delete", param);




            return data;


        }
    }
}
