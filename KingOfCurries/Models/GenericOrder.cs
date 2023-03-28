namespace KingofCurries.Models
{
    public class GenericOrder
    {

        public OrdersMain OrdersMain { get; set; }=new OrdersMain();

        public List<OrdersMain> ListOrdersMains { get; set; } = new List<OrdersMain>();

        public OrdersDetail OrdersDetail { get; set; }=new OrdersDetail();

        public List<OrdersDetail> ListOrderDetail { get; set; }= new List<OrdersDetail>();
    }
}
