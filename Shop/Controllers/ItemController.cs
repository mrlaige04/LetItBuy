using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.BLL.DTO;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;
using Shop.UI.Clients.APICLIENTS;
using Shop.UI.Models.ViewDTO;
using System.Security.Claims;

namespace Shop.UI.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly ItemApiClient itemApiClient;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _db;
        public ItemController(ItemApiClient itemApi, IMapper mapper, ApplicationDBContext db)
        {
            itemApiClient = itemApi;
            _mapper = mapper;
            _db = db;
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetItem(string id)
        {
            if (Guid.TryParse(id, out Guid itemId))
            {
                var item = await _db.Items
                    .Include(x => x.Photos)
                    .Include(x => x.NumberCriteriaValues)
                    .Include(x => x.StringCriteriaValues)
                    .Include(x => x.OwnerUser).FirstOrDefaultAsync(x => x.ID.ToString() == itemId.ToString());//await itemApiClient.Get(itemId);
                if (item == null) return RedirectToAction("NotFoundPage", "Home");
                var itemDTO = _mapper.Map<ItemDTO>(item);
                itemDTO.IsYours = User.Identity.IsAuthenticated && User.FindFirstValue(ClaimTypes.NameIdentifier) == item.OwnerUser.Id.ToString();
                return View("ItemPage", new ItemViewModel() { Item = itemDTO });
            }
            else
            {
                return BadRequest("Invalid id");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Update()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> ConfirmSell(Guid id)
        {
            var sell = await _db.Sells.Where(x => x.OrderID == id).FirstOrDefaultAsync();
            if (sell == null) return RedirectToAction("NotFoundPage", "Home");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != sell.OwnerID.ToString()) return Unauthorized();
            sell.Status = OrderStatus.Confirmed;
            try
            {
                _db.Sells.Update(sell);
                await _db.SaveChangesAsync();
                return RedirectToAction("MySells", "User");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> CancelSell(Guid id, string returnUri)
        {
            var sell = await _db.Sells.Where(x => x.OrderID == id).FirstOrDefaultAsync();
            if (sell == null) return RedirectToAction("MySells", "User");
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.Parse(userid) != sell.BuyerID && Guid.Parse(userid) != sell.OwnerID) return Unauthorized();
            sell.Status = OrderStatus.Canceled;
            try
            {
                _db.Sells.Update(sell);
                await _db.SaveChangesAsync();
                return LocalRedirect(returnUri);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet]
        [HttpDelete]
        public async Task<IActionResult> DeleteSell(Guid id, string returnUri)
        {
            var sell = await _db.Sells.Where(x => x.OrderID == id).FirstOrDefaultAsync();
            if (sell == null) return RedirectToAction("MySells", "User");
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (Guid.Parse(userid) != sell.BuyerID && Guid.Parse(userid) != sell.OwnerID) return Unauthorized();

            try
            {
                _db.Sells.Remove(sell);
                await _db.SaveChangesAsync();
                return LocalRedirect(returnUri);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> AddDeliveryInfo(Guid id, string tracknumber)
        {
            var sell = await _db.Sells.Include(x => x.DeliveryInfo).Where(x => x.OrderID == id).FirstOrDefaultAsync();
            if (sell == null) return RedirectToAction("MySells", "User");
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid != sell.OwnerID.ToString()) return Unauthorized();

            sell.DeliveryInfo.TrackNumber = tracknumber;
            sell.Status = OrderStatus.Delivering;
            try
            {
                _db.Sells.Update(sell);
                await _db.SaveChangesAsync();
                return RedirectToAction("MySells", "User");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
