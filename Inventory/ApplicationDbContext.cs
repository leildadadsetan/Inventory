using Inventory.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet برای تمام جداول
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyProduct> CompanyProducts { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }  // توجه داشته باشید که از ApplicationUserLogin استفاده می‌کنید
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>()
                          .HasOne(ur => ur.User)
                          .WithMany(u => u.UserRoles)
                          .HasForeignKey(ur => ur.UserId)
                          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // تنظیمات برای Warehouse
            modelBuilder.Entity<Warehouse>()
         .HasOne(w => w.Tenant)
         .WithOne(t => t.Warehouse)
         .HasForeignKey<Warehouse>(w => w.TenantId)
         .IsRequired(false);

            // تنظیمات برای Tenant و SubGroup
            // تنظیمات برای Tenant و SubGroup
            modelBuilder.Entity<SubGroup>()
                .HasOne(sg => sg.Tenant)
                .WithMany(t => t.SubGroups)
                .HasForeignKey(sg => sg.TenantId)
                .OnDelete(DeleteBehavior.SetNull); // تنظیم به SetNull

            // تنظیمات برای SubGroup و Group
            modelBuilder.Entity<SubGroup>()
                .HasOne(sg => sg.Group)
                .WithMany(g => g.SubGroups)
                .HasForeignKey(sg => sg.GroupId)
                .OnDelete(DeleteBehavior.Restrict); // تنظیم به Restrict

            // تنظیمات برای Product و SubGroup
            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubGroup)
                .WithMany(sg => sg.Products)
                .HasForeignKey(p => p.SubGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // تنظیمات برای Product و Company
            modelBuilder.Entity<CompanyProduct>()
                .HasKey(cp => new { cp.CompanyId, cp.ProductId });

            modelBuilder.Entity<CompanyProduct>()
         .HasOne(cp => cp.Company)
         .WithMany(c => c.CompanyProducts)
         .HasForeignKey(cp => cp.CompanyId)
         .OnDelete(DeleteBehavior.NoAction); // استفاده از NO ACTION

            modelBuilder.Entity<CompanyProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CompanyProducts)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // استفاده از NO ACTION



        }
    }
}
