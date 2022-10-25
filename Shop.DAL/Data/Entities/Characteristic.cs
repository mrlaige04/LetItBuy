using System.ComponentModel.DataAnnotations;
namespace Shop.DAL.Data.Entities
{
    public class Characteristic
    {
        [Key]
        public Guid ID { get; set; }
        public Item Item { get; set; }
        public Guid ItemID { get; set; }


        public ICollection<NumberCriteriaValue>? NumberCriteriaValues { get; set; } = null!;
        public ICollection<StringCriteriaValue>? StringCriteriaValues { get; set; } = null!;
    }
}
