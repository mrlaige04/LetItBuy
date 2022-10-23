namespace Shop.BLL.Models
{
    public class AddModel
    {
        public string CategoryName { get; set; }

        public List<NumberCriteriaEntry> NumberCriterias { get; set; } = new List<NumberCriteriaEntry>();
        public List<StringCriteriaEntry> StringCriterias { get; set; } = new List<StringCriteriaEntry>();
    }

    public record NumberCriteriaEntry(string name, List<double> values, bool multiple);
    public record StringCriteriaEntry(string name, List<string> values, bool multiple);
}
