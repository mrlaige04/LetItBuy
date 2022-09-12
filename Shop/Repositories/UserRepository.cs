using Shop.Data;
using Shop.Models;

namespace Shop.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDBContext _db;
        public UserRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public void Add(User entity)
        {
            _db.Users.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(User entity)
        {
            _db.Users.Remove(entity);
            _db.SaveChanges();
        }
        
        public IEnumerable<User> GetAll()
        {
            var users = _db.Users.ToList();
            return users;
        }



        public User GetById(string id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public void Update(User entity)
        {
            var use = _db.Users.Update(entity);
        }
    }
}
