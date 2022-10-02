using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    public class Criteria
    {
        [Key]
        [Required]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public CriteriaTypes Type { get; set; }
        public Category? Category { get; set; }
        [Display(Name = "CategoryName")]
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
