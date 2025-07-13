using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;
using System.Data;

namespace PriceCalculatorApi.Services;

public class ProductService(PriceCalculatorDbContext db, IMapper mapper, ItemService itemService)
{
    private decimal? _cachesHourlyRate;

    public async Task<List<ProductListModel>> GetProductList()
    {
        return await db.Products
            .ProjectTo<ProductListModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
    public async Task<ProductListModel?> GetProduct(int id)
    {
        return await db.Products
            .Where(x => x.ProductId == id)
            .ProjectTo<ProductListModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
    public async Task<ProductModel?> GetProductDetails(int id)
    {
        var product = await db.Products
            .Where(pi => pi.ProductId == id)
            .Include(pi => pi.ProductIngredients)
            .Include(pl => pl.ProductLabors)
            .ProjectTo<ProductModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if(product == null)
            return null;

        return product;
    }

    public async Task<ProductListModel> CreateProduct(ProductEditModel model)
    {
        var entity = mapper.Map<Product>(model);
        await db.Products.AddAsync(entity);
        await db.SaveChangesAsync();
        return await GetProduct(entity.ProductId);
    }

    public async Task<ProductListModel?> UpdateProduct(ProductModel model)
    {
        var existing = await db.Products
            .Include(pi => pi.ProductIngredients)
            .Include(pl => pl.ProductLabors)
            .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);
        if (existing == null) return null;

        mapper.Map(model, existing);

        await RecalculateAffectedItems(model.ProductId);

        await db.SaveChangesAsync();

        return mapper.Map<ProductListModel>(existing);
    }


    public async Task<bool> DeleteProduct(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product == null) return false;

        bool hasChild = await db.ItemProducts.AnyAsync(ip => ip.ProductId == id);
        if (hasChild)
            throw new InvalidOperationException("This recipe cannot be deleted because it is used in one or more items.");

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return true;
    }

    public static void CalculateIngredientCost(ProductIngredient productIngredient)
    {
        if (productIngredient == null || productIngredient.Ingredient == null)
        {
            productIngredient.TotalCostPerItem = 0;
            return ;
        }

        productIngredient.TotalCostPerItem = productIngredient.Unit switch
        {
            Units.Cups => productIngredient.Quantity * productIngredient.Ingredient.PricePerCup!.Value,
            Units.Tbs => productIngredient.Quantity * productIngredient.Ingredient.PricePerTbs!.Value,
            Units.Tsp => productIngredient.Quantity * productIngredient.Ingredient.PricePerTsp!.Value,
            Units.Pieces => productIngredient.Quantity * productIngredient.Ingredient.PricePerPiece!.Value,
            Units.Pounds => productIngredient.Quantity * productIngredient.Ingredient.PricePerPound!.Value,
            Units.Oz => productIngredient.Quantity * productIngredient.Ingredient.PricePerOz!.Value,
            _ => 0
        };
    }

    public async Task<decimal> calculateIngredientCost(PiModel productIngredient)
    {
        var ingredient = await db.Ingredients.FirstOrDefaultAsync(i => i.IngredientId == productIngredient.IngredientId);
        if (productIngredient == null || ingredient == null)
            return 0;

        decimal totalCostPerItem = productIngredient.Unit switch
        {
            Units.Cups => productIngredient.Quantity * ingredient.PricePerCup!.Value,
            Units.Tbs => productIngredient.Quantity * ingredient.PricePerTbs!.Value,
            Units.Tsp => productIngredient.Quantity * ingredient.PricePerTsp!.Value,
            Units.Pieces => productIngredient.Quantity * ingredient.PricePerPiece!.Value,
            Units.Pounds => productIngredient.Quantity * ingredient.PricePerPound!.Value,
            Units.Oz => productIngredient.Quantity * ingredient.PricePerOz!.Value,
            _ => 0
        };
        return totalCostPerItem;
    }
    public TimeSpan CalculateTotaltime(PlModel labor)
    {
        if (labor.Yields > 0)
        {
            return TimeSpan.FromTicks(labor.Duration.Ticks / labor.Yields);
        }
        else
        {
            return TimeSpan.Zero;
        }
    }

