namespace Inventory.Models
{
    public class Warehouse
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int? TenantId { get; set; }  // This is nullable since not all warehouses need a Tenant
        public Tenant? Tenant { get; set; }

    }
}
