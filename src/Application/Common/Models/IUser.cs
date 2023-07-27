using Domain.Entities;

namespace Application.Common.Models
{
    public interface IUser
    {
        public Guid Id { get; set; }
        public ICollection<Product> Products { get; }
        public Cart Cart { get; }
        //public ICollection<Order> Orders { get; }
    }
}
