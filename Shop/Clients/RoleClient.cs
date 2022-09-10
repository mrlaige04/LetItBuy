using Microsoft.AspNetCore.Identity;
using Shop.Models;
using Shop.Models.ClientsModels;

namespace Shop.Clients
{
    public class RoleClient
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleClient(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //public async Task<ClientsResultModel> CreateRole(string roleName)
        //{
            
        //}

        //public async Task<ClientsResultModel> DeleteRole(string roleName)
        //{

        //}

        public async Task<ClientsResultModel> AddUserToRoles(User user, params string[] roles)
        {
            foreach (var rol in roles)
            {
                var res = await AddUserToRole(user, rol);
                if (res.ResultCode == ResultCodes.Failed) continue;
            }
            return new ClientsResultModel { ResultCode = ResultCodes.Successed };
        }
        private async Task<ClientsResultModel> AddUserToRole(User user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var cr_rol_res = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!cr_rol_res.Succeeded) return new ClientsResultModel { ResultCode = ResultCodes.Failed, Errors = cr_rol_res.Errors.Select(x => x.Description) };
            }
            var add_rol_res = await _userManager.AddToRoleAsync(user, role);
            if (!add_rol_res.Succeeded) return new ClientsResultModel { ResultCode = ResultCodes.Failed, Errors = add_rol_res.Errors.Select(x => x.Description) };
            return new ClientsResultModel { ResultCode = ResultCodes.Successed };
        }
    }
}
