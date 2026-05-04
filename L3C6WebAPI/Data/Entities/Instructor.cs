using System.ComponentModel.DataAnnotations;

namespace L3C6WebAPI.Data.Entities;

public class Instructor
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    [StringLength(100)]
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }

    public ICollection<ModuleInstructor> Modules { get; set; } = [];
}