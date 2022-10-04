using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Models.ViewDTO
{
    public class ExternalRegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect address")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
