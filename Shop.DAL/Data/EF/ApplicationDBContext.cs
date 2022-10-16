using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Shop.Core.Classes;
using Shop.DAL.Data.Entities;

namespace Shop.DAL.Data.EF
{
    public class ApplicationDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Sell> Sells { get; set; } = null!;
        
        public DbSet<Characteristic> Characteristics { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<User>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser)
                .HasForeignKey(x => x.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            
            
            builder.Entity<User>()
                .HasOne(x => x.Cart)
                .WithOne(x => x.UserOwner)
                .HasForeignKey<Cart>(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder.Entity<User>()
                .HasMany(x => x.Notifications)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);



            #region Criterias
            builder.Entity<NumberCriteria>()
                .HasMany(x => x.DefaultValues)
                .WithMany(x => x.numberCriterias)
                .UsingEntity("NumberCriteriasDefaultValues");
            builder.Entity<StringCriteria>()
                .HasMany(x=>x.DefaultValues)
                .WithMany(x=>x.stringCriterias)
                .UsingEntity("StringCriteriasDefaultValues");
            builder.Entity<DateCriteria>()
                .HasMany(x => x.DefaultValues)
                .WithMany(x => x.DateCriterias)
                .UsingEntity("DateCriteriasDefaultValues");

            builder.Entity<Category>()
                .HasMany(x=>x.DateCriterias)
                .WithMany(x=>x.Categories)
                .UsingEntity("CategoriesDateCriterias");
            builder.Entity<Category>()
                .HasMany(x => x.NumberCriterias)
                .WithMany(x => x.Categories)
                .UsingEntity("CategoriesNumberCriterias");
            builder.Entity<Category>()
                .HasMany(x => x.StringCriterias)
                .WithMany(x => x.Categories)
                .UsingEntity("CategoriesStringCriterias");


            builder.Entity<Item>()
                .HasOne(x => x.Characteristic)
                .WithOne(x => x.Item)
                .HasForeignKey<Characteristic>(x => x.ItemID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Characteristic>()
                .HasMany(x => x.NumberCriterias)
                .WithMany(x => x.Characteristics)
                .UsingEntity("CharacteristicsNumberCriterias");
            builder.Entity<Characteristic>()
                .HasMany(x => x.StringCriterias)
                .WithMany(x => x.Characteristics)
                .UsingEntity("CharacteristicsStringCriterias");
            builder.Entity<Characteristic>()
                .HasMany(x => x.DateCriterias)
                .WithMany(x => x.Characteristics)
                .UsingEntity("CharacteristicsDateCriterias");


            

            
            #endregion


            


            builder.Entity<Category>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryID)
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
