using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceCalculatorApi.Data;

public class ItemIngredient
{
    public int ItemIngredientId { get; set; }

    public int ItemId { get; set; }
    public int IngredientId { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal Quantity { get; set; }
    [Column(TypeName = "nvarchar(40)")]
    public Units Unit { get; set; }
    [Range(0, 100)]
    public int Yields { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal TotalCostPerItem { get; set; }

    public Item Item { get; set; } = null!;
    public Ingredient Ingredient { get; set; } = null!;
}
