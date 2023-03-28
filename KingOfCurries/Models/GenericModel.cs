namespace KingofCurries.Models
{
    public class GenericModel
    {
        public Category Category { get; set; }= new Category();
        public List<Category> ListCategory { get; set; }=new List<Category>();
        public SubCategory SubCategory { get; set; } = new SubCategory();
        public List<SubCategory> ListSubCategory { get; set; } = new List<SubCategory>();
        public Item Item { get; set; } = new Item();
        public List<Item> ListItem { get; set; } = new List<Item>();
        public List<OrdersMain> LOrder { get; set; } = new List<OrdersMain>();

        public Allergens allergens { get; set; }=new Allergens();

        public List<Allergens> ListAllergens { get; set; } = new List<Allergens>();

        public OrderSummary OrderSummary { get; set; }=new OrderSummary();
        public int CategoryId { get; set; } = 0;
        public int SubCategoryId { get; set; } = 0;
        public int ItemId { get; set; } = 0;
    }
}
