using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Models.Identity
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        public IEnumerable<AuthenticationScheme>? ExternalProviders { get; set; }
    }
}
