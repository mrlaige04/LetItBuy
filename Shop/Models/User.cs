using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Users")]
    public class User : IdentityUser<Guid>
    {       
        public ICollection<Item>? Items { get; set; }
        public Cart Cart { get; set; }
        public Guid CartID { get; set; }

        public string? ImageURL { get; set; }

        
    }    
}
