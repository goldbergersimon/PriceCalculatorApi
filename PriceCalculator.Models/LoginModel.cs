using System.ComponentModel.DataAnnotations;

namespace PriceCalculatorApi.Models;

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; }
}