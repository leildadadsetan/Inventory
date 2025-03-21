using Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var subGroups = _context.SubGroups.Include(sg => sg.Tenant).Include(sg => sg.Group);
            return View(await subGroups.ToListAsync());
        }

        // GET: SubGroup/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subGroup = await _context.SubGroups
                .Include(sg => sg.Tenant)
                .Include(sg => sg.Group)
                .FirstOrDefaultAsync(m => m.SubGroupId == id);
            if (subGroup == null)
            {
                return NotFound();
            }

            return View(subGroup);
        }

        // GET: SubGroup/Create
        public IActionResult Create()
        {
            // پر کردن داده‌های Dropdown برای انتخاب Tenant و Group
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        // POST: SubGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,TenantId,GroupId")] SubGroup subGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // در صورت وجود خطا، داده‌ها را دوباره در فرم قرار می‌دهیم
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", subGroup.TenantId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", subGroup.GroupId);
            return View(subGroup);
        }

        // GET: SubGroup/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subGroup = await _context.SubGroups.FindAsync(id);
            if (subGroup == null)
            {
                return NotFound();
            }
            // پر کردن داده‌های Dropdown برای ویرایش Tenant و Group
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", subGroup.TenantId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", subGroup.GroupId);
            return View(subGroup);
        }

        // POST: SubGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,TenantId,GroupId")] SubGroup subGroup)
        {
            if (id != subGroup.SubGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubGroupExists(subGroup.SubGroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // در صورت وجود خطا، داده‌ها را دوباره در فرم قرار می‌دهیم
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", subGroup.TenantId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", subGroup.GroupId);
            return View(subGroup);
        }

        // GET: SubGroup/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subGroup = await _context.SubGroups
                .Include(sg => sg.Tenant)
                .Include(sg => sg.Group)
                .FirstOrDefaultAsync(m => m.SubGroupId == id);
            if (subGroup == null)
            {
                return NotFound();
            }

            return View(subGroup);
        }

        // POST: SubGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subGroup = await _context.SubGroups.FindAsync(id);
            _context.SubGroups.Remove(subGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubGroupExists(int id)
        {
            return _context.SubGroups.Any(e => e.SubGroupId == id);
        }
    }
}