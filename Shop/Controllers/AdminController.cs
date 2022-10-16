using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
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
            return View(new CreateCatalogViewModel()
            {
                
            });
        }



        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CreateCatalogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                NumberCriterias = new List<NumberCriteria>(),
                StringCriterias = new List<StringCriteria>(),
                DateCriterias = new List<DateCriteria>()
            };

            if (model.numbers != null)
            {
                foreach (var number in model.numbers)
                {
                    var numberCriteria = new NumberCriteria()
                    {
                        ID = Guid.NewGuid(),
                        Name = number.name,
                        DefaultValues = number.values.Select(x => new NumberValue() { ValueID = Guid.NewGuid(), Value = x }).ToList(),
                    };
                    category.NumberCriterias.Add(numberCriteria);
                }
            }

            if (model.strings != null)
            {
                foreach (var str in model.strings)
                {
                    var stringCriteria = new StringCriteria()
                    {
                        ID = Guid.NewGuid(),
                        Name = str.name,
                        DefaultValues = str.values.Select(x => new StringValue() { ValueID = Guid.NewGuid(), Value = x }).ToList(),
                    };
                    category.StringCriterias.Add(stringCriteria);
                }
            }

            if (model.dates != null)
            {
                foreach (var date in model.dates)
                {
                    var dateCriteria = new DateCriteria()
                    {
                        ID = Guid.NewGuid(),
                        Name = date.name,
                        DefaultValues = date.values.Select(x => new DateValue() { ValueID = Guid.NewGuid(), Value = x }).ToList(),
                    };
                    category.DateCriterias.Add(dateCriteria);
                }
            }

            //try
            //{
            //    await _db.Categories.AddAsync(category);
            //    await _db.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("", ex.Message);
            //    return View(model);
            //}
            return Content(JsonConvert.SerializeObject(category));
        }

        public async Task RemoveAllUsers()
        {
            try
            {
                _db.Users.RemoveRange(_db.Users);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            var users = _userManager.Users.Skip(0).Take(10).ToList();
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
