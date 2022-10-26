namespace Shop.BLL.Models
{
    public class FilterExpression
    {
        public decimal minPrice { get; set; } = decimal.MinValue;
        public decimal maxPrice { get; set; } = decimal.MaxValue;

        public IEnumerable<FilterValue> FilterValues { get; set; }


        public string GetValue(string name)
        {
            return FilterValues?.FirstOrDefault(x => x.CriteriaName == "C-" + name)?.Value;
        }
    }

    public class FilterValue
    {
        public string CriteriaName { get; set; }
        public string Value { get; set; }
    }
}
