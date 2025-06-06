using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class ItemProduct
{
    public int ItemProductId { get; set; }
    public int ProductId { get; set; }
    public int ItemId { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal Quantity { get; set; }
    [Column(TypeName = "nvarchar(40)")]
    public ItemUnits Unit { get; set; }
    [Range(0, 100)]
    public int Yields { get; set; }
    [Column(TypeName = "decimal(6,2)")]
    public decimal Total { get; set; }

    public Item Item { get; set; } = null!;
    public Product Product { get; set; } = null!;
}