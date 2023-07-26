using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.Entities;


namespace Shop.DAL.Data.EF
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Order> Sells { get; set; } = null!;
        public DbSet<DeliveryInfo> Deliveries { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<ItemPhoto> ItemPhotos { get; set; } = null!;

        public DbSet<NumberCriteriaValue> NumberCriteriaValues { get; set; } = null!;
        public DbSet<StringCriteriaValue> StringCriteriaValues { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region baseModelsettings

            builder.Entity<ApplicationUser>(x =>
            {
                x.Ignore("LockoutEnd");
                x.Ignore("LockoutEnabled");
                x.Ignore("AccessFailedCount");
                x.Ignore("TwoFactorEnabled");
            });
            builder.Entity<Category>()
                .HasKey(x => new { x.Id, x.Name });

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser)
                .HasForeignKey(x => x.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);






            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Notifications)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Order>()
                .HasOne(x => x.DeliveryInfo)
                .WithOne(x => x.Order)
                .HasForeignKey<DeliveryInfo>(x => x.OrderID)
                .OnDelete(DeleteBehavior.Cascade);




            builder.Entity<Item>()
                .HasMany(x => x.Photos)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemID)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            builder.Entity<StringCriteriaValue>().HasKey(x => new { x.CriteriaID, x.ValueID, x.CategoryID });
            builder.Entity<NumberCriteriaValue>().HasKey(x => new { x.CriteriaID, x.ValueID, x.CategoryID });


            builder.Entity<Item>()
                .HasMany(x => x.NumberCriteriaValues)
                .WithMany(x => x.Items)
                .UsingEntity("ItemNumberCriteriasValues");
            builder.Entity<Item>()
                .HasMany(x => x.StringCriteriaValues)
                .WithMany(x => x.Items)
                .UsingEntity("ItemStringCriteriasValues");


            builder.Entity<Category>()
                .HasMany(x => x.NumberCriteriasValues)
                .WithOne(x => x.Category)
                .HasForeignKey(x => new { x.CategoryID, x.CategoryName })
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Category>()
                .HasMany(x => x.StringCriteriasValues)
                .WithOne(x => x.Category)
                .HasForeignKey(x => new { x.CategoryID, x.CategoryName })
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
