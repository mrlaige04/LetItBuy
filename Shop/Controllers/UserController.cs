using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public UserController()
        {

        }


        [HttpPost]
        public void AddItem(Item item)
        {
            
        }
        
        [HttpDelete]
        public void RemoveItem(Item item)
        {
            
        }

        [HttpPost]
        public void EditItem(Item item)
        {
            
        }
    }
}
