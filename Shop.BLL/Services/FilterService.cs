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

        //public async Task<IQueryable<Item>> Filter(FilterDTO dto)
        //{
        //    IQueryable<Item> items = db.Items.Include(x=>x.Characteristics);
        //    if (dto.CategoryID != Guid.Empty)
        //    {
        //        items = items.Where(x => x.CategoryID == dto.CategoryID);
        //    }
        //    if (dto.query != string.Empty && !string.IsNullOrEmpty(dto.query))
        //    {
        //        items = items.Where(x => x.Name.ToLower().Contains(dto.query.ToLower()));
        //        //items = items.Where(x => Regex.IsMatch(x.Name, $".*{dto.query}.*", RegexOptions.IgnoreCase));
        //    }
        //    items = items.Where(x => x.Price >= dto.minPrice && x.Price <= dto.maxPrice);

        //    return items;
        //    // TODO : EXception - Make Filtering
        //}
    }
}
