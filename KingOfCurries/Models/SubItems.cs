
    namespace KingofCurries.Models
    {
        public class SubItems
        {
            public int SubItemId { get; set; }
            public string SubItemTitle { get; set; }
            public string SubItemDetails { get; set; }
            public string ItemTitle { get; set; }
            public string SubItemImage { get; set; }
            public IFormFile SubItemImageUrl { get; set; }
            public string SubItemThumbnail { get; set; }
            public string Slug { get; set; }
            public IFormFile SubItemThumbnailUrl { get; set; }
            public int ItemId { get; set; }
            public int Priority { get; set; }
            public int UserId { get; set; }
            public double SubItemPrice { get; set; } 
		public int Quantity { get; set; }  

		public decimal TotalPrice
		{
			get
			{
				return (decimal)(SubItemPrice * Quantity);
			}
		}

		public SubItems()
            {
                SubItemId = 0;
                SubItemTitle = String.Empty;
                SubItemDetails = String.Empty;
                ItemTitle = String.Empty;
                SubItemImage = String.Empty;
                SubItemThumbnail = String.Empty;
                Slug = String.Empty;
                ItemId = 0;
                Priority = 0;
                UserId = 0;
            SubItemPrice = 0.0;
            }
        }
    }

