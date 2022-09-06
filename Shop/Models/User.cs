using Microsoft.AspNetCore.Identity;
namespace Shop.Models
{
    public class User : IdentityUser<Guid>
    {       
        public List<Item> Items { get; set; } = new();
        public Cart Cart { get; set; }
    }    
}
