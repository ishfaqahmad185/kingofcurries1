using System;

namespace KingofCurries.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IFormFile image { get; set; }
        public string ImageUrl { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public string Slug { get; set; }
        public int Priority { get; set; }
        public bool Features { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int updatedBy { get; set; }
        public int UserId { get; set; }

        public bool isTakeAway { get; set; }

        public SubCategory()
        {
            SubCategoryId = 0;
            CategoryId = 0;
            ImageUrl = String.Empty;
            CategoryName = String.Empty;
            SubCategoryName = string.Empty;
            Slug = string.Empty;
            SubCategoryDescription = string.Empty;
            Priority = 0;
            Features = false;
            UserId = 0;
            isTakeAway = true;
        }

    }

}

