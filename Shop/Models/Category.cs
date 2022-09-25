using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Category? Parent { get; set; }

        public ICollection<Criteria> Criterias { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
