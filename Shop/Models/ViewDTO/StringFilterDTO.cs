namespace Shop.UI.Models.ViewDTO
{
    public class StringFilterDTO
    {
        public Guid CriteriaID { get; set; }
        public Guid ValueID { get; set; }
        public FilterType FilterType { get; set; }
    }
}
