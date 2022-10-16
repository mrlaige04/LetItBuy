using Shop.DAL.Data.Entities;
using Shop.UI.Models.ViewDTO;

namespace Shop.Models.ViewDTO
{
    public class SearchViewModel
    {
        public string? query { get; set; }
        public IEnumerable<Item>? items { get; set; }
        public FilterViewModel Filter { get; set; }
    }
}
