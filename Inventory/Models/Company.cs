using Microsoft.CodeAnalysis;
using System.Data;

namespace Inventory.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public int CompanyTypeId { get; set; }  // اشاره به نوع کمپانی

        public string Address { get; set; }
        public string CompanyCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }

        public CompanyType CompanyType { get; set; } // رابطه با CompanyType
        public ICollection<Product> Products { get; set; } // محصولات این کمپانی
        public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();


    }
}
