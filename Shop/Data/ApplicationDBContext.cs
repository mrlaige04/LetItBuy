using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Catalog> Catalogs { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Sell> Sells { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {          
            base.OnModelCreating(builder);            
            
            builder.Entity<User>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser)
                .HasForeignKey(x => x.OwnerID)               
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Catalog>()
                .HasMany(x => x.Characteristics)
                .WithOne(x => x.Catalog)
                .HasForeignKey(x=>x.CatalogID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasOne(x => x.Cart)
                .WithOne(x => x.UserOwner)
                .HasForeignKey<Cart>(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.Entity<Cart>()
                .HasMany(x => x.ItemsInCart)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartItemID)
                .OnDelete(DeleteBehavior.Restrict)
                ;      
        }
    }
}
