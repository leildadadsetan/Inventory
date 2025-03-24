using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    [Authorize(Roles = "admin")] // فقط ادمین‌ها می‌توانند به این کنترلر دسترسی داشته باشند
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> ManageUserRoles()
        {
            var users = _userManager.Users.ToList();  
            ViewBag.RoleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();  
            ViewData["UserManager"] = _userManager;  
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !string.IsNullOrWhiteSpace(roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "نقش با موفقیت به کاربر تخصیص داده شد.";
                    return RedirectToAction("ManageUserRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData["ErrorMessage"] = "تخصیص نقش ناموفق بود.";
            return RedirectToAction("ManageUserRoles");
        }

        // اکشن برای حذف نقش از کاربر
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !string.IsNullOrWhiteSpace(roleName))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "نقش با موفقیت از کاربر حذف شد.";
                    return RedirectToAction("ManageUserRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData["ErrorMessage"] = "حذف نقش ناموفق بود.";
            return RedirectToAction("ManageUserRoles");
        }

    }
}