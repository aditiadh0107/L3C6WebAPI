using L3C6WebAPI.DTO.Request;
using L3C6WebAPI.Services.Interfaces;
using L3C6WebAPI.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace L3C6WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterInstructor(RegisterDto dto)
    {
        var result = await authService.RegisterInstructorAsync(dto);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }
        return Ok(new { Message = "Registered successfully. Please check your email to confirm your account." });
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);
        if (!result!.Success)
        {
            // Return 403 if email not confirmed, 401 for bad credentials
            if (result.Message != null && result.Message.Contains("Email not confirmed"))
                return StatusCode(403, result);
            return Unauthorized(result);
        }
        return Ok(result);
    }

    // GET /api/auth/confirm-email?userId=...&token=...
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string userId, [FromQuery] string token)
    {
        var success = await authService.ConfirmEmailAsync(userId, token);
        if (!success)
            return BadRequest(new { Message = "Invalid or expired confirmation link." });

        return Ok(new { Message = "Email confirmed successfully. You can now log in." });
    }
}
