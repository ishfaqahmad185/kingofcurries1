//using _Helper;
//using Models;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using ExtensionMethods;
//using System.Data.SqlClient;
//using API.Utility;
//using KingofCurries.Models;

//namespace Repository
//{
//    public class UserDataAccessLayer : IUserDataAccessLayer
//    {


//        public UserDataAccessLayer()
//        {

//        }


//        public int AddUsers(Users Users)
//        {


//            SqlParameter[] param =
//                  {
//                new SqlParameter("usrID", Users.UserId),
//            new SqlParameter("usrName", Users.UserName),
//            new SqlParameter("email", Users.EmailAddress),
//            new SqlParameter("Pass", Hashing.HashPassword(Users.Password)),
//            new SqlParameter("crId", Users.CreatedBy),
//            new SqlParameter("StsId", Users.StatusId),
//            new SqlParameter("usrType", Users.UserType),

//                };

//            return SqlHelper.ExecuteProcedureReturnString("spUser_Add", param).ToInt32();

//        }
//        public IEnumerable<Users> GetUsers(Users users)
//        {
//            List<Users> Balobj = new List<Users>();

//            SqlParameter[] param =
//                   {

//            new SqlParameter("usrName", users.UserName),
//            new SqlParameter("email", users.EmailAddress),
//        };
//            DataTable dataTable = SqlHelper.GetTableFromSP("spUserGetAll", param);
//            foreach (DataRow dataRow in dataTable.Rows)
//            {
//                Users Users = new Users();
//                Users.UserId = Convert.ToInt32(dataRow["UserId"].ToString());
//                Users.UserName = dataRow["UserName"].ToString();
//                Users.StoreName = dataRow["storeName"].ToString();
//                Users.EmailAddress = dataRow["EmailAddress"].ToString();
//                Users.UserType = Convert.ToInt32(dataRow["UserType"].ToString());

//                Balobj.Add(Users);
//            }
//            return Balobj;
//        }
//        public Users GetUsersById(int usrId)
//        {
//            Users Users = new Users();

//            SqlParameter[] param =
//                    {
//                 new SqlParameter("usrId", usrId),
//        };

//            DataTable dataTable = SqlHelper.GetTableFromSP("spGetUserById", param);
//            foreach (DataRow dataRow in dataTable.Rows)
//            {
//                Users.UserId = Convert.ToInt32(dataRow["UserId"].ToString());
//                Users.UserName = dataRow["UserName"].ToString();
//                Users.UserType = Convert.ToInt32(dataRow["UserType"].ToString());
//                Users.EmailAddress = dataRow["EmailAddress"].ToString();
//                Users.Password = dataRow["Password"].ToString();

//            }
//            return Users;
//        }
//        public int UpdateUsers(Users Users)
//        {
//            SqlParameter[] param =
//                     {

//            new SqlParameter("usrId", Users.UserId),
//            new SqlParameter("usrName", Users.UserName),
//            new SqlParameter("email", Users.EmailAddress),
//            new SqlParameter("UpdBy", Users.UpdatedBy),
//            new SqlParameter("usrType", Users.UserType),
//        };
//            return SqlHelper.ExecuteProcedureReturnString("spUserUpdate").ToInt32();
//        }

//        public bool DeleteUsers(int UsrID, int UpdateBy)
//        {
//            SqlParameter[] param =
//                     {

//            new SqlParameter("usrId", UsrID),
//            new SqlParameter("UpdId", UpdateBy),
//        };
//            return SqlHelper.ExecuteProcedureNonQuery("spUserdelete");
//        }
//        public GenericUser GetUserById(int CusId)
//        {
//            GenericUser users = new GenericUser();

//            SqlParameter[] param =
//                      {
//            new SqlParameter("UsrId", CusId),
//        };
//            DataTable dataTable = SqlHelper.GetTableFromSP("spUser_GetById", param);
//            foreach (DataRow dataRow in dataTable.Rows)
//            {
//                users.UserId = Convert.ToInt32(dataRow["UserId"]);
//                users.UserName = dataRow["UserName"].ToString();
//                users.UserEmail = dataRow["EmailAddress"].ToString();
//                users.userAddress.UserAddressId = Convert.ToInt32(dataRow["AddressId"]);
//                users.userAddress.UserCountryNmae = dataRow["CountryName"].ToString();
//                users.userAddress.UserCountyName = dataRow["CountyName"].ToString();
//                users.userAddress.UserPostalCode = dataRow["PostalCode"].ToString();
//                users.userAddress.Address = dataRow["Address"].ToString();
//                users.userAddress.Town = dataRow["Town"].ToString();
//                users.userAddress.state = dataRow["State"].ToString();
//                users.UserType = Convert.ToInt32(dataRow["UserType"].ToString());
//                users.userAddress.UserContactNumber = dataRow["ContactNo"].ToString();

