using PriceCalculatorApi.Data;

namespace PriceCalculatorApi.Models;

public class ProductListModel
{
    public int ProductID { get; set; }
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
}

public class ProductModel : ProductEditModel
{
    public int ProductID { get; set; }
    public List<ProductIngredientModel> Ingredients { get; set; } = [];
}

public class ProductIngredientEditModel
{
    public int ProductID { get; set; }
    public int IngredientID { get; set; }
    public decimal Quantity { get; set; }
    public Units Unit { get; set; }
    public decimal TotalCostPerItem { get; set; }
}

public class ProductIngredientModel: ProductIngredientEditModel
{
    public int ProductIngredientID { get; set; }
}


