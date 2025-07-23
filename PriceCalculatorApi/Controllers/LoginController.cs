using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PriceCalculatorApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        //var user = AuthenticateUser(model);
        if (model.Username == "admin" && model.Password == "12345")
        {
            var token = GenerateJwtToken(model.Username);
            return Ok(new { token });
        }
    
            return Unauthorized();
    }
    

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: [new Claim(ClaimTypes.Name, username)],
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
