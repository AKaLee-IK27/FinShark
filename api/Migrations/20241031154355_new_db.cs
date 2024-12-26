using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class new_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b477e99e-19df-4a5b-ba1d-b8046b8380c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e530489b-0492-4de2-82e4-c64bf03cb2ff");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e05fd7a-5a50-4df8-94fb-6254e4e18bdf", null, "User", "USER" },
                    { "4effacfb-cd94-46e3-8342-6c3cf192ea2d", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e05fd7a-5a50-4df8-94fb-6254e4e18bdf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4effacfb-cd94-46e3-8342-6c3cf192ea2d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b477e99e-19df-4a5b-ba1d-b8046b8380c3", null, "Admin", "ADMIN" },
                    { "e530489b-0492-4de2-82e4-c64bf03cb2ff", null, "User", "USER" }
                });
        }
    }
}
