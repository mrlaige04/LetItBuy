using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Controllers;
using Shop.Models;

namespace Shop.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {            
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser);
            
            builder.Entity<Charatrestic>().HasKey(x => x.Name);
            
            builder.Entity<Catalog>()
                .HasMany(x => x.Charatrestics);

            builder.Entity<Cart>()
                .HasMany(x => x.ItemsInCart)
                .WithOne();
            
            
            //builder.Entity<User>()
            //    .HasMany(x => x.Items)
            //    .WithOne(x => x.OwnerUser);

            //builder.Entity<Cart>()
            //    .HasMany(x => x.ItemsInCart);
            
            
                
            
            
            
            //builder.Entity<Cart>().HasMany(x => x.ItemsInCart);
            //builder.Entity<Charatrestic>().HasKey(x=>x.Name);
            //builder.Entity<Catalog>().HasMany(x => x.Charatrestics);
        }
    }
}