//            }
//            return users;
//        }
//        public Users UserLogin(string Email, string Password)
//        {
//            Users user = new Users();
//            Status sts = new Status();

//            SqlParameter[] param =
//                       {
//            new SqlParameter("EmailAdd", Email),
//            };

//            DataTable dataTable = SqlHelper.GetTableFromSP("spGetUserBy_email", param);
//            foreach (DataRow dataRow in dataTable.Rows)
//            {
//                sts = Hashing.ValidatePassword(Password, dataRow["Password"].ToString());
//                if (sts.Success)
//                {
//                    user.UserId = Convert.ToInt32(dataRow["UserId"]);
//                    user.UserName = dataRow["UserName"].ToString();
//                    user.EmailAddress = dataRow["EmailAddress"].ToString();
//                    user.UserType = dataRow["UserType"].ToString().ToInt32();
//                }



//            }
//            return user;
//        }


//        public Users UserGetByEmail(string Email)
//        {
//            Users user = new Users();
//            Status sts = new Status();

//            SqlParameter[] param =
//                     {
//            new SqlParameter("EmailAdd", Email),
//        };
//            DataTable dataTable = SqlHelper.GetTableFromSP("spGetUserBy_email", param);
//            foreach (DataRow dataRow in dataTable.Rows)
//            {
//                user.UserId = Convert.ToInt32(dataRow["UserId"]);
//                user.UserName = dataRow["UserName"].ToString();
//                user.EmailAddress = dataRow["EmailAddress"].ToString();
//                user.UserType = dataRow["UserType"].ToString().ToInt32();

//            }
//            return user;
//        }
//        public void ResetUserPassword(int UserId, string newPassword)
//        {

//            SqlParameter[] param =
//                    {
//            new SqlParameter("usrId", UserId),
//            new SqlParameter("newPassword", Hashing.HashPassword(newPassword)),
//        };


//            SqlHelper.ExecuteProcedureNonQuery("spUser_ResetPassword", param);

//        }

//        public void AddOrUpdateUserAddress(UserAddress address)
//        {
//            SqlParameter[] param =
//                  {


//            new SqlParameter("AddrId", address.UserAddressId),
//            new SqlParameter("UsrId", address.UserId),
//            new SqlParameter("CountryNam", address.UserCountryNmae),
//            new SqlParameter("CountyNam", address.UserCountyName),
//            new SqlParameter("PostalCod", address.UserPostalCode),
//            new SqlParameter("Addres", address.Address),
//            new SqlParameter("Contact", address.Contact),
//            new SqlParameter("StsId", address.StatusId),
//            new SqlParameter("twn", address.Town),
//            new SqlParameter("Statee", address.state),
//        };
//            SqlHelper.ExecuteProcedureNonQuery("spUserAddress_Insertion_Or_Update", param);
//        }
//        public bool UpdateUserName(int UsrID, int UpdateBy, string UserName)
//        {

//            SqlParameter[] param =
//                  {
//            new SqlParameter("usrId", UsrID),
//            new SqlParameter("UpdId", UpdateBy),
//            new SqlParameter("usrName", UserName),
//        };
//            return SqlHelper.ExecuteProcedureNonQuery("spUser_UpdateUserName", param);
//        }
//        public bool UpdateUserPassowrd(int UsrID, int UpdateBy, string password, string NewPassword)
//        {
//            Status sts = new Status();
//            Users user = GetUsersById(UsrID);
//            sts = Hashing.ValidatePassword(password, user.Password.ToString());
//            if (sts.Success)
//            {
//                SqlParameter[] param =
//                  {


//                new SqlParameter("usrId", UsrID),
//                new SqlParameter("UpdId", UpdateBy),
//                new SqlParameter("usrPassword", Hashing.HashPassword(NewPassword)),
//            };
//                SqlHelper.ExecuteProcedureNonQuery("spUser_UpdateUserPassword", param);
//                return true;
//            }
//            else
//            {
//                return false;


//            }

//        }

//    }
//}
