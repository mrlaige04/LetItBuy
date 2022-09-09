using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Models.Identity;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                var addRoleResult = await _userManager.AddToRoleAsync(user, "simpleUser");
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {            
                    return RedirectToAction("Login", "Account");
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

    }
}
