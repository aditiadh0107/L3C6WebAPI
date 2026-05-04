using L3C6WebAPI.Data;
using L3C6WebAPI.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace L3C6WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext dbContext;
    private readonly IMemoryCache _cache;
    private const string CoursesCacheKey = "all_courses";

    public CoursesController(AppDbContext dbContext, IMemoryCache cache)
    {
        this.dbContext = dbContext;
        _cache = cache;
    }

    // GET /api/courses — cached for 5 minutes
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Try to get data from cache
        if (_cache.TryGetValue(CoursesCacheKey, out List<object>? cachedCourses))
        {
            return Ok(cachedCourses);
        }

        // Cache miss — fetch from database
        var courses = await dbContext.Courses
            .Select(c => (object)new { c.Id, c.Name, ModuleCount = c.Modules.Count })
            .ToListAsync();

        _cache.Set(CoursesCacheKey, courses, TimeSpan.FromMinutes(5));

        return Ok(courses);
    }

    // GET /api/courses/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var course = dbContext.Courses
            .Include(c => c.Modules)
            .FirstOrDefault(c => c.Id == id);
        if (course == null) return NotFound();
        return Ok(course);
    }
    

    // POST /api/courses
    [HttpPost]
    public IActionResult AddCourse(Course course)
    {
        dbContext.Courses.Add(course);
        dbContext.SaveChanges();
        return Ok("Successfully Created!");
    }

    // POST /api/courses/{id}/modules
    [HttpPost("{id}/modules")]
    public IActionResult AddModule(int id, Module module)
    {
        var course = dbContext.Courses.Find(id);
        if (course == null) return NotFound();
        module.CourseId = id;
        dbContext.Modules.Add(module);
        dbContext.SaveChanges();
        return Ok("Module Added!");
    }

    // PUT /api/courses/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateCourse(int id, Course updated)
    {
        var course = dbContext.Courses.Include(c => c.Modules).FirstOrDefault(c => c.Id == id);
        if (course == null) return NotFound();
        course.Name = updated.Name;
        dbContext.Modules.RemoveRange(course.Modules);
        course.Modules = updated.Modules;
        dbContext.SaveChanges();
        return Ok("Successfully Updated!");
    }

    // DELETE /api/courses/{id} — requires Admin role
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteCourse(int id)
    {
        var course = dbContext.Courses.Find(id);
        if (course == null) return NotFound();
        dbContext.Courses.Remove(course);
        dbContext.SaveChanges();
        return Ok("Successfully Deleted!");
    }

    // POST /api/courses/bulk
    [HttpPost("bulk")]
    public IActionResult BulkInsert(List<Course> courses)
    {
        dbContext.Courses.AddRange(courses);
        dbContext.SaveChanges();
        return Ok($"{courses.Count} Courses Created!");
    }
    

    // GET /api/courses/count
    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await dbContext.Courses.CountAsync();
        return Ok(new { CourseCount = count});
    }

    // GET /api/courses/total-credits
    [HttpGet("total-credits")]
    public IActionResult TotalCredits()
    {
        var total = dbContext.Modules.Sum(m => m.Credits);
        return Ok(total);
    }
    
}