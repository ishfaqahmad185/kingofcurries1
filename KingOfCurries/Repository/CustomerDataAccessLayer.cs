using _Helper;
using API.Utility;
using ExtensionMethods;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class CustomerDataAccessLayer : ICustomerDataAccessLayer
    {

        public CustomerDataAccessLayer()
        {

        }

        public int AddCustomer(Customers customer)
        {

            SqlParameter[] param =
           {
                new SqlParameter("@UserName", customer.CustomerName),
            new SqlParameter("@EmailAddress", customer.CustomerEmailAddress),
            new SqlParameter("@CustomerPhone", customer.CustomerPhone),
            new SqlParameter("@Password", Hashing.HashPassword(customer.CustomerPassword)),
            new SqlParameter("@StatusId", 1),
            new SqlParameter("@UserType", 3),
            new SqlParameter("@CreatedBy", -1),

                };

            return SqlHelper.ExecuteProcedureReturnString("sp_User_Insert", param).ToInt32();






        }

        public GenericUser GetCustomerById(int CusId)
        {
            GenericUser customers = new GenericUser();
            SqlParameter[] param =
         {
                new SqlParameter("CusId", CusId),


                };

            DataTable dataTable = SqlHelper.GetTableFromSP("spCustomer_GetById", param);



            //foreach (DataRow dataRow in dataTable.Rows)
            //{
            //    customers.UserId = Convert.ToInt32(dataRow["customerId"]);
            //    customers.UserName = dataRow["customerName"].ToString();
            //    customers.UserEmail = dataRow["emailAddress"].ToString();
            //    customers.userAddress.UserAddressId = Convert.ToInt32(dataRow["UserAddressId"]);
            //    customers.userAddress.UserCountryNmae = dataRow["CountryName"].ToString();
            //    customers.userAddress.UserCountyName = dataRow["CountyName"].ToString();
            //    customers.userAddress.UserPostalCode = dataRow["PostalCode"].ToString();
            //    customers.userAddress.Address = dataRow["Address"].ToString();
            //    customers.userAddress.UserContactNumber = dataRow["ContactNo"].ToString();

            //}
            return customers;
        }
        public Customers CustomerLogin(string Email, string Password)
        {
            Customers user = new Customers();
            Status sts = new Status();

            SqlParameter[] param =
   {
                new SqlParameter("EmailAdd", Email),


                };

            DataTable dataTable = SqlHelper.GetTableFromSP("spCustomer_GetByEmail", param);


            foreach (DataRow dataRow in dataTable.Rows)
            {
                sts = Hashing.ValidatePassword(Password, dataRow["customerPassword"].ToString());
                if (sts.Success)
                {
                    user.CustomerId = Convert.ToInt32(dataRow["customerId"]);
                    user.CustomerName = dataRow["customerName"].ToString();
                    user.CustomerEmailAddress = dataRow["emailAddress"].ToString();
                }
            }
            return user;
        }

    }
}
