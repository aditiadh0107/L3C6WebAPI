using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using L3C6WebAPI.Services.Interfaces;
using L3C6WebAPI.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace L3C6WebAPI.Services.Implementation;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(IList<string> roles)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: GetExpiry(),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpiry()
    {
        var minutes = double.Parse(configuration["Jwt:ExpiryInMinutes"]!);
        return DateTime.UtcNow.AddMinutes(minutes);
    }
}