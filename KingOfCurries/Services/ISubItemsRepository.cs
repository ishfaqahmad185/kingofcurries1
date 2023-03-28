

using KingofCurries.Models;

namespace HarrysRestro.Repository
{
    public interface ISubItemsRepository
    {
        bool DeleteSubItem(int id, int UserId);
        IEnumerable<SubItems> GetAllSubItems();

        public IEnumerable<Item> GetAllItems();
        SubItems GetAllSubItemsById(int id);
        bool InsertSubItems(SubItems subItems);
        bool UpdateSubItems(SubItems subItems);
    }
}