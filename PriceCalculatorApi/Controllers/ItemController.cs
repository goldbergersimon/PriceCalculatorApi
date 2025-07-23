using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ItemController(ItemService itemService, SettingsService settingsService) : ControllerBase
{
    [HttpGet]
    public async Task<List<ItemListModel>> GetAllItems()
    {
        var items = await itemService.GetItemLists();
        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemModel>> GetItemDetails(int id)
    {
        var item = await itemService.GetItemDetails(id);

        if (item == null)
            return NotFound();

        return item;
    }


    [HttpPost]
    public async Task<ItemListModel> CreateItem([FromBody] ItemEditModel model) =>
        await itemService.CreateItem(model);

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemListModel>> UpdateItem(int id, [FromBody] ItemModel model)
    {
        if (id != model.ItemId) return BadRequest();

        try
        {
            var updated = await itemService.UpdateItem(model);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        bool success = await itemService.DeleteItem(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("calculate-ingredient")]
    public async Task<decimal> CalculateIngredient([FromBody] IiModel model)
    {
        decimal result = await itemService.calculateIngredientCost(model);

        return result;
    }

    [HttpPost("calculate-product")]
    public async Task<decimal> CalculateProduct([FromBody] IpModel model)
    {
        decimal result = await itemService.CalculateProduct(model);

        return result;
    }

    [HttpPost("calculate-profit")]
    public async Task<Sp> CalculateProfit([FromBody] MarginInput input)
    {
        Console.WriteLine($"margin and cost {input.Cost} {input.Margin}");
        var (selling, profit) = ItemService.CalculateSellingAndProfit(input.Cost, input.Margin);
        Console.WriteLine($"selling and profit {selling}{profit}");

        return await Task.FromResult(new Sp
        {
            Selling = selling,
            Profit = profit
        });
    }

    [HttpPost("calculate-margin-profit")]
    public async Task<Pm> CalculateMarginProfit([FromBody] SellingInput input)
    {
        var (profit, margin) = ItemService.CalculateMarginAndProfit(input.Cost, input.Selling);

        return await Task.FromResult(new Pm
        {
            Profit = profit,
            Margin = margin
        });
    }

    [HttpGet("get-office-expences")]
    public async Task<decimal> GetOfficeExpences() =>
          await settingsService.GetOfficeExpenses();

}