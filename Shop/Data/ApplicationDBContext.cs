using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
