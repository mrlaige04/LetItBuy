namespace Shop.BLL.DTO
{
    public class FilterDTO
    {
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = 999999999;
        public string query { get; set; }
        public Guid? CategoryID { get; set; }

        public List<NumberFilterCriteriaModel>? NumberFilters { get; set; } = new List<NumberFilterCriteriaModel>();
        public List<StringFilterCriteriaModel>? StringFilters { get; set; } = new List<StringFilterCriteriaModel>();
    }
    
    public record NumberFilterCriteriaModel(Guid CriteriaID, IEnumerable<string>? ValueIDS);
    public record StringFilterCriteriaModel(Guid CriteriaID, IEnumerable<string>? ValueIDS);
}
