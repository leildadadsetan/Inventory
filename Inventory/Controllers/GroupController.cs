using Inventory.Dtos;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Inventory.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Group
        public async Task<IActionResult> Index()
        {
            var groups = await _context.Groups.Include(g => g.Tenant).ToListAsync();
            return View(groups);
        }

        // GET: Group/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.Include(g => g.Tenant).FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Group/Create
        public async Task<IActionResult> Create()
        {
            var tenants = await _context.Tenants
                .Select(t => new TenantDto
                {
                    TenantId = t.TenantId,
                    Name = t.Name
                })
                .ToListAsync();

            ViewData["Tenants"] = tenants;
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupName,GroupCode,TenantId")] GroupCreateDto groupDto)
        {
            // اعتبارسنجی کد گروه
            if (!IsGroupCodeValid(groupDto.GroupCode))
            {
                ModelState.AddModelError("GroupCode", "کد گروه باید حتماً دو رقم باشد.");
            }

            if (ModelState.IsValid)
            {
                // بررسی وجود کد گروه تکراری
                if (await IsGroupCodeDuplicate(groupDto.GroupCode))
                {
                    string errorMessage = "این کد گروه قبلاً ثبت شده است.";
                    ModelState.AddModelError(string.Empty, errorMessage);
                    throw new InvalidOperationException(errorMessage);
                 }
      
                if (ModelState.IsValid) // دوباره بررسی اعتبارسنجی
                {
                    var group = new Group
                    {
                        GroupName = groupDto.GroupName,
                        GroupCode = groupDto.GroupCode,
                        TenantId = groupDto.TenantId
                    };

                    _context.Add(group);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Tenants"] = await _context.Tenants.ToListAsync();
            return View(groupDto);
        }

        // GET: Group/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var tenants = await _context.Tenants
                .Select(t => new TenantDto
                {
                    TenantId = t.TenantId,
                    Name = t.Name
                })
                .ToListAsync();

            ViewData["Tenants"] = tenants;
            var groupDto = new GroupEditDto
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupCode = group.GroupCode,
                TenantId = group.TenantId
            };

            return View(groupDto);
        }

        // POST: Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupCode,TenantId")] GroupEditDto groupDto)
        {
            if (id != groupDto.GroupId)
            {
                return NotFound();
            }
            // اعتبارسنجی کد گروه
            if (!IsGroupCodeValid(groupDto.GroupCode))
            {
                ModelState.AddModelError("GroupCode", "کد گروه باید حتماً دو رقم باشد.");
            }
            if (ModelState.IsValid)
            {
                // بررسی وجود کد گروه تکراری
                if (await IsGroupCodeDuplicate(groupDto.GroupCode, id))
                {
                    ModelState.AddModelError("GroupCode", "این کد گروه قبلاً ثبت شده است.");
                }

                if (ModelState.IsValid) // دوباره بررسی اعتبارسنجی
                {
                    try
                    {
                        var group = await _context.Groups.FindAsync(id);
                        if (group == null)
                        {
                            return NotFound();
                        }

                        group.GroupName = groupDto.GroupName;
                        group.GroupCode = groupDto.GroupCode;
                        group.TenantId = groupDto.TenantId;

                        _context.Update(group);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GroupExists(groupDto.GroupId))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                }
            }

            // بارگذاری مجدد Tenant ها برای ویو در صورت بروز خطا
            ViewData["Tenants"] = await _context.Tenants.ToListAsync();
            return View(groupDto); // بازگشت به ویو با اطلاعات ورودی
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var group = await _context.Groups
                .Include(g => g.Tenant) // اگر نیاز به بارگذاری اطلاعات Tenant باشد
                .FirstOrDefaultAsync(m => m.GroupId == id);

            if (group == null) return NotFound();

            var groupDto = new GroupDeleteDto
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupCode = group.GroupCode,
                TenantName = group.Tenant?.Name // با استفاده از ? برای جلوگیری از NullReferenceException
            };

            return View(groupDto);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }

        private bool IsGroupCodeValid(string groupCode)
        {
            return !string.IsNullOrEmpty(groupCode) && groupCode.Length == 2 && int.TryParse(groupCode, out _);
        }

        private async Task<bool> IsGroupCodeDuplicate(string groupCode, int? currentGroupId = null)
        {
            return await _context.Groups.AnyAsync(g => g.GroupCode == groupCode && (!currentGroupId.HasValue || g.GroupId != currentGroupId));
        }
    }
}