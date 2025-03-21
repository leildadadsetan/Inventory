namespace Inventory.Models
{
    public class SubGroup
    {
        public int SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public int GroupId { get; set; }
        public int? TenantId { get; set; }

        // ارتباط یک‌به‌چند با محصولات
        public ICollection<Product> Products { get; set; } = new List<Product>();

        // ارتباط یک زیرگروه با یک گروه
        public Group Group { get; set; }
        public Tenant Tenant { get; set; }
    }
}
