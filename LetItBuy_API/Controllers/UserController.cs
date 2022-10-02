using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Providers;
using Shop.DAL.Data.EF;

namespace Shop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class UserController : Controller
    {
        // GET: UserController
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _env;
        public UserController(ApplicationDBContext db, IWebHostEnvironment env)
        {
            _db = db;
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet("Token")]
        public async Task<IActionResult> Token(string userId)
        {
            JwtTokenProvider jwtTokenProvider = new JwtTokenProvider(_db, _env);
            var token = await jwtTokenProvider.GenerateToken(userId);
            return Content(token);
        }

        //// GET: UserController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: UserController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UserController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
