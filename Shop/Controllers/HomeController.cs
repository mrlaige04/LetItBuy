using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.BLL.Models;
using Shop.BLL.Services;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.Models.ViewDTO;
using Shop.UI.Models.ViewDTO;
using System.Security.Claims;



namespace Shop.Controllers
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


        public IActionResult NotFoundPage() => View("NotFound");





        public IActionResult Filter(FilterViewModel filter)
        {
            
            filterService.Filter()
            return View("SearchPage");
        }

    }
}
