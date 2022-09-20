using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using Shop.Controllers;
using Shop.Models;
using Shop.Models.ClientsModels;
using Shop.Models.DTO;
using Shop.Models.Identity;
using Shop.Services;
using System.Text.RegularExpressions;

namespace Shop.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly ICustomEmailSender _emailSender;
        private readonly RoleService _roleClient;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public IUrlHelper urlHelper;
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountService> logger, ICustomEmailSender emailSender, RoleService roleClient, IStringLocalizer<SharedResource> localizer)
        {
            _roleClient = roleClient;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        public async Task<ServicesResultModel> RegisterAsync(RegisterDTOModel reg_dto)
        {
            var cartId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            
            User user = new User
            {
                Email = reg_dto.Email,
                UserName = reg_dto.Username,
                Cart = new Cart()
                {
                    CartID = cartId,
                    UserID = userId,
                    ItemsInCart = new List<CartItem>() { }
                },
                CartID = cartId,
                Items = new List<Item>(),
                Id = userId
            };
            user.Cart.UserOwner = user;

            
            

            var result = await _userManager.CreateAsync(user, reg_dto.Password);
            if (result.Succeeded) {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                
                var callbackUrl = "https://" + reg_dto.HostUrl + "/Account/ConfirmEmail" + $"?userId={user.Id}&code={code}";

                var text = GetEmailConfirmText(callbackUrl);
                var sendEmailResult = await _emailSender.SendEmailAsync(user.Email, "Confirm your account",
                        text);
                if (!sendEmailResult)
                {
                    return await DeleteMyAccountByEmailAsync(user.Email);                     
                }
                var ad_rol_res = await _roleClient.AddUserToRoles(user, "simpleUser");

                return new UserRegisterResultModel { ResultCode = ResultCodes.Successed };
            }
            else return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = result.Errors.Select(x => x.Description) };
        }
        public async Task<ServicesResultModel> ConfirmEmailAsync(string userId, string code)
        {
            if ((await _userManager.FindByIdAsync(userId)).EmailConfirmed) return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Email is already confirmed" } };
            code = Regex.Replace(code, " ", "+");
            if (userId == null || code == null)
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "UserID or Code is Null" } };

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = result.Errors.Select(x => x.Description) };
        }
        public async Task<ServicesResultModel> LoginAsync(LoginDTOModel log_model)
        {
            var user = await _userManager.FindByEmailAsync(log_model.Email);
            if (user == null)
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { _localizer["UserNotFound"] } };
            var sign_res = await _signInManager.PasswordSignInAsync(user, log_model.Password, log_model.RememberMe, false);           
            if (sign_res.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { _localizer["WrongPassOrEm"] } };
        }                 
        public async Task<ServicesResultModel> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new ServicesResultModel { ResultCode = ResultCodes.Successed };
        }
        public async Task<ServicesResultModel> ForgotPasswordAsync(ForgotPasswordDTOModel fp_model)
        {
            var user = await _userManager.FindByEmailAsync(fp_model.Email);
            if (user == null) return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = "https://" + fp_model.HostUrl + "/Account/ResetPassword" + $"?userId={user.Id}&code={code}";
            
            var send_em_res = await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            if (!send_em_res)
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Email not sent. Try one more time!" } };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
        }
        public async Task<ServicesResultModel> DeleteMyAccountAsync(string email, string password)
        {            
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password)==PasswordVerificationResult.Failed)
                {
                    return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Wrong password" } };
                }
        
                var delRes = await _userManager.DeleteAsync(user);
                if (!delRes.Succeeded)
                {
                    return new ServicesResultModel
                    {
                        ResultCode = ResultCodes.Failed,
                        Errors = delRes.Errors.Select(x => x.Description)
                    };
                }
                else return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            }
            else return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Couldn't find a user" } };
        } 
        public async Task<ServicesResultModel> DeleteMyAccountByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var delRes = await _userManager.DeleteAsync(user);
                if (!delRes.Succeeded)
                {
                    return new ServicesResultModel
                    {
                        ResultCode = ResultCodes.Failed,
                        Errors = delRes.Errors.Select(x => x.Description)
                    };
                }
                else return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            }
            else return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Couldn't find a user" } };
        }
        public async Task<ServicesResultModel> ChangePasswordAsync(ChangePasswordDTOModel ch_pmodel)
        {
            var user = await _userManager.FindByEmailAsync(ch_pmodel.Email);
            var ch_p_res = await _userManager.ChangePasswordAsync(user, ch_pmodel.OldPassword, ch_pmodel.NewPassword);
            if (ch_p_res.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = ch_p_res.Errors.Select(x => x.Description) };
        }
        public async Task<ServicesResultModel> ChangeEmailAsync(ChangeEmailDTOModel ch_emodel)
        {
            var user = await _userManager.FindByEmailAsync(ch_emodel.OldEmail);
            if (user == null) return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            var changeEmail_Token = await _userManager.GenerateChangeEmailTokenAsync(user, ch_emodel.NewEmail);

            var emailText = urlHelper?.ActionLink("ConfirmChangeEmailAsync", "Account", new
            {
                userId = user.Id,
                newEmail = ch_emodel.NewEmail,
                code = changeEmail_Token
            }); 
            
            var send_email_result = await _emailSender.SendEmailAsync(ch_emodel.OldEmail, "Change Email", $"Please change your email by clicking here: <a href='{emailText}'>link</a>");
            if (send_email_result)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "Email not sent. Try one more time!" } };
        }
        public async Task<ServicesResultModel> ConfirmChangeEmailAsync(string token, string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = new List<string> { "User not found" } };
            var ch_e_res = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (ch_e_res.Succeeded)
                return new ServicesResultModel { ResultCode = ResultCodes.Successed };
            else
                return new ServicesResultModel { ResultCode = ResultCodes.Failed, Errors = ch_e_res.Errors.Select(x => x.Description) };
        }
        public string GetEmailConfirmText(string urlCallback)
        {
            var html = string.Empty;
            using (StreamReader reader = new StreamReader("./Texts/confirmemail.html"))
            {
                html = reader.ReadToEnd();
            }
            var str = html.Replace("{0}",
                urlCallback);
            return str;
        }
    }
}
