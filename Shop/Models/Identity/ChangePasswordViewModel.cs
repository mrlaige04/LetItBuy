using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Models.Identity
{
    public class ChangePasswordViewModel
    {
        public string? StatusMessage { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Currentpassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "NewPassLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Newpassword")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPass")]
        [Compare("NewPassword", ErrorMessage = "DoNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}
