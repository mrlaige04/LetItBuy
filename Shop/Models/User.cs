using Microsoft.AspNetCore.Identity;
namespace Shop.Models
{
    public class User : IdentityUser<Guid>
    {       
        public ICollection<Item>? Items { get; set; }
        public Cart Cart { get; set; }
        
    }    
}
