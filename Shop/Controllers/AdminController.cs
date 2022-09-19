using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Models.Admin;
using Shop.Models.UserModels;

namespace Shop.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationDBContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, ApplicationDBContext dBContext, SignInManager<User> signInManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = dBContext;
            _signInManager = signInManager;
            
        }

        [HttpGet]
        public IActionResult GetAdminPanel()
        {
            return View("AdminPanel");
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

        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList().Select(x=> new UserDTO { 
                Username = x.UserName,
                Email = x.Email,
                IsAdmin = _userManager.IsInRoleAsync(x, "Admin").Result
            }).ToList();
            
            return View("Users", users);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AddAdminViewModel addAdminViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Email = addAdminViewModel.Email,
                    UserName = addAdminViewModel.UserName,
                    EmailConfirmed = true,
                    Id = Guid.NewGuid()
                };
                var createAdmin_Result = await _userManager.CreateAsync(user, addAdminViewModel.Password);
                if (!createAdmin_Result.Succeeded)
                {
                    foreach (var error in createAdmin_Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                var addRole_Result = await _userManager.AddToRoleAsync(user, "Admin");
                return View("AddAdminConfirmation");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect data");
                return View(addAdminViewModel);
            }
        }

        public async Task<IActionResult> DeleteUser(string email)
        {
            if (ModelState.IsValid)
            {
                if (email == _config["Admin:Email"])
                {
                    ModelState.AddModelError("", "You can't delete admin");
                    return RedirectToAction("GetAllUsers");
                }
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null) await _userManager.DeleteAsync(user);
                return RedirectToAction("GetAllUsers");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect data");      
                return BadRequest("Incorrect data");
            }
        }
    }
}
