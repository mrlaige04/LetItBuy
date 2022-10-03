using System.ComponentModel.DataAnnotations;

namespace Shop.WEB.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }
    }
}
