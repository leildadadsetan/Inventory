using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Inventory.Models;

public class RolesController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // نمایش لیست نقش‌ها
    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    // نمایش فرم ایجاد نقش جدید
    public IActionResult Create()
    {
        return View();
    }

    // ایجاد نقش جدید
    [HttpPost]
    public async Task<IActionResult> Create(string roleName)
    {
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            // اگر ایجاد نقش ناموفق بود، پیام خطا را اضافه کنید
            ModelState.AddModelError("", "خطا در ایجاد نقش.");
        }
        return View();
    }

    // نمایش فرم تخصیص نقش به کاربران
    public IActionResult Assign()
    {
        var users = _userManager.Users.ToList();
        var roles = _roleManager.Roles.ToList(); // لیست نقش‌ها
        ViewData["Roles"] = roles; // ذخیره‌سازی نقش‌ها در ViewData
        return View(users);
    }

    // تخصیص نقش به کاربر
    [HttpPost]
    public async Task<IActionResult> AssignRole(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null && !string.IsNullOrWhiteSpace(roleName))
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            // اگر تخصیص نقش ناموفق بود، پیام خطا را اضافه کنید
            ModelState.AddModelError("", "خطا در تخصیص نقش.");
        }
        return RedirectToAction("Assign");
    }

    // متد برای ویرایش یک نقش
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationRole role)
    {
        if (ModelState.IsValid)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            if (existingRole != null)
            {
                existingRole.Name = role.Name; // به‌روزرسانی نام نقش
                var result = await _roleManager.UpdateAsync(existingRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "خطا در به‌روزرسانی نقش.");
            }
        }
        return View(role);
    }

    // متد برای حذف یک نقش
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ApplicationRole role)
    {
        var existingRole = await _roleManager.FindByIdAsync(role.Id.ToString());
        if (existingRole != null)
        {
            var result = await _roleManager.DeleteAsync(existingRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "خطا در حذف نقش.");
        }
        return View(role);
    }
}