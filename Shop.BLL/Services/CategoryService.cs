using Shop.BLL.Models;
using Shop.DAL.Data.EF;
using Shop.DAL.Data.Entities;


namespace Shop.BLL.Services
{
    public class CategoryService
    {
        private readonly ApplicationDBContext _context;
        public CategoryService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories() => _context.Categories.ToList();
        
        public async Task<ServicesResultModel> AddCategoryAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Success };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Fail, Errors = new List<string> { ex.Message } };
            }
        }

        public Category GetCategoryById(string id) => _context.Categories.FirstOrDefault(x => x.Id.ToString() == id);
    }
}
