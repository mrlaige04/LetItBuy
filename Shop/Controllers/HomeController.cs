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
            
            return View("WelcomePage");
        }
        
        [HttpGet]
        public IActionResult GetItem(Guid id)
        {
            return View("ItemPage");
        }

        
    }
}
