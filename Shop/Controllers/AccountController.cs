using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shop.Clients;
using Shop.Models;
using Shop.Models.ClientsModels;
using Shop.Models.DTO;
using Shop.Models.Identity;
using Shop.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomEmailSender _emailSender;
        private readonly ILogger<AccountController> _acc_contr_logger;
        private readonly ILogger<IdentityAccountClient> _acc_client_logger;

        private readonly IdentityAccountClient accountClient;
        private readonly RoleClient roleClient;
        public AccountController(IServiceProvider provider)
        {

            _userManager = provider.GetRequiredService<UserManager<User>>();
            _signInManager = provider.GetRequiredService<SignInManager<User>>();
            _roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            _emailSender = provider.GetRequiredService<ICustomEmailSender>();
            _acc_contr_logger = provider.GetRequiredService<ILogger<AccountController>>();
            _acc_client_logger = provider.GetRequiredService<ILogger<IdentityAccountClient>>();

            
            roleClient = new RoleClient(_userManager, _roleManager);
            accountClient = new IdentityAccountClient(_userManager, _signInManager, _acc_client_logger, _emailSender, roleClient);
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {           
            if (ModelState.IsValid)
            {              
                var register_result = await accountClient.RegisterAsync(new RegisterDTOModel { Email = model.Email, Password = model.Password, Username = model.UserName, HostUrl = Request.Host.Value });
                if (register_result.ResultCode == ResultCodes.Failed)
                {
                    foreach (var item in register_result.Errors)
                    {
                        _acc_contr_logger.LogError(item);
                        ModelState.AddModelError("", item);
                    }                    
                } else return View("ConfirmEmail");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if (ModelState.IsValid)
            {
                var conf_em_res = await accountClient.ConfirmEmailAsync(userId, code);
                if (conf_em_res.ResultCode == ResultCodes.Successed)
                    return RedirectToAction("Login", "Account");
                else
                    return View("Error", new ErrorViewModel { ErrorMessages = conf_em_res.Errors });
            }
            else return View("Error", new ErrorViewModel { ErrorMessages = new List<string> { "Invalid model state" } });
        }
 
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return View("Error", new ErrorViewModel { ErrorMessages = new List<string> { "You are already logged in" } });
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login_result = await accountClient.LoginAsync(new LoginDTOModel { Email = model.Email, Password = model.Password, RememberMe = model.RememberMe, ReturnUrl = model.ReturnUrl, urlHelper = Url });
                if (login_result.ResultCode == ResultCodes.Successed)
                    return Redirect(model.ReturnUrl ?? "../");
                else
                {
                    foreach (var item in login_result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("GetWelcomePage", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fp_res = await accountClient.ForgotPasswordAsync(new ForgotPasswordDTOModel { Email = model.Email, HostUrl = Request.Host.Value });
                if (fp_res.ResultCode == ResultCodes.Successed)
                    return View("ForgotPasswordConfirmation");
                else
                {
                    foreach (var item in fp_res.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(model);
        }

        
        [HttpGet]
        public IActionResult ChangePasswordAsync() => View("ChangePassword");

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync (ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var cp_res = await accountClient.ChangePasswordAsync(new ChangePasswordDTOModel { Email = email, NewPassword = model.NewPassword, OldPassword = model.OldPassword });
                if (cp_res.ResultCode == ResultCodes.Successed)
                    return View("ChangePasswordConfirmation");
                else
                {
                    foreach (var item in cp_res.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(model);
        }
        


        [HttpPost]
        public async Task<IActionResult> DeleteMyAccount(string email)
        {

            var del_ac_res = await accountClient.DeleteMyAccountAsync(email);
            if (del_ac_res.ResultCode == ResultCodes.Successed)
                return RedirectToAction("GetWelcomePage", "Home");
            else
                return View("Error", new ErrorViewModel { ErrorMessages = del_ac_res.Errors });
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
