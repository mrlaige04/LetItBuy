namespace Shop.BLL.DTO
{
    public class NumberFilterDTO
    {
        public Guid CriteriaID { get; set; }
        public Guid ValueID { get; set; }
        public FilterType FilterType { get; set; }
    }  
}
