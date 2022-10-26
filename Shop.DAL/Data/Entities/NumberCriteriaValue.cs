using System.ComponentModel.DataAnnotations;

namespace Shop.DAL.Data.Entities
{
    public class NumberCriteriaValue
    {
        public double Value { get; set; }
        [Key]
        public Guid ValueID { get; set; }
        public string CriteriaName { get; set; }
        [Key]
        public Guid CriteriaID { get; set; }
        [Key]
        public Guid CategoryID { get; set; }
        public Category Category { get; set; }
        public string CategoryName { get; set; }

        public bool multiple { get; set; }


        public ICollection<Item>? Items { get; set; } = null!;
    }
}
