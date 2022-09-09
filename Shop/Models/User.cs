using Microsoft.AspNetCore.Identity;
namespace Shop.Models
{
    public class User : IdentityUser
    {       
        
        public ICollection<Item>? Items { get; set; }
        public Cart? Cart { get; set; }
        public Guid CartID { get; set; }

        public ICollection<IdentityRole> Roles { get; set; }
    }    
}
