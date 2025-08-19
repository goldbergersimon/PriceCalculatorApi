using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class ItemLabor
{
    public int ItemLaborId { get; set; }
    [MaxLength(100)]
    public string LaborName { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    [Range(0, 100)]
    public int Workers { get; set; }
    [Range(0, 100)]
    public int Yields { get; set; }
    public TimeSpan TotalLaborPerItem { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}