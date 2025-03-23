using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models; // برای مدل‌ها
using Inventory.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering; // برای SelectList
using System.Threading.Tasks;
using System.Linq;

namespace Inventory.Controllers
{
    public class SubGroupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubGroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubGroup
        public async Task<IActionResult> Index()
        {
            var subGroups = await _context.SubGroups
                .Include(s => s.Group) // بارگذاری اطلاعات گروه
                .Include(s => s.Tenant) // بارگذاری اطلاعات Tenant
                .ToListAsync();

            var subGroupDtos = subGroups.Select(s => new SubGroupDto
            {
                SubGroupId = s.SubGroupId,
                SubGroupName = s.SubGroupName,
                SubGroupCode = s.SubGroupCode,
                GroupId = s.GroupId,
                GroupName = s.Group.GroupName,
                TenantId = s.TenantId,
                TenantName = s.Tenant.Name
            }).ToList();

            return View(subGroupDtos);
        }

        // GET: SubGroup/Create
        public async Task<IActionResult> Create()
        {
            var groups = await _context.Groups.ToListAsync(); // بارگذاری گروه‌ها
            ViewBag.Groups = new SelectList(groups, "GroupId", "GroupName"); // ایجاد SelectList برای dropdown
            return View();
        }

        // POST: SubGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubGroupCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var group = await _context.Groups.FindAsync(dto.GroupId);
                int? tenantId = group?.TenantId;

                var subGroup = new SubGroup
                {
                    SubGroupName = dto.SubGroupName,
                    SubGroupCode = dto.SubGroupCode,
                    GroupId = dto.GroupId,
                    TenantId = tenantId
                };

                _context.SubGroups.Add(subGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var groups = await _context.Groups.ToListAsync();
            ViewBag.Groups = new SelectList(groups, "GroupId", "GroupName");
            return View(dto);
        }

        // GET: SubGroup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subGroup = await _context.SubGroups.FindAsync(id);
            if (subGroup == null) return NotFound();

            var dto = new SubGroupEditDto
            {
                SubGroupId = subGroup.SubGroupId,
                SubGroupName = subGroup.SubGroupName,
                SubGroupCode = subGroup.SubGroupCode,
                GroupId = subGroup.GroupId,
                TenantId = subGroup.TenantId
            };

            // بارگذاری گروه‌ها برای دراپ‌داون
            var groups = await _context.Groups.ToListAsync();
            ViewBag.Groups = new SelectList(groups, "GroupId", "GroupName", dto.GroupId); // انتخاب گروه فعلی

            return View(dto);
        }

        // POST: SubGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubGroupEditDto dto)
        {
            if (id != dto.SubGroupId) return NotFound();

            if (ModelState.IsValid)
            {
                var subGroup = await _context.SubGroups.FindAsync(id);
                if (subGroup == null) return NotFound();

                // فقط اجازه ویرایش نام و کد زیرگروه
                subGroup.SubGroupName = dto.SubGroupName;
                subGroup.SubGroupCode = dto.SubGroupCode;

                // اگر نیاز به تنظیم TenantId بر اساس GroupId باشد
                var group = await _context.Groups.FindAsync(dto.GroupId);
                subGroup.TenantId = group?.TenantId;

                _context.Update(subGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // بارگذاری گروه‌ها برای دراپ‌داون در صورت نامعتبر بودن مدل
            var groups = await _context.Groups.ToListAsync();
            ViewBag.Groups = new SelectList(groups, "GroupId", "GroupName", dto.GroupId);
            return View(dto);
        }

        // GET: SubGroup/Delete/5
        // GET: SubGroup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subGroup = await _context.SubGroups
                .Include(s => s.Group) // بارگذاری اطلاعات گروه
                .Include(s => s.Tenant) // بارگذاری اطلاعات Tenant
                .FirstOrDefaultAsync(m => m.SubGroupId == id);

            if (subGroup == null) return NotFound();

            var dto = new SubGroupDto
            {
                SubGroupId = subGroup.SubGroupId,
                SubGroupName = subGroup.SubGroupName,
                GroupId = subGroup.GroupId,
                TenantId = subGroup.TenantId,
                GroupName = subGroup.Group.GroupName // اضافه کردن نام گروه
            };

            return View(dto); // اطمینان از اینکه dto به عنوان مدل ارسال می‌شود
        }

        // POST: SubGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var subGroup = await _context.SubGroups.FindAsync(id);
            if (subGroup != null)
            {
                _context.SubGroups.Remove(subGroup);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // هدایت به صفحه لیست زیرگروه‌ها
        }
    }
}