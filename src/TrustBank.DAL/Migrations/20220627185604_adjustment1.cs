using Microsoft.EntityFrameworkCore.Migrations;

namespace TrustBank.Infrastructure.Migrations
{
    public partial class adjustment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "Products",
                newName: "ProductType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "Products",
                newName: "AccountType");
        }
    }
}
