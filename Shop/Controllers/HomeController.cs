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
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomEmailSender _sender;
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private FilterService filterService { get; set; }
        public HomeController(
            ICustomEmailSender sender, 
            ApplicationDBContext db, 
            FilterService filterService, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager
        )
        {
            _sender = sender;
            _db = db;
            this.filterService = filterService;
            _mapper = mapper;
            _userManager = userManager;
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
        public IActionResult GetWelcomePage() => View("WelcomePage");
        


        [HttpGet]
        public async Task<IActionResult> Index(SearchViewModel search) // TODO : FILTRATION BY FILTER DTO, PAGINATION, SORTING
        {
            var items = await filterService.Filter(search.Filter);
            var categories = await _db.Categories.Include(x=>x.NumberCriteriasValues)
                    .Include(x=>x.StringCriteriasValues)
                    .ToListAsync();
            return View("Index", categories);
        }


        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await _db.Categories.ToListAsync();
            return View("Categories", categories);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByCategory(Guid categoryId)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid categoryId");
            Category? category = await _db.Categories
                .Include(x => x.NumberCriteriasValues)
                .Include(x => x.StringCriteriasValues)
                .FirstOrDefaultAsync(x => x.Id == categoryId);
            
            if (category == null) return NotFound("Category not found");
            var categotyDTO = _mapper.Map<Category, CategoryDTO>(category);
            var items = _db.Items.Where(x => x.Category_Id == categoryId);
            

            
            return View("SearchPage", new SearchViewModel() { items = items.ToList(), Category = categotyDTO, Filter = new FilterDTO()  });
        }

        [HttpGet]
        public IActionResult SearchPage() => View(new SearchViewModel() { });


        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items
                .Include(x=>x.NumberCriteriaValues)
                .Include(x=>x.StringCriteriaValues)
                .ToListAsync();
            return View("Allitems", items);
        }


        [HttpPost]
        public async Task<IActionResult> BuyItem(BuyViewModel model)
        {
            var item = await _db.Items.FirstOrDefaultAsync(x => x.ID == model.ItemID);
            if (item == null) return RedirectToAction("NotFoundPage", "Home");

            var sell = _mapper.Map<BuyViewModel, Order>(model);
            sell.SellID = Guid.NewGuid();
            sell.DeliveryInfo = _mapper.Map<BuyViewModel, DeliveryInfo>(model);
            sell.DeliveryInfo.DeliveryID = Guid.NewGuid();
            sell.DeliveryInfo.Sell = sell;
            sell.DeliveryInfo.SellID = sell.SellID;   
            sell.Status = OrderStatus.Created;
            sell.ItemName = item.Name;
            
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sell.BuyerID = Guid.Parse(userId);
            }

            try
            {
                await _db.Sells.AddAsync(sell);
                await _db.SaveChangesAsync();
                return RedirectToAction("GetWelcomePage");
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        public IActionResult NotFoundPage() => View("NotFound");


        [HttpGet]
        public async Task<IActionResult> GetCriterias(Guid categoryId)
        {
            var category = await _db.Categories
                .Include(x => x.NumberCriteriasValues)
                .Include(x => x.StringCriteriasValues)
                .FirstOrDefaultAsync(x=>x.Id == categoryId);

            if (category == null) return NotFound("Category not found");

            var numCrits = category.NumberCriteriasValues
                                .DistinctBy(x => x.CriteriaName)
                                .Select(x => x.CriteriaName)
                                .ToList();
            var strCrits = category.StringCriteriasValues
                                .DistinctBy(x => x.CriteriaName)
                                .Select(x => x.CriteriaName)
                                .ToList();

            return Json(new CategoryCriterias (numCrits,strCrits));
        }
    }
    public record CategoryCriterias ( List<string> numCriterias, List<string> strCriteriass );
}
