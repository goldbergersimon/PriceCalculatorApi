using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculatorApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Cups = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    PricePerCup = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Tbs = table.Column<int>(type: "int", nullable: true),
                    PricePerTbs = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Tsp = table.Column<int>(type: "int", nullable: true),
                    PricePerTsp = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Pieces = table.Column<int>(type: "int", nullable: true),
                    PricePerPiece = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Containers = table.Column<int>(type: "int", nullable: true),
                    PricePerContainer = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Pounds = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    PricePerPound = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Oz = table.Column<int>(type: "int", nullable: true),
                    PricePerOz = table.Column<decimal>(type: "decimal(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    IngredientCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    LaborCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Oz = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Container = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Pieces = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "IngredientProduct",
                columns: table => new
                {
                    IngredientsIngredientID = table.Column<int>(type: "int", nullable: false),
                    ProductsProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientProduct", x => new { x.IngredientsIngredientID, x.ProductsProductID });
                    table.ForeignKey(
                        name: "FK_IngredientProduct_Ingredients_IngredientsIngredientID",
                        column: x => x.IngredientsIngredientID,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientProduct_Products_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    OwnPrice = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    MaterialCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    LaborCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    RetailProfit = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WholesaleProfit = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    OwnProfit = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    RetailMargin = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    WholesaleMargin = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    OwnMargin = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    OfficeExpenses = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductIngredients",
                columns: table => new
                {
                    ProductIngredientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    TotalCostPerItem = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIngredients", x => x.ProductIngredientID);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Ingredients_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductLabors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Workers = table.Column<int>(type: "int", nullable: false),
                    Yields = table.Column<int>(type: "int", nullable: false),
                    TotalLaborPerItem = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalLaborCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLabors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLabors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemLabors",
                columns: table => new
                {
                    ItemLaborId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaborName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Workers = table.Column<int>(type: "int", nullable: false),
                    Yields = table.Column<int>(type: "int", nullable: false),
                    TotalLaborPerItem = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalLaborCost = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLabors", x => x.ItemLaborId);
                    table.ForeignKey(
                        name: "FK_ItemLabors_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemProducts",
                columns: table => new
                {
                    ItemProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    Yields = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProducts", x => x.ItemProductId);
                    table.ForeignKey(
                        name: "FK_ItemProducts_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientProduct_ProductsProductID",
                table: "IngredientProduct",
                column: "ProductsProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLabors_ItemId",
                table: "ItemLabors",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProducts_ItemId",
                table: "ItemProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProducts_ProductId",
                table: "ItemProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductID",
                table: "Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_IngredientID",
                table: "ProductIngredients",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_ProductID",
                table: "ProductIngredients",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLabors_ProductId",
                table: "ProductLabors",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientProduct");

            migrationBuilder.DropTable(
                name: "ItemLabors");

            migrationBuilder.DropTable(
                name: "ItemProducts");

            migrationBuilder.DropTable(
                name: "ProductIngredients");

            migrationBuilder.DropTable(
                name: "ProductLabors");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
