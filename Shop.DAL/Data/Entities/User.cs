using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shop.DAL.Data.Entities
{
    [Table("Users")]
    public class User : IdentityUser<Guid>
    {
        public ICollection<Item>? Items { get; set; }
        public Cart Cart { get; set; }
        public Guid CartID { get; set; }

        public string? ImageURL { get; set; }

        public string? AboutMe { get; set; }
        public string Phone { get; set; }
    }
}
