using API.Utility;
using KingofCurries.Models;
using System.Data.SqlClient;
using System.Data;
using ExtensionMethods;
using Services;

namespace Repository
{
    public class BankHolidayRepository : IBankHolidayRepository 
    {
        public BankHolidayRepository()
        {

        }

        public Boolean Insert(BankHoliday bankHoliday)
        {


            DataTable datatable = new DataTable();
            SqlParameter[] param =
            {
                    new SqlParameter("@Title",bankHoliday.Title),
                    new SqlParameter("@Dates", bankHoliday.Dates.ToDate()),

                    new SqlParameter("@Status",bankHoliday.Status),
                    new SqlParameter("@UserId",bankHoliday.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_BankHoliday_Insert", param);




            return data;
        }

        public IEnumerable<BankHoliday> GetAllbankHoliday()
        {
            List<BankHoliday> Balobj = new List<BankHoliday>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_BankHoliday_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                BankHoliday bankHoliday = new BankHoliday();

                bankHoliday.Title = dataRow["Title"].ToString();
                bankHoliday.Dates = Convert.ToDateTime(dataRow["Dates"]); ;

                bankHoliday.Status = dataRow["Status"].ToBool();
                bankHoliday.BankHolidayId = dataRow["BankHolidayId"].ToInt32();
                bankHoliday.SysTime = Convert.ToDateTime(dataRow["Dates"]).ToString("dd-MMM-yyyy");

                Balobj.Add(bankHoliday);
            }
            return Balobj;

            //return datatable;


        }

        public BankHoliday GetAllbankHolidayById(int id)
        {
            BankHoliday bankHoliday = new BankHoliday();
            SqlParameter[] param =
             {
                new SqlParameter("@BankHolidayId",id)

                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_BankHoliday_GetById", param);

            foreach (DataRow dataRow in datatable.Rows)
            {

                bankHoliday.Title = dataRow["Title"].ToString();
                bankHoliday.Dates = Convert.ToDateTime(dataRow["Dates"]); ;

                bankHoliday.Status = dataRow["Status"].ToBool();
                bankHoliday.BankHolidayId = dataRow["BankHolidayId"].ToInt32();

            }
            return bankHoliday;
        }



        public Boolean UpdatebankHoliday(BankHoliday bankHoliday)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
                new SqlParameter("@Title",bankHoliday.Title),
                    new SqlParameter("@Dates", bankHoliday.Dates.ToDate()),

                    new SqlParameter("@Status",bankHoliday.Status),
                    new SqlParameter("@BankHolidayId",bankHoliday.BankHolidayId),
                    new SqlParameter("@UserId",bankHoliday.UserId),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_bankHoliday_Update", param);




            return data;


        }
        public Boolean DeletebankHoliday(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@BankHolidayId",id),
          new SqlParameter("@UserId",UserId),

       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_BankHoliday_Delete", param);




            return data;


        }









    }
}
