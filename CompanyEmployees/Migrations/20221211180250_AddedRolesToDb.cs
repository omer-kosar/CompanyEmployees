using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0422ce44-b3c6-4d79-9cb4-0f474ef2fe9d", "4fd4b84b-75b1-42f9-9c47-84f3a4a2b974", "Administrator", "ADMINISTRATOR" },
                    { "5385a398-c53f-4928-8afc-b64c66028e49", "b1e3b1e5-1eb1-45f2-9468-55a891b007df", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0422ce44-b3c6-4d79-9cb4-0f474ef2fe9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5385a398-c53f-4928-8afc-b64c66028e49");
        }
    }
}
