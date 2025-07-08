using System.Text.Json.Serialization;

namespace PriceCalculatorApi.Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemUnits
{
    None = 0,
    Oz,
    Containers,
    Pieces
}