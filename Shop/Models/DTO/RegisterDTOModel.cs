using Microsoft.AspNetCore.Mvc;

namespace Shop.Models.DTO
{
    public class RegisterDTOModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        public string HostUrl { get; set; }      
    }
}
