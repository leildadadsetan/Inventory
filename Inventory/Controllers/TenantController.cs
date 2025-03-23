using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Collections.Generic; // اطمینان از اضافه بودن این namespace
using System.Linq;
using System.Threading.Tasks;
using Inventory;

[Authorize] // اطمینان از ورود کاربر
public class TenantController : Controller
{
    private readonly ApplicationDbContext _context;

    public TenantController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Tenants
    public async Task<IActionResult> Index()
    {
        var tenants = await _context.Tenants.Include(t => t.Warehouse).ToListAsync();
        return View(tenants);
    }

    // GET: Tenants/Create
    public async Task<IActionResult> Create()
    {
        var availableWarehouses = await GetAvailableWarehousesAsync();
        ViewBag.Warehouses = availableWarehouses; // ذخیره لیست انبارها در ViewBag
        return View();
    }

    // POST: Tenants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tenant tenant)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tenant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // اگر مدل نامعتبر است، لیست انبارها را دوباره بارگذاری کنید
        var availableWarehouses = await GetAvailableWarehousesAsync();
        ViewBag.Warehouses = availableWarehouses;
        return View(tenant);
    }

    // GET: Tenants/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }

        // بارگذاری انبارهای مجاز برای ویرایش
        var availableWarehouses = await GetAvailableWarehousesForEditAsync(tenant.WarehouseId);
        ViewBag.Warehouses = availableWarehouses;
        return View(tenant);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tenant tenant)
    {
        if (id != tenant.TenantId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // در اینجا بررسی کنید که آیا WarehouseId موجود است
                if (tenant.WarehouseId.HasValue)
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("WarehouseId", "لطفاً یک انبار معتبر انتخاب کنید.");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(tenant.TenantId))
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

        // اگر مدل نامعتبر است، لیست انبارها را دوباره بارگذاری کنید
        var availableWarehouses = await GetAvailableWarehousesForEditAsync(tenant.WarehouseId ?? 0); // برای استفاده از مقدار پیش‌فرض
        ViewBag.Warehouses = availableWarehouses;
        return View(tenant);
    }

    // GET: Tenants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        var tenant = await _context.Tenants
            .Include(t => t.Warehouse)
            .FirstOrDefaultAsync(m => m.TenantId == id);
        if (tenant == null)
        {
            return NotFound();
        }

        return View(tenant);
    }

    // POST: Tenants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TenantExists(int id)
    {
        return _context.Tenants.Any(e => e.TenantId == id);
    }

    private async Task<List<Warehouse>> GetAvailableWarehousesAsync()
    {
        // انبارهایی را که به هیچ Tenant ای متصل نیستند، برمی‌گرداند
        return await _context.Warehouses
            .Where(w => !_context.Tenants.Any(t => t.WarehouseId == w.WarehouseId))
            .ToListAsync();
    }

    private async Task<List<Warehouse>> GetAvailableWarehousesForEditAsync(int? currentWarehouseId)
    {
        // انبارهایی را که به هیچ Tenant ای متصل نیستند یا انبار فعلی را برمی‌گرداند
        return await _context.Warehouses
            .Where(w => w.WarehouseId == currentWarehouseId ||
                         !_context.Tenants.Any(t => t.WarehouseId == w.WarehouseId))
            .ToListAsync();
    }
}