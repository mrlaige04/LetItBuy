using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Shop.DAL.Data.Entities;
using Shop.UI.Models;
using Shop.UI.Models.Identity;
using Shop.BLL.Services;
using Shop.BLL.DTO;
using Shop.BLL.Models;
using Microsoft.Extensions.Localization;
using Shop.UI;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using Shop.UI.Models.ViewDTO;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {       
        private readonly ILogger<AccountController> _logger;
        
        private readonly SignInManager<ApplicationUser> _signinmanager;
        private readonly AccountService _accountService;
        private readonly IConfiguration _config;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AccountController(AccountService accountClient, ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, IConfiguration config, IStringLocalizer<SharedResource> localizer)
        {
            _accountService = accountClient;
            _logger = logger;
            _signinmanager = signInManager;
            _config = config;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            var externalProviders = await _signinmanager.GetExternalAuthenticationSchemesAsync();
            return View(new RegisterViewModel { ReturnUrl = returnUrl, ExternalProviders = externalProviders });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {           
            if (ModelState.IsValid)
            {              
                var register_result = await _accountService.RegisterAsync(new RegisterDTOModel { Email = model.Email, Password = model.Password, Username = model.UserName, HostUrl = Request.Host.Value });
                if (register_result.ResultCode == ResultCodes.Fail)
                {
                    foreach (var item in register_result.Errors)
                    {
                        _logger.LogError(item);
                        ModelState.AddModelError("", item);
                    }                    
                } else return Redirect(model.ReturnUrl ?? "/");
            }
            if (model.ExternalProviders == null) model.ExternalProviders = await _signinmanager.GetExternalAuthenticationSchemesAsync();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if (ModelState.IsValid)
            {                
                var conf_em_res = await _accountService.ConfirmEmailAsync(userId, code);
                if (conf_em_res.ResultCode == ResultCodes.Success)
                    return RedirectToAction("Login", "Account");
                else
                    return View("Error", new ErrorViewModel { Errors = conf_em_res.Errors });
            }
            else return View("Error", new ErrorViewModel { Errors = new List<string> { "Invalid model state" } });
        }
 
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return View("Error", new ErrorViewModel { Errors = new List<string> { "You are already logged in" } });
            var externalProviders = await _signinmanager.GetExternalAuthenticationSchemesAsync();
            return View(new LoginViewModel { ReturnUrl = returnUrl, ExternalProviders = externalProviders });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login_result = await _accountService.LoginAsync(new LoginDTOModel { Email = model.Email, Password = model.Password, RememberMe = model.RememberMe, ReturnUrl = model.ReturnUrl, urlHelper = Url });
                if (login_result.ResultCode == ResultCodes.Success)
                    return Redirect(model.ReturnUrl ?? "../");
                else
                {
                    foreach (var item in login_result.Errors)
                    {
                        ModelState.AddModelError("", _localizer[item]);
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
            return LocalRedirect(returnUrl ?? "/");
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
                if (fp_res.ResultCode == ResultCodes.Success)
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
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var cp_res = await _accountService.ChangePasswordAsync(new ChangePasswordDTOModel { Email = email, NewPassword = model.NewPassword, OldPassword = model.OldPassword });
                if (cp_res.ResultCode == ResultCodes.Success)
                    return Ok();
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
                    return View("Error", new ErrorViewModel { Errors = new List<string> { "You can't delete admin account" } });
                }
                var del_ac_res = await _accountService.DeleteMyAccountAsync(userEmail, password);
                if (del_ac_res.ResultCode == ResultCodes.Success)
                {
                    await _signinmanager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    return View("Error", new ErrorViewModel { Errors = del_ac_res.Errors });
            }
            else {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Invalid password" } });
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
                if (changeemail_result.ResultCode == ResultCodes.Success)
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
                if (conf_ch_em_res.ResultCode == ResultCodes.Success)
                {
                    await _accountService.LogoutAsync();
                    return RedirectToAction("Login", "Account");
                }           
                else
                    return View("Error", new ErrorViewModel { Errors = conf_ch_em_res.Errors });
            }
            else return View("Error", new ErrorViewModel { Errors = new List<string> { "Invalid token" } });
        }


        





        // TODO :FACEBOOK LOGIN PRIVACY AND TERMS LINK ERRORS
        
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var returnUri = returnUrl ?? Url.Content("~/");
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUri });
            var properties = _signinmanager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            return Challenge(properties, provider);
        }


        public async Task<IActionResult> ExternalLoginCallback(string returnUri)
        {
            var info = await _signinmanager.GetExternalLoginInfoAsync();

            if (info == null)
                return RedirectToAction("Login");

            

            string? userName = info.Principal.FindFirst(ClaimTypes.Name)?.Value.Split(' ')[0];
            string? userSurname = info.Principal.FindFirst(ClaimTypes.Surname)?.Value;
            var userEmail = info.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var userPhoneNumber = info.Principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

            return RedirectToAction("ExternalRegister", new ExternalRegisterViewModel
            {
                UserName = userName + userSurname,
                Email = userEmail,
                PhoneNumber = userPhoneNumber,
                ReturnUrl = returnUri
            });
        }

        public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
        {
            var externalLogin_Result = await _accountService.ExternalLoginAsync(new ExternalLoginDTO()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                PhoneNumber = vm.PhoneNumber
            });
            
            if (externalLogin_Result.ResultCode == ResultCodes.Success)
            {
                if (string.IsNullOrEmpty(vm.ReturnUrl))
                {
                    return Redirect("/");
                }
                else
                {
                    return Redirect(vm.ReturnUrl);
                }
            }

            else
            {
                return View("Error", new ErrorViewModel { Errors = externalLogin_Result.Errors });
            }
        }




    }
    
}
