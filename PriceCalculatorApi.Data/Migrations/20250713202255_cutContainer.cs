using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculatorApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class cutContainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Containers",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PricePerContainer",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Containers",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerContainer",
                table: "Ingredients",
                type: "decimal(6,2)",
                nullable: true);
        }
    }
}
