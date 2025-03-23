using Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
 

public class InvoiceController : Controller
{
    private readonly ApplicationDbContext _context;

    public InvoiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Invoice
    public async Task<IActionResult> Index()
    {
        var invoices = await _context.Invoices.ToListAsync();
        return View(invoices);
    }
}