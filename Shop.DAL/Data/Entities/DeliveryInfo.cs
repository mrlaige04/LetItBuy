using System.ComponentModel.DataAnnotations;

namespace Shop.DAL.Data.Entities
{
    public class DeliveryInfo
    {
        [Key]
        public Guid DeliveryID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string PostName { get; set; }
        public string PostNumber { get; set; }
        public string PostAddress { get; set; }
        public string? TrackNumber { get; set; }
        public Order Sell { get; set; }
        public Guid SellID { get; set; }
    }
}
