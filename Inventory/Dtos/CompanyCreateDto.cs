namespace Inventory.Dtos
{
    public class CompanyCreateDto
    {
        public string Name { get; set; }
        public int CompanyTypeId { get; set; }  // اشاره به نوع کمپانی
        //public string CompanyTypeName { get; set; } // نام نوع کمپانی
        public string Address { get; set; }
        public string CompanyCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
    }
}
