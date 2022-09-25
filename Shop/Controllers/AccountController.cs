using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Services;
using Shop.Models;
using Shop.Models.ClientsModels;
using Shop.Models.DTO;
using Shop.Models.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {       
        private readonly ILogger<AccountController> _logger;

        private readonly SignInManager<User> _signinmanager;
        private readonly AccountService _accountService;
        private readonly IConfiguration _config;
        public AccountController(AccountService accountClient, ILogger<AccountController> logger, SignInManager<User> signInManager, IConfiguration config)
        {
            _accountService = accountClient;
            _logger = logger;
            _signinmanager = signInManager;
            _config = config;
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
                var register_result = await _accountService.RegisterAsync(new RegisterDTOModel { Email = model.Email, Password = model.Password, Username = model.UserName, HostUrl = Request.Host.Value });
                if (register_result.ResultCode == ResultCodes.Failed)
                {
                    foreach (var item in register_result.Errors)
                    {
                        _logger.LogError(item);
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
                var conf_em_res = await _accountService.ConfirmEmailAsync(userId, code);
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
                var login_result = await _accountService.LoginAsync(new LoginDTOModel { Email = model.Email, Password = model.Password, RememberMe = model.RememberMe, ReturnUrl = model.ReturnUrl, urlHelper = Url });
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
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _accountService.LogoutAsync();
            return LocalRedirect(returnUrl);
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
                var fp_res = await _accountService.ForgotPasswordAsync(new ForgotPasswordDTOModel { Email = model.Email, HostUrl = Request.Host.Value });
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
        [Authorize]
        public IActionResult ChangePassword() => View("ChangePassword");

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync (ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var cp_res = await _accountService.ChangePasswordAsync(new ChangePasswordDTOModel { Email = email, NewPassword = model.NewPassword, OldPassword = model.OldPassword });
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
        public async Task<IActionResult> DeleteMyAccount(string password)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if(userEmail == _config["Admin:Email"])
                {
                    ModelState.AddModelError("", "You can't delete admin account");
                    return View("Error", new ErrorViewModel { ErrorMessages = new List<string> { "You can't delete admin account" } });
                }
                var del_ac_res = await _accountService.DeleteMyAccountAsync(userEmail, password);
                if (del_ac_res.ResultCode == ResultCodes.Successed)
                {
                    await _signinmanager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    return View("Error", new ErrorViewModel { ErrorMessages = del_ac_res.Errors });
            }
            else {
                return View("Error", new ErrorViewModel { ErrorMessages = new List<string> { "Invalid password" } });
            }
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        [HttpGet]
        [Authorize]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeEmailAsync(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                _accountService.urlHelper = Url;
                var changeemail_result = await _accountService.ChangeEmailAsync(new ChangeEmailDTOModel { OldEmail = model.OldEmail, NewEmail = model.NewEmail });
                if (changeemail_result.ResultCode == ResultCodes.Successed)
                    return View("ChangeEmailConfirmation");
                else
                {
                    
                    foreach (var item in changeemail_result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmChangeEmailAsync(string userId, string newEmail, string token)
        {
            if (ModelState.IsValid)
            {
                var conf_ch_em_res = await _accountService.ConfirmChangeEmailAsync(token, userId, newEmail);
                if (conf_ch_em_res.ResultCode == ResultCodes.Successed)
                {
                    await _accountService.LogoutAsync();
                    return RedirectToAction("Login", "Account");
                }           
                else
                    return View("Error", new ErrorViewModel { ErrorMessages = conf_ch_em_res.Errors });
            }
            else return View("Error", new ErrorViewModel { ErrorMessages = new List<string> { "Invalid token" } });
        }
    }
}
