using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
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
        var warehouses = await _context.Warehouses.ToListAsync();
        ViewBag.Warehouses = warehouses; // ذخیره لیست انبارها در ViewBag
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
        var warehouses = await _context.Warehouses.ToListAsync();
        ViewBag.Warehouses = warehouses;
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

        var warehouses = await _context.Warehouses.ToListAsync();
        ViewBag.Warehouses = warehouses;
        return View(tenant);
    }

    // POST: Tenants/Edit/5
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
                _context.Update(tenant);
                await _context.SaveChangesAsync();
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
        var warehouses = await _context.Warehouses.ToListAsync();
        ViewBag.Warehouses = warehouses;
        return View(tenant);
    }

    // GET: Tenants/Delete/5
    public async Task<IActionResult> Delete(int id)
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
    public async Task<IActionResult> DeleteConfirmed(int id)
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
}