using Microsoft.AspNetCore.Http;
using System;

namespace KingofCurries.Models
{
    public class FreeItems
    {
        public int FreeItemId { get; set; }
        public string FreeItemTitle { get; set; }
        public string FreeItemDetails { get; set; }
        public string ItemTitle { get; set; }
        public string FreeItemImage { get; set; }
        public IFormFile FreeItemImageUrl { get; set; }
        public string FreeItemThumbnail { get; set; }
        public IFormFile FreeItemThumbnailUrl { get; set; }
        public int ItemId { get; set; }
        public int Priority { get; set; }
        public int UserId { get; set; }
        public string Slug { get; set; }


        public FreeItems()
        {
            FreeItemId = 0;
            FreeItemTitle = String.Empty;
            FreeItemDetails =String.Empty;
            ItemTitle = String.Empty;
            FreeItemImage=String.Empty;
            FreeItemThumbnail =String.Empty;
            Slug = String.Empty;
            ItemId =0;
            Priority =0;
            UserId =0;
        }
    }
}
