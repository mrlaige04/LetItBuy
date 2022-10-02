using Shop.DAL.Data.Entities;

namespace Shop.UI.Models.ViewDTO
{
    public class FilterViewModel
    {
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }

        public IEnumerable<Criteria> Criterias { get; set; }
    }
}
