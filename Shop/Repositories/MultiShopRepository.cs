using Shop.Data;
using Shop.Models;

namespace Shop.Repositories
{
    public class MultiShopRepository : IRepository
    {
        private ApplicationDBContext _db { get; }
        public MultiShopRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAllUser()
        {
            return _db.Users.ToList();
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _db.Items.ToList();
        }

        public Item GetItemByID(Guid ItemId)
        {
            return _db.Items.AsEnumerable().FirstOrDefault(i => i.ItemId == ItemId);
        }

        public IEnumerable<Item> GetItemsByName(string name)
        {
            return _db.Items.Where(i => i.ItemName == name).ToList();
        }
    }
}
