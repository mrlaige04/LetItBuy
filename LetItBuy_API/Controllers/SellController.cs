using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.API.Controllers
{
    [Authorize]
    public class SellController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
