using Microsoft.AspNetCore.Identity;
namespace Shop.Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; } = new();        
    }
    
}
