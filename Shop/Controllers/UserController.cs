using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    [Authorize(Roles="simpleUser")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
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
        
        [HttpDelete]
        public async void RemoveItem(string email, Guid itemId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
            }
            else
            {
                var item = user.Items?.FirstOrDefault(i => i.ItemId == itemId);
                if (item == null)
                {
                    ModelState.AddModelError("", "Item not found");
                }
                else
                {
                    user.Items?.Remove(item);
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Error while removing item");
                    }
                }
            }
        }

        [HttpPost]
        public void EditItem(Item item)
        {
            
        }
    }
}
