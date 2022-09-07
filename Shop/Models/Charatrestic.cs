using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    
    public class Charatrestic
    {
        public Guid ID { get; set; }
        public Catalog Catalog { get; set; }
        [Key]        
        public string Name { get; set; }
        public string Value { get; set; }
    }
}