namespace KingofCurries.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }
         public string CategoryImageurl { get; set; }
        public string CategoryDescription { get; set; }
        public string Slug { get; set; }
        public int CategoryPriority { get; set; }
        public Boolean isFeature { get; set; }
        public int UserId { get; set; }

        public Category()
        {
            CategoryId = 0;
            CategoryName = string.Empty;
            CategoryImageurl = string.Empty;

            CategoryDescription = string.Empty;
            CategoryPriority = 0;
            isFeature = false;
            UserId = 0;
            Slug= string.Empty;
        }
    }
}
