using Shop.BLL.Services.Interfaces;
using Shop.Core.Classes;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class DBInitializer : IDBInitializer, IDisposable
    {
        private readonly ApplicationDBContext _db;
        public DBInitializer(ApplicationDBContext db)
        {
            _db = db;
        }

        public void Dispose()
        {
            
        }

        public async Task InitializeAsync()
        {
            Category phone = new Category() { Name = "Phone", Id = Guid.NewGuid(), 
                NumberCriteriasValues = new List<NumberCriteriaValue>(),
                StringCriteriasValues= new List<StringCriteriaValue>() };
            ICollection<NumberCriteriaValue> numberCriteriaValues = new List<NumberCriteriaValue>();
            ICollection<StringCriteriaValue> stringCriteriaValues = new List<StringCriteriaValue>();
            foreach (var item in Enumerable.Range(1,12))
            {
                numberCriteriaValues.Add(new NumberCriteriaValue()
                {
                    CategoryID = phone.Id,
                    Category = phone,
                    CriteriaID = Guid.NewGuid(),
                    CriteriaName = "RAM",
                    Value = item,
                    ValueID = Guid.NewGuid()
                });
            }
            for (int i = 32; i <= 1024; i+=16)
            {
                numberCriteriaValues.Add(new NumberCriteriaValue()
                {
                    CategoryID = phone.Id,
                    Category = phone,
                    CriteriaID = Guid.NewGuid(),
                    CriteriaName = "ROM",
                    Value = i,
                    ValueID = Guid.NewGuid()                
                });
            }
            List<string> brands = new List<string>()
            {
                "2E", "AGM", "ALCATEL", "ASUS",
                "Apple", "Assistant", "Astro",
                "Blackview", "Bravis", "CAT",
                "Coolpad", "Cubot", "DIZO",
                "DOOGEE", "ERGO", "FiGi",
                "Fly", "General Mobile", "Google",
                "HTC", "Huawei", "Honor",
                "Hotwav", "Infinix", "KXD",
                "LG", "Lenovo", "Maxcom",
                "Maxfone", "Microsoft", "Motorola",
                "Nokia", "Nomi", "Nomu",
                "Nothing", "OPPO", "OnePlus",
                "Oukitel", "Philips", "Samsung",
                "Santin", "Servo", "Sigma",
                "Sony", "TCL", "TP-Link Neffos",
                "Tecno", "UMIDIGI", "Ulefone",
                "VERICO", "Viaan", "Wiko",
                "Xiaomi", "ZTE", "Zopo", "myPhone",
                "realme", "vivo"
            };
            foreach (var brand in brands)
            {
                stringCriteriaValues.Add(new StringCriteriaValue()
                {
                    CategoryID = phone.Id,
                    Category = phone,
                    CriteriaID = Guid.NewGuid(),
                    CriteriaName = "Brand",
                    Value = brand,
                    ValueID = Guid.NewGuid()
                });
            }
            phone.NumberCriteriasValues.ToList().AddRange(numberCriteriaValues);
            foreach (var item in numberCriteriaValues)
            {
                phone.NumberCriteriasValues.Add(item);
            }
            foreach (var item in stringCriteriaValues)
            {
                phone.StringCriteriasValues.Add(item);
            }
            await _db.Categories.AddAsync(phone);

            await _db.SaveChangesAsync();
            //throw new NotImplementedException();
        }
    }
}
