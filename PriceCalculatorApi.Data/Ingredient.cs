using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class Ingredient
{
    public int IngredientId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Column(TypeName = "decimal(6,2)")]
    public decimal? TotalCost { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? Cups { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerCup { get; set; }
    [Range(0, 10000)]
    public int? Tbs { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerTbs { get; set; }
    [Range(0, 10000)]
    public int? Tsp { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerTsp { get; set; }
    [Range(0, 10000)]
    public int? Pieces { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerPiece { get; set; }
    [Range(0, 10000)]
    public int? Containers { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerContainer { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? Pounds { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerPound { get; set; }
    [Range(0, 10000)]
    public int? Oz { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal? PricePerOz { get; set; }
    public ICollection<ProductIngredient> ProductIngredients { get; set; } = [];
}
