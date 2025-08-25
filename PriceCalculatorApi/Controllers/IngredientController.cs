using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class IngredientController(IngredientService ingredientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IngredientModel>> GetAllIngredients()
    {
        var ingredients = await ingredientService.GetIngredientList();
        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientModel>> GetIngredient(int id)
    {
        var ingredient = await ingredientService.GetIngredient(id);
        if (ingredient == null) 
            return NotFound();
        return ingredient;
    }

    [HttpPost]
    public async Task<IngredientModel?> CreateIngredient([FromBody] IngredientEditModel model) =>
          await ingredientService.AddIngredient(model);


    [HttpPut("{id}")]
    public async Task<ActionResult<IngredientModel>> UpdateIngredient(int id, [FromBody] IngredientModel model)
    {
        if (id != model.IngredientId)
            return BadRequest("Id mismatch");

        var updated = await ingredientService.UpdateIngredient(id, model);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        try
        {
            bool success = await ingredientService.DeleteIngredient(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch(InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
