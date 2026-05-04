using L3C6WebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace L3C6WebAPI.Data;

public class AppDbContext : IdentityDbContext<Users>

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ModuleInstructor> ModuleInstructors { get; set; }

    protected override void OnModelCreating(ModelBuilder Builder)
    {
        base.OnModelCreating(Builder);
        
        Builder.Entity<Users>().ToTable("Users");
        Builder.Entity<IdentityRole>().ToTable("Roles");
        Builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

        Builder.Entity<Enrollment>().HasKey(e => new { e.StudentId, e.CourseId });
        Builder.Entity<ModuleInstructor>().HasKey(mi => new { mi.ModuleId, mi.InstructorId });
        
        SeedRoles(Builder);
    }

    public void SeedRoles(ModelBuilder builder)
    {
        List<IdentityRole> roles =
        [
            new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }, new IdentityRole{
                Id = Guid.NewGuid().ToString(),
                Name = "Instructor",
                NormalizedName = "INSTRUCTOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }, new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Student",
                NormalizedName = "STUDENT",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        ];
        
        builder.Entity<IdentityRole>().HasData(roles);
    }
    
}