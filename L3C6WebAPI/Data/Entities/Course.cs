using System.ComponentModel.DataAnnotations;

namespace L3C6WebAPI.Data.Entities;

public class Course
{
    [Key]
    public int Id {get; set;}
    [StringLength(50)]
    public string Name { get; set; } = null! ;
    public int DurationYears {get; set;}
    
    public List<Module>? Modules {get; set;}
    
}