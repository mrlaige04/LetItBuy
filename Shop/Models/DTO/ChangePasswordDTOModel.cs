using System.ComponentModel.DataAnnotations;

namespace Shop.Models.DTO
{
    public class ChangePasswordDTOModel
    {
        public string StatusMessage { get; set; }             
        public string OldPassword { get; set; }     
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }       
        public string Email { get; set; }
    }
}
