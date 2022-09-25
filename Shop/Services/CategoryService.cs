using Shop.Data;
using Shop.Models.ClientsModels;
using Shop.Models;
namespace Shop.Services
{
    public class CategoryService
    {
        private readonly ApplicationDBContext _context;
        public CategoryService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories() => _context.Categories.ToList();
        public List<Category> GetSubCategories(string currentCategoryID) => _context.Categories.Where(x => x.Parent.Id.ToString() == currentCategoryID).ToList();
        public async Task<ServicesResultModel> AddCategoryAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new ServicesResultModel() { ResultCode = ResultCodes.Successed };
            }
            catch (Exception ex)
            {
                return new ServicesResultModel() { ResultCode = ResultCodes.Failed, Errors = new List<string> { ex.Message } };
            }
        }

        public Category GetCategoryById(string id) => _context.Categories.FirstOrDefault(x => x.Id.ToString() == id);
    }
}
