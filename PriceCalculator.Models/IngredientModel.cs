namespace PriceCalculatorApi.Models;

public class IngredientModel : IngredientEditModel
{
    public int IngredientID { get; set; }

}

public class IngredientEditModel
{
    public string Name { get; set; } = null!;
    public decimal TotalCost { get; set; }
    public decimal Cups { get; set; }
    public decimal PricePerCup { get; set; }
    public int Tbs { get; set; }
    public decimal PricePerTbs { get; set; }
    public int Tsp { get; set; }
    public decimal PricePerTsp { get; set; }
    public int Pieces { get; set; }
    public decimal PricePerPiece { get; set; }
    public int Containers { get; set; }
    public decimal PricePerContainer { get; set; }
    public decimal Pounds { get; set; }
    public decimal PricePerPound { get; set; }
    public int Oz { get; set; }
    public decimal PricePerOz { get; set; }
}
