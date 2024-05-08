using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace security {
public static class JwtUtils
{
    // Generate JWT Token
    public static string GenerateJwtToken(string username, IConfiguration configuration)
    {
        var secret = configuration["JwtConfig:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var expirationMinutes = configuration["JwtConfig:AccessTokenExpirationMinutes"];
        var expirationTime = DateTime.Now.AddMinutes(double.Parse(expirationMinutes ?? "0"));
        var token = new JwtSecurityToken(
            issuer: configuration["JwtConfig:Issuer"],
            audience: configuration["JwtConfig:Audience"],
            claims: claims,
            expires: expirationTime,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Optionally, add more methods related to JWT here, such as validation or parsing
}
}
