using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceCalculatorApi.Data;
using PriceCalculatorApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PriceCalculatorApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(IConfiguration config, PriceCalculatorDbContext db) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        //var user = AuthenticateUser(model);
        if (model.Username == "admin" && model.Password == "sG@2114")
        {
            var accessToken = GenerateJwtToken(model.Username);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshToken(model.Username, refreshToken, model.DeviceId);

            return Ok(new { accessToken, refreshToken });
        }
    
            return BadRequest(new {message = "Invalid username or password"});
    }

    private async Task SaveRefreshToken(string username, string refreshToken, string deviceId) 
    {
        var existingToken = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Username == username && rt.DeviceId == deviceId);
        if (existingToken != null)
        {
            existingToken.Token = refreshToken;
            existingToken.ExpireyDate = DateTime.UtcNow.AddDays(30);
        }
        else
        {
            await db.RefreshTokens.AddAsync(new RefreshToken
            {
                Username = username,
                Token = refreshToken,
                DeviceId = deviceId,
                ExpireyDate = DateTime.UtcNow.AddDays(30)
            });
        }
        var expiredTokens = db.RefreshTokens.Where(rt => rt.Username == username && rt.ExpireyDate < DateTime.UtcNow);
        db.RefreshTokens.RemoveRange(expiredTokens);
        await db.SaveChangesAsync();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest model)
    {
        var username = GetUsernameFromExpiredToken(model.AccessToken);

        if (username == null || !await ValidateRefreshToken(username, model.RefreshToken, model.DeviceId))
            return Unauthorized();

        var newAccessToken = GenerateJwtToken(username);
        var newRefreshToken = GenerateRefreshToken();
        await SaveRefreshToken(username, newRefreshToken, model.DeviceId);

        return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
    }

    private async Task<bool> ValidateRefreshToken(string username, string refreshToken, string deviceId)
    {
        var storedtoken = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Username == username &&
                                                                           rt.Token == refreshToken &&
                                                                           rt.DeviceId == deviceId &&
                                                                           rt.ExpireyDate > DateTime.UtcNow);

        return storedtoken != null;
    }

    private string? GetUsernameFromExpiredToken(string token)
    {
        try
        {
            var jwtSettings = config.GetSection("Jwt");
            var tokenvalidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!))
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var princepal = tokenhandler.ValidateToken(token, tokenvalidationParameters, out var validatedToken);

            var username = princepal?.Identity?.Name;
            return username;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: [new Claim(ClaimTypes.Name, username)],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
