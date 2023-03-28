using KingofCurries.Models;
using Models;
using System.Collections.Generic;

namespace Services
{
     public interface IUserRepository 
    {
        int InsertUsers(Users Users);
        IEnumerable<Users> GetUsers();
        Users GetUsersById(int Userid);
        bool UpdateUsers(Users Users);
        bool DeleteUsers(int Userid, int UpdateBy);
        Users UserGetByEmail(string Email);
        void ResetUserPassword(int UserId, string newPassword);
        Users UserLogin(string Email, string Password);

        bool UserInsert(Address address);
        bool UpdateAddress(Address address);
        public bool Delete(int id, int UserId);

        IEnumerable<Address> GetAllUsersAddress();
       bool UpdateUserPassowrd(int UsrID, string password, string NewPassword);

		IEnumerable<Customers> CustomersGetAllCustomerAndOrders();


	}
}
