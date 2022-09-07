using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Repositories;
using Shop.Services;
using System.Text.Json;

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
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWelcomePage()
        {
            var guid = Guid.NewGuid();
            var user = await SendEmail(guid);
            Console.WriteLine(user is null);
            Console.WriteLine(user.Email);
            Console.WriteLine(JsonSerializer.Serialize(user));
            return View("WelcomePage");
        }
        
        [HttpGet]
        public IActionResult GetItem(Guid id)
        {
            return View("ItemPage");
        }

        private async Task<User> SendEmail(Guid guid)
        {
            
            _repository.AddUser(new Models.User()
            {
                Email = "someemail3@gmail.com",
                Id = guid,
                Items = new List<Item>() {
                    new Item() {
                        Description = "New tovar",
                        ItemId = Guid.NewGuid(),
                        ItemName = "New phone"
                    }
                }
                });
            var user = _repository.GetUser(guid);
            return user;
            //await _sender.SendEmailAsync("illia.rudiakov11@gmail.com", "NEW ITEM", "NEW PHONE: https://google.com/");
        }
    }
}
