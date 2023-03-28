using KingofCurries.Models;

namespace Services
{
    public interface IOrderRepository
    {
        bool InsertInvoiceDetail(int invoiceId, CartProducts placeOrder);
        bool OrdersConfirm(int id, int statusId);
        int InsertInvoiceMain(PlaceOrder placeOrder);

        IEnumerable<OrdersMain> GetAllOrdersMain(int OrderId);
        IEnumerable<OrdersDetail> GetAllOrderDetail(int id);
        IEnumerable<OrdersMain> GetAllOrders();

        IEnumerable<OrdersMain> GetAllOrdersSearch(string UserName, string OrderNo, string DeliveryType, DateTime FromDate, DateTime ToDate, int Status);


        bool InvoiceMainGetNewOrder();

        Boolean ChangeOrderTime(int id, int orderTime, int OrderType);

        Boolean OrderMainOrderConfirmation(int id, int statusId, string duration);
        IEnumerable<OrdersMain> GetAllOrderStatus();

    }
}