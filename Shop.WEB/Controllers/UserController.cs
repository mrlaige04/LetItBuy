using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.Entities;
using Shop.DAL.Data.EF;
using Shop.BLL.Services;
using Shop.BLL.Models;
using Shop.WEB.Models;

namespace Shop.WEB.Controllers
{
    [Authorize(Roles="simpleUser, Admin")]
    
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHost;
        private readonly ApplicationDBContext _db;
        private readonly UserService _userService;
        private readonly PhotoService _photoService;
        public UserController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment, ApplicationDBContext db, UserService userService, PhotoService photoService)
        {
            _userManager = userManager;
            _webHost = webHostEnvironment;
            _db = db;
            _userService = userService;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var returlUrl = Request.Path + Request.QueryString;
            if (user == null) return RedirectToAction("Logout", "Account", returlUrl);
            ViewData["webrootpath"] = Request.Host.Value;
            return View("MyProfile", new ProfileViewModel { UserName = user.UserName, Email = user.Email, ImageUrl = user.ImageURL });
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

        [HttpGet]
        public IActionResult AddItemPage()
        {
            return View();
        }



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
            if (ModelState.IsValid)
            {
                var isnew = Request.Form["IsNew"];
                var image = Request.Form.Files["itemImage"];
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Logout", "Account");
                Category? category = _db.Categories
                    .Include(x=>x.Criterias)
                    .FirstOrDefault(x => x.Id.ToString() == viewItem.CategoryID);
                
                Item item = new Item
                {
                    ItemId = Guid.NewGuid(),
                    Description = viewItem.Description,
                    ItemName = viewItem.Name,
                    ItemPrice = viewItem.Price,
                    Currency = viewItem.Currency,
                    Category = category,
                    Category_ID = category.Id,
                    CategoryName = category.Name,
                    Characteristics = new List<Characteristic>(),  
                    IsNew = viewItem.IsNew
                };
                if (image != null)
                {
                    string path = Path.Combine("wwwroot\\ItemPhotos\\", item.ItemId.ToString() + image.FileName.Substring(image.FileName.LastIndexOf('.')));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    item.ImageUrl = item.ItemId.ToString() + image.FileName.Substring(image.FileName.LastIndexOf('.'));
                }
                for (int i = 0; i < category?.Criterias.Count; i++)
                {
                    var criteria = category.Criterias.ToList()[i];
                    var str = Request.Form[$"x{criteria.Name}"];
                    item.Characteristics.Add(new Characteristic()
                    {
                        ID = Guid.NewGuid(),
                        CriteriaID = criteria.ID,
                        Value = str,
                        Item = item,
                        ItemID = item.ItemId,
                        Name = criteria.Name,
                        CriteriaName = criteria.Name
                    });
                }
                
                var addItem_Result = await _userService.AddItemAsync(user, item);
                if (addItem_Result.ResultCode == ResultCodes.Success)
                {
                    return RedirectToAction("MyItems");
                }
                else
                {
                    foreach (var error in addItem_Result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            
            return View("AddItemPage");
            
        }

        
        [HttpGet]
        public IActionResult EditItem(Guid ItemId)
        {
            var item = _db.Items.FirstOrDefault(x => x.ItemId == ItemId);
            if (item == null) return RedirectToAction("MyItems");
            return View(new EditItemViewModel
            {
                Description = item.Description,
                Name = item.ItemName,
                Price = item.ItemPrice,
                ItemId = item.ItemId
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
                    ItemName = item.Name,
                    ItemPrice = item.Price,
                    ItemId = item.ItemId,
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
            var sells = _db.Sells.Where(x => x.SellerID.ToString() == userId).ToList();
            return View(sells);
        }

        [HttpGet]
        public IActionResult MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Logout", "Account");
            var orders = _db.Sells.Where(x => x.BuyerID.ToString() == userId).ToList();
            return View(orders);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> BuyItem(string itemId)
        {
            if (ModelState.IsValid)
            {
                var ownerId = _db.Items.FirstOrDefault(x => x.ItemId.ToString() == itemId).OwnerID;
                var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);



                if (ownerId.ToString() == buyerId)
                {
                    return BadRequest("You cannot order your item!");
                }


                Sell sell = new Sell
                {
                    Id = Guid.NewGuid(),
                    BuyerID = Guid.Parse(buyerId),
                    ItemId = Guid.Parse(itemId),
                    SellerID = ownerId,
                    Date = DateTime.Now,
                    Status = SellStatus.WaitForOwner
                };
                try
                {
                    _db.Sells.Add(sell);
                    await _db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else return BadRequest();
            return RedirectToAction("MyOrders", "User");
        } // TODO : BUY


        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> AddToCart(string itemId) // TODO :CART
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = _db.Carts.Include(x=>x.ItemsInCart).FirstOrDefault(x => x.UserID == user.Id);
            if (user == null) return RedirectToAction("Logout", "Account");
            var item = _db.Items.FirstOrDefault(x => x.ItemId.ToString() == itemId);
            if (item == null) return BadRequest();
            cart.ItemsInCart.Add(new CartItem()
            {
                Cart = cart,
                CartItemID = cart.CartID,
                Item = item,
            });
            _db.Carts.Update(cart);
            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        public IActionResult Cart() // TODO : CART VIEW
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _db.Carts
                .Include(x => x.ItemsInCart)
                    .ThenInclude(x=>x.Item)
                .FirstOrDefault(x => x.UserID.ToString() == userId);
            return View(cart);
        }
    }  

}
