namespace Inventory.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int SupplierId { get; set; }
        public string UserId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
