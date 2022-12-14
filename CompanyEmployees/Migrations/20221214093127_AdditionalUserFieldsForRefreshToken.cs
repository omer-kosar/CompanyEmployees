using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalUserFieldsForRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0422ce44-b3c6-4d79-9cb4-0f474ef2fe9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5385a398-c53f-4928-8afc-b64c66028e49");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0792c270-fd07-4361-9d9a-4100884b1925", "1f643e5e-5882-43a9-9bf7-ecb3fbac2ad8", "Administrator", "ADMINISTRATOR" },
                    { "63c36e61-3896-402c-8003-5c3ac9d56fb4", "00315a26-e7b4-4410-aca7-b88ac76f8059", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0792c270-fd07-4361-9d9a-4100884b1925");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63c36e61-3896-402c-8003-5c3ac9d56fb4");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0422ce44-b3c6-4d79-9cb4-0f474ef2fe9d", "4fd4b84b-75b1-42f9-9c47-84f3a4a2b974", "Administrator", "ADMINISTRATOR" },
                    { "5385a398-c53f-4928-8afc-b64c66028e49", "b1e3b1e5-1eb1-45f2-9468-55a891b007df", "Manager", "MANAGER" }
                });
        }
    }
}
