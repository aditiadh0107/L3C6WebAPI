using L3C6WebAPI.Data.Entities;

namespace L3C6WebAPI.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken( IList<string> roles);
    DateTime GetExpiry();
}