using Shop.BLL.Providers.Interfaces;

using Shop.DAL.Data.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shop.UI.Clients.APICLIENTS
{
    public class ItemApiClient
    {
        private readonly string baseUrl = "https://localhost:7102/api/items";
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessage requestMessage;
        public ItemApiClient(IJwtTokenProvider tokenProvider)
        {
            _jwtTokenProvider = tokenProvider;
            _httpClient = new HttpClient();
            requestMessage = new HttpRequestMessage();
        }
        public async Task<Item?> Get(Guid id)
        {
            var path = $"{baseUrl}/{id}";
            requestMessage.RequestUri = new Uri(path);
            var response = await _httpClient.GetAsync(requestMessage.RequestUri);
            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<Item>();
                return item;
            }
            else
            {
                return null;
            }
        }

        public void GetAll()
        {
            
        }

        public void Take(int skip, int n)
        {
            
        }

        public async void Add(string userId)
        {
            string token = await _jwtTokenProvider.GenerateTokenAsync(userId);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Item item = new Item()
            {
                
                Name = "Test",
                Description = "Test",
                Price = 100,
                ImageUrl = "Test",
                OwnerID = new Guid("c9c9c9c9-c9c9-c9c9-c9c9-c9c9c9c9c9c9")
            };

            var response = await _httpClient.PostAsJsonAsync(new Uri("https://localhost:7102/" + "api/advert/create"), item);
        }

        public void Delete()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
