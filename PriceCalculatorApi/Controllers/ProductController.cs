using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<List<ProductListModel>> GetAllProducts()
    {
        var pruducts = await productService.GetProductLists();
        return pruducts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductModel>> GetProduct(int id)
    {
        var pruduct = await productService.GetProduct(id);
        if (pruduct == null)
            return NotFound();

        return pruduct;
    }
}
