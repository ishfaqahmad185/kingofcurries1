namespace KingofCurries.Models
{
    public class DeliveryCharges
    {
       
            public int DeliveryChargesId { get; set; }
            public int UserId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }


        public DeliveryCharges()
        {
            DeliveryChargesId = 0;
            UserId = 0;
            Title = String.Empty;
            Price = 0;
        }

    }
} 
