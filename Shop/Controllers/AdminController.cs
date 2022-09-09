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

        public void CreateCategory(CreateCatalogViewModel model)
        {
            if(ModelState.IsValid)
            {
                var catalog = new Catalog();
                _db.Catalogs.Add(catalog);
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
