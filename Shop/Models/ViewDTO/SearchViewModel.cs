namespace Shop.Models.ViewDTO
{
    public class SearchViewModel
    {
        public Output output { get; set; }
        public Input input { get; set; }
        public class Output
        {
            public IEnumerable<ItemViewDTO> Items { get; set; }
            public IEnumerable<Category> Categories { get; set; }
            public IEnumerable<Criteria> Criterias { get; set; }
        }

        public class Input
        {
            public decimal minPrice { get; set; }
            public decimal maxPrice { get; set; }
        }
    }
}
