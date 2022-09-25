using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetItBuy_API.Controllers
{
    [ApiController]
    
    public class AdvertController : Controller
    {

        [HttpGet]
        [Route("api/[controller]/Adverts/{id:Guid}")]
        public ActionResult GetAdvert(Guid id)
        {
            return Ok();
        }


        [HttpGet]
        [Route("api/[controller]/Adverts")]
        public IActionResult GetAdverts()
        {
            return Ok();
        }

        [HttpPost]
        [Route("api/[controller]/Adverts/Create")]
        public ActionResult Create(object name)
        {
            return Ok();
        }

        //// POST: AdvertController/Create
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

        //// GET: AdvertController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AdvertController/Edit/5
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

        //// GET: AdvertController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AdvertController/Delete/5
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
