using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<List<ProductListModel>> GetAllProducts()
    {
        var pruducts = await productService.GetProductList();
        return pruducts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProductDetails(int id)
    {
        var product = await productService.GetProductDetails(id);

        if (product == null)
            return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ProductListModel> CreateProduct([FromBody] ProductEditModel model) =>
        await productService.CreateProduct(model);

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductListModel>> UpdateProduct(int id, [FromBody] ProductModel model)
    {
        if (id != model.ProductId) return BadRequest();

        try
        {
            var updated = await productService.UpdateProduct(model);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            bool success = await productService.DeleteProduct(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("calculate-ingredient")]
    public async Task<decimal> CalculateIngredient([FromBody] PiModel model)
    {
        decimal result = await productService.calculateIngredientCost(model);

        return result;
    }

    [HttpPost("calculate-labor")]
    public TimeSpan CalculateLabor([FromBody] PlModel model)
    {
        TimeSpan total = productService.CalculateTotaltime(model);
        return total;
    }

    [HttpPost("calculate-total-ingredient-cost")]
    public decimal CalculateTotalIngredientCost([FromBody] List<decimal> totals)
    {
        return totals.Sum();
    }

    [HttpPost("calculate-total-labor-cost")]
    public async Task<decimal> CalculateTotalLaborCost([FromBody] List<TlModel> models)
    {
        decimal total = await productService.CalculateTotalLaborCost(models);
        return total;
    }

}
