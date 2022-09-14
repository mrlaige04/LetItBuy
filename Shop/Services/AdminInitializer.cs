using Microsoft.AspNetCore.Identity;
using Shop.Models;

namespace Shop.Services
{
    public class AdminInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        public AdminInitializer(UserManager<User> userManamer, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _userManager = userManamer;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task InitializeAdminAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }
            var cartId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            User user = new User()
            {
                Email = _configuration["Admin:Email"],
                UserName = _configuration["Admin:UserName"],
                EmailConfirmed = true,
                Id = userId
                
            };
            var createAdmin_Result = await _userManager.CreateAsync(user, _configuration["Admin:Password"]);
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
