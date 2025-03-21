namespace Inventory.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdate { get; set; }

        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
