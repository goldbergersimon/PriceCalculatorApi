using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCalculatorApi.Data;

public class ProductIngredient
{
    public int ProductIngredientID { get; set; }
    public int ProductID { get; set; }
    public int IngredientID { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal Quantity { get; set; }
    [Column(TypeName = "nvarchar(40)")]
    public Units Unit { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal TotalCostPerItem { get; set; }

    public Product Product { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}