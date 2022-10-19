
using Shop.BLL.DTO;
using Shop.UI.Models.Identity;
using Shop.UI.Models.ViewDTO;

namespace Shop.Models.UserModels
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string AboutMe { get; set; }

        public EditProfileViewModel EditProfile { get; set; }
        public ChangePasswordViewModel ChangePassword { get; set; }
    }
}
