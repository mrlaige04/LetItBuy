using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class AdminInitializer : IDisposable
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        public AdminInitializer(UserManager<ApplicationUser> userManamer, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _userManager = userManamer;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public void Dispose()
        {
            
        }

        public async Task InitializeAdminAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("simpleUser"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("simpleUser"));
            }

            var userId = Guid.NewGuid();
            
            ApplicationUser user = new ApplicationUser()
            {
                Email = _configuration["Admin:Email"],
                UserName = _configuration["Admin:UserName"],
                EmailConfirmed = true,
                Id = userId
            };

            var createAdmin_Result = await _userManager.CreateAsync(user, _configuration["Admin:Password"]);
            if (createAdmin_Result.Succeeded) await _userManager.AddToRoleAsync(user, "Admin");


            var user2 = new ApplicationUser()
            {
                Email = "sabfasvf2b@gmail.com",
                UserName = "TestUser",
                EmailConfirmed = true,
                Id = Guid.NewGuid()
            };
            var createAdmin_Result2 = await _userManager.CreateAsync(user2, "Laige123");
            if (createAdmin_Result2.Succeeded) await _userManager.AddToRoleAsync(user2, "simpleUser");
        }
    }
}
