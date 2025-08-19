using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculatorApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPieces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnwPieces",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RetailPieces",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WhledalePieces",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnwPieces",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RetailPieces",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WhledalePieces",
                table: "Items");
        }
    }
}
