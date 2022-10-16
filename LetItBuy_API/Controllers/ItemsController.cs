using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace LetItBuy_API.Controllers
{
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly ApplicationDBContext _db;
        public ItemsController(ApplicationDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets Advert by ID
        /// </summary>
        /// <param name="id">Id of advert</param>
        /// <returns>Advert</returns>
        [HttpGet]
        [Route("api/[controller]/{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid id");
            var advert = await _db.Items.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (advert == null) return NotFound();
            return Ok(advert);
        }

        
        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _db.Items.ToListAsync();
            if (items == null) return NotFound();
            return Ok(items);
        }

        [HttpPost]
        [Authorize]
        [Route("api/[controller]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Item item)
        {
            try
            {
                await _db.Items.AddAsync(item);
                await _db.SaveChangesAsync();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }


        
        [HttpPost]
        [Authorize]
        [Route("api/[controller]/{id:Guid}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] Item item)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid id");
            }
            if (item == null)
            {
                return BadRequest("Invalid item");
            }
            var itemFromDB = await _db.Items.FirstAsync(x => x.ID == id);
            if (itemFromDB == null) return NotFound();
            try
            {
                itemFromDB = item;
                _db.Items.Update(itemFromDB);
                await _db.SaveChangesAsync();
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete]
        [Authorize]
        [Route("api/[controller]/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Invalid id");
            var item = await _db.Items.FirstAsync(x => x.ID == id);
            if (item == null) return NotFound();
            try
            {
                _db.Items.Remove(item);
                await _db.SaveChangesAsync();
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
