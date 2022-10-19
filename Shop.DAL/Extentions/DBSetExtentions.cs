using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.Entities;
using System.Linq;

namespace Shop.DAL.Extentions
{
    public static class DBSetExtentions
    {
        public static IEnumerable<NumberValue> GetRange(this DbSet<NumberValue> dbset, double min, double max)
        {
            return dbset.Where(x => x.Value >= min && x.Value <= max);
        }

        public static IEnumerable<NumberValue> GetRange(this DbSet<NumberValue> dbset, IEnumerable<double> values)
        {
            return dbset.Where(x => values.Contains(x.Value));
        }

        public static IEnumerable<StringValue> GetRange(this DbSet<StringValue> dbset, IEnumerable<string> values)
        {
            return dbset.Where(x => values.Contains(x.Value));
        }

        public static IEnumerable<NumberValue> GetNumberValues(this DbSet<NumberValue> dbset, IEnumerable<double> values)
        {
            
            return dbset.Where(x => values.Contains(x.Value));
            
        }

        
    }
}
