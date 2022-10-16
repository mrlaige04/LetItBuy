using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.DTO
{
    public class FilterDTO
    {
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = decimal.MaxValue;
        public string? query = string.Empty;
        public Guid CategoryID { get; set; } = Guid.Empty;

        public IEnumerable<NumberFilterDTO>? NumberFilters { get; set; }
        public IEnumerable<StringFilterDTO>? StringFilters { get; set; }
    }
}
