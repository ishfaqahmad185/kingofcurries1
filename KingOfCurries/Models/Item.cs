namespace KingofCurries.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public string ItemDetail { get; set; }
        public string ItemImage { get; set; }

        public IFormFile IImage { get; set; }   
        public IFormFile TImage { get; set; }
        public string ThumbnailImage { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int CreatedBy { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }

        public string Slug { get; set; }
        public double Price { get; set; }
        public int Priority { get; set; }
        public Boolean IsDiscount { get; set; }
        public double DiscountAmount { get; set; }
        public Boolean IsFeatureItem { get; set; }
        public Boolean IsFlatDiscount { get; set; }  
        public Boolean IsSubItems { get; set; }
        public Boolean IsFreeItems { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get
            {
               return (decimal)(Price * Quantity);
            }
        }

        public int UserId { get; set; }

        public string ItemAllergen { get; set; }

        public Item()
        {
            CreatedBy = 0;
            ItemId = 0;
            ItemTitle = String.Empty;
            ItemDetail = String.Empty;
            ItemImage = String.Empty;
            ThumbnailImage = String.Empty;
            CategoryId = 0;
            SubCategoryId = 0;
            Price = 0;
            Slug = String.Empty;
            Priority = 0;
            IsDiscount = false;
            DiscountAmount =0;
            CategoryName=String.Empty;
            SubCategoryName=String.Empty;
            IsFeatureItem = false;
            IsFlatDiscount = false;
            IsSubItems= false;
            IsFreeItems=false;
            UserId = 0;
            ItemAllergen=string.Empty;

        }


}
  
}
