namespace Inventory.Dtos
{
    public class SubGroupEditDto
    {
        public int GroupId { get; set; }
        public int SubGroupId { get; set; }
        public int? TenantId { get; set; }

        public string SubGroupName { get; set; }
        public string SubGroupCode { get; set; } // اگر نیاز دارید
 
    }
}
