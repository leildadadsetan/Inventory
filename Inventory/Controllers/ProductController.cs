using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Company)
                .Include(p => p.Company.CompanyType)
                .Include(p => p.SubGroup)
                .Include(p => p.Group)
                .ToListAsync();
            var productDtos = products.Select(x => new ProductDto
            {
                CompanyId = x.CompanyId,
                GroupCode = x.Group.GroupCode,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductId = x.ProductId,
                SubGroupCode = x.SubGroup.SubGroupCode,
                SubGroupId = x.SubGroupId,
                GroupName= x.Group.GroupName,
                SubGroupName=x.SubGroup.SubGroupName,
                TenantId = x.TenantId
            });
            return View(productDtos);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var companyTypes = await _context.Companies.ToListAsync();
            var subGroups = await _context.SubGroups.ToListAsync();
            var groups = await _context.Groups.ToListAsync();
            var tenants = await _context.Tenants.ToListAsync();

            var tenantDtos = tenants.Select(t => new TenantDto
            {
                Name = t.Name,
                TenantId = t.TenantId
            }).ToList();

            var groupDtos = groups.Select(c => new GroupList
            {
                GroupId = c.GroupId,
                GroupName = c.GroupName,
            }).ToList();

            var subGroupDtos = subGroups.Select(c => new SubGroupList
            {
                SubGroupId = c.SubGroupId,
                SubGroupName = c.SubGroupName,
            }).ToList();

            var companyDtos = companyTypes.Select(ct => new CompanyList
            {
                CompanyId = ct.CompanyId,
                Name = ct.Name
            }).ToList();

            ViewBag.Companies = new SelectList(companyDtos, "CompanyId", "Name");
            ViewBag.Tenants = new SelectList(tenantDtos, "TenantId", "Name");
            ViewBag.SubGroups = new SelectList(subGroupDtos, "SubGroupId", "SubGroupName");
            ViewBag.Groups = new SelectList(groupDtos, "GroupId", "GroupName");

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                // بررسی وجود کد محصول
                var existingProduct = await _context.Products
                    .AnyAsync(p => p.ProductCode == dto.ProductCode);

                if (existingProduct)
                {
                    string errorMessage = "کد محصول قبلا استفاده شده است";
                    ModelState.AddModelError(dto.ProductCode, errorMessage);
                    return View(dto);
                }
                else
                {
                    var product = new Product
                    {
                        ProductName = dto.ProductName,
                        ProductCode = dto.ProductCode,
                        TenantId = dto.TenantId,
                        CompanyId = dto.CompanyId,
                        SubGroupId = dto.SubGroupId,
                        GroupId = dto.GroupId,
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // بارگذاری دوباره داده‌ها در صورت نامعتبر بودن مدل
            ViewBag.Tenants = new SelectList(await _context.Tenants.ToListAsync(), "TenantId", "Name", dto.TenantId);
            ViewBag.Companies = new SelectList(await _context.Companies.ToListAsync(), "CompanyId", "Name", dto.CompanyId);
            ViewBag.SubGroups = new SelectList(await _context.SubGroups.ToListAsync(), "SubGroupId", "SubGroupName", dto.SubGroupId);
            ViewBag.Groups = new SelectList(await _context.Groups.ToListAsync(), "GroupId", "GroupName", dto.GroupId);
            return View(dto);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var companyTypes = await _context.Companies.ToListAsync();
            var subGroups = await _context.SubGroups.ToListAsync();
            var tenants = await _context.Tenants.ToListAsync();

            var tenantDtos = tenants.Select(t => new TenantDto
            {
                Name = t.Name,
                TenantId = t.TenantId
            }).ToList();

            var subGroupDtos = subGroups.Select(c => new SubGroupList
            {
                SubGroupId = c.SubGroupId,
                SubGroupName = c.SubGroupName,
            }).ToList();

            var companyDtos = companyTypes.Select(ct => new CompanyList
            {
                CompanyId = ct.CompanyId,
                Name = ct.Name
            }).ToList();

            ViewBag.Companies = new SelectList(companyDtos, "CompanyId", "Name");
            ViewBag.Tenants = new SelectList(tenantDtos, "TenantId", "Name");
            ViewBag.SubGroups = new SelectList(subGroupDtos, "SubGroupId", "SubGroupName");

            var dto = new ProductEditDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                TenantId = product.TenantId,
                CompanyId = product.CompanyId,
                SubGroupId = product.SubGroupId,
                GroupId = (int)product.GroupId 
            };

            return View(dto);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditDto dto)
        {
            if (id != dto.ProductId) return NotFound();

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products
                    .Where(p => p.ProductId != id) 
                    .AnyAsync(p => p.ProductCode == dto.ProductCode);

                if (existingProduct)
                {
                    ModelState.AddModelError("ProductCode", "کد محصول قبلا استفاده شده است.");
                }
                else
                {
                    try
                    {
                        var product = await _context.Products.FindAsync(id);
                        if (product == null) return NotFound();

                        // به‌روزرسانی اطلاعات محصول
                        product.ProductName = dto.ProductName;
                        product.ProductCode = dto.ProductCode;
                        product.TenantId = dto.TenantId;
                        product.CompanyId = dto.CompanyId;
                        product.SubGroupId = dto.SubGroupId;
                        product.GroupId = dto.GroupId;  

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(dto.ProductId))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            var companies = await _context.Companies.ToListAsync();
            var subGroups = await _context.SubGroups.ToListAsync();
            var tenants = await _context.Tenants.ToListAsync();
            var tenantDtos = tenants.Select(t => new TenantDto
            {
                Name = t.Name,
                TenantId = t.TenantId
            }).ToList();

            var subGroupDtos = subGroups.Select(c => new SubGroupList
            {
                SubGroupId = c.SubGroupId,
                SubGroupName = c.SubGroupName,
            }).ToList();

            var companyDtos = companies.Select(ct => new CompanyList
            {
                CompanyId = ct.CompanyId,
                Name = ct.Name
            }).ToList();

            ViewBag.Companies = new SelectList(companyDtos, "CompanyId", "Name");
            ViewBag.Tenants = new SelectList(tenantDtos, "TenantId", "Name");
            ViewBag.SubGroups = new SelectList(subGroupDtos, "SubGroupId", "SubGroupName");
            return View(dto);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Company)
                .Include(p => p.SubGroup)
                .Include(p => p.Group)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

         public async Task<IActionResult> GetSubGroups(int groupId)
        {
            var subGroups = await _context.SubGroups
                .Where(sg => sg.GroupId == groupId)  
                .ToListAsync();

            return Json(subGroups.Select(sg => new
            {
                sg.SubGroupId,
                sg.SubGroupName
            }));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}