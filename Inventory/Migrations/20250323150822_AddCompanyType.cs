using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyType_CompanyTypeId",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyType",
                table: "CompanyType");

            migrationBuilder.RenameTable(
                name: "CompanyType",
                newName: "CompanyTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyTypes",
                table: "CompanyTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyTypes_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyTypes_CompanyTypeId",
                table: "Companies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyTypes",
                table: "CompanyTypes");

            migrationBuilder.RenameTable(
                name: "CompanyTypes",
                newName: "CompanyType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyType",
                table: "CompanyType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyType_CompanyTypeId",
                table: "Companies",
                column: "CompanyTypeId",
                principalTable: "CompanyType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
