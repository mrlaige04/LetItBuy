using Microsoft.AspNetCore.Mvc;
using Shop.Services;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _sender;
        public HomeController(IEmailSender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> IndexAsync()
        {
            await SendEmail();
            return View();
        }

        [HttpGet]
        public IActionResult GetWelcomePage()
        {
            return View("WelcomePage");
        }
        
        [HttpGet]
        public IActionResult GetItem(Guid id)
        {
            return View("ItemPage");
        }

        private async Task SendEmail()
        {
            await _sender.SendEmailAsync("illia.rudiakov11@gmail.com", "NEW ITEM", "NEW PHONE: https://google.com/");
        }
    }
}
