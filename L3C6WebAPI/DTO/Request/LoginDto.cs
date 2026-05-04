using System.ComponentModel.DataAnnotations;

namespace L3C6WebAPI.DTO.Request;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}