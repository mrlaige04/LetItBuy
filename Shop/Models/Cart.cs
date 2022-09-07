
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public Guid UserID { get; set; }
        public User UserOwner { get; set; }
        public  ICollection<CartItem>? ItemsInCart { get; set; }
        
    }
}
