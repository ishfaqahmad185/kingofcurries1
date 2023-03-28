
using Models;

namespace Services
{
    public interface ICustomerDataAccessLayer
    {
        int AddCustomer(Customers customer);
        Customers CustomerLogin(string Email, string Password);
        GenericUser GetCustomerById(int CusId);
    }
}