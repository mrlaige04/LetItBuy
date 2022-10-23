using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;
using Shop.BLL.Models;
using Shop.BLL.Services.Interfaces;
using Shop.Core.Classes;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class DBInitializer : IDBInitializer, IDisposable
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public DBInitializer(ApplicationDBContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public void Dispose()
        {
            
        }

        public async Task InitializeAsync()
        {
            List<Category> Categories = new List<Category>();
            List<AddModel>? addModels = null!;
            
            using (StreamReader sr = new StreamReader("bdinitializedata.json"))
            {
                string text = await sr.ReadToEndAsync();
                addModels = JsonConvert.DeserializeObject<List<AddModel>>(text);
            }
            if (await _db.Categories.FirstOrDefaultAsync(x => x.Name == "Phone") != null 
                && await _db.Categories.FirstOrDefaultAsync(x=>x.Name=="Laptops") != null) return;
            if (addModels != null)
            {
                Parallel.ForEach(addModels, item =>
                {
                    
                    Category cat = new Category
                    {
                        Name = item.CategoryName,
                        Id = Guid.NewGuid(),
                        NumberCriteriasValues = new List<NumberCriteriaValue>(),
                        StringCriteriasValues = new List<StringCriteriaValue>()
                    };
                   
                    foreach (var x in item.NumberCriterias)
                    {
                        Guid criteriaID = Guid.NewGuid();
                        foreach (var value in x.values)
                        {
                            cat.NumberCriteriasValues.Add(new NumberCriteriaValue()
                            {
                                Category = cat,
                                CategoryID = cat.Id,
                                CategoryName = cat.Name,
                                CriteriaID = criteriaID,
                                CriteriaName = x.name,
                                Value = value,
                                ValueID = Guid.NewGuid(),
                                multiple = x.multiple
                            });
                        }
                    }
                    foreach (var x in item.StringCriterias)
                    {
                        Guid criteriaID = Guid.NewGuid();
                        foreach (var value in x.values)
                        {
                            cat.StringCriteriasValues.Add(new StringCriteriaValue()
                            {
                                Category = cat,
                                CategoryID = cat.Id,
                                CategoryName = cat.Name,
                                CriteriaID = criteriaID,
                                CriteriaName = x.name,
                                Value = value,
                                ValueID = Guid.NewGuid(),
                                multiple = x.multiple
                            });
                        }
                    }
                    Categories.Add(cat);
                });
                
            }

            

            var users = new Faker<ApplicationUser>()
                .RuleFor(x=>x.Id,f=>f.Random.Guid())
                .RuleFor(x => x.AboutMe, f => f.Lorem.Sentence())
                .RuleFor(x => x.UserName, f => f.Internet.UserName())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(x => x.EmailConfirmed, f => true).Generate(20);


            await _db.Users.AddRangeAsync(users);
            await _db.SaveChangesAsync();
            
            var phones = new Faker<Item>()
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Price, f => f.Random.Decimal(0, 99999))
                .RuleFor(x => x.ID, f => Guid.NewGuid())
                .RuleFor(x => x.Category_Id, f => Categories[0].Id)
                .RuleFor(x=>x.CategoryName, f => Categories[0].Name)
                .RuleFor(x=>x.IsNew , f=>f.Random.Bool())
                .RuleFor(x=>x.Currency, f=>Currency.UAH)
                .RuleFor(x=>x.OwnerUser,f=>_db.Users.First(x=>x.Email=="multishopannounce@gmail.com"))
                .RuleFor(x=>x.NumberCriteriaValues,f=>
                {
                    return new List<NumberCriteriaValue>()
                    {
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="RAM")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="ROM")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Battery Capacity")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="ScreenSize")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Year Production")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Sim Count")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Main Camera MP")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Front Camera MP")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Display Frequency")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Width")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Height")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Depth")),
                        f.PickRandom(Categories[0].NumberCriteriasValues.Where(x=>x.CriteriaName=="Weight")),
                    };
                })
                .RuleFor(x => x.StringCriteriaValues, f =>
                {
                    return new List<StringCriteriaValue>()
                    {
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Brand")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="CPU")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Communication")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Mattrix")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Connection type")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Resolution")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Color")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Material")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Camera Functions")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Dynamic")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Charging")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="Security")),
                        f.PickRandom(Categories[0].StringCriteriasValues.Where(x=>x.CriteriaName=="SIM")),
                    };
                })
                .Generate(50);




            var laptops = new Faker<Item>()
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Price, f => f.Random.Decimal(0, 99999))
                .RuleFor(x => x.ID, f => Guid.NewGuid())
                .RuleFor(x => x.Category_Id, f => Categories[1].Id)
                .RuleFor(x => x.CategoryName, f => Categories[1].Name)
                .RuleFor(x => x.IsNew, f => f.Random.Bool())
                .RuleFor(x => x.Currency, f => Currency.USD)
                .RuleFor(x => x.OwnerUser, f => _db.Users.First(x=>x.Email=="sabfasvf2b@gmail.com"))
                .RuleFor(x=>x.NumberCriteriaValues, f=>
                {
                    return new List<NumberCriteriaValue>()
                    {
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="Screen diagonal")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="RAM")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="SSD")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="Year")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="MAX RAM")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="Screen Frequency")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="Weight")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="Brightness")),
                        f.PickRandom(Categories[1].NumberCriteriasValues.Where(x=>x.CriteriaName=="HDD"))
                    };
                })
                .RuleFor(x=>x.StringCriteriaValues, f=>
                {
                    return new List<StringCriteriaValue>()
                    {
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="Mattrix")),
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="OS")),
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="Material")),
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="Color")),
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="Connection")),
                        f.PickRandom(Categories[1].StringCriteriasValues.Where(x=>x.CriteriaName=="DVD")),
                    };
                })
                .Generate(50);

            

            

            await _db.Categories.AddRangeAsync(Categories);
            await _db.Items.AddRangeAsync(phones);
            await _db.Items.AddRangeAsync(laptops);
            await _db.SaveChangesAsync();
        }
    }
}
