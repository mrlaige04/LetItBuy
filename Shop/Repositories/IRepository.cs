using Shop.Models;

namespace Shop.Repositories
{
    public interface IRepository
    {
        IEnumerable<Item> GetAllItems();
        Item GetItemByID(Guid ItemId);
        IEnumerable<Item> GetItemsByName(string name);
    }
}
