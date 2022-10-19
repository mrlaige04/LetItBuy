using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Services;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.Models.ViewDTO;
using Shop.UI.Models.ViewDTO;
using System.Security.Claims;

using AutoMapper;
using Shop.BLL.DTO;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomEmailSender _sender;
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private FilterService filterService { get; set; }
        public HomeController(ICustomEmailSender sender, ApplicationDBContext db, FilterService filterService, IMapper mapper)
        {
            _sender = sender;
            _db = db;
            this.filterService = filterService;
            _mapper = mapper;
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
            var item = _db.Items
                .Include(x => x.OwnerUser)
                .FirstOrDefault(x => x.ID.ToString() == id);

            if (item == null)
            {
                return NotFound();
            }



            var itemdto = _mapper.Map<Item, ItemDTO>(item);

            if (item != null) return View("ItemPage", itemdto);
            else return NotFound();
        }
        //[HttpGet]
        //public IActionResult GetCriterias(string categoryId)
        //{
        //    var criterias = _db.Criterias.Where(x => x.CategoryID.ToString() == categoryId).ToList();
        //    return Json(criterias);
        //}



        [HttpGet]
        public IActionResult SearchPage() => View(new SearchViewModel() { Filter = new FilterViewModel()});


        [HttpGet]
        public async Task<IActionResult> Index(SearchViewModel q)
        {
            if (q.Filter.maxPrice < q.Filter.minPrice)
            {
                q.Filter.maxPrice = decimal.MaxValue;
            }
            //var items = await filterService.Filter(new FilterDTO()
            //{
            //    CategoryID = q.Filter.CategoryID,
            //    maxPrice = q.Filter.maxPrice,
            //    minPrice = q.Filter.minPrice,
            //    query = q.query
            //});
            //q.items = await items.ToListAsync();
            return View("SearchPage", q);
        }
        

        public IActionResult BuyItemByPhone(string itemId) // TODO :BUY BY PHONE
        {
            var item = Request.Form["itemId"];
            var phone = Request.Form["phonenumber"];

            var sellerID = _db.Items.FirstOrDefault(x => x.ID.ToString() == itemId)?.OwnerID;
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

            //filterService.Filter();
            return View("SearchPage");
        }

    }
}
