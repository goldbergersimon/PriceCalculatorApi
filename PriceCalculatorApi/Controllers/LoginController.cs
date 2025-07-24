using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PriceCalculatorApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var refreshToken = GenerateRefrefhToken();
            return Ok(new { token, refreshToken });
        }
    
            return Unauthorized();
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] TokenRefreshRequest model)
    {
        var principal = GetPrincipalFromExpiredtoken(model.AccessToken);
        var username = principal.Identity?.Name;

        if (string.IsNullOrEmpty(model.RefreshToken) || username == null)
        return Unauthorized();

        var newAccessToken = GenerateJwtToken(username);
        var newRefreshToken = GenerateRefrefhToken();

        return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
    }

    private ClaimsPrincipal GetPrincipalFromExpiredtoken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]));
        var tokenvalidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var princepal = tokenHandler.ValidateToken(token, tokenvalidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return princepal;
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
    private string GenerateRefrefhToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
