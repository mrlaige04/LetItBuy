using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }
    }
}
