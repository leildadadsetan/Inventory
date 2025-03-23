namespace Inventory.Dtos
{
    public class ProductEditDto
    {
        public int ProductId { get; set; } // شناسه محصول
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int TenantId { get; set; }
        public int CompanyId { get; set; }
        public int SubGroupId { get; set; }
    }
}
