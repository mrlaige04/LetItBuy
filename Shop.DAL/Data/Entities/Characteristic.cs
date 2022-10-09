using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    public class Characteristic
    {
        [Key]
        public Guid ID { get; set; }
        public Guid CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public Guid ValueId { get; set; }

        public Item Item { get; set; }
        public Guid ItemID { get; set; }
        public string Name { get; set; }
    }
}
