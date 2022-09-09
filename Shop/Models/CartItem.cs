using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class CartItem
    {
        public Cart? Cart { get; set; }

        public Guid CartItemID { get; set; }
        public Item Item { get; set; }
    }
}
