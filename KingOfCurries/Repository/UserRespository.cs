using _Helper;
using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HarrysRestro.Repository
{
    public class UserRespository : IUserRepository
    {

        public UserRespository() {

}

        public int InsertUsers(Users Users)
        {

            DataTable datatable = new DataTable();

            SqlParameter[] param =
                  {
                //new SqlParameter("UserId", Users.UserId),
            new SqlParameter("@UserName", Users.UserName),
            new SqlParameter("@EmailAddress", Users.EmailAddress),
            new SqlParameter("@CustomerPhone", ""),
              new SqlParameter("@StatusId", Users.StatusId),
            new SqlParameter("@UserType", Users.UserType),
            new SqlParameter("@Password", Hashing.HashPassword(Users.Password)),
            new SqlParameter("@CreatedBy", 1),
          

                };

         
            return  SqlHelper.ExecuteProcedureReturnString("sp_User_Insert", param).ToInt32();

           
        }
        public IEnumerable<Users> GetUsers()
        {
            List<Users> Balobj = new List<Users>();

            SqlParameter[] param =
                   {

           
        };
            DataTable dataTable = SqlHelper.GetTableFromSP("sp_User_GetAll", param);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Users Users = new Users();
                Users.UserId = Convert.ToInt32(dataRow["UserId"].ToString());
                Users.UserName = dataRow["UserName"].ToString();
                Users.Phone = dataRow["CustomerPhone"].ToString();
                //Users.StoreName = dataRow["storeName"].ToString();
                Users.EmailAddress = dataRow["EmailAddress"].ToString();
                Users.StatusId = Convert.ToBoolean(dataRow["StatusId"].ToString());
                Users.UserType = Convert.ToInt32(dataRow["UserType"].ToString());
                //Users.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                //Users.CreatedBy = Convert.ToInt32(dataRow["CreatedBy"].ToString());
                //Users.UpdatedAt = Convert.ToDateTime(dataRow["UpdatedAt"].ToString());
                //Users.UpdatedBy = Convert.ToInt32(dataRow["UpdatedBy"].ToString());

                Balobj.Add(Users);
            }
            return Balobj;
        }
        public Users GetUsersById(int Userid)
        {
            Users Users = new Users();

            SqlParameter[] param =
                    {
                 new SqlParameter("UserId", Userid),
        };

            DataTable dataTable = SqlHelper.GetTableFromSP("sp_User_GetById", param);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                Users.UserId = Convert.ToInt32(dataRow["UserId"].ToString());
                Users.UserName = dataRow["UserName"].ToString();
                Users.Phone = dataRow["CustomerPhone"].ToString();
                Users.UserType = Convert.ToInt32(dataRow["UserType"].ToString());
                Users.StatusId = Convert.ToBoolean(dataRow["StatusId"].ToString());
                Users.EmailAddress = dataRow["EmailAddress"].ToString();
                Users.Password = dataRow["Password"].ToString();
                //Users.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"].ToString());
                //Users.CreatedBy = Convert.ToInt32(dataRow["CreatedBy"].ToString());
                //Users.UpdatedAt = Convert.ToDateTime(dataRow["UpdatedAt"].ToString());
                //Users.UpdatedBy = Convert.ToInt32(dataRow["UpdatedBy"].ToString());

            }
            return Users;
        }
        public Boolean UpdateUsers(Users Users)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
                     {

            new SqlParameter("UserId", Users.UserId),
            new SqlParameter("UserName", Users.UserName),
            new SqlParameter("EmailAddress", Users.EmailAddress),
           new SqlParameter("Password", Hashing.HashPassword(Users.Password)),
            new SqlParameter("UpdatedBy", Users.UpdatedBy),
            new SqlParameter("StatusId", Users.StatusId),
            new SqlParameter("UserType", Users.UserType),
        };
            

            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_User_Update", param);

            return data;
        }

        public Boolean DeleteUsers(int Userid, int UpdateBy)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
                     {

            new SqlParameter("UserId", Userid),
            new SqlParameter("UpdateBy", UpdateBy),
        };
           
            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_User_Delete", param);

            return data;
        }

        public Users UserLogin(string Email, string Password)
        {
            Users user = new Users();
            Status sts = new Status();

            SqlParameter[] param =
                       {
            new SqlParameter("EmailAdd", Email),
            };

            DataTable dataTable = SqlHelper.GetTableFromSP("spGetUserBy_email", param);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                sts = Hashing.ValidatePassword(Password, dataRow["Password"].ToString());
                if (sts.Success)
                {
                    user.UserId = Convert.ToInt32(dataRow["UserId"]);
                    user.UserName = dataRow["UserName"].ToString();
                    user.EmailAddress = dataRow["EmailAddress"].ToString();
                    user.UserType = dataRow["UserType"].ToString().ToInt32();
                }



            }
            return user;
        }


        public Users UserGetByEmail(string Email)
        {
            Users user = new Users();
            Status sts = new Status();

            SqlParameter[] param =
                     {
            new SqlParameter("EmailAdd", Email),
        };
            DataTable dataTable = SqlHelper.GetTableFromSP("spGetUserBy_email", param);
            if (dataTable == null)
            {
                return user;
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                user.UserId = Convert.ToInt32(dataRow["UserId"]);
                user.UserName = dataRow["UserName"].ToString();
                user.EmailAddress = dataRow["EmailAddress"].ToString();
                user.UserType = dataRow["UserType"].ToString().ToInt32();

            }
            return user;
        }
        public void ResetUserPassword(int UserId, string newPassword)
        {

            SqlParameter[] param =
                    {
            new SqlParameter("usrId", UserId),
            new SqlParameter("newPassword", Hashing.HashPassword(newPassword)),
        };


            SqlHelper.ExecuteProcedureNonQuery("spUser_ResetPassword", param);

        }

        public Boolean UserInsert(Address address)
        {


            DataTable datatable = new DataTable();
            SqlParameter[] param =
            {
                    new SqlParameter("@CountryName",address.Country),
                    new SqlParameter("@Town",address.Town),
                    new SqlParameter("@State",address.State),
                    new SqlParameter("@Address",address.Addresss),

                    new SqlParameter("@PostalCode",address.PostalCode),
                    new SqlParameter("@AddressType",address.AddreessType),
                    new SqlParameter("@ContactNo",address.ContactNo),
                    new SqlParameter("@UserId",address.UserId),




                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Address_Insert", param);




            return data;
        }

        public Boolean UpdateAddress(Address address)
        {

            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
            new SqlParameter("@CountryName",address.Country),
                    new SqlParameter("@Town",address.Town),
                    new SqlParameter("@State",address.State),
                    new SqlParameter("@Address",address.Addresss),

                    new SqlParameter("@PostalCode",address.PostalCode),
                    new SqlParameter("@AddressType",address.AddreessType),
                    new SqlParameter("@ContactNo",address.ContactNo),
                    new SqlParameter("@UserId",address.UserId),
                    new SqlParameter("@Id",address.Id),



                };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Address_Update", param);




            return data;


        }

        public IEnumerable<Address> GetAllUsersAddress()
        {
            List<Address> Balobj = new List<Address>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_Address_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                Address address = new Address();

                address.Country = dataRow["CountryName"].ToString();
                address.PostalCode = dataRow["PostalCode"].ToString();
                address.ContactNo = dataRow["ContactNo"].ToString();
                address.Town = dataRow["Town"].ToString();
                address.State = dataRow["State"].ToString();
                address.AddreessType = dataRow["AddressType"].ToString();
                address.Addresss = dataRow["Address"].ToString();


                address.Id = dataRow["AddressId"].ToInt32();
                address.UserId = dataRow["UserId"].ToInt32();
                Balobj.Add(address);
            }
            return Balobj;

            //return datatable;


        }


        public bool Delete(int id, int UserId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@Id",id),
          new SqlParameter("@UserId",UserId),

       };


            var data = SqlHelper.ExecuteProcedureNonQuery("sp_Address_Delete", param);




            return data;


        }

        public bool UpdateUserPassowrd(int UsrID, string password, string NewPassword)
        {
            Status sts = new Status();
            Users user = GetUsersById(UsrID);
            sts = Hashing.ValidatePassword(password, user.Password.ToString());
           


           

            if (sts.Success)
            {
                DataTable datatable = new DataTable();
                SqlParameter[] param =
                 {
          new SqlParameter("@usrId",UsrID),
          new SqlParameter("@usrPassword",Hashing.HashPassword(NewPassword)),

       };


                var data = SqlHelper.ExecuteProcedureNonQuery("spUsers_ChangePassword", param);


                return true;
            }
            else
            {
                return false;


            }

        }

		public IEnumerable<Customers> CustomersGetAllCustomerAndOrders()
		{
			List<Customers> Balobj = new List<Customers>();
			SqlParameter[] param =
			 {


				};


			DataTable datatable = SqlHelper.GetTableFromSP("spCustomers_GetAllCustomerAndOrders", param);

			foreach (DataRow dataRow in datatable.Rows)
			{
				Customers customers = new Customers();

	
			

				customers.CustomerId = dataRow["UserId"].ToInt32();
				customers.TotalOrderCount = dataRow["TotalOrders"].ToInt32();
				customers.CustomerName = dataRow["UserName"].ToString();
				customers.CustomerEmailAddress = dataRow["EmailAddress"].ToString();
				customers.CustomerPhone = dataRow["CustomerPhone"].ToString();
				customers.CreatedAt = dataRow["CreatedAt"].ToDate();

				Balobj.Add(customers);
			}
			return Balobj;

			//return datatable;


		}

	}
}
