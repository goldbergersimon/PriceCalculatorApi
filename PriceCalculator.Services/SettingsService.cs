using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;

namespace PriceCalculatorApi.Services;

public class SettingsService(PriceCalculatorDbContext db, IMapper mapper)
{
    public async Task<List<Settings>> GetSettings()
    {
        return await db.Settings.ToListAsync();
    }

    public async Task SaveSettings(List<SettingsModel> settings)
    {
        var oldSettings = await db.Settings.ToListAsync();

        foreach (var setting in settings)
        { 
            var existing = oldSettings.FirstOrDefault(s => s.Key == setting.Key);

            mapper.Map(setting, existing);
        }
        await db.SaveChangesAsync();
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
