using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Models.UserModels;
using System.Security.Claims;

namespace Shop.Controllers
{
    [Authorize(Roles="simpleUser")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHost;
        public UserController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHost = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["webrootpath"] = Request.Host.Value;
            return View("MyProfile", new ProfileViewModel { user = user });
        }

        
        [HttpPost]
        public async Task<IActionResult> SetPhoto(IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var ext = photo.FileName.Substring(photo.FileName.LastIndexOf('.'));
                string path = Path.Combine("UserPhotos", $"{userId}{ext}");

                using (var fs = new FileStream(Path.Combine("wwwroot", path), FileMode.Create))
                {
                    photo.CopyTo(fs);
                }
                
                var user = await _userManager.GetUserAsync(User);
                user.HasPhoto = true;
                var res = await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("GetProfile");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserImage()
        {
            var user = await _userManager.GetUserAsync(User);

            //System.IO.File.Delete("wwwroot\\" + user.ImageURL);
            user.HasPhoto = false;
            var res = await _userManager.UpdateAsync(user);
            
            return RedirectToAction("GetProfile");
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(string email, Item item)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user==null)
            {
                ModelState.AddModelError("", "User not found");
                return BadRequest(ModelState);
            }
            else
            {
                if (user.Items == null)
                {
                    user.Items = new List<Item>();
                }
                user?.Items?.Add(item);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error while adding item");
                }
                return RedirectToAction("MyItems", "User");  
            }           
        }
        
        //[HttpDelete]
        //public async void RemoveItem(string email, Guid itemId)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "User not found");
        //    }
        //    else
        //    {
        //        var item = user.Items?.FirstOrDefault(i => i.ItemId == itemId);
        //        if (item == null)
        //        {
        //            ModelState.AddModelError("", "Item not found");
        //        }
        //        else
        //        {
        //            user.Items?.Remove(item);
        //            var result = await _userManager.UpdateAsync(user);
        //            if (!result.Succeeded)
        //            {
        //                ModelState.AddModelError("", "Error while removing item");
        //            }
        //        }
        //    }
        //}

        [HttpPost]
        public void EditItem(Item item)
        {
            
        }
    }
}
