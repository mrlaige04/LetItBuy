using Shop.BLL.Services.Interfaces;
using Shop.Core.Classes;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDBContext _db;
        public DBInitializer(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task InitializeAsync()
        {
            await _db.NumberValues.AddRangeAsync(RangeGenerator.DoubleRange(0, 20000, 0.1).Select(x => (NumberValue)x));
            await _db.StringCriterias.AddRangeAsync(new List<StringCriteria>()
            {
                new StringCriteria()
                {
                    ID = Guid.NewGuid(),
                    Name = "Brand",
                    DefaultValues = new List<StringValue>()
                    {
                        "Apple", "Google", "Samsung",
                        "Microsoft", "Huawei", "Xiaomi",
                        "LG", "Sony", "HTC", "Nokia",
                        "Lenovo", "Asus", "Motorola",
                        "OnePlus", "ZTE", "Alcatel",
                        "BlackBerry", "TCL", "Razer",
                        "Vivo", "Oppo", "Realme",
                        "Meizu", "Honor", "Poco",
                        "Nubia", "Xiaomi", "Vsmart",
                        "Sharp", "Panasonic", "Toshiba",
                        "JBL", "Bose", "Beats",
                        "Sony", "Jabra", "Sennheiser",
                        "B&O", "JVC", "Philips",
                        "Bosch", "Siemens", "Electrolux",
                        "LG", "Samsung", "Beko",
                        "AEG", "Miele", "Whirlpool",
                        "Indesit", "Zanussi", "Hotpoint",
                        "Purina", "Royal Canin", "Hill's",
                        "Eukanuba", "Iams", "Pedigree",
                        "Whiskas", "Felix", "Sheba",
                        "Fulda", "Continental", "Dunlop",
                        "Michelin", "Bridgestone", "Goodyear",
                        "Pirelli", "Nokian", "Kumho",
                        "Yokohama", "Barum", "Nexen",
                        "Hankook", "Toyo", "Kleber",
                        "Hyundai", "Kia", "Toyota",
                        "Honda", "Mazda", "Nissan",
                        "Mitsubishi", "Suzuki", "Subaru",
                        "Lexus", "Infiniti", "Audi",
                        "BMW", "Mercedes-Benz", "Volkswagen",
                        "Volvo", "Skoda", "Porsche",
                        "Land Rover", "Jaguar", "Mini",
                        "Peugeot", "Citroen", "Renault",
                        "Dacia", "Opel", "Fiat",
                        "Ford", "Alfa Romeo", "Maserati",
                        "Dodge", "Jeep", "Chrysler",
                        "Lada", "Aston Martin", "Bentley",
                        "Bugatti", "Cadillac", "Chevrolet",
                        "Ferrari", "Lamborghini", "Mclaren",
                        "Rolls-Royce", "Tesla", "Morgan",
                        "Smart", "Seat", "DS",
                        "Mazda", "Nissan", "Mitsubishi",
                        "Suzuki", "Subaru", "Lexus"
                    }
                },
                new StringCriteria()
                {
                    ID = Guid.NewGuid(),
                    Name = "Color",
                    DefaultValues = new List<StringValue>()
                    {
                        "Blue", "Red", "Green",
                        "Yellow", "White", "Black",
                        "Orange", "Purple", "Brown",
                        "Grey", "Pink", "Gold",
                        "Silver", "Cyan", "Magenta",
                        "Lime", "Maroon", "Olive",
                        "Navy", "Teal", "Aquamarine",
                        "Turquoise", "Violet", "Azure",
                        "Beige", "Bisque", "Coral",
                        "Crimson", "Khaki", "Lavender",
                        "Lemon", "Mint", "Moccasin",
                        "Plum", "Salmon", "Tan"
                    }
                },
                new StringCriteria()
                {
                    ID = Guid.NewGuid(),
                    Name = "OS",
                    DefaultValues = new List<StringValue>() {
                        "Android", "iOS", "Windows",
                        "Linux", "MacOS", "ChromeOS",
                        "Ubuntu", "Debian", "HarmonyOS"
                    }
                }
            });

            
            
            await _db.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}
