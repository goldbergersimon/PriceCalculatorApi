using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class Product
{
    public int ProductId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Column(TypeName = "decimal(6,2)")]
    public decimal CostPrice { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal IngredientCost { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal LaborCost { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? Oz { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? Container { get; set; }
    [Range(0, 10000)]
    public int? Pieces { get; set; }
    public ICollection<ProductIngredient> ProductIngredients { get; set; } = [];
    public ICollection<ProductLabor> ProductLabors { get; set; } = [];
    public ICollection<ItemProduct> ItemProducts { get; set; } = [];
}