using Shop.DAL.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("NumberValues")]
    public class NumberValue : IValue
    {
        [Key]
        public Guid ValueID { get ; set ; }
        public double Value { get; set; }
        public ICollection<NumberCriteria>? numberCriterias { get; set; }
    }
}
