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
        private readonly ICustomEmailSender _sender;
        //private readonly IRepository _repository;
        public HomeController(ICustomEmailSender sender, ApplicationDBContext db)
        {
            _sender = sender;
            //_repository = new MultiShopRepository(db);
        }

        [HttpGet]
        public IActionResult GetWelcomePage()
        {
            return View("WelcomePage");
        }



        [HttpGet]
        public IActionResult SearchItems(string q)
        {
            return Ok();
        }



        [HttpGet]
        public IActionResult GetItem(string name)
        {
            return Ok();
        }
    }
}
