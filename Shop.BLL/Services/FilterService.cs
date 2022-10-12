using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.BLL.Models;
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

        //public void SetData(IEnumerable<Item> items, Guid categoryID)
        //{
        //    this.items = items;
        //    category = db.Categories
        //        .Include(x => x.Criterias)
        //        .FirstOrDefault(x => x.Id == categoryID) ??
        //        throw new ArgumentException("Category not found");
        //}

        //public IEnumerable<Item> FilterItems(FilterExpression expression)
        //{
        //    #region PriceCheck
        //    if (expression.minPrice != decimal.MinValue)
        //    {
        //        items = items.Where(x => x.ItemPrice >= expression.minPrice);
        //    }
        //    if (expression.maxPrice != decimal.MaxValue)
        //    {
        //        items = items.Where(x => x.ItemPrice <= expression.maxPrice);
        //    }
        //    #endregion

        //    #region CheckByCriterias
        //    foreach (var criteria in category.Criterias)
        //    {
        //        if (criteria.Type == CriteriaTypes.Number)
        //        {
        //            var minValue = expression.GetValue(criteria.Name + "-min");
        //            var maxValue = expression.GetValue(criteria.Name + "-max");
        //            if (double.TryParse(minValue, out double minVal))
        //            {
        //                if (double.TryParse(maxValue, out double maxVal))
        //                {
        //                    items = items.Where(x => double.Parse(x[criteria.Name]) >= minVal && double.Parse(x[criteria.Name]) <= maxVal);
        //                }
        //                else
        //                {
        //                    items = items.Where(x => double.Parse(x[criteria.Name]) >= minVal);
        //                }
        //            }
        //        }
        //        else if (criteria.Type == CriteriaTypes.String)
        //        {
        //            var value = expression.GetValue(criteria.Name);
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                items = items.Where(x => x[criteria.Name].ToLower() == value.ToLower());
        //            }
        //        }
        //        else if (criteria.Type == CriteriaTypes.Boolean)
        //        {
        //            var value = expression.GetValue(criteria.Name);
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                items = items.Where(x => x[criteria.Name] == value);
        //            }
        //        }

        //    }
        //    #endregion
        //    return items;
        //}


        //public IEnumerable<Item> Filter(FilterDTO filterModel)
        //{
        //    IQueryable<Item> items = db.Items.Where(x => x.Category_ID == filterModel.CategoryID);

        //    items = items
        //        .Where(x => x.ItemPrice >= filterModel.minPrice &&
        //            x.ItemPrice <= filterModel.maxPrice);

        //    foreach (var criteria in filterModel.NumberFilters)
        //    {
                
        //    }
        //}
    }
}
