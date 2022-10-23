using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models.UserModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.Entities;
using Shop.DAL.Data.EF;
using Shop.BLL.Services;
using Shop.BLL.Models;
using Shop.UI.Clients.APICLIENTS;
using AutoMapper;
using Shop.BLL.DTO;
using Shop.UI.Models.ViewDTO;

namespace Shop.Controllers
{
    [Authorize(Roles="simpleUser, Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHost;
        private readonly ApplicationDBContext _db;
        private readonly UserService _userService;
        private readonly PhotoService _photoService;
        private readonly ItemApiClient _apiClient;
        private readonly IMapper _mapper;
        public UserController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, ApplicationDBContext db, UserService userService, PhotoService photoService, ItemApiClient advertApiClient, IMapper mapper)
        {
            _userManager = userManager;
            _webHost = webHostEnvironment;
            _db = db;
            _userService = userService;
            _photoService = photoService;

            _apiClient = advertApiClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var userDTO = _mapper.Map<ApplicationUser, ProfileViewModel>(user);
            ViewData["webrootpath"] = Request.Host.Value;
            return View("MyProfile", userDTO);
        }


        [HttpPost]

        public async Task<IActionResult> EditProfile(EditProfileViewModel editModel)
        {
            if (!ModelState.IsValid) return View("MyProfile", editModel);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            user.AboutMe = editModel.AboutMe;
            user.PhoneNumber = editModel.PhoneNumber;
            user.UserName = editModel.UserName;
            await _userManager.UpdateAsync(user);
            return View("MyProfile");
        }

        
        [HttpPost]
        public async Task<IActionResult> SetUserImage(IFormFile photo)
        {        
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    ModelState.AddModelError("", "No user found");
                    return RedirectToAction("Logout", "Account");
                }
                var setUserPhoto_Result = await _photoService.SetUserProfileImage(photo, userId);
                if (setUserPhoto_Result.ResultCode == ResultCodes.Fail)
                {
                    foreach (var error in setUserPhoto_Result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return RedirectToAction("GetProfile");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserImage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleteUserImage = await _photoService.DeleteUserImage(userId);
            if (deleteUserImage.ResultCode == ResultCodes.Fail)
            {
                foreach (var error in deleteUserImage.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return RedirectToAction("GetProfile");
        }

        [HttpGet] public IActionResult AddItemPage() => View();

        
        

        [HttpGet]
        public IActionResult MyItems()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) return RedirectToAction("Logout", "Account");
            var items = _db.Items.Where(x=>x.OwnerID.ToString() == id).ToList();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemAddViewModel viewItem)
        {
            //if (ModelState.IsValid)
            //{
            //    var isnew = Request.Form["IsNew"];
            //    var image = Request.Form.Files["itemImage"];
            //    var user = await _userManager.GetUserAsync(User);
            //    if (user == null) return RedirectToAction("Logout", "Account");
            //    Category? category = _db.Categories
            //        .Include(x=>x.Criterias)
            //        .FirstOrDefault(x => x.Id.ToString() == viewItem.CategoryID);
                
            //    Item item = new Item
            //    {
            //        ID = Guid.NewGuid(),
            //        Description = viewItem.Description,
            //        Name = viewItem.Name,
            //        Price = viewItem.Price,
            //        Currency = viewItem.Currency,
            //        Category = category,
            //        CategoryID = category.Id,
            //        CategoryName = category.Name,
            //        Characteristics = new List<Characteristic>(),  
            //        IsNew = viewItem.IsNew
            //    };
            //    if (image != null)
            //    {
            //        string path = Path.Combine("wwwroot\\ItemPhotos\\", item.ID.ToString() + image.FileName.Substring(image.FileName.LastIndexOf('.')));
            //        using (var stream = new FileStream(path, FileMode.Create))
            //        {
            //            await image.CopyToAsync(stream);
            //        }
            //        item.ImageUrl = item.ID.ToString() + image.FileName.Substring(image.FileName.LastIndexOf('.'));
            //    }
            //    for (int i = 0; i < category?.Criterias.Count; i++)
            //    {
            //        var criteria = category.Criterias.ToList()[i];
            //        var str = Request.Form[$"x{criteria.Name}"];
            //        item.Characteristics.Add(new Characteristic()
            //        {
            //            ID = Guid.NewGuid(),
            //            CriteriaID = criteria.ID,
            //            //Value = str,
            //            Item = item,
            //            ItemID = item.ID,
            //            Name = criteria.Name,
            //            CriteriaName = criteria.Name
            //        });
            //    }
                
            //    var addItem_Result = await _userService.AddItemAsync(user, item);
            //    if (addItem_Result.ResultCode == ResultCodes.Success)
            //    {
            //        return RedirectToAction("MyItems");
            //    }
            //    else
            //    {
            //        foreach (var error in addItem_Result.Errors)
            //        {
            //            ModelState.AddModelError("", error);
            //        }
            //    }
            //}
            
            return View("AddItemPage");
            
        }

        
        [HttpGet]
        public IActionResult EditItem(Guid ItemId)
        {
            var item = _db.Items.FirstOrDefault(x => x.ID == ItemId);
            if (item == null) return RedirectToAction("MyItems");
            return View(new EditItemViewModel
            {
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                ItemId = item.ID
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(EditItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Logout", "Account");
                var editItem_Result = await _userService.EditItemAsync(user, new Item
                {
                    Description = item.Description,
                    Name = item.Name,
                    Price = item.Price,
                    ID = item.ItemId,
                    Currency = item.Currency
                });
                if (editItem_Result.ResultCode == ResultCodes.Success) return RedirectToAction("MyItems");
                else
                {
                    foreach (var error in editItem_Result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View("EditItem", item);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(Guid ItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Logout", "Account");
            var deleteItem_Result = await _userService.DeleteItemAsync(user, ItemId);
            if (deleteItem_Result.ResultCode == ResultCodes.Success) return RedirectToAction("MyItems");
            else
            {
                foreach (var error in deleteItem_Result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return RedirectToAction("MyItems");
        }

        [HttpGet]
        public IActionResult MySells()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Logout", "Account");
            var sells = _db.Sells.Include(x=>x.DeliveryInfo).Where(x => x.OwnerID.ToString() == userId).ToList();
            return View(sells);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Logout", "Account");
            var orders = await _db.Orders.Include(x=>x.DeliveryInfo).Where(x => x.BuyerID.ToString() == userId).ToListAsync();
            return View(orders);
        }
    }  
}
