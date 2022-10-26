using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.BLL.Models;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHost;
        public UserService(UserManager<ApplicationUser> userManager, ApplicationDBContext db, IWebHostEnvironment webhost)
        {
            _userManager = userManager;
            _db = db;
            _webHost = webhost;
        }

        public async Task<ServicesResultModel> AddItemAsync(ApplicationUser user, Item item)
        {
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "User not found" } };
            item.OwnerUser = user;
            item.OwnerID = user.Id;
            try
            {
                await _db.Items.AddAsync(item);
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Success };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ServicesResultModel> EditItemAsync(ApplicationUser user, Item item)
        {
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "User not found" } };
            var itemFromDb = await _db.Items.FirstOrDefaultAsync(i => i.ID == item.ID);
            if (itemFromDb == null) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "Item not found" } };
            if (itemFromDb.OwnerID != user.Id) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "You can't edit this item" } };
            itemFromDb.Name = item.Name;
            itemFromDb.Description = item.Description;
            itemFromDb.Price = item.Price;
            itemFromDb.Currency = item.Currency;
            //itemFromDb.Category = item.Category;
            //itemFromDb.Image = item.Image;
            try
            {
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Success };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ServicesResultModel> DeleteItemAsync(ApplicationUser user, Guid itemId)
        {
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "User not found" } };
            var itemFromDb = await _db.Items.FirstOrDefaultAsync(i => i.ID == itemId);
            if (itemFromDb == null) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "Item not found" } };
            if (itemFromDb.OwnerID != user.Id) return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { "You can't delete this item" } };
            try
            {
                if (itemFromDb.ImageUrl != null)
                {
                    var path = Path.Combine(_webHost.ContentRootPath, "\\wwwroot\\ItemPhotos\\", itemFromDb.ImageUrl);
                    File.Delete(_webHost.ContentRootPath + "\\wwwroot\\ItemPhotos\\" + itemFromDb.ImageUrl);
                }

                _db.Items.Remove(itemFromDb);
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Success };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { ex.Message } };
            }
        }
    }
}
