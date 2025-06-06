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

public class ItemModel : ItemEditModel
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
}

