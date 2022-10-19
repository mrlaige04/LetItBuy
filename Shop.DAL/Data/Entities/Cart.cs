using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public ApplicationUser UserOwner { get; set; }
        public ICollection<CartItem>? ItemsInCart { get; set; }
    }
}
