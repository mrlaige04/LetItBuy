using Shop.Models;

namespace Shop.Repositories
{
    public interface IRepository
    {
        IEnumerable<User> GetAllUser();
        IEnumerable<Item> GetAllItems();
        Item GetItemByID(Guid ItemId);
        User GetUser(Guid id);
        void AddUser(User user);
    }
}
