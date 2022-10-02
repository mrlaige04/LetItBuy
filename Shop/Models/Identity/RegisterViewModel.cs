using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Models.Identity
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage="EmailIsRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
    }
}
