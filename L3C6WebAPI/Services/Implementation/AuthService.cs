using L3C6WebAPI.Data;
using L3C6WebAPI.Data.Entities;
using L3C6WebAPI.DTO.Request;
using L3C6WebAPI.DTO.Response;
using L3C6WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace L3C6WebAPI.Services.Implementation;

public class AuthService(
    UserManager<Users> userManager,
    JwtService jwtService,
    SignInManager<Users> signInManager,
    AppDbContext dbContext,
    IEmailService emailService,
    IConfiguration configuration
    ) : IAuthService
{
    public async Task<IdentityResult> RegisterInstructorAsync(RegisterDto dto)
    {
        
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var user = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

          
            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return result;
            }

         
            var roleResult = await userManager.AddToRoleAsync(user, "Instructor");
            if (!roleResult.Succeeded)
            {
                
                await transaction.RollbackAsync();
                return roleResult;
            }

            
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var appUrl = configuration["AppUrl"];
            var confirmUrl = $"{appUrl}/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            var emailBody = $@"
                <h2>Confirm Your Email</h2>
                <p>Hi {user.FirstName},</p>
                <p>Please confirm your email by clicking the link below:</p>
                <a href='{confirmUrl}'>Confirm Email</a>";

            await emailService.SendEmailAsync(user.Email!, "Confirm Your Email", emailBody);

         
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid Email or Password"
            };
        }

        // Reject login if email is not confirmed
        if (!user.EmailConfirmed)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Email not confirmed. Please check your inbox and confirm your email before logging in."
            };
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!signInResult.Succeeded)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid Email or Password"
            };
        }

        var roles = await userManager.GetRolesAsync(user);
        
        return new LoginResponse
        {
            Success = true,
            Message = signInResult.ToString(),
            Token = jwtService.GenerateToken(roles)
        };
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }
}
