using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;

namespace PriceCalculatorApi.Services;

public class SettingsService(PriceCalculatorDbContext db, IMapper mapper, ItemService itemService)
{

    public async Task<List<Settings>> GetSettings()
    {
        return await db.Settings.ToListAsync();
    }

    public async Task SaveSettings(List<SettingsModel> settings)
    {
        var oldSettings = await db.Settings.ToListAsync();
        var changedSettings = new List<SettingsModel>();

        foreach (var setting in settings)
        { 
            var existing = oldSettings.FirstOrDefault(s => s.Key == setting.Key);
            if (existing != null && existing.Value != setting.Value)
                changedSettings.Add(setting);

            mapper.Map(setting, existing);
        }
        await db.SaveChangesAsync();
        await Recalculate(changedSettings);
    }

    private async Task Recalculate(List<SettingsModel> changedSettings)
    {
        foreach (var setting in changedSettings)
        {
            switch (setting.Key)
            {
                case SettingKeys.HourlyRate:
                    await RecalculateAllProductsAndItems();
                    break;
                case SettingKeys.Rent:
                case SettingKeys.OfficePayroll:
                case SettingKeys.Machinery:
                case SettingKeys.UnitsPerMonth:
                case SettingKeys.Insurance:
                case SettingKeys.Marketing:
                case SettingKeys.SoftwareExpenses:
                case SettingKeys.Supplies:
                case SettingKeys.Utilities:
                    await RecalculateAllItemsTotal();
                    break;
            }
        }
    }

    private async Task RecalculateAllItemsTotal()
    {
        decimal officeExpenses = await GetOfficeExpenses();

        var items = await db.Items.ToListAsync();

        foreach (var item in items)
        {
            item.OfficeExpenses = officeExpenses;

            item.CostPrice = item.MaterialCost + item.LaborCost + item.OfficeExpenses;

            (decimal retailSelling, decimal retailProfit) = ItemService.CalculateSellingAndProfit(item.MaterialCost + item.LaborCost + item.OfficeExpenses, item.RetailMargin);
            item.RetailPrice = retailSelling;
            item.RetailProfit = retailProfit;
            item.RetailBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.RetailPrice);

            (decimal wholeSelling, decimal wholeProfit) = ItemService.CalculateSellingAndProfit(item.MaterialCost + item.LaborCost + item.OfficeExpenses, item.WholesaleMargin);
            item.WholesalePrice = wholeSelling;
            item.WholesaleProfit = wholeProfit;
            item.WholesaleBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.WholesalePrice);

