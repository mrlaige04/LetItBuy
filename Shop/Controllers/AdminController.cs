using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDBContext _db;
        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDBContext dBContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = dBContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        
        [HttpPost]
        public void CreateCategory(CreateCatalogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var catalog = new Catalog()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Parent = model.Parent,
                    Characteristics = model.Charatrestics
                };
                _db.Catalogs.Add(catalog);
                _db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "");
            }
        }

        public void RemoveAllUsers()
        {
            
        }

        public void AddAdmin()
        {
            
        }

        public void DeleteAdmin()
        {
            
        }
    }
}
