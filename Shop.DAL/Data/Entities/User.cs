using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
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
