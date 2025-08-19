using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculatorApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class takeOffProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLaborCost",
                table: "ProductLabors");

            migrationBuilder.DropColumn(
                name: "TotalLaborCost",
                table: "ItemLabors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalLaborCost",
                table: "ProductLabors",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalLaborCost",
                table: "ItemLabors",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
