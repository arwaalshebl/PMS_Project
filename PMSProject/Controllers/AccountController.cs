
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PMSProject.Models;

namespace PMSProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- صفحة تسجيل الدخول (GET) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName,  model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {

                    TempData["ShowNotificationRequest"] = true;

                    return RedirectToAction("IndexP", "Projects");
                }

                ModelState.AddModelError(string.Empty, "محاولة دخول غير صحيحة.");



            }
            return View(model);
        }


        // --- صفحة تسجيل جديد ) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Developer");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("IndexP", "Projects");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // --- عملية تسجيل الخروج ---
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("IndexP", "Projects");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
