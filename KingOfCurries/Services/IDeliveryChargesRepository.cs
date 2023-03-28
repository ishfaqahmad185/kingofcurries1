using KingofCurries.Models;

namespace Services
{
    public interface IDeliveryChargesRepository
    {
        bool DeleteDeliveryCharges(int id, int UserId);
        IEnumerable<DeliveryCharges> GetAllDeliveryCharges();
        DeliveryCharges GetAllDeliveryChargesById(int id); 
        bool Insert(DeliveryCharges deliveryCharges);
        bool UpdateDeliveryCharges(DeliveryCharges deliveryCharges);
    }
}