using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mapping1.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Products", columns: new[] { "Id", "Name" }, values: new object[,] {
                { 1, "Visual Studio Code" },
                { 2, "Vim" },
                { 3, "Surf appliance" },
                { 4, "Swimming googles" },
                { 5, "Pixel 6" },
                { 6, "Notebook" }
            });

            migrationBuilder.InsertData(table: "Customers", columns: new[] { "Id", "Name", "CountryId" }, values: new object[,] {
                { 1, "Homero Simpson", 1 },
                { 2, "Marge Simpson", 1 },
                { 3, "Moe", 1 },
                { 4, "Mr. Burns", 1}
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Customers", "Id", 1);
            migrationBuilder.DeleteData(table: "Customers", "Id", 2);
            migrationBuilder.DeleteData(table: "Customers", "Id", 3);
            migrationBuilder.DeleteData(table: "Customers", "Id", 4);

            migrationBuilder.DeleteData(table: "Products", "Id", 1);
            migrationBuilder.DeleteData(table: "Products", "Id", 2);
            migrationBuilder.DeleteData(table: "Products", "Id", 3);
            migrationBuilder.DeleteData(table: "Products", "Id", 4);
            migrationBuilder.DeleteData(table: "Products", "Id", 5);
            migrationBuilder.DeleteData(table: "Products", "Id", 6);
        }
    }
}
