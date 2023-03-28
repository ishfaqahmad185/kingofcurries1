namespace KingofCurries.Models
{
    public class OrderSummary
    {
        public double TotalCollectionPayment { get; set; } = 0;
        public int TotalCollectionOrders { get; set; } = 0;
        public double TotalDeliveryPayment { get; set; } = 0;
        public int TotalDeliveryOrders { get; set; } = 0;
        public double CashPayment { get; set; } = 0;
        public double CardPayment { get; set; } = 0;
        public double TotalPayment { get; set; } = 0;
        public double DeliveryCharges { get; set; } = 0;
    }
}
