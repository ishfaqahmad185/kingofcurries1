namespace KingofCurries.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string ImageUrl { get; set; }

        public Gallery()
        {
            Id = 0; 
            Name =String.Empty;
            Count = 0;
            ImageUrl = String.Empty;

        }
    }
}
