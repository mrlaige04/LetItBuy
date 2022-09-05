using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Shop.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        
        public AdminController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }

        public void CreateCategory()
        {
            
        }
    }
}
