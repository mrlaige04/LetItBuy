using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shop.Models;
using Shop.Models.Identity;
using Shop.Services;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ICustomEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
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
                var cartId = Guid.NewGuid();
                var userId = Guid.NewGuid().ToString();
                User user = new User {
                    Email = model.Email, 
                    UserName = model.Email,
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
                if (!await _roleManager.RoleExistsAsync("simpleUser"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("simpleUser"));
                }
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, "simpleUser");
                    if (!addRoleResult.Succeeded)
                    {
                        foreach (var item in addRoleResult.Errors)
                        {
                            _logger.LogError(item.Description);
                        }
                    }
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);   
                    
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        $"Confirm Registration, follow the: <h2><a href='{callbackUrl}'>link</a></h2>");
                    return View("ConfirmEmail");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error");
        }



        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
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
        //[ValidateAntiForgeryToken]
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
