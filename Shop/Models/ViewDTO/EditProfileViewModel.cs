namespace Shop.UI.Models.ViewDTO
{
    public class EditProfileViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AboutMe { get; set; }
        public IFormFile? Image { get; set; }
    }
}
