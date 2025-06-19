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
        var pruducts = await productService.GetProductList();
        return pruducts;
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<ProductListModel>> GetProduct(int id)
    //{
    //    var pruduct = await productService.GetProduct(id);
    //    if (pruduct == null)
    //        return NotFound();

    //    return pruduct;
    //}

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
}
