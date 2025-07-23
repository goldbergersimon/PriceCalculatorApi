using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;
using PriceCalculatorApi.Services;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class SettingsController(SettingsService settingsService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Settings>> GetSettings()
    {
        var settings = await settingsService.GetSettings();

        return settings;
    }

    [HttpPost]
    public async Task SaveSettings( [FromBody] List<SettingsModel> settings)
    {
        await settingsService.SaveSettings(settings);
    }
}
