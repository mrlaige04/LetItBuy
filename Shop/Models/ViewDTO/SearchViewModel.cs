using Shop.BLL.DTO;
using Shop.DAL.Data.Entities;
using Shop.UI.Models.ViewDTO;

namespace Shop.Models.ViewDTO
{
    public class SearchViewModel
    {
        public Guid? CategoryID { get; set; }
        public IEnumerable<Item>? items { get; set; }
        public CategoryDTO? Category { get; set; }
        public FilterDTO Filter { get; set; } = new FilterDTO();
    }
}
