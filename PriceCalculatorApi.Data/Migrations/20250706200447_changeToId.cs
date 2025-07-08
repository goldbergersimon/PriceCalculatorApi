using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceCalculatorApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemIngredients_Ingredients_IngredientID",
                table: "ItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientID",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ProductID",
                table: "ProductIngredients");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductIngredients",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "IngredientID",
                table: "ProductIngredients",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "ProductIngredientID",
                table: "ProductIngredients",
                newName: "ProductIngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ProductID",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_IngredientID",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_IngredientId");

            migrationBuilder.RenameColumn(
                name: "IngredientID",
                table: "ItemIngredients",
                newName: "IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemIngredients_IngredientID",
                table: "ItemIngredients",
                newName: "IX_ItemIngredients_IngredientId");

            migrationBuilder.RenameColumn(
                name: "IngredientID",
                table: "Ingredients",
                newName: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemIngredients_Ingredients_IngredientId",
                table: "ItemIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientId",
                table: "ProductIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemIngredients_Ingredients_IngredientId",
                table: "ItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientId",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductIngredients",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "ProductIngredients",
                newName: "IngredientID");

            migrationBuilder.RenameColumn(
                name: "ProductIngredientId",
                table: "ProductIngredients",
                newName: "ProductIngredientID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_IngredientId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_IngredientID");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "ItemIngredients",
                newName: "IngredientID");

            migrationBuilder.RenameIndex(
                name: "IX_ItemIngredients_IngredientId",
                table: "ItemIngredients",
                newName: "IX_ItemIngredients_IngredientID");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "Ingredients",
                newName: "IngredientID");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemIngredients_Ingredients_IngredientID",
                table: "ItemIngredients",
                column: "IngredientID",
                principalTable: "Ingredients",
                principalColumn: "IngredientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Ingredients_IngredientID",
                table: "ProductIngredients",
                column: "IngredientID",
                principalTable: "Ingredients",
                principalColumn: "IngredientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ProductID",
                table: "ProductIngredients",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
