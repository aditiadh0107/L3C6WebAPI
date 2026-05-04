using System.ComponentModel.DataAnnotations;

namespace L3C6WebAPI.Data.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    [StringLength(20)]
    public string Phone { get; set; } = null!;
    [StringLength(100)]
    public string Email { get; set; } = null!;
    [StringLength(200)]
    public string? Address { get; set; }
}