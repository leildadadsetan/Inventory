using Inventory.Models;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "این ایمیل قبلاً ثبت‌نام شده است. لطفاً از ایمیل دیگری استفاده کنید.");
                    return View(model);

                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName, 
                    LastName = model.LastName,   
                    PhoneNumber = model.PhoneNumber 
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // می‌توانید به صفحه ورود هدایت کنید یا عملیات دیگری انجام دهید
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    string errorMessage = "رمز عبور باید حداقل 8 کاراکتر باشد و شامل حداقل یک حرف بزرگ، یک حرف کوچک، یک عدد و یک نماد غیر الفبایی باشد.";
                    ModelState.AddModelError(string.Empty, error.Description);
                    throw new InvalidOperationException(errorMessage);
                }
            }
            return View(model);
        }
    }
}

