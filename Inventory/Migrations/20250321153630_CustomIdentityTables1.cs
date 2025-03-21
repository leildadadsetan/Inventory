using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class CustomIdentityTables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Warehouses_WarehouseId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_WarehouseId",
                table: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Warehouses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses",
                column: "TenantId",
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Tenants_TenantId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_TenantId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Warehouses");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_WarehouseId",
                table: "Tenants",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Warehouses_WarehouseId",
                table: "Tenants",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
