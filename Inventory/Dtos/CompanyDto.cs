namespace Inventory.Dtos
{
    public class CompanyDto
    {
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public int CompanyTypeId { get; set; }  
        public string CompanyTypeName { get; set; } 
        public string Address { get; set; }
        public string CompanyCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
    }
}
