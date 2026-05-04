using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using L3C6WebAPI.Data.Entities;
using L3C6WebAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace L3C6WebAPI.Services;

public class JwtService(IConfiguration  config) 
{
    public string GenerateToken()
    {
        var jwtOptions = config.GetSection("Jwt");
        var secretKey = jwtOptions["SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var tokenObj = new JwtSecurityToken(
            issuer: jwtOptions["Issuer"],
             audience: jwtOptions["Audience"],
            signingCredentials: signingCredentials,
            claims: [],
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtOptions["ExpiryInMinutes"]))
            );
        
        var token = new JwtSecurityTokenHandler().WriteToken(tokenObj);
        return token;
    }
}