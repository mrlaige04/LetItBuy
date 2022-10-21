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

        public ICollection<NumberCriteriaValue> NumberCriteriasValues { get; set; } = null!;
        public ICollection<StringCriteriaValue> StringCriteriasValues { get; set; } = null!;



        
    }
}