    public static decimal CalculateTotalIngredientCost(IEnumerable<ProductIngredient> ingredients)
    {
        decimal totalIngredientCost = 0;

        foreach (var ingredient in ingredients)
        {
            totalIngredientCost += ingredient.TotalCostPerItem;
        }
        return totalIngredientCost;
    }

    public static decimal CalculateTotalProductCost(decimal ingredientCost, decimal laborCost)
    {
        return ingredientCost + laborCost;
    }

    public static void CalculateTimePerItem(IEnumerable<ProductLabor> labors)
    {
        foreach (var labor in labors)
        {
            if (labor.Yields > 0)
            {
                labor.TotalLaborPerItem = TimeSpan.FromTicks(labor.Duration.Ticks / labor.Yields);
            }
            else
            {
                labor.TotalLaborPerItem = TimeSpan.Zero;
            }
        }
    }

    public async Task<decimal> CalculateTotalLaborCost(IEnumerable<TlModel> labors)
    {
        decimal hourlyRate = await GetHourlyRate();
        decimal totalLaborCost = 0;
        foreach (var labor in labors)
        {
            if (labor.Workers > 0)
            {
                decimal cost = (decimal)labor.TotalLaborPerItem.TotalHours * hourlyRate * labor.Workers;
                totalLaborCost += cost;
            }
        }
        return totalLaborCost;
    }


    public async Task<decimal> GetHourlyRate()
    {
        if (_cachesHourlyRate.HasValue)
            return _cachesHourlyRate.Value;

        var setting = await db.Settings.FirstOrDefaultAsync(s => s.Key == SettingKeys.HourlyRate);
        if (setting != null && decimal.TryParse(setting.Value, out decimal rate))
        {
            _cachesHourlyRate = rate;
            return rate;
        }
        _cachesHourlyRate = 0;
        return 0;
    }

     public async Task RecalculateAffectedItems(int id)
     {
        if (id == 0) return;

        var hourlyRate = await GetHourlyRate();

        var items = await db.Items
            .Include(ip => ip.ItemProducts)
            .ThenInclude(p => p.Product)
            .Include(ii => ii.ItemIngredients)
            .ThenInclude(i => i.Ingredient)
            .Include(pl => pl.ItemLabors)
            .Where(i => i.ItemProducts.Any(ip => ip.ProductId == id))
            .ToListAsync();

        foreach (var item in items)
        {
            decimal totalMaterialCost = 0;
            foreach (var ip in item.ItemProducts)
            {
                ItemService.CalculateProductCost(ip);
                totalMaterialCost += ip.Total;
            }
            foreach (var ii in item.ItemIngredients)
            {
                itemService.CalculateIngredientCost(ii);
                totalMaterialCost += ii.TotalCostPerItem;
            }

            decimal totalLaborCost = 0;
            foreach (var labor in item.ItemLabors)
            {
                if (labor.Yields > 0)
                {
                    TimeSpan timePerItem = labor.Duration / labor.Yields;
                    timePerItem = new(timePerItem.Hours, timePerItem.Minutes, timePerItem.Seconds);
                    labor.TotalLaborPerItem = timePerItem;

                    decimal laborcost = (decimal)timePerItem.TotalHours * hourlyRate * labor.Workers;
                    totalLaborCost += laborcost;
                }
                else
                {
                    labor.TotalLaborPerItem = TimeSpan.Zero;
                }
            }

            item.MaterialCost = totalMaterialCost;
            item.LaborCost = totalLaborCost;
            item.CostPrice = ItemService.CalculateTotalItemCost(totalMaterialCost, totalLaborCost, item.OfficeExpenses);

            var (retailSelling, retailProfit) = ItemService.CalculateSellingAndProfit(item.CostPrice, item.RetailMargin);
            item.RetailPrice = retailSelling;
            item.RetailProfit = retailProfit;

            var (wholSelling, wholProfit) = ItemService.CalculateSellingAndProfit(item.CostPrice, item.WholesaleMargin);
            item.WholesalePrice = wholSelling;
            item.WholesaleProfit = wholProfit;

            var (ownMargin, ownProfit) = ItemService.CalculateMarginAndProfit(item.CostPrice, item.OwnPrice);
            item.OwnMargin = ownMargin;
            item.OwnProfit = ownProfit;
        }
     }
}

