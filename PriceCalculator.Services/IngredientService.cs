using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;

namespace PriceCalculatorApi.Services;

public class IngredientService(PriceCalculatorDbContext db, ProductService productService, IMapper mapper)
{
    //private readonly IMapper mapper = new MapperConfiguration(i => i.AddProfile<AutoMapperProfile>()).CreateMapper();

    public async Task<List<IngredientModel>> GetIngredientList()
    {
        return await db.Ingredients
            .ProjectTo<IngredientModel>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IngredientModel?> GetIngredient(int id)
    {
        return await db.Ingredients
            .Where(x => x.IngredientID == id)
            .ProjectTo<IngredientModel>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<IngredientModel?> AddIngredient(IngredientEditModel model)
    {
        var entity = mapper.Map<Ingredient>(model);
        db.Ingredients.Add(entity);
        await db.SaveChangesAsync();
        return await GetIngredient(entity.IngredientID);
    }

    public async Task<IngredientModel?> UpdateIngredient(int id, IngredientModel model)
    {
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient == null)
            return null;

        mapper.Map(model, ingredient);

        await RecalculateAffectedProductsAndItems(id);

        await db.SaveChangesAsync();

        return await GetIngredient(id);
    }

    public async Task<bool> DeleteIngredient(int id)
    {
        var ingredient = await db.Ingredients.FindAsync(id);
        if (ingredient == null)
            return false;

        var hasChild = await db.ProductIngredients.AnyAsync(pi => pi.IngredientID == id);

        if (hasChild)
            throw new InvalidOperationException("This ingredient cannot be deleted because it is used in one or more products.");
                
        db.Ingredients.Remove(ingredient);
        await db.SaveChangesAsync();

        return true;
    }



    public async Task RecalculateAffectedProductsAndItems(int ingredientId)
    {
        if (ingredientId == 0) return;

        var horlyRate = await productService.GetHourlyRate();

        var products = await db.Products
            .Include(pi => pi.ProductIngredients)
            .ThenInclude(i => i.Ingredient)
            .Include(pl => pl.ProductLabors)
            .Where(p => p.ProductIngredients.Any(pi => pi.IngredientID == ingredientId))
            .ToListAsync();

        var updatedProductIds = new List<int>();

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

                    decimal laborCost = (decimal)timePerItem.TotalHours * horlyRate * labor.Workers;
                    totalLaborCost += laborCost;
                }
                else
                {
                    labor.TotalLaborPerItem = TimeSpan.Zero;
                }
            }

            product.IngredientCost = totalIngredientCost;
            product.LaborCost = totalLaborCost;

            product.CostPrice = ProductService.CalculateTotalProductCost(totalIngredientCost, totalLaborCost);
            updatedProductIds.Add(product.ProductID);
        }

        await RecalculateAffectedItems(updatedProductIds, horlyRate);
    }

    public async Task RecalculateAffectedItems(List<int> productIds, decimal horlyRate)
    {
        var items = await db.Items
            .Include(ip => ip.ItemProducts)
            .Where(i => i.ItemProducts.Any(ip => productIds.Contains(ip.ProductId)))
            .Include(il => il.ItemLabors)
            .ToListAsync();

        foreach (var item in items)
        {
            decimal totalMaterialCost = 0;
            foreach (var ip in item.ItemProducts)
            {
                ItemService.CalculateProductCost(ip);
                totalMaterialCost += ip.Total;
            }

            decimal totalLaborCost = 0;
            foreach (var labor in item.ItemLabors)
            {
                if (labor.Yields > 0)
                {
                    TimeSpan timePerItem = labor.Duration / labor.Yields;
                    timePerItem = new(timePerItem.Hours, timePerItem.Minutes, timePerItem.Seconds);
                    labor.TotalLaborPerItem = timePerItem;

                    decimal cost = (decimal)timePerItem.TotalHours * horlyRate * labor.Workers;
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

            var (retailSelling, retailProfit) = ItemService.CalculateSellingAndProfit(item.CostPrice, item.OfficeExpenses, item.RetailMargin);
            item.RetailPrice = retailSelling;
            item.RetailProfit = retailProfit;

            var (wholSelling, wholProfit) = ItemService.CalculateSellingAndProfit(item.CostPrice, item.OfficeExpenses, item.WholesaleMargin);
            item.WholesalePrice = wholSelling;
            item.WholesaleProfit = wholProfit;

            var (ownMargin, ownProfit) = ItemService.CalculateMarginAndProfit(item.CostPrice, item.OfficeExpenses, item.OwnPrice);
            item.OwnMargin = ownMargin;
            item.OwnProfit = ownProfit;
        }
    }










    //public IngredientEditModel CalculatePricePer(IngredientEditModel model)
    //{
    //    if (model.TotalCost > 0)
    //    {
    //        if (model.Cups > 0)
    //            model.PricePerCup = model.TotalCost / model.Cups;
    //        if (model.Tbs > 0)
    //            model.PricePerTbs = model.TotalCost / model.Tbs;
    //        if (model.Tsp > 0)
    //            model.PricePerTsp = model.TotalCost / model.Tsp;
    //        if (model.Pieces > 0)
    //            model.PricePerPiece = model.TotalCost / model.Pieces;
    //        if (model.Containers > 0)
    //            model.PricePerContainer = model.TotalCost / model.Containers;
    //        if (model.Pounds > 0)
    //            model.PricePerPound = model.TotalCost / model.Pounds;
    //        if (model.Oz > 0)
    //            model.PricePerOz = model.TotalCost / model.Oz;
    //    }
    //    return model;
    //}

    //public decimal CalculatePricePer(decimal price, decimal measure)
    //{
    //    decimal pricePer = 0;
    //    if (price > 0 && measure > 0)
    //    {
    //        pricePer = price / measure;
    //    }
    //    return pricePer;
    //}

    //public IngredientEditModel CalcTbsTsp(IngredientEditModel model)
    //{
    //    if (model.Cups > 0)
    //    {
    //        model.Tbs = (int)(model.Cups * 16);
    //        model.Tsp = (int)(model.Cups * 48);
    //    }
    //    return model;
    //}

    //public static (decimal Tbs, decimal Tsp) ConvertBetweenMeasures(decimal cups)
    //{
    //    decimal tbs = 0;
    //    decimal tsp = 0;
    //    if (cups > 0)
    //    {
    //        tbs = cups * 16;
    //        tsp = cups * 48;
    //    }
    //    return (tbs, tsp);
    //}
}
