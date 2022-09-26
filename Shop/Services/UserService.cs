using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Models.ClientsModels;

namespace Shop.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHost;
        public UserService(UserManager<User> userManager, ApplicationDBContext db, IWebHostEnvironment webhost)
        {
            _userManager = userManager;
            _db = db;
            _webHost = webhost;
        }

        public async Task<ServicesResultModel> AddItemAsync(User user, Item item)
        {           
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            item.OwnerUser = user;
            item.OwnerID = user.Id;
            try
            {
                await _db.Items.AddAsync(item);
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Successed };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ServicesResultModel> EditItemAsync (User user, Item item)
        {
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            var itemFromDb = await _db.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
            if (itemFromDb == null) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Item not found" } };
            if (itemFromDb.OwnerID != user.Id) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "You can't edit this item" } };
            itemFromDb.ItemName = item.ItemName;
            itemFromDb.Description = item.Description;
            itemFromDb.ItemPrice = item.ItemPrice;
            itemFromDb.Currency = item.Currency;
            //itemFromDb.Category = item.Category;
            //itemFromDb.Image = item.Image;
            try
            {
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Successed };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<ServicesResultModel> DeleteItemAsync(User user, Guid itemId)
        {
            if (user == null) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            var itemFromDb = await _db.Items.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (itemFromDb == null) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Item not found" } };
            if (itemFromDb.OwnerID != user.Id) return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { "You can't delete this item" } };
            try
            {
                if(itemFromDb.ImageUrl != null) {
                    var path = Path.Combine(_webHost.ContentRootPath, "\\wwwroot\\ItemPhotos\\", itemFromDb.ImageUrl);
                    File.Delete(_webHost.ContentRootPath + "\\wwwroot\\ItemPhotos\\" + itemFromDb.ImageUrl);
                }
                
                _db.Items.Remove(itemFromDb);
                await _db.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Successed };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { ex.Message } };
            }
        }
    }
}
