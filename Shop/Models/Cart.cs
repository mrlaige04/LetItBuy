
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Carts")]
    public class Cart
    {
        public Guid UserID { get; set; }
        public List<Item> ItemsInCart { get; set; } = new();      
    }
}
