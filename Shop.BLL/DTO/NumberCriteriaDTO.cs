namespace Shop.BLL.DTO
{
    public class NumberCriteriaDTO
    {
        public string Name { get; set; }
        public bool Multiple { get; set; }
        public Guid CriteriaID { get; set; }
        public IEnumerable<NumberValueId> Values { get; set; }
    }

    public record NumberValueId(double value, Guid valueid);
}
