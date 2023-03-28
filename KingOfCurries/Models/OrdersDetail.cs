namespace KingofCurries.Models
{
    public class OrdersDetail
    {
        public int OrderDetailId { get; set; } = 0;

        public int InvoiceId { get; set; } = 0;

        public string InvoiceNo { get; set; }=string.Empty;

        public int ItemId { get; set; } = 0;

        public string ItemName { get; set; } = string.Empty;
        public string ItemImage { get; set; } = string.Empty;



        public int? SubItemId { get; set; } = 0;

        public string SubName { get; set; } = string.Empty;
        public string SubImage { get; set; } = string.Empty;

        public int? FreeItemId { get; set; } = 0;
        public string FreeName { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 0;

        public decimal TotalPrice { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        public bool? mDelete { get; set; } = false;
    }
}
