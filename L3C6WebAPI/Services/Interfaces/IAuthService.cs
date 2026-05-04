using L3C6WebAPI.DTO.Request;
using L3C6WebAPI.DTO.Response;
using Microsoft.AspNetCore.Identity;

namespace L3C6WebAPI.Services.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> RegisterInstructorAsync(RegisterDto dto);
    Task<LoginResponse?> LoginAsync(LoginDto dto);
    Task<bool> ConfirmEmailAsync(string userId, string token);
}