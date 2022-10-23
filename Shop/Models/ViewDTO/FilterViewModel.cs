using Shop.BLL.DTO;
using Shop.DAL.Data.Entities;

namespace Shop.UI.Models.ViewDTO
{
    public class FilterViewModel
    {
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = decimal.MaxValue;

        public Guid? CategoryID { get; set; }

        public FilterDTO filterModel { get; set; }
    }
}
