using Shop.DAL.Data.Entities;

namespace Shop.WEB.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public int PageSize { get; set; }


        public string SearchString { get; set; }
        
    }
}
