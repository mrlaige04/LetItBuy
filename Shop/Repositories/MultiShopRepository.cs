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
        public IEnumerable<Item> GetAllItems()
        {
            var items = _db.Items.AsEnumerable();
            return items;
        }

        public IEnumerable<User> GetAllUser()
        {
            var users = _db.Users.AsEnumerable();
            return users;
        }

        public Item GetItemByID(Guid ItemId)
        {
            var item = _db.Items.FirstOrDefault(x => x.ItemId == ItemId);
            return item;
        }

        public User GetUser(Guid id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
    }
}
