namespace Inventory.Models
{
    public class ProductUsage
    {
        public int UsageId { get; set; }
        public int ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PatientFrom { get; set; }
        public string PatientTo { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }

        public Product Product { get; set; }
    }
}
