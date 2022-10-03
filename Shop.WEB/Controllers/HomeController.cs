using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.BLL.Models;
using Shop.BLL.Services;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.WEB.Models;
using System.Security.Claims;



namespace Shop.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomEmailSender _sender;
        private readonly ApplicationDBContext _db;
        private FilterService filterService { get; set; }
        public HomeController(ICustomEmailSender sender, ApplicationDBContext db, FilterService filterService)
        {
            _sender = sender;
            _db = db;
            this.filterService = filterService;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        [HttpGet]
        public IActionResult GetWelcomePage()
        {
            return View("WelcomePage");
        }
        [HttpGet]
        public IActionResult ItemPage(string id)
        {
            var item = _db.Items.Include(x => x.Characteristics).AsEnumerable().FirstOrDefault(x => x.ItemId.ToString() == id);
            return View("ItemPage", item);
        }
        [HttpGet]
        public IActionResult GetCriterias(string categoryId)
        {
            var criterias = _db.Criterias.Where(x => x.CategoryID.ToString() == categoryId).ToList();
            return Json(criterias);
        }



        [HttpGet]
        public IActionResult SearchPage() => View(new SearchViewModel() { PageSize = 10 });


        
        [HttpGet]
        public async Task<IActionResult> Search(SearchViewModel searchViewModel, int PageNumber = 1)
        {
            if (searchViewModel.PageSize == 0) searchViewModel.PageSize = 10;
            var itemsName = _db.Items.Where(x => x.ItemName.ToLower().Contains(searchViewModel.SearchString.ToLower()));
            if (itemsName.Count() < searchViewModel.PageSize)
            {
                searchViewModel.Items = await itemsName.ToListAsync();
                return View("SearchPage", searchViewModel);
            }
            var items = itemsName.Skip((PageNumber - 1) * searchViewModel.PageSize).Take(searchViewModel.PageSize);
            searchViewModel.Items = items.ToList();
            return View("SearchPage", searchViewModel);
        }





        [HttpGet]
        public IActionResult ItemsByCategory(string categoryId)
        {
            var items = _db.Items.Where(x => x.Category_ID.ToString() == categoryId).ToList();
            var itemViews = items.Select(x => new ItemViewDTO
            {
                ItemId = x.ItemId,
                ItemPrice = x.ItemPrice,
                ItemName = x.ItemName,
                Currency = x.Currency,
                Description = x.Description,
                ImageURL = x.ImageUrl
            }).ToList();
            
            Category category = _db.Categories
                .Include(x=>x.Criterias)
                .FirstOrDefault(x => x.Id.ToString() == categoryId);
            
            
            return View("Index", new IndexViewModel {
                Items = items,
                FilterViewModel = new FilterViewModel
                {
                    Criterias = category.Criterias,
                },
                Category = category,
                CategoryID = category.Id 
            });
        }
        

        public IActionResult BuyItemByPhone(string itemId) // TODO :BUY BY PHONE
        {
            var item = Request.Form["itemId"];
            var phone = Request.Form["phonenumber"];

            var sellerID = _db.Items.FirstOrDefault(x => x.ItemId.ToString() == itemId)?.OwnerID;
            if(sellerID != null)
            {
                Sell sell = new Sell
                {
                    Id = Guid.NewGuid(),
                    PhoneNumber = phone,
                    Status = SellStatus.WaitForOwner,
                    ItemId = Guid.Parse(itemId),
                    SellerID = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Date = DateTime.Now
                };
            }
            throw new NotImplementedException();
        }


















        public async Task<IActionResult> Index(string? q, int page = 1)
        {
            int pageSize = 10;
            IQueryable<Item> source = _db.Items.Where(x=>x.ItemName.ToLower().Contains(q.ToLower()));
            
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Items = items,
                FilterViewModel = new FilterViewModel()
                {
                    minPrice = 0,
                    maxPrice = decimal.MaxValue,
                }
            };
            return View(viewModel);
        }

        [HttpGet]
        public void Index(IndexViewModel model)
        {
            var sort = model.SortViewModel;
            var filter = model.FilterViewModel;
            var paging = model.PageViewModel;


            IQueryable<Item> source = _db.Items;
            if (sort != null)
            {
                switch (sort.SortState)
                {
                    case SortState.NameAsc:
                        source = source.OrderBy(s => s.ItemName);
                        break;
                    case SortState.NameDesc:
                        source = source.OrderByDescending(s => s.ItemName);
                        break;
                    case SortState.PriceAsc:
                        source = source.OrderBy(s => s.ItemPrice);
                        break;
                    case SortState.PriceDesc:
                        source = source.OrderByDescending(s => s.ItemPrice);
                        break;
                }
            }
        }


        //[HttpPost]
        //[HttpGet]
        //public IActionResult Index2(IndexViewModel model)
        //{
        //    if (model != null)
        //    {
        //        var sort = model.SortViewModel;
        //        var filter = model.FilterViewModel;
        //        var paging = model.PageViewModel;

        //        if (filter != null)
        //        {
        //            try
        //            {
        //                filterService.SetData(model.Items, model.CategoryID);
        //            }
        //            catch (Exception e)
        //            {
        //                return BadRequest(e.Message);
        //            }
                    
                    
        //            FilterExpression expression = new FilterExpression()
        //            {
        //                maxPrice = filter.maxPrice,
        //                minPrice = filter.minPrice,
        //                FilterValues = Request.Form.Where(x => x.Key.StartsWith("C-")).Select(x => new FilterValue
        //                {
        //                    CriteriaName = x.Key.Split('-')[1],
        //                    Value = x.Value
        //                }).ToList()
        //            };
        //            model.Items = filterService.FilterItems(expression);
        //        }

                
        //        if (sort != null)
        //        {
        //            switch (model.SortViewModel?.SortState)
        //            {
        //                case SortState.NameAsc:
        //                    model.Items = model.Items.OrderBy(s => s.ItemName);
        //                    break;
        //                case SortState.NameDesc:
        //                    model.Items = model.Items.OrderByDescending(s => s.ItemName);
        //                    break;
        //                case SortState.PriceAsc:
        //                    model.Items = model.Items.OrderBy(s => s.ItemPrice);
        //                    break;
        //                case SortState.PriceDesc:
        //                    model.Items = model.Items.OrderByDescending(s => s.ItemPrice);
        //                    break;
        //            }
        //        }
        //        if (paging != null)
        //        {

        //        }
        //    }
        //    return View("Index", model);
        //}
        //[HttpPost]
        //public void Filter(FilterViewModel filter) // TODO: FILTERING
        //{
        //    var categoryId = "0e2d9904-63d9-486e-ade2-a9d9a32f3168";
        //    // Items from db by such category
        //    IQueryable<Item> items = _db.Items
        //        .Include(x=>x.Characteristics)
        //        .Where(x=>x.Category_ID.ToString() == categoryId);
        //    // Category
        //    Category category = _db.Categories
        //        .Include(x=>x.Criterias)
        //        .FirstOrDefault(x => x.Id.ToString() == categoryId);
            
        //    var minPrice = filter?.minPrice;
        //    var maxPrice = filter?.maxPrice;
        //    if (minPrice != 0)
        //    {
        //        items = items.Where(x => x.ItemPrice >= minPrice);
        //    }
        //    if (maxPrice != 0)
        //    {
        //        items = items.Where(x => x.ItemPrice <= maxPrice);
        //    }


        //    // Foreach in criterias of category
        //    foreach (var criteria in category?.Criterias)
        //    {
        //        // Check criteria type and get values
        //        if (criteria.Type == CriteriaTypes.String)
        //        {
        //            var value = Request.Form["C-" + criteria.Name];
        //            if (!string.IsNullOrEmpty(value))
        //            {
        //                items = from item in items
        //                        from j in item.Characteristics
        //                        where j.CriteriaID == criteria.ID && j.Value == value
        //                        select item;  
        //            }
        //        }
        //        else if (criteria.Type == CriteriaTypes.Number || criteria.Type == CriteriaTypes.NumberMoreThanZero)
        //        {
        //            var minA = Request.Form["C-" + criteria.Name + "-min"];
        //            var maxA = Request.Form["C-" + criteria.Name + "-max"];
        //            var minNum = minA is { } ? double.PositiveInfinity : double.Parse(Request.Form[minA]);
        //            var maxNum = maxA is { } ? double.PositiveInfinity : double.Parse(Request.Form[maxA]);

        //            if(minNum != double.PositiveInfinity)
        //            {
        //                if (maxNum != double.PositiveInfinity)
        //                {
        //                    items = from item in items
        //                            from j in item.Characteristics
        //                            where j.CriteriaID == criteria.ID && double.Parse(j.Value) >= minNum && double.Parse(j.Value) <= maxNum
        //                            select item;
        //                }
        //                else
        //                {
        //                    items = from item in items
        //                            from j in item.Characteristics
        //                            where j.CriteriaID == criteria.ID && double.Parse(j.Value) >= minNum
        //                            select item;
        //                }
        //            }
        //        }  
        //        else if (criteria.Type == CriteriaTypes.Boolean)
        //        {
        //            var value = bool.Parse(Request.Form["C-" + criteria.Name]);
        //            items = (from item in items
        //                     from j in item.Characteristics
        //                     where j.CriteriaID == criteria.ID && bool.Parse(j.Value) == value
        //                     select item);
        //        }   
        //    }   
        //}
    }
}
