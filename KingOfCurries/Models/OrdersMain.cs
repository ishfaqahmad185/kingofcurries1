namespace KingofCurries.Models
{
    public class OrdersMain
    {
        public int OrderId { get; set; } = 0;

        public string OrderNo { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.MinValue;
        public string OrderStatus { get; set; } = string.Empty;
        public int UserId { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string ContactNo { get; set; } = string.Empty;

        public int StatusId { get; set; } = 0;

        public string StatusName { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Town { get; set; } = string.Empty;

        public int PaymentId { get; set; } = 0;

        public string paymentType { get; set; } = string.Empty;

        public string DeliveryType { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;

        public int DeliveryId { get; set; } = 0;

        public decimal TotalAmount { get; set; } = 0;

        public int DeliveryChargesId { get; set; } = 0;

        public decimal DeliveryCharges { get; set; } = 0;

        public decimal GrandTotal { get; set; } = 0;
        public int DeliveryTimeId { get; set; } = 0;

        public string Address2 { get; set; } = string.Empty;

        public bool mDelete { get; set; } = false;

        public int PraperationDuration { get; set; }
        public int RemainingTime { get; set; }=0;

        public string Systime { get; set; } = string.Empty;
        public string DeliveryLocation { get; set; } = string.Empty;
        public string Systime2 { get {
                return OrderDate.ToString("dd MMMM, yyyy");
            } }

        public DateTime DeliveryDate
        {
            get { return OrderDate.AddMinutes(PraperationDuration); }

        }
      
       
    
    }
}
