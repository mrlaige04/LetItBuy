using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.BLL.Models;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using System.Text.RegularExpressions;

namespace Shop.BLL.Services
{
    public class FilterService
    {
        private ApplicationDBContext db { get; set; }
        public FilterService(ApplicationDBContext db)
        {
            this.db = db;
        }

        public async Task<IQueryable<Item>> Filter(FilterDTO dto)
        {
            IQueryable<Item> items = db.Items.Include(i => i.NumberCriteriaValues)
                                        .Include(i => i.StringCriteriaValues);
            items = items.Where(i => i.Category_Id == dto.CategoryID);
            items = items.Where(i => i.Price >= dto.minPrice && i.Price <= dto.maxPrice);
            if (dto.NumberFilters != null) {
                foreach (var nfilter in dto.NumberFilters)
                {
                    items = items.Where(i => i.NumberCriteriaValues
                        .Any(n => n.CriteriaID == nfilter.CriteriaID && n.ValueID == nfilter.ValueID));
                }
            }
            if (dto.StringFilters != null)
            {
                foreach (var sfilter in dto.StringFilters)
                {
                    items = items.Where(i => i.StringCriteriaValues
                        .Any(s => s.CriteriaID == sfilter.CriteriaID && s.ValueID == sfilter.ValueID));
                }
            }
            if (dto.query != null)
            {
                items = items.Where(i => Regex.IsMatch(i.Name, dto.query, RegexOptions.IgnoreCase));
            }
            return items;
            // TODO : EXception - Make Filtering
        }
    }
}
