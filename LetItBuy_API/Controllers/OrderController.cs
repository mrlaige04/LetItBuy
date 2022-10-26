using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using System.Security.Claims;

namespace Shop.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]/{id?}")]
    public class OrderController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDBContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var owner = await _userManager.FindByIdAsync(order.OwnerID.ToString());
                    if (owner == null) return NotFound("Owner not found");

                    if (order.BuyerID != null)
                    {
                        var buyer = await _userManager.FindByIdAsync(order.BuyerID.ToString());
                        if (buyer == null) return NotFound("Buyer not found");
                    }
                    await _db.AddAsync(order);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId != order.OwnerID.ToString() && userId != order.BuyerID.ToString()) return Unauthorized();
                    if (await _db.Orders.FirstOrDefaultAsync(x => x.OrderID == order.OrderID) == null) return NotFound();
                    _db.Update(order);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid ID");
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.OrderID == id);
                if (order == null) return NotFound();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != order.OwnerID.ToString() && userId != order.BuyerID.ToString()) return Unauthorized();
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid ID");
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.OrderID == id);
                if (order == null) return NotFound();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != order.OwnerID.ToString() && userId != order.BuyerID.ToString()) return Unauthorized();
                order.Status = OrderStatus.Canceled;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delivery(Guid id, string tracknumber)
        {
            if (id == Guid.Empty) return BadRequest("Invalid id");
            if (string.IsNullOrEmpty(tracknumber) || string.IsNullOrWhiteSpace(tracknumber)) return BadRequest("Invalid track number");

            try
            {
                var order = await _db.Orders.Include(x => x.DeliveryInfo).FirstOrDefaultAsync(x => x.OrderID == id);
                if (order == null) return NotFound();
                if (order.DeliveryInfo == null) return StatusCode(404, "Delivery info not found");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != order.OwnerID.ToString()) return Unauthorized();
                order.DeliveryInfo.TrackNumber = tracknumber;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
