using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Repositories;
using Shop.Services;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _sender;
        private readonly IRepository _repository;
        public HomeController(IEmailSender sender, ApplicationDBContext db)
        {
            _sender = sender;
            _repository = new MultiShopRepository(db);
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
            _repository.AddUser(new Models.User() { });
            await _sender.SendEmailAsync("illia.rudiakov11@gmail.com", "NEW ITEM", "NEW PHONE: https://google.com/");
        }
    }
}
