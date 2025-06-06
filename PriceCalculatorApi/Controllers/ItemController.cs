using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(ItemService itemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ItemListModel>> GetAllItems()
        {
            var items = await itemService.GetItemLists();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemModel>> GetItem(int id)
        {
            var items = await itemService.GetItem(id);
            if (items == null) 
                return NotFound();
            return items;
        }
    }
}
