using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // نمایش لیست محصولات
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.SubGroup).ToListAsync();
            return View(products);
        }

        // صفحه ایجاد محصول جدید
        public IActionResult Create()
        {
            ViewBag.SubGroups = _context.SubGroups.ToList();
            return View();
        }

        // ذخیره محصول جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SubGroups = _context.SubGroups.ToList();
            return View(product);
        }

        // صفحه ویرایش محصول
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.SubGroups = _context.SubGroups.ToList();
            return View(product);
        }

        // ثبت ویرایش محصول
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SubGroups = _context.SubGroups.ToList();
            return View(product);
        }

        // صفحه تایید حذف محصول
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // حذف محصول
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
