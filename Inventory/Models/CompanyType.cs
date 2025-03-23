namespace Inventory.Models
{
    public class CompanyType
    {
        public int CompanyTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Company> Companies { get; set; }
    }
}
