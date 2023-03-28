using Newtonsoft.Json.Linq;

namespace KingofCurries.Models
{
    public class CartModel
    {
        public int Id { get; set; } = 0;
        public int DeliveryId { get; set; } = 0;
        public string Name { get; set; }=string.Empty;


       public List<CartProducts> productIds=new List<CartProducts>();

		public decimal TotalPrice { get; set; } = 0;
		public decimal GrandTotal {
			get
			{

				return (DeliveryCharges == 0 ? TotalPrice : DeliveryCharges + TotalPrice);
			}
		}
		public decimal DeliveryCharges { get; set; } = 0;
    }


    public class CartProducts
	{
		public string UniqueId { get;set; }=string.Empty;
		public int ProductId { get; set; } = -1;
		public int Quantity { get; set; } = 0;
		public int SubProductId { get; set; } = 0;
		public int FreeProductId { get; set; } = 0;
		public decimal Price { get; set; } = 0;



		public decimal TotalPrice
		{
			get
			{
				return (decimal)(Price * Quantity);
			}
		}




	}


  public class ListCart
	{
		public int ProductId { get; set; } = 0; 
		public string ProductSlug { get; set; } = string.Empty; 
		public int SubItemId { get; set;} = 0;
		public int FreeItemId { get; set; } = 0;

		public double Price { get; set; } = 0.0;
		public int Quantity { get; set; } = 0;
		
		public SubItems SubItems { get; set;}=new SubItems();
		public FreeItems FreeItems { get; set;}=new FreeItems();
		public Item Items { get; set;}=new Item();

		public double TotalPrice { get {
				return Price * Quantity;
			} }
	}
}
