﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Sells")]
    public class Sell
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid SellerID { get; set; }
        public Guid? BuyerID { get; set; }
        public string? PhoneNumber { get; set; }
        public SellStatus Status { get; set; }
        public DateTime Date { get; set; }
    }

    public enum SellStatus
    {
        WaitForOwner,
        Completed,
        Canceled
    }
}
