using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Catalog> Catalogs { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);
            builder.Entity<User>().HasKey(x => x.Id);
            builder.Entity<User>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser)
                .HasForeignKey(x => x.OwnerID)
                
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Catalog>()
                .HasMany(x => x.Characteristics)
                .WithOne(x => x.Catalog)
                .HasForeignKey(x=>x.CatalogID)
                .OnDelete(DeleteBehavior.Restrict)
                ;

            builder.Entity<Cart>()
                .HasOne(x => x.UserOwner)
                .WithOne(x => x.Cart)
                .HasForeignKey<Cart>(x=>x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cart>()
                .HasMany(x => x.ItemsInCart)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartItemID)
                .OnDelete(DeleteBehavior.Restrict)
                ;



            
        }
    }
}
