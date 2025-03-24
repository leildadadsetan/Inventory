using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models; // برای مدل‌ها
using Inventory.Dtos; // برای DTOها
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies
                .Include(c => c.CompanyType) // بارگذاری نوع کمپانی
                .ToListAsync();

            var companyDtos = companies.Select(c => new CompanyDto
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                CompanyTypeId = c.CompanyTypeId,
                CompanyTypeName=c.CompanyType.Name,
                Address = c.Address,
                CompanyCode = c.CompanyCode,
                Country = c.Country,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                TaxCode = c.TaxCode
            }).ToList();

            return View(companyDtos);
        }

        // GET: Company/Create
        public async Task<IActionResult> Create()
        {
            var companyTypes = await _context.CompanyTypes.ToListAsync();

            var companyTypeDtos = companyTypes.Select(ct => new CompanyTypeDto
            {
                CompanyTypeId = ct.CompanyTypeId,
                Name = ct.Name
            }).ToList();

            ViewBag.CompanyTypes = new SelectList(companyTypeDtos, "CompanyTypeId", "Name"); // ایجاد SelectList برای دراپ‌داون
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                // بررسی وجود رکورد تکراری بر اساس نام شرکت یا کد شرکت
                bool companyExists = await _context.Companies
                    .AnyAsync(c => c.Name == dto.Name || c.CompanyCode == dto.CompanyCode);

                if (companyExists)
                {
                    string errorMessage = "شرکت با این نام یا کد قبلاً ثبت شده است.";
                    ModelState.AddModelError(string.Empty, errorMessage);

                    // پرتاب یک استثنا
                    throw new InvalidOperationException(errorMessage);
                }
                else
                {
                    var company = new Company
                    {
                        Name = dto.Name,
                        CompanyTypeId = dto.CompanyTypeId,
                        Address = dto.Address,
                        CompanyCode = dto.CompanyCode,
                        Country = dto.Country,
                        Email = dto.Email,
                        PhoneNumber = dto.PhoneNumber,
                        TaxCode = dto.TaxCode
                    };

                    _context.Companies.Add(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // بارگذاری دوباره نوع کمپانی‌ها در صورت نامعتبر بودن مدل
            var companyTypes = await _context.CompanyTypes.ToListAsync();
            ViewBag.CompanyTypes = new SelectList(companyTypes, "CompanyTypeId", "Name", dto.CompanyTypeId);
            return View(dto);
        }
        // GET: Company/Edit/5
         public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            var dto = new CompanyEditDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                CompanyTypeId = company.CompanyTypeId,
                Address = company.Address,
                CompanyCode = company.CompanyCode,
                Country = company.Country,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                TaxCode = company.TaxCode
            };

            // بارگذاری نوع کمپانی‌ها برای دراپ‌داون
            var companyTypes = await _context.CompanyTypes.ToListAsync();
            ViewBag.CompanyTypes = new SelectList(companyTypes, "CompanyTypeId", "Name", dto.CompanyTypeId);
            return View(dto);
        }
        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyEditDto dto)
        {
            if (id != dto.CompanyId) return NotFound();

            if (ModelState.IsValid)
            {
                // بررسی وجود رکورد تکراری بر اساس نام شرکت یا کد شرکت
                bool companyExists = await _context.Companies
                    .AnyAsync(c => (c.Name == dto.Name || c.CompanyCode == dto.CompanyCode) && c.CompanyId != id);

                if (companyExists)
                {
                    string errorMessage = "شرکت با این نام یا کد قبلاً ثبت شده است.";
                    ModelState.AddModelError(string.Empty, errorMessage);

                    // پرتاب یک استثنا
                    throw new InvalidOperationException(errorMessage);
                }
                else
                {
                    var company = await _context.Companies.FindAsync(id);
                    if (company == null) return NotFound();

                    // به روزرسانی اطلاعات شرکت
                    company.Name = dto.Name;
                    company.CompanyTypeId = dto.CompanyTypeId;
                    company.Address = dto.Address;
                    company.CompanyCode = dto.CompanyCode;
                    company.Country = dto.Country;
                    company.Email = dto.Email;
                    company.PhoneNumber = dto.PhoneNumber;
                    company.TaxCode = dto.TaxCode;

                    _context.Update(company);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // بارگذاری دوباره نوع کمپانی‌ها در صورت نامعتبر بودن مدل
            var companyTypes = await _context.CompanyTypes.ToListAsync();
            ViewBag.CompanyTypes = new SelectList(companyTypes, "CompanyTypeId", "Name", dto.CompanyTypeId);
            return View(dto);
        }
        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var company = await _context.Companies
                .Include(c => c.CompanyType) // بارگذاری نوع کمپانی
                .FirstOrDefaultAsync(m => m.CompanyId == id);

            if (company == null) return NotFound();

            var dto = new CompanyDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                CompanyTypeId = company.CompanyTypeId,
                Address = company.Address,
                CompanyCode = company.CompanyCode,
                Country = company.Country,
                Email = company.Email,
                PhoneNumber = company.PhoneNumber,
                TaxCode = company.TaxCode
            };

            return View(dto);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}