using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data.Entities.Interfaces
{
    public interface ICriteria
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
