using Shop.DAL.Data.Entities;

namespace Shop.UI.Models.ViewDTO
{
    public class IndexViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public PageViewModel? PageViewModel { get; set; }

        public FilterViewModel? FilterViewModel { get; set; }

        public SortViewModel? SortViewModel { get; set; }

        public Category? Category { get; set; }
        public Guid CategoryID { get; set; }
    }
}
