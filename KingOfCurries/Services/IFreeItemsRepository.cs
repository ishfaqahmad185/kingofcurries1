using KingofCurries.Models;

namespace HarrysRestro.Repository
{
    public interface IFreeItemsRepository
    {
        bool DeleteFreeItem(int id, int UserId);
        IEnumerable<FreeItems> GetAllFreeItems();

        public IEnumerable<Item> GetAllItems();
        FreeItems GetAllFreeItemsById(int id);
        bool InsertFreeItems(FreeItems freeItems);
        bool UpdateFreeItems(FreeItems freeItems);
    }
}