using Shop.BLL.Models;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class ItemService
    {
        private readonly ApplicationDBContext _db;
        public ItemService(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<ServicesResultModel> CreateItem(Item item, User user)
        {
            var result = new ServicesResultModel();
            if (item == null)
            {
                result.ResultCode = ResultCodes.Fail;
                result.Errors.Add("Item is null");
                return result;
            }
            if (user == null)
            {
                result.ResultCode = ResultCodes.Fail;
                result.Errors.Add("User is null");
                return result;
            }
            item.OwnerID = user.Id;
            item.OwnerUser = user;
            try
            {
                await _db.Items.AddAsync(item);
                await _db.SaveChangesAsync();
                result.ResultCode = ResultCodes.Success;
                return result;
            }
            catch (Exception e)
            {
                result.ResultCode = ResultCodes.Fail;
                result.Errors.Add(e.Message);
                return result;
            }

        }

        public async Task<ServicesResultModel> EditItem(Item item)
        {
            var result = new ServicesResultModel();
            if (item == null)
            {
                result.ResultCode = ResultCodes.Fail;
                result.Errors.Add("Item is null");
                return result;
            }
            var itemFromDb = await _db.Items.FindAsync(item.ItemId);
            if (itemFromDb == null)
            {
                result.ResultCode = ResultCodes.Fail;
                result.Errors.Add("Item not found");
                return result;
            }
            _db.Items.Update(item);
            await _db.SaveChangesAsync();
            return result;
        }

        public async Task<ServicesResultModel> DeleteItem(Item item)
        {
            var result = new ServicesResultModel();
            if (item == null)
            {
                result.Errors.Add("Item is null");
                return result;
            }
            var itemFromDb = await _db.Items.FindAsync(item.ItemId);
            if (itemFromDb == null)
            {
                result.Errors.Add("Item not found");
                return result;
            }
            _db.Items.Remove(itemFromDb);
            await _db.SaveChangesAsync();
            return result;
        }
    }
}
