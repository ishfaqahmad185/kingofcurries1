using KingofCurries.Models;

namespace Services
{
    public interface ISubscriptionsRepository
    {
        void InsertSubscriptions(string email, string type);
        bool DeleteSubscriptions(int id);
        IEnumerable<Subcriptions> GetAllSucriptions();

    }
}