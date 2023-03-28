using KingofCurries.Models;

namespace Repository
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAllItems();
        bool DeleteItems(int id, int UserId);
        bool InsertItems(Item item);
        bool UpdateItem(Item item);
        IEnumerable<Item> GetAllSubCategories();
        IEnumerable<Item> GetAllFilterItems(string name);
         Item GetAllItemsById(int id);
		IEnumerable<Allergens> GetAllAllergens();
	}
}