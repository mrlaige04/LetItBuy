namespace Shop.Clients
{
    public class API_Client
    {
        private HttpClient _client;
        private HttpRequestMessage _message;
        private string _baseAddress;
        public API_Client()
        {
            _client = new();
            _baseAddress = "https://localhost:7102/";
            _message = new();
        }

        public void CreateAdvert()
        {
            
        }

        public void DeleteAdvert()
        {
            
        }

        public void GetAdvertById()
        {
            
        }

        public void GetAdvertsByName()
        {
            
        }    
    }
}
