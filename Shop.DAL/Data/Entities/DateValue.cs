using Shop.DAL.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("DateValues")]
    public class DateValue : IValue
    {
        [Key]
        public Guid ValueID { get; set; }
        public DateTime Value { get; set; }
        public ICollection<DateCriteria>? DateCriterias { get; set; }
    }
}
