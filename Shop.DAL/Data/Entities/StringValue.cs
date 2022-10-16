using Shop.DAL.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("StringValues")]
    public class StringValue : IValue
    {
        [Key]
        public Guid ValueID { get ; set ; }
        public string Value { get; set; }
        public ICollection<StringCriteria>? stringCriterias { get; set; }
    }
}
