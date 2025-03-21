using Microsoft.AspNetCore.Identity;

namespace Inventory.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
