using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Criteria
    {
        [Key]
        [Required]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public CriteriaTypes Type { get; set; }
        public Category? Category { get; set; }
        [Display(Name="CategoryName")]
        public Guid CategoryID { get; set; }
    }
    public enum CriteriaTypes
    {
        String,
        NumberMoreThanZero,
        Number,
        Date,
        Boolean
    }
}
