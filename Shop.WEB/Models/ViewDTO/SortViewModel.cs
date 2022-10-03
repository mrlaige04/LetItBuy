namespace Shop.WEB.Models
{
    public class SortViewModel
    {
        public SortState SortState { get; set; }
        public SortViewModel(SortState sortOrder)
        {
            SortState = sortOrder;
        }
    }
    public enum SortState
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }
}
