using System.ComponentModel.DataAnnotations;

namespace Inventory.Dtos
{
    public class GroupEditDto
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "نام گروه الزامی است.")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "کد گروه الزامی است.")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "کد گروه باید حتماً دو رقم باشد.")]
        public string GroupCode { get; set; }

        [Required(ErrorMessage = "شناسه ازمایشگاه الزامی است.")]
        public int TenantId { get; set; }
    }
}