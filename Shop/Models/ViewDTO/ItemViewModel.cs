using Shop.BLL.DTO;

namespace Shop.UI.Models.ViewDTO
{
    public class ItemViewModel
    {
        public ItemDTO Item { get; set; }

        public BuyViewModel? BuyModel { get; set; }
    }
}
