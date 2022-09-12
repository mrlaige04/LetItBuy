using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Shop.Repositories;
using Shop.Services;
using System.Text.Json;

namespace Shop.Controllers
{
    //public class HomeController : Controller
    //{
    //    private readonly ICustomEmailSender _sender;
    //    //private readonly IRepository _repository;
    //    public HomeController(ICustomEmailSender sender, ApplicationDBContext db)
    //    {
    //        _sender = sender;
    //        //_repository = new MultiShopRepository(db);
    //    }
        
    //    [HttpGet]
    //    public IActionResult GetWelcomePage()
    //    { 
    //        return View("WelcomePage");
    //    }  

        

    //    [HttpGet]
    //    public IActionResult SearchItems(string q)
    //    {           
    //        if(Guid.TryParse(q, out Guid guid))
    //        {
    //            //var item = _repository.GetItemByID(guid);
    //            return View("ManyItems", item);
    //        } else
    //        {
    //            //var items = _repository.GetItemsByName(q);
    //            return View("ManyItems", items);
    //        }           
    //    }
        
        

    //    [HttpGet]
    //    public IActionResult GetItem(string name)
    //    {
    //        var items = _repository.GetItemsByName(name);
    //        if (items != null)
    //            return View("ManyItems", items);
    //        else
    //            return NotFound();
    //    }
    //}
}
