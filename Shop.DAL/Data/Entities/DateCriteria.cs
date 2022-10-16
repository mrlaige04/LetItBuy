using Shop.DAL.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    public class DateCriteria : ICriteria
    {
        public Guid ID { get ; set ; }
        public string Name { get; set; }
        public IEnumerable<DateValue>? DefaultValues { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Characteristic>? Characteristics { get; set; }
    }
}
