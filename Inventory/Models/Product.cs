namespace Inventory.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int TenantId { get; set; }
        //public string UserId { get; set; }
        public int CompanyId { get; set; }
        public int SubGroupId { get; set; }
        public int? GroupId { get; set; }

        public Company Company { get; set; }
        public Group Group { get; set; }
        public SubGroup SubGroup { get; set; }
        public ICollection<CompanyProduct> CompanyProducts { get; set; } = new List<CompanyProduct>();

    }
}
