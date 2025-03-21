namespace Inventory.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public Warehouse Warehouse { get; set; }
        public ICollection<SubGroup> SubGroups { get; set; } = new List<SubGroup>();
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
