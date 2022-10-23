using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.BLL.Models;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using System.Security.Cryptography;
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
            throw new NotImplementedException();
            //IQueryable<Item> items = db.Items.Include(i => i.NumberCriteriaValues)
            //                            .Include(i => i.StringCriteriaValues);
            //items = items.Where(i => i.Category_Id == dto.CategoryID);
            //items = items.Where(i => i.Price >= dto.minPrice && i.Price <= dto.maxPrice);
            //if (dto.query != null)
            //{
            //    items = items.Where(i => Regex.IsMatch(i.Name, dto.query, RegexOptions.IgnoreCase));
            //}

            //if (dto.NumberFilters != null)
            //{
            //    foreach (var nfilter in dto.NumberFilters)
            //    {
            //        // Variant 1
            //        var a = items
            //            .Where(x => x.NumberCriteriaValues
            //                .Where(x => x.CriteriaID == nfilter.CriteriaID && x.CategoryID == nfilter.CategoryID)
            //                .Select(x => new { x.ValueID, x.CategoryID, x.CriteriaID })
            //                .Equals(nfilter.ValueIDS.Select(x => x)));
            //        // Variant 2
            //        var b = from item in items
            //                from numbercrits in item.NumberCriteriaValues
            //                from ValueID in nfilter.ValueIDS
            //                where numbercrits.CriteriaID == nfilter.CriteriaID &&
            //                      numbercrits.CategoryID == nfilter.CategoryID &&
            //                      numbercrits.ValueID == ValueID
            //                select item;

            //        Console.WriteLine(b.Count());
            //    }
            //}
            //if (dto.StringFilters != null)
            //{
            //    foreach (var sfilter in dto.StringFilters)
            //    {

            //    }
            //}


            //return items;
        }
    }
}
