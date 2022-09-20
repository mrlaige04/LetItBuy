using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Sells")]
    public class Sell
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid SellerID { get; set; }
        public Guid BuyerID { get; set; }
    }
}
