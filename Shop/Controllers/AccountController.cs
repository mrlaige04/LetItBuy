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
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                    return View(model);
                }
                return View("ConfirmEmail");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            code = Regex.Replace(code, " ", "+");
            if (userId == null || code == null)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "UserID or Code is Null"});
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "User not found" });
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error", new ErrorViewModel { ErrorMessage = "Invalid Token" });
        }



        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return View("Error", new ErrorViewModel { ErrorMessage = "You are already logged in" });
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                

                
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("GetWelcomePage", "Home");
                }
                else
                    ModelState.AddModelError("", "Wrong username and/or password");
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("Error", new ErrorViewModel()
                    {
                        ErrorMessage = "User with this email doesn't exist"
                    });
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    $"To reset your password follow this link: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteMyAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user==null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                return View("GetWelcomePage", "Home");
            }
            return View("Error", new ErrorViewModel()
            {
                ErrorMessage = "Your account was not deleted... Try again later."
            });
        }


        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
