using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mapping1.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToProductAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPriceWhenOrdered",
                table: "OrderItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 1, columns: new[] { "Price" }, values: new object[] { 240.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 2, columns: new[] { "Price" }, values: new object[] {  10.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 3, columns: new[] { "Price" }, values: new object[] { 120.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 4, columns: new[] { "Price" }, values: new object[] {  10.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 5, columns: new[] { "Price" }, values: new object[] {  30.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 6, columns: new[] { "Price" }, values: new object[] { 600.0m });
            migrationBuilder.UpdateData(table: "Products", keyColumn: "Id", keyValue: 1, columns: new[] { "Price" }, values: new object[] { 999.0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPriceWhenOrdered",
                table: "OrderItems");
        }
    }
}
