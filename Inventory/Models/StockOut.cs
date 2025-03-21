namespace Inventory.Models
{
    public class StockOut
    {
        public int StockOutId { get; set; }
        public string RequestNumber { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ApprovedBy { get; set; }
        public string TakenBy { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime OutDate { get; set; }

        public Product Product { get; set; }
    }
}
