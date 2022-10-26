using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shop.DAL.Data.Entities
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Item>? Items { get; set; }


        public string? AboutMe { get; set; }

        public ICollection<Notification>? Notifications { get; set; }
    }
}
