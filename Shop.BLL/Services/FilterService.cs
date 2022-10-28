using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

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
            IQueryable<Item> items = db.Items
                .Include(i => i.NumberCriteriaValues)
                .Include(i => i.StringCriteriaValues);

            if (!string.IsNullOrEmpty(dto.query))
            {
                items = items.Where(x => x.Name.ToLower().Contains(dto.query.ToLower()));
            }
            try
            {
                if (dto.minPrice <= dto.maxPrice)
                {
                    items = items.Where(x => x.Price >= dto.minPrice && x.Price < dto.maxPrice-1);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            
            
            if (dto.CategoryID == null) return items;
            
            items = items.Where(x => x.Category_Id == dto.CategoryID);
            
            if (dto.NumberFilters != null && dto.NumberFilters.Count()>0)
            {
                foreach (var numFilter in dto.NumberFilters)
                {
                    if (numFilter.ValueIDS != null && numFilter.ValueIDS.Count() > 0)
                    {
                        items = items.Where(x =>
                            x.NumberCriteriaValues
                            .Any(x => x.CriteriaID == numFilter.CriteriaID && numFilter.ValueIDS.Contains(x.ValueID.ToString())));
                    }
                }
            }
            if (dto.StringFilters != null && dto.StringFilters.Count() > 0)
            {
                foreach (var strFilter in dto.StringFilters)
                {
                    if (strFilter.ValueIDS != null && strFilter.ValueIDS.Count() > 0)
                    {
                        items = items.Where(x =>
                            x.StringCriteriaValues
                            .Any(x => x.CriteriaID == strFilter.CriteriaID && strFilter.ValueIDS.Contains(x.ValueID.ToString())));
                    }
                }
            }

            return items;
            
        }
    }
}
