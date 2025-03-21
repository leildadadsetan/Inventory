using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام الزامی است.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "شماره تماس الزامی است.")]
        [Phone(ErrorMessage = "شماره تماس معتبر نیست.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأیید رمز عبور")]
        [Compare("Password", ErrorMessage = "رمز عبور و تأیید رمز عبور مطابقت ندارند.")]
        public string ConfirmPassword { get; set; }
    }
}
