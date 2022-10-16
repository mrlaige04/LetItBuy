using Shop.DAL.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities
{
    public class StringCriteria : ICriteria
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        
        public ICollection<Category>? Categories { get; set; }
        public IEnumerable<StringValue>? DefaultValues { get; set; }
        public ICollection<Characteristic>? Characteristics { get; set; }
    }
}
