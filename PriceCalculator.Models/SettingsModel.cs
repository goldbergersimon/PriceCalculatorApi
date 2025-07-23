using PriceCalculatorApi.Data;

namespace PriceCalculatorApi.Models;

public class SettingsModel
{
    public SettingKeys Key { get; set; }
    public string Value { get; set; }
}
