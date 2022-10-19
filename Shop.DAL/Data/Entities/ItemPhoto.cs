using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.DAL.Data.Entities
{
    [Table("ItemPhotos")]
    public class ItemPhoto
    {
        [Key]
        public Guid ID { get; set; }
        public string FileName { get; set; }
        public Item Item { get; set; }
        
        public Guid ItemID { get; set; }
        public Guid OwnerID { get; set; }
    }
}
