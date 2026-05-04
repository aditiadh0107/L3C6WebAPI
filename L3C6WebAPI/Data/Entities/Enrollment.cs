using System.ComponentModel.DataAnnotations.Schema;

namespace L3C6WebAPI.Data.Entities;

public class Enrollment
{
    [ForeignKey("Student")]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [ForeignKey("Course")]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime EnrolledDate { get; set; }
}