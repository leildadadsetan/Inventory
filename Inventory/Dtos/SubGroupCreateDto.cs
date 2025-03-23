namespace Inventory.Dtos
{
    public class SubGroupCreateDto
    {
        public string SubGroupName { get; set; }
        public string SubGroupCode { get; set; } // اگر نیاز دارید
        public int GroupId { get; set; }

    }
}
