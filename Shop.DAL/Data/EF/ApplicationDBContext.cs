using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Classes;
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
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Sell> Sells { get; set; } = null!;



        public DbSet<NumberCriteriaValue> NumberCriteriaValues { get; set; } = null!;
        public DbSet<StringCriteriaValue> StringCriteriaValues { get; set; } = null!;
        protected  override void OnModelCreating(ModelBuilder builder)
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
                

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Items)
                .WithOne(x => x.OwnerUser)
                .HasForeignKey(x => x.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            
            
            builder.Entity<ApplicationUser>()
                .HasOne(x => x.Cart)
                .WithOne(x => x.UserOwner)
                .HasForeignKey<Cart>(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Notifications)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            



            builder.Entity<Cart>()
                .HasMany(x => x.ItemsInCart)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartItemID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Item>()
                .HasMany(x => x.Photos)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemID)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            builder.Entity<StringCriteriaValue>().HasKey(x => new { x.CriteriaID, x.ValueID });
            builder.Entity<NumberCriteriaValue>().HasKey(x => new { x.CriteriaID, x.ValueID });


            builder.Entity<Item>()
                .HasMany(x => x.NumberCriteriaValues)
                .WithMany(x => x.Items)
                .UsingEntity("ItemNumberCriteriasValues");
            builder.Entity<Item>()
                .HasMany(x => x.StringCriteriaValues)
                .WithMany(x => x.Items)
                .UsingEntity("ItemStringCriteriasValues");


            builder.Entity<Category>()
                .HasMany(x=>x.NumberCriteriasValues)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Category>()
                .HasMany(x => x.StringCriteriasValues)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);




            




            //Categories.AddRangeAsync(new List<Category>()
            //{
            //    new Category()
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Phone",
            //        StringCriteriasValues = new List<StringCriteriaValue>()
            //        {
            //            new StringCriteriaValue() {
            //                CriteriaName = "Brand",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "2E", "AGM", "ALCATEL", "ASUS",
            //                    "Apple", "Assistant", "Astro",
            //                    "Blackview", "Bravis", "CAT",
            //                    "Coolpad", "Cubot", "DIZO",
            //                    "DOOGEE", "ERGO", "FiGi",
            //                    "Fly", "General Mobile", "Google",
            //                    "HTC", "Huawei", "Honor",
            //                    "Hotwav", "Infinix", "KXD",
            //                    "LG", "Lenovo", "Maxcom",
            //                    "Maxfone", "Microsoft", "Motorola",
            //                    "Nokia", "Nomi", "Nomu",
            //                    "Nothing", "OPPO", "OnePlus",
            //                    "Oukitel", "Philips", "Samsung",
            //                    "Santin", "Servo", "Sigma",
            //                    "Sony", "TCL", "TP-Link Neffos",
            //                    "Tecno", "UMIDIGI", "Ulefone",
            //                    "VERICO", "Viaan", "Wiko",
            //                    "Xiaomi", "ZTE", "Zopo", "myPhone",
            //                    "realme", "vivo"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "CPU",
            //                DefaultValues = new List<StringValue>() {
            //                    "Qualcom Snapdragon 8 gen X",
            //                    "Qualcom Snapdragon 8xx",
            //                    "Qualcom Snapdragon 7xx",
            //                    "Qualcom Snapdragon 6xx",
            //                    "Qualcom Snapdragon 4xx",
            //                    "MediaTek Dimensity",
            //                    "MediaTek Helio Gxx",
            //                    "MediaTek Helio Pxx",
            //                    "MediaTek Helio Axx",
            //                    "Samsung Exynos",
            //                    "HUAWEI Kirin"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Communication",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "NFC", "USB-C", "GPS",
            //                    "Bluetooth", "802.11ax", "802.11ac",
            //                    "Infrared port", "SD-Card"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Mattrix",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "AMOLED", "IPS", "OLED"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Connection type",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "5G", "4G", "3G", "2G"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Resolution",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "4K", "Quad HD", "Full HD+",
            //                    "Full HD", "HD+","HD"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Color",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Black", "White", "Gray",
            //                    "Beige", "Brown", "Orange",
            //                    "Gold", "Yellow", "Pink gold",
            //                    "Red", "Blue", "Green",
            //                    "Purple", "Dark gray"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Material",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Metal", "Glass", "Plastic",
            //                    "Ceramic", "Rubber"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Camera Functions",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Stabilization", "Macro", "Ultra height"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Dynamic",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Stereo", "Mono", "aptX"
            //                }
            //            },
            //            new StringCriteria() {
            //                Name = "Charging",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Quick", "Wireless"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "Security",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Fingerprint", "FaceID"
            //                }
            //            },
            //            new StringCriteria()
            //            {
            //                Name = "FrontalCameraType",
            //                DefaultValues = new List<StringValue>()
            //                {
            //                    "Cutout", "Drop"
            //                }
            //            }
            //        },
            //        NumberCriterias = new List<NumberCriteria>()
            //        {
            //            new NumberCriteria()
            //            {
            //                Name = "ROM",
            //                DefaultValues = new List<NumberValue>()
            //                {
            //                    1000, 512, 256, 128, 64, 32, 16, 8, 4
            //                }
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "RAM",
            //                DefaultValues = RangeGenerator.DoubleRange(2,16,1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Year Production",
            //                DefaultValues = RangeGenerator.DoubleRange(DateTime.UtcNow.Year-4, DateTime.UtcNow.Year, 1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Screen Resolution",
            //                DefaultValues = RangeGenerator.DoubleRange(3, 8, 0.01)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Battery capacity",
            //                DefaultValues = RangeGenerator.IntRange(100,1000, 100)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "SIM Count",
            //                DefaultValues = RangeGenerator.DoubleRange(1,4,1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Main camera MP",
            //                DefaultValues = RangeGenerator.IntRange(5,200,1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Front camera MP",
            //                DefaultValues = RangeGenerator.IntRange(5,200,1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Display Frequency",
            //                DefaultValues = new List<NumberValue> {60,90,120,144}
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Width",
            //                DefaultValues = RangeGenerator.DoubleRange(50,80,0.1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Height",
            //                DefaultValues = RangeGenerator.IntRange(100,170,1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Depth",
            //                DefaultValues = RangeGenerator.DoubleRange(5,11,0.1)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            },
            //            new NumberCriteria()
            //            {
            //                Name = "Weight",
            //                DefaultValues = RangeGenerator.DoubleRange(50,250,10)
            //                .Select(x=>new NumberValue(x)).ToList()
            //            }
            //        }
            //    }
            //});
        }       
    }
}
