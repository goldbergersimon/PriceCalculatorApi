using PriceCalculatorApi.Data;

namespace PriceCalculatorApi.Models;

public class ItemListModel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public decimal CostPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal OwnPrice { get; set; }
}

public class ItemModel: ItemEditModel
{
    public int ItemId { get; set; }
}
public class ItemEditModel
{
    public string ItemName { get; set; } = null!;
    public decimal CostPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal OwnPrice { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal RetailProfit { get; set; }
    public decimal WholesaleProfit { get; set; }
    public decimal OwnProfit { get; set; }
    public decimal RetailMargin { get; set; }
    public decimal WholesaleMargin { get; set; }
    public decimal OwnMargin { get; set; }
    public decimal OfficeExpenses { get; set; }

    public List<ItemProductModel> Products { get; set; } = [];
    public List<ItemLaborModel> Labors { get; set; } = [];
    public List<ItemIngredientModel> Ingredients { get; set; } = [];
}

public class ItemProductModel
{
    public int ItemProductId { get; set; }
    public int ProductId { get; set; }
    public int ItemId { get; set; }
    public decimal Quantity { get; set; }
    public ItemUnits Unit { get; set; }
    public int Yields { get; set; }
    public decimal Total { get; set; }
}

public class ItemLaborModel
{
    public int ItemLaborId { get; set; }
    public string LaborName { get; set; } = null!;
    public TimeSpan Duration { get; set; }
    public int Workers { get; set; }
    public int Yields { get; set; }
    public TimeSpan TotalLaborPerItem { get; set; }
    public int ItemId { get; set; }
}

public class ItemIngredientModel
{
    public int ItemIngredientId { get; set; }

    public int ItemId { get; set; }
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Units Unit { get; set; }
    public int Yields { get; set; }
    public decimal TotalCostPerItem { get; set; }
}

public class IiModel
{
    public int IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Units Unit { get; set; }
    public int Yields { get; set; }
}

public class IpModel
{
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public ItemUnits Unit { get; set; }
    public int Yields { get; set; }
}

public class MarginInput
{
    public decimal Margin { get; set; }
    public decimal Cost { get; set; }
}

public class Sp
{
    public decimal Selling { get; set; }
    public decimal Profit { get; set; }
}

public class SellingInput
{
    public decimal Selling { get; set; }
    public decimal Cost { get; set; }
}

public class Pm
{
    public decimal Margin { get; set; }
    public decimal Profit { get; set; }
}

