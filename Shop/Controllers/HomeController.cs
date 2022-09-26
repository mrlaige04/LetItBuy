using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Models.ViewDTO;
using Shop.Services;
using System.Resources;
using System.Security.Claims;
using System.Text.Json;


namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomEmailSender _sender;
        //private readonly IRepository _repository;
        private readonly ApplicationDBContext _db;
        public HomeController(ICustomEmailSender sender, ApplicationDBContext db)
        {
            _sender = sender;
            //_repository = new MultiShopRepository(db);
            _db = db;
            
            
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



        [HttpGet] // TODO : PAGING LIKE A SKIP(N).TAKE(K);
        public IActionResult SearchItems( string q)
        {
            if (ModelState.IsValid)
            {
                List<ItemViewDTO> result;
                if (Guid.TryParse(q, out Guid itemId))
                {
                    result = _db.Items.AsEnumerable().Where(i => i.ItemId == itemId).Select(x => new ItemViewDTO
                    {
                        ItemId = x.ItemId,
                        ItemPrice = x.ItemPrice,
                        ItemName = x.ItemName,
                        Currency = x.Currency,
                        Description = x.Description,
                        ImageURL = x.ImageUrl
                    }).ToList();
                }
                else
                {
                    result = _db.Items.AsEnumerable().Where(i => i.ItemName.ToLower().Contains(q.ToLower())).Select(x => new ItemViewDTO
                    {
                        ItemId = x.ItemId,
                        ItemPrice = x.ItemPrice,
                        ItemName = x.ItemName,
                        Currency = x.Currency,
                        Description = x.Description,
                        ImageURL = x.ImageUrl
                    }).ToList();
                }

                return View("ManyItems", result);
            } return BadRequest();
        }



        [HttpGet]
        public IActionResult ItemPage(string id)
        {
            var item = _db.Items.Include(x=>x.Characteristics).AsEnumerable().FirstOrDefault(x => x.ItemId.ToString() == id);
            return View("ItemPage", item);
        }

        
        [HttpGet]
        public IActionResult GetCriterias (string categoryId)
        {
            var criterias = _db.Criterias.AsEnumerable().Where(x => x.CategoryID.ToString() == categoryId).ToList();
            return Json(criterias);
        }

        [HttpGet]
        public IActionResult ItemsByCategory(string categoryId)
        {
            var items = _db.Items.AsEnumerable().Where(x => x.Category_ID.ToString() == categoryId).ToList();
            var itemViews = items.Select(x => new ItemViewDTO
            {
                ItemId = x.ItemId,
                ItemPrice = x.ItemPrice,
                ItemName = x.ItemName,
                Currency = x.Currency,
                Description = x.Description,
                ImageURL = x.ImageUrl
            }).ToList();
            var criterias = _db.Criterias.AsEnumerable().Where(x => x.CategoryID.ToString() == categoryId).ToList();
            SearchViewModel search = new SearchViewModel()
            {
                output = new SearchViewModel.Output()
                {
                    Items = itemViews,
                    Criterias = criterias,
                },
            };
            
            return View("ManyItems", search);
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

        

    }
}
