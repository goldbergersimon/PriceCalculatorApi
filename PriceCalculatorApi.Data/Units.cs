using System.Text.Json.Serialization;

namespace PriceCalculatorApi.Data;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Units
{
    None = 0,
    Cups,
    Tbs,
    Tsp,
    Pieces,
    Pounds,
    Oz
}