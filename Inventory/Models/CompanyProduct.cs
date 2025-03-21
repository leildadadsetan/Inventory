using Microsoft.CodeAnalysis;

namespace Inventory.Models
{
    public class CompanyProduct
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }

        public Company Company { get; set; }
        public Product Product { get; set; }
    }
}
