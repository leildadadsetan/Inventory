namespace Inventory.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime DateRangeStart { get; set; }
        public DateTime DateRangeEnd { get; set; }
        public int PurchasedPackages { get; set; }
        public int PurchasedUnits { get; set; }
        public int SoldPackages { get; set; }
        public int SoldUnits { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal CostRatio { get; set; }
    }
}
