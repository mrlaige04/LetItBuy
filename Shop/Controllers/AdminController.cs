using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public async Task<IActionResult> CreateCategoryAsync()
        {
            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Criterias = new List<Criteria>()
            };
            var categoryName = Request.Form["categoryName"];
            var criteriaNames = Request.Form["criteriaName"];
            var criteriaTypes = Request.Form["criteriaType"];
            if (categoryName.ToString() == null || (criteriaNames.Count != criteriaTypes.Count)) return BadRequest("Incorrect data");
            category.Name = categoryName[0];
            for (int i = 0; i < criteriaNames.Count; i++)
            {
                category.Criterias.Add(new Criteria()
                {
                    ID = Guid.NewGuid(),
                    Name = criteriaNames[i].ToString(),
                    Type = (CriteriaTypes)Enum.Parse(typeof(CriteriaTypes), criteriaTypes[i]),
                    Category = category,
                    CategoryID = category.Id
                });
            }
            try
            {
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();

            } catch (Exception e)
            {
                return Problem(e.Message);
            }
            return RedirectToAction("ManageCategories");
        }

        
        public void RemoveAllUsers()
        {
            
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (ModelState.IsValid)
            {
                var category = _db.Categories.FirstOrDefault(x => x.Id.ToString() == id);
                try
                {
                    _db.Categories.Remove(category);
                    
                    await _db.SaveChangesAsync();
                    return RedirectToAction("ManageCategories");
                } catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else return BadRequest("Invalid id");
        }

        [HttpGet]
        public IActionResult EditCategory(string id)
        {
            var category = _db.Categories
                .Include(x => x.Criterias)
                .FirstOrDefault(x => x.Id.ToString() == id);
            return View(category);
        }

        [HttpPost] 
        public async Task<IActionResult> EditCategory(Category category)
        {
            var categoryFromDB = await _db.Categories.Include(x => x.Criterias).FirstOrDefaultAsync(x => x.Id == category.Id);
            if (categoryFromDB == null) return NotFound();
            categoryFromDB.Criterias.Clear();
            _db.Criterias.RemoveRange(_db.Criterias.Where(x => x.CategoryID == categoryFromDB.Id));
            await _db.SaveChangesAsync();

            var categoryName = Request.Form["categoryName"];
            var criteriaNames = Request.Form["criteriaName"];
            var criteriaTypes = Request.Form["criteriaType"];
            if (categoryName.ToString() == null || (criteriaNames.Count != criteriaTypes.Count)) return BadRequest("Incorrect data");
            categoryFromDB.Name = categoryName[0];
            for (int i = 0; i < criteriaNames.Count; i++)
            {
                _db.Criterias.Add(new Criteria()
                {
                    ID = Guid.NewGuid(),
                    Name = criteriaNames[i].ToString(),
                    Type = (CriteriaTypes)Enum.Parse(typeof(CriteriaTypes), criteriaTypes[i]),
                    Category = categoryFromDB,
                    CategoryID = categoryFromDB.Id
                });
            }

            try
            {
                _db.Categories.Update(categoryFromDB);
                await _db.SaveChangesAsync();
            } catch (Exception e)
            {
                return Problem(e.Message);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult ManageCategories()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
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
