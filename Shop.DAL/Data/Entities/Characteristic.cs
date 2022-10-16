using System.ComponentModel.DataAnnotations;
namespace Shop.DAL.Data.Entities
{
    public class Characteristic
    {
        [Key]
        public Guid ID { get; set; }
        public Item Item { get; set; }
        public Guid ItemID { get; set; }

        public ICollection<NumberCriteria> NumberCriterias { get; set; }
        public ICollection<StringCriteria> StringCriterias { get; set; }
        public ICollection<DateCriteria> DateCriterias { get; set; }  
    }
}
