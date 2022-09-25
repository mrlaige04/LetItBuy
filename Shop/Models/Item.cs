using Shop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Items")]
    public class Item
    {
        public Guid ItemId { get; set; }
        public User OwnerUser { get; set; }
        public string ItemName { get; set; }
        
        public Guid OwnerID { get; set; }
        public string? Description { get; set; }
        public Category? ItemCatalog { get; set; }               
        public decimal ItemPrice { get; set; }
        public Currency Currency { get; set; }

        public Guid IDCatalog { get; set; }
        public string CategoryName { get; set; } // TODO : CATEGORIES, CHARACTERISTICS 
        public string? ImageUrl { get; set; }


        public ICollection<Characteristic> Characteristics { get; set; }

        // TODO : Item characteristics
    }
    
    public enum Currency
    {
        USD,
        UAH,
        EUR
    }
}
