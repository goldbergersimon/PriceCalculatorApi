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
public class LoginController(IConfiguration config, PriceCalculatorDbContext db, ILogger<LoginController> logger) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        string username = model.Username.ToLower();
        //var user = AuthenticateUser(model);
        if (username == "admin" && model.Password == "pass@123")
        {
            var accessToken = GenerateJwtToken(username);
            var refreshToken = GenerateRefreshToken();

            await SaveRefreshToken(username, refreshToken, model.DeviceId);

            logger.LogInformation("User {Username} logged in successfully.", username);

            return Ok(new { accessToken, refreshToken });
        }

        logger.LogWarning("Invalid login attempt for user {Username}.", username);

        return BadRequest(new {message = "Invalid username or password"});
    }

    private async Task SaveRefreshToken(string username, string refreshToken, string deviceId) 
    {
        logger.LogDebug("Saving refresh token for {Username} on device {DeviceId}", username, deviceId);
        var existingToken = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Username == username && rt.DeviceId == deviceId);
        if (existingToken != null)
        {
            logger.LogDebug("Updating existing refresh token for {Username} on device {DeviceId}", username, deviceId);
            existingToken.Token = refreshToken;
            existingToken.ExpireyDate = DateTime.UtcNow.AddDays(30);
        }
        else
        {
            logger.LogDebug("Creating new refresh token for {Username} on device {DeviceId}", username, deviceId);
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
        logger.LogInformation("Refresh request received for device {DeviceId}", model.DeviceId);
        var username = GetUsernameFromExpiredToken(model.AccessToken);

        if (username == null)
        {
            logger.LogWarning("Refresh failed: could not extract username from access token for device {DeviceId}", model.DeviceId);
            return Unauthorized();
        }

        if (!await ValidateRefreshToken(username, model.RefreshToken, model.DeviceId))
        {
            logger.LogWarning("Refresh failed: invalid refresh token for user {Username}, device {DeviceId}", username, model.DeviceId);
            return Unauthorized();
        }

        var newAccessToken = GenerateJwtToken(username);
        var newRefreshToken = GenerateRefreshToken();
        await SaveRefreshToken(username, newRefreshToken, model.DeviceId);

        logger.LogInformation("Refresh successful for user {Username}, device {DeviceId}", username, model.DeviceId);

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
        catch (Exception ex) 
        {
            logger.LogError(ex, "Error validating expired token.");
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
        return Convert.ToBase64String(randomNumber)
                 .Replace("+", "-")
                 .Replace("/", "_")
                 .Replace("=", "");
    }
}
