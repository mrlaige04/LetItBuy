using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Shop.BLL.Models;
using Shop.BLL.Services;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.Models.UserModels;
using Shop.UI.Clients.APICLIENTS;
using Shop.UI.Models.ViewDTO;
using System.Security.Claims;

namespace Shop.Controllers
{
    [Authorize(Roles = "simpleUser, Admin")]
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

        public async Task<IActionResult> EditProfile(EditProfileViewModel editModel) // TODO : Upload user image
        {
            if (!ModelState.IsValid) return View("MyProfile", editModel);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            user.AboutMe = editModel.AboutMe;
            user.PhoneNumber = editModel.PhoneNumber;
            user.UserName = editModel.UserName;

            var photo = editModel.Image;
            if (photo != null) {
                var ext = photo.FileName.Substring(photo.FileName.LastIndexOf('.'));
                var path = string.Format(_webHost.WebRootPath + "\\UserPhotos\\" + user.Id + ext);
                using (var str = new FileStream(Path.Combine(_webHost.WebRootPath, "\\UserPhotos\\", photo.FileName), FileMode.Create))
                {
                    await str.CopyToAsync(photo.OpenReadStream());
                }
                user.ImageURL = user.Id + ext;
            }
            await _userManager.UpdateAsync(user);
            return View("MyProfile");
        }


        [HttpPost]
        public async Task<IActionResult> SetProfilePhoto(IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    ModelState.AddModelError("", "No user found");
                    return RedirectToAction("Logout", "Account");
                }
                var ext = photo.FileName.Substring(photo.FileName.LastIndexOf('.'));
                var path = string.Format(_webHost.WebRootPath + "\\UserPhotos\\" + user.Id + ext);
                using (var str = new FileStream(Path.Combine(_webHost.WebRootPath, "\\UserPhotos\\", photo.FileName), FileMode.Create))
                {
                    await str.CopyToAsync(photo.OpenReadStream());
                }
                try
                {
                    user.ImageURL = user.Id + ext;
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            return RedirectToAction("GetProfile");
        }

        [HttpPost]
        [HttpDelete]
        [HttpGet]
        public async Task<IActionResult> DeleteImagePhoto()
        {
            var user = await _userManager.GetUserAsync(User);
            System.IO.File.Delete(string.Format("..\\UserPhotos\\" + user.ImageURL));
            user.ImageURL = null;
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
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
            var items = _db.Items.Where(x => x.OwnerID.ToString() == id).ToList();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemAddViewModel viewItem, IFormFileCollection photos)
        {
            if (ModelState.IsValid)
            {
                var category = await _db.Categories.Include(x => x.NumberCriteriasValues)
                    .Include(x => x.StringCriteriasValues)
                    .FirstOrDefaultAsync(x => x.Id == viewItem.CategoryID);

                if (category == null) return NotFound();
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();
                Item item = new Item()
                {
                    ID = Guid.NewGuid(),
                    Category_Id = category.Id,
                    CategoryName = category.Name,
                    Currency = viewItem.Currency,
                    Description = viewItem.Description,
                    IsNew = viewItem.IsNew,
                    Name = viewItem.Name,
                    Price = viewItem.Price,
                    OwnerUser = user,
                    OwnerID = user.Id,
                    NumberCriteriaValues = new List<NumberCriteriaValue>(),
                    StringCriteriaValues = new List<StringCriteriaValue>(),
                    Photos = new List<ItemPhoto>()
                };

                await Parallel.ForEachAsync(photos, async (photo, y) =>
                {
                    ItemPhoto itemPhoto = new ItemPhoto()
                    {
                        Item = item,
                        ItemID = item.ID,
                        FileName = photo.FileName,
                        OwnerID = user.Id,
                        ID = Guid.NewGuid()
                    };
                    var path = _webHost.WebRootPath + "\\ItemPhotos\\" + photo.FileName;
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        await photo.CopyToAsync(fs);
                    }
                    item.Photos.Add(itemPhoto);
                });


                var numCriteriasIDS = category.NumberCriteriasValues.Select(x => x.CriteriaID);
                var strCriteriasIDS = category.StringCriteriasValues.Select(x => x.CriteriaID);

                foreach (var numCrit in numCriteriasIDS)
                {
                    var valuesIDS = Request.Form[numCrit.ToString()].ToList();
                    var values = category.NumberCriteriasValues.Where(x => valuesIDS.Contains(x.ValueID.ToString()) && x.CriteriaID == numCrit).ToList();
                    item.NumberCriteriaValues.AddRange(values);
                }

                foreach (var strCrit in strCriteriasIDS)
                {
                    var valuesIDS = Request.Form[strCrit.ToString()].ToList();
                    var values = category.StringCriteriasValues.Where(x => valuesIDS.Contains(x.ValueID.ToString()) && x.CriteriaID == strCrit).ToList();
                    item.StringCriteriaValues.AddRange(values);
                }
                item.NumberCriteriaValues = item.NumberCriteriaValues.DistinctBy(x => new { x.CriteriaID, x.ValueID }).ToList();
                item.StringCriteriaValues = item.StringCriteriaValues.DistinctBy(x => new { x.CriteriaID, x.ValueID }).ToList();
                try
                {
                    await _db.Items.AddAsync(item);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return BadRequest(ModelState);
                }
            }
            else return View("AddItemPage", viewItem);
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
            var sells = _db.Sells.Include(x => x.DeliveryInfo).Where(x => x.OwnerID.ToString() == userId).ToList();
            return View(sells);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Logout", "Account");
            var orders = await _db.Orders.Include(x => x.DeliveryInfo).Where(x => x.BuyerID.ToString() == userId).ToListAsync();
            return View(orders);
        }
    }
}
