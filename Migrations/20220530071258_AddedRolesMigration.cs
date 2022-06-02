using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelsListing.Migrations
{
    public partial class AddedRolesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4e3cb1f0-db37-4fcb-b170-7c85306528fd", "3032c7e0-f2dc-4fe3-9731-38652a09eaa3", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "601fd2c5-7910-44b8-8357-8fc81da67ac7", "070cfa09-041e-4cf4-afa9-928de7d2dadd", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e3cb1f0-db37-4fcb-b170-7c85306528fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "601fd2c5-7910-44b8-8357-8fc81da67ac7");
        }
    }
}
