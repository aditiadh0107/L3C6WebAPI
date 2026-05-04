using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L3C6WebAPI.Data.Entities;

public class Module
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string Title { get; set; } = null!;
    public int Credits { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public List<ModuleInstructor> ModuleInstructors { get; set; } = [];
}