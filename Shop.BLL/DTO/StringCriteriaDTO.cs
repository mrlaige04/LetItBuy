namespace Shop.BLL.DTO
{
    public class StringCriteriaDTO
    {
        public string Name { get; set; }
        public bool Multiple { get; set; }
        public Guid CriteriaID { get; set; }
        public IEnumerable<StringValueId> Values { get; set; }
    }
    public record StringValueId(string value, Guid valueid);
}
