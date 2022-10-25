
using Shop.DAL.Data.Entities;

namespace Shop.BLL.DTO
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<NumberCriteriaViewModel>? NumberCriterias { get; set; }
        public IEnumerable<StringCriteriaViewModel>? StringCriterias { get; set; }
    }
    public record GroupAbleCriteria(Guid CriteriaID, string CriteriaName, bool multiple);
    public record NumberCriteriaViewModel(GroupAbleCriteria key, IEnumerable<NumberCriteriaValue> value);
    public record StringCriteriaViewModel(GroupAbleCriteria key, IEnumerable<StringCriteriaValue> value);
}
