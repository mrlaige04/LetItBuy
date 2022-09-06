using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Shop.Controllers
{
    [Authorize(Roles="Admin")]
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
            //var forms = HttpContext.Request.Form;
        }
    }
}
