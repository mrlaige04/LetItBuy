using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shop.DAL.Data.Entities
{
    [Table("Sells")]
    public class Order
    {
        [Key]
        public Guid SellID { get; set; }

        public string ItemName { get; set; }
        public Guid? ItemID { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }

        public OrderStatus Status { get; set; }

        public Guid OwnerID { get; set; }


        public Guid? BuyerID { get; set; }
    }

    public enum OrderStatus
    {
        Created,
        Confirmed,
        Delivering,
        Delivered,
        Succeded,
        Canceled
    }
}
