using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    
    public class Characteristic
    {
        [Key]
        public Guid ID { get; set; }
        public Criteria Criteria { get; set; }
        public string Value { get; set; }

        public Item Item { get; set; }
        public Guid ItemID { get; set; }
    }
}