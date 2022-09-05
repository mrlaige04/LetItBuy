using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
