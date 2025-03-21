using Microsoft.AspNetCore.Identity;

namespace Inventory.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }  // استفاده از ApplicationUserLogin
        public ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserToken> UserTokens { get; set; }  // اینجا اضافه شد

    }
}
