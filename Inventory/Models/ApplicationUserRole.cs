﻿using Microsoft.AspNetCore.Identity;

namespace Inventory.Models
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {

        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
