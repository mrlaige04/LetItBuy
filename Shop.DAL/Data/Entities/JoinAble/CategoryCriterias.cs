using System.ComponentModel.DataAnnotations;

namespace Shop.DAL.Data.Entities.JoinAble
{
    public class CategoryCriterias
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }


        public ICollection<Guid> NumberCriteriasValuesIds { get; set; } = null!;
        public ICollection<Guid> StringCriteriasValuesIds { get; set; } = null!;


        public ICollection<Item> Items { get; set; }

        public ICollection<NumberCriteriaValue>? NumberCriteriaValues { get; set; } = null!;
        public ICollection<StringCriteriaValue>? StringCriteriaValues { get; set; } = null!;
    }
}
