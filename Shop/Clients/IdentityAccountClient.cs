using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Controllers;
using Shop.Models;
using Shop.Models.ClientsModels;
using Shop.Models.DTO;
using Shop.Models.Identity;
using Shop.Services;


namespace Shop.Clients
{
    public class IdentityAccountClient
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<IdentityAccountClient> _logger;
        private readonly ICustomEmailSender _emailSender;
        private readonly RoleClient _roleClient;
        public IdentityAccountClient(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<IdentityAccountClient> logger, ICustomEmailSender emailSender, RoleClient roleClient)
        {
            _roleClient = roleClient;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        
        
        public async Task<ClientsResultModel> RegisterAsync(RegisterDTOModel reg_dto)
        {
            var cartId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            User user = new User
            {
                Email = reg_dto.Email,
                UserName = reg_dto.Username,
                Cart = new Cart()
                {
                    CartID = cartId,
                    UserID = userId,
                    ItemsInCart = new List<CartItem>()
                },
                CartID = cartId,
                Items = new List<Item>(),
                Id = userId
            };

            var result = await _userManager.CreateAsync(user, reg_dto.Password);
            if (result.Succeeded) {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                
                var callbackUrl = "https://" + reg_dto.HostUrl + "/Account/ConfirmEmail" + $"?userId={user.Id}&code={code}";

                var sendEmailResult = await _emailSender.SendEmailAsync(user.Email, "Confirm your account",
                        $"Confirm Registration, follow the: <a href='{callbackUrl}'>link</a>");
                if (!sendEmailResult)
                {
                    return await DeleteMyAccountAsync(user.Email);                     
                }
                var ad_rol_res = await _roleClient.AddUserToRoles(user, "simpleUser");

                return new UserRegisterResultModel { ResultCode = ResultCodes.Successed };
            }
            else return new ClientsResultModel { ResultCode = ResultCodes.Failed, Errors = result.Errors.Select(x => x.Description) };
        }
        public void Login()
        {
            
        }

        public void Logout()
        {
            
        }
        
       
        public void ConfirmEmail()
        {
            
        }

        public void ForgotPassword()
        {
            
        }
        
        public void ResetPassword()
        {
            
        }

        public async Task<ClientsResultModel> DeleteMyAccountAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var delRes = await _userManager.DeleteAsync(user);
                if (!delRes.Succeeded)
                {
                    return new ClientsResultModel
                    {
                        ResultCode = ResultCodes.Failed,
                        Errors = delRes.Errors.Select(x => x.Description)
                    };
                }
                else return new ClientsResultModel { ResultCode = ResultCodes.Successed };
            }
            else return new ClientsResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Couldn't find a user" } };
        }       
    }
}
