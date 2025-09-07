using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class Item
{
    public int ItemId { get; set; }
    [MaxLength(100)]
    public string ItemName { get; set; } = null!;
    [Column(TypeName = "decimal(6,2)")]
    public decimal CostPrice { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal WholesalePrice { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal RetailPrice { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal OwnPrice { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal MaterialCost { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal LaborCost { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal RetailProfit { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal WholesaleProfit { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal OwnProfit { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal RetailMargin { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal WholesaleMargin { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal OwnMargin { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal OfficeExpenses { get; set; }
    public bool IncludeOfficeExpenses { get; set; }
    [Range(0, 1000)]
    public int PiecesPerBox { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal RetailBox { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal WholesaleBox { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal OwnBox { get; set; }

    public ICollection<ItemProduct> ItemProducts { get; set; } = [];
    public ICollection<ItemLabor> ItemLabors { get; set; } = [];
    public ICollection<ItemIngredient> ItemIngredients { get; set; } = [];
}