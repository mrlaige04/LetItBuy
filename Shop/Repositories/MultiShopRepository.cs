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

        public User GetUser(string id)
        {
            return _db.Users.AsEnumerable().FirstOrDefault(i => i.Id == id);
        }

        public void AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}
