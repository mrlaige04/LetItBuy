namespace Shop.UI.Models.ViewDTO
{
    public class BuyViewModel
    {
        public Guid? ItemID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string City { get; set; }
        public string PostName { get; set; }
        public string? OtherPostName { get; set; }
        public string PostNumber { get; set; }
        public string PostAddress { get; set; }


        public Guid OwnerID { get; set; }
    }
}
