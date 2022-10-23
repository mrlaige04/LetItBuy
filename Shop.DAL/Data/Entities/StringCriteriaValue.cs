using System.ComponentModel.DataAnnotations;

namespace Shop.DAL.Data.Entities
{
    public class StringCriteriaValue
    {
        public string Value { get; set; }
        [Key]
        public Guid ValueID { get; set; }
        public string CriteriaName { get; set; }
        [Key]
        public Guid CriteriaID { get; set; }

        public Category Category { get; set; }
        [Key]
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }

        
        public bool multiple { get; set; }

        
        public ICollection<Item> Items { get; set; } = null!;
    }
}
