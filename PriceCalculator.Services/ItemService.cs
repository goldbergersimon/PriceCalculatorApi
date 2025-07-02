using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;

namespace PriceCalculatorApi.Services;

public class ItemService(PriceCalculatorDbContext db, IMapper mapper)
{
    private decimal? _cachesHourlyRate;

    public async Task<List<ItemListModel>> GetItemLists()
    {
        return await db.Items
            .ProjectTo<ItemListModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
    public async Task<ItemModel?> GetItem(int id)
    {
        return await db.Items
            .Where(x => x.ItemId == id)
            .ProjectTo<ItemModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
    public async Task<List<ItemListModel>> CreateItem(ItemListModel item)
    {
        var entity = mapper.Map<Item>(item);
        db.Items.Add(entity);
        await db.SaveChangesAsync();

        return await GetItemLists();
    }
    public static void CalculateProductCost(ItemProduct itemProduct)
    {
        if (itemProduct == null || itemProduct.Product == null)
        {
            itemProduct.Total = 0;
            return;
        }
        if (itemProduct.Yields == 0)
        {
            itemProduct.Total = 0;
            return;
        }

        static decimal GetSafeValue(decimal? value) => value ?? 0;

        itemProduct.Total = itemProduct.Unit switch
        {
            ItemUnits.Container => itemProduct.Quantity * itemProduct.Product.CostPrice / GetSafeValue(itemProduct.Product.Container) / itemProduct.Yields,
            ItemUnits.Oz => itemProduct.Quantity * itemProduct.Product.CostPrice / GetSafeValue(itemProduct.Product.Oz) / itemProduct.Yields,
            ItemUnits.Pieces => itemProduct.Quantity * itemProduct.Product.CostPrice / GetSafeValue(itemProduct.Product.Pieces) / itemProduct.Yields,
            _ => 0
        };
    }

    public static decimal CalculateTotalMaterialCost(IEnumerable<ItemProduct> products)
    {
        decimal totaMaterialCost = 0;

        foreach (var product in products)
        {
            totaMaterialCost += product.Total;
        }
        return totaMaterialCost;
    }

    public static void CalculateTimePerItem(IEnumerable<ItemLabor> labors)
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

    public static decimal CalculateTotalLaborCost(IEnumerable<ItemLabor> labors, decimal hourlyRate)
    {
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


    public static decimal CalculateTotalItemCost(decimal materialCost, decimal laborCost, decimal officecost)
    {
        return materialCost + laborCost + officecost;
    }

    public static (decimal Selling, decimal Profit) CalculateSellingAndProfit(decimal cost, decimal officeCost, decimal margin)
    {
        decimal totalCost = cost + officeCost;
        decimal sellingPrice = totalCost * (1 + margin / 100m);
        decimal profit = sellingPrice - totalCost;
        return (sellingPrice, profit);
    }

    public static (decimal Profit, decimal Margin) CalculateMarginAndProfit(decimal cost, decimal officeCost, decimal selling)
    {
        decimal totalCost = cost + officeCost;
        decimal profit = selling - totalCost;
        decimal margin = totalCost == 0 ? 0 : (profit / totalCost) * 100m;
        return (profit, margin);
    }

    public async Task<decimal> GetHourlyRate()
    {
        if (_cachesHourlyRate.HasValue)
            return _cachesHourlyRate.Value;

        //using DessertPriceDbContext db = new();
        var setting = await db.Settings.FirstOrDefaultAsync(s => s.Key == SettingKeys.HourlyRate);
        if (setting != null && decimal.TryParse(setting.Value, out decimal rate))
        {
            _cachesHourlyRate = rate;
            return rate;
        }

        _cachesHourlyRate = 0;
        return 0;
    }
}
