using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mapping1.Migrations
{
    /// <inheritdoc />
    public partial class AddValidDatesToDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ValidOn_End",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidOn_Start",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidOn_End",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "ValidOn_Start",
                table: "Discounts");
        }
    }
}
