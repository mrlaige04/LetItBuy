using Microsoft.AspNetCore.Mvc;

namespace Shop.Models.DTO
{
    public class LoginDTOModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }

        public IUrlHelper urlHelper { get; set; }
    }
}