            (decimal ownProfit, decimal ownMargin) = ItemService.CalculateMarginAndProfit(item.MaterialCost + item.LaborCost + item.OfficeExpenses, item.OwnPrice);
            item.OwnProfit = ownProfit;
            item.OwnMargin = ownMargin;
            item.OwnBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.OwnPrice);
        }
        await db.SaveChangesAsync();
    }

    private async Task RecalculateAllProductsAndItems()
    {
        await RecalculateAllProducts();
    }

    private async Task RecalculateAllProducts()
    {

        var hourlyRate = await GetHourlyRate();

        var products = await db.Products
            .Include(pi => pi.ProductIngredients)
            .ThenInclude(i => i.Ingredient)
            .Include(pl => pl.ProductLabors)
            .ToListAsync();

        foreach (var product in products)
        {
            decimal totalIngredientCost = 0;
            foreach (var pi in product.ProductIngredients)
            {
                ProductService.CalculateIngredientCost(pi);
                totalIngredientCost += pi.TotalCostPerItem;
            }


            decimal totalLaborCost = 0;
            foreach (var labor in product.ProductLabors)
            {
                if (labor.Yields > 0)
                {
                    TimeSpan timePerItem = labor.Duration / labor.Yields;
                    timePerItem = new TimeSpan(timePerItem.Hours, timePerItem.Minutes, timePerItem.Seconds);
                    labor.TotalLaborPerItem = timePerItem;

                    decimal laborCost = (decimal)timePerItem.TotalHours * hourlyRate * labor.Workers;
                    totalLaborCost += laborCost;
                }
                else
                {
                    labor.TotalLaborPerItem = TimeSpan.Zero;
                }

                product.IngredientCost = totalIngredientCost;
                product.LaborCost = totalLaborCost;
                product.CostPrice = ProductService.CalculateTotalProductCost(totalIngredientCost, totalLaborCost);
            }
        }
        await RecalculateAllItems(hourlyRate);
    }

    private async Task RecalculateAllItems(decimal hourlyRate)
    {
        var items = await db.Items
              .Include(ip => ip.ItemProducts)
              .Include(il => il.ItemLabors)
              .Include(ii => ii.ItemIngredients)
              .ThenInclude(i => i.Ingredient)
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
                    timePerItem = new TimeSpan(timePerItem.Hours, timePerItem.Minutes, timePerItem.Seconds);
                    labor.TotalLaborPerItem = timePerItem;

                    decimal cost = (decimal)timePerItem.TotalHours * hourlyRate * labor.Workers;
                    totalLaborCost += cost;
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
            item.RetailBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.RetailPrice);

            var (wholSelling, wholProfit) = ItemService.CalculateSellingAndProfit(item.CostPrice, item.WholesaleMargin);
            item.WholesalePrice = wholSelling;
            item.WholesaleProfit = wholProfit;
            item.WholesaleBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.WholesalePrice);

            var (ownMargin, ownProfit) = ItemService.CalculateMarginAndProfit(item.CostPrice, item.OwnPrice);
            item.OwnMargin = ownMargin;
            item.OwnProfit = ownProfit;
            item.OwnBox = itemService.CalculateBoxPrice(item.PiecesPerBox, item.OwnPrice);
        }
        await db.SaveChangesAsync();
    }

    private async Task<decimal> GetHourlyRate()
    {
        var setting = await db.Settings.FirstOrDefaultAsync(s => s.Key == SettingKeys.HourlyRate);
        if (setting != null && decimal.TryParse(setting.Value, out var rate))
            return rate;
        return 0;
    }

    public async Task<decimal> GetOfficeExpenses()
    {
        var settings = await db.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

        decimal rent = settings.ContainsKey(SettingKeys.Rent) &&
            decimal.TryParse(settings[SettingKeys.Rent], out var r) ? r : 0;
        decimal utilities = settings.ContainsKey(SettingKeys.Utilities) &&
            decimal.TryParse(settings[SettingKeys.Utilities], out var u) ? u : 0;
        int units = settings.ContainsKey(SettingKeys.UnitsPerMonth) &&
            int.TryParse(settings[SettingKeys.UnitsPerMonth], out var upm) ? upm : 0;
        decimal officePayroll = settings.ContainsKey(SettingKeys.OfficePayroll) &&
            decimal.TryParse(settings[SettingKeys.OfficePayroll], out var op) ? op : 0;
        decimal marketing = settings.ContainsKey(SettingKeys.Marketing) &&
            decimal.TryParse(settings[SettingKeys.Marketing], out var mark) ? mark : 0;
        decimal softwareExpenses = settings.ContainsKey(SettingKeys.SoftwareExpenses) &&
            decimal.TryParse(settings[SettingKeys.SoftwareExpenses], out var se) ? se : 0;
        decimal machinery = settings.ContainsKey(SettingKeys.Machinery) &&
            decimal.TryParse(settings[SettingKeys.Machinery], out var mas) ? mas : 0;
        decimal supplies = settings.ContainsKey(SettingKeys.Supplies) &&
            decimal.TryParse(settings[SettingKeys.Supplies], out var s) ? s : 0;
        decimal insurance = settings.ContainsKey(SettingKeys.Insurance) &&
            decimal.TryParse(settings[SettingKeys.Insurance], out var ins) ? ins : 0;

        return units > 0 ? (rent + utilities + officePayroll + marketing + softwareExpenses + machinery + supplies + insurance) / units : 0;
    }
}
