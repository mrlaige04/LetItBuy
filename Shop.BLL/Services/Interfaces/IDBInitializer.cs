

using Shop.BLL.Models;

namespace Shop.BLL.Services.Interfaces
{
    public interface IDBInitializer
    {
        public Task InitializeAsync();
    }
}
