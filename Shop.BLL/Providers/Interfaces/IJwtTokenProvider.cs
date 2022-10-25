namespace Shop.BLL.Providers.Interfaces
{
    public interface IJwtTokenProvider
    {
        public Task<string> GenerateTokenAsync(string userId);
    }
}
