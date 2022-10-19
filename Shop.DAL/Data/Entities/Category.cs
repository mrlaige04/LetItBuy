using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }


        public ICollection<NumberCriteria>? NumberCriterias { get; set; }
        public ICollection<StringCriteria>? StringCriterias { get; set; }
       

        public ICollection<Item> Items { get; set; }
    }
}
