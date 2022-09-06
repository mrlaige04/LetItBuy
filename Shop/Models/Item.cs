using Shop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Items")]
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public Guid OwnerID { get; set; }
        public string Description { get; set; }
        public Catalog ItemCatalog { get; set; }               
        public double ItemPrice { get; set; }       
    }
}
