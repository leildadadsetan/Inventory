using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var warehouses = await _context.Warehouses.Include(w => w.Tenant).ToListAsync();
            return View(warehouses);
        }
        // GET: Warehouse/Create
        public IActionResult Create()
        {
            // دریافت تمام tenants برای پر کردن dropdown
            ViewData["Tenants"] = _context.Tenants.ToList();
            return View();
        }

        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // بعد از ثبت موفق به صفحه Index هدایت شود
            }

            // در صورت بروز خطا، لیست tenants دوباره ارسال شود
            ViewData["Tenants"] = _context.Tenants.ToList();
            return View(warehouse);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return NotFound();

            ViewData["Tenants"] = _context.Tenants.ToList();
            return View(warehouse);
        }

        // POST: Warehouse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.WarehouseId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Tenants"] = _context.Tenants.ToList();
            return View(warehouse);
        }

        // GET: Warehouse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var warehouse = await _context.Warehouses
                .Include(w => w.Tenant)
                .FirstOrDefaultAsync(m => m.WarehouseId == id);

            if (warehouse == null) return NotFound();

            return View(warehouse);
        }

        // POST: Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseId == id);
        }
    }
}
