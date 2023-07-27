using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IUser
    { 
        public ApplicationUser()
        {
            Products = new List<Product>();
            Cart = new Cart();
        }

        public ICollection<Product> Products { get; set; } = null!;

        public Cart Cart { get; set; } = null!;
    }
}
