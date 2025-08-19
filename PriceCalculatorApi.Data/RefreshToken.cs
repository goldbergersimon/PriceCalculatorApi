using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PriceCalculatorApi.Data;

public class RefreshToken
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string Username { get; set; } = null!;
    [MaxLength(60)]
    public string Token { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
    public DateTime? ExpireyDate { get; set; }
}
