namespace Inventory.Dtos
{
    public class GroupDeleteDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string TenantName { get; set; } // برای نمایش نام Tenant
    }
}
