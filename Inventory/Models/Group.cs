using Microsoft.CodeAnalysis;

namespace Inventory.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public int TenantId { get; set; }

        // یک گروه دارای چندین زیرگروه است
        public ICollection<SubGroup> SubGroups { get; set; } = new List<SubGroup>();

        // یک گروه دارای چندین محصول است
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Tenant Tenant { get; set; }
    }
}
