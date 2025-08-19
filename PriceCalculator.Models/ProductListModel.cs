using PriceCalculatorApi.Data;

namespace PriceCalculatorApi.Models;

public class ProductListModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal CostPrice { get; set; }
    public decimal? Oz { get; set; }
    public decimal? Container { get; set; }
    public int? Pieces { get; set; }
}

public class ProductEditModel 
{
    public string Name { get; set; } = null!;
    public decimal CostPrice { get; set; }
    public decimal IngredientCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal? Oz { get; set; }
    public decimal? Container { get; set; }
    public int? Pieces { get; set; }
    public List<ProductIngredientEditModel> Ingredients { get; set; } = [];
    public List<ProductLaborEditModel> Labors { get; set; } = [];
}

public class ProductModel: ProductEditModel
{
    public int ProductId { get; set; }
}

public class ProductIngredientEditModel
{
    public int ProductIngredientId { get; set; }
    public int ProductId { get; set; }
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Units Unit { get; set; }
    public decimal TotalCostPerItem { get; set; }
}


public class ProductLaborEditModel
{
    public int Id { get; set; }
    public string LaborName { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    public int Workers { get; set; }
    public int Yields { get; set; }
    public TimeSpan TotalLaborPerItem { get; set; }
    public int ProductId { get; set; }
}

public class PiModel
{
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Units Unit { get; set; } 
}

public class PlModel 
{
    public string LaborName { get; set; }
    public TimeSpan Duration { get; set; }
    public int Workers { get; set; }
    public int Yields { get; set; }
}

public class TlModel
{
    public int Workers { get; set; }
    public TimeSpan TotalLaborPerItem { get; set; }
}




