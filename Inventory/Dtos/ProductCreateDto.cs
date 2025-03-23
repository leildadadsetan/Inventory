namespace Inventory.Dtos
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public int TenantId { get; set; }
        //public string UserId { get; set; }
        public int CompanyId { get; set; }
        public int SubGroupId { get; set; }
 

    }
}
