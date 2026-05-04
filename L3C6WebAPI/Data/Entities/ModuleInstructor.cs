using System.ComponentModel.DataAnnotations.Schema;

namespace L3C6WebAPI.Data.Entities;

public class ModuleInstructor
{
    [ForeignKey("Module")]
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    [ForeignKey("Instructor")]
    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; } 
    
}
