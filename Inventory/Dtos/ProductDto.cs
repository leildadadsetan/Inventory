namespace Inventory.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }  
        public string ProductName { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string ProductCode { get; set; }
        public string GroupCode { get; set; }
        public string SubGroupCode { get; set; }
        public int TenantId { get; set; }
        public int CompanyId { get; set; }
        public int SubGroupId { get; set; }
    }
}
