namespace Inventory.Dtos
{
    public class SubGroupDto
    {
        public int SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public string SubGroupCode { get; set; } // اگر نیاز دارید
        public int GroupId { get; set; }
        public string GroupName { get; set; }  
        public int? TenantId { get; set; }
        public string TenantName { get; set; } 
    }
}
