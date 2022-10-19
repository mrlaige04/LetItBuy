using System.ComponentModel.DataAnnotations;
namespace Shop.DAL.Data.Entities
{
    public class Characteristic
    {
        [Key]
        public Guid ID { get; set; }
        public Item Item { get; set; }
        public Guid ItemID { get; set; }
        
        public Dictionary<Guid, Guid> NumberCriterias { get; set; }
        public Dictionary<Guid, Guid> StringCriterias { get; set; }   
    }
}
