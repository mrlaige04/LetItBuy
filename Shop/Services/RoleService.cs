using Microsoft.AspNetCore.Identity;
using Shop.Models;
using Shop.Models.ClientsModels;

namespace Shop.Services
{
    public class RoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<ServicesResultModel> DeleteRole(string roleName)
        {
            var delRes = await _roleManager.DeleteAsync(new IdentityRole<Guid>(roleName));
            if (delRes.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = delRes.Errors.Select(x => x.Description).ToList() };
        }
        public async Task<ServicesResultModel> AddUserToRoles(User user, params string[] roles)
        {
            foreach (var item in roles)
            {
                if (!await _roleManager.RoleExistsAsync(item)) await _roleManager.CreateAsync(new IdentityRole<Guid>(item));
            }
            var res = await _userManager.AddToRolesAsync(user, roles);
            if (res.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = res.Errors.Select(x => x.Description) };
        }
        public async Task<ServicesResultModel> DeleteUserFromRoles(User user, params string[] roles)
        {
            var res = await _userManager.RemoveFromRolesAsync(user, roles);
            if (res.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = res.Errors.Select(x => x.Description) };
        }       
    }
}
