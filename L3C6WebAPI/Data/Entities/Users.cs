using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace L3C6WebAPI.Data.Entities;

[Table("Users")]
public class Users : IdentityUser
{
    
    
    [Required, StringLength(50)] 
    public string FirstName { get; set; } = null!;
    
    [Required, StringLength(50)]
    public string LastName { get; set; } = null!;

    public string? Address { get; set; }

    [StringLength(500)]
    public string? ProfilePicturePath { get; set; }
}