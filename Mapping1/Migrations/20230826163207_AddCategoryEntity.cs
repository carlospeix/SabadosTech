using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mapping1.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 1); // Warning: Custom addition

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            // Warning: Custom addition
            migrationBuilder.InsertData(table: "Categories", columns: new[] { "Id", "Name" }, values: new object[,] {
                { 1, "Unknown" },
                { 2, "Programming" },
                { 3, "Sports" },
                { 4, "Electronics" }
            });

            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 1, column: "CategoryId", value: 2);
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 2, column: "CategoryId", value: 2);
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 3, column: "CategoryId", value: 3);
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 4, column: "CategoryId", value: 3);
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 5, column: "CategoryId", value: 4);
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 6, column: "CategoryId", value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");
        }
    }
}
