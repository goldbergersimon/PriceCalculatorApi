using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Data;

public class ProductLabor
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string LaborName { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    [Range(0, 100)]
    public int Workers { get; set; }
    [Range(0, 1000)]
    public int Yields { get; set; }
    public TimeSpan TotalLaborPerItem { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}