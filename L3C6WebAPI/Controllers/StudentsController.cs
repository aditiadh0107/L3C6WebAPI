using L3C6WebAPI.Data;
using L3C6WebAPI.Data.Entities;
using L3C6WebAPI.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L3C6WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(AppDbContext dbContext) : ControllerBase
{
 
    [HttpGet("filtered")]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] int? age,
        [FromQuery] string? address,
        [FromQuery] bool useQueryable = true)
    {
        if (!useQueryable)
        {
            var allStudents = await dbContext.Students.ToListAsync();

            var filtered = allStudents.AsEnumerable();

            if (age.HasValue)
            {
                var cutoffDate = DateTime.UtcNow.AddYears(-age.Value);
                filtered = filtered.Where(s =>
                    s.DateOfBirth.Year <= cutoffDate.Year);
            }

            if (!string.IsNullOrEmpty(address))
            {
                filtered = filtered.Where(s =>
                    s.Address != null && s.Address.Contains(address, StringComparison.OrdinalIgnoreCase));
            }

            var result = filtered.Select(s => new StudentFilteredDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Age = DateTime.UtcNow.Year - s.DateOfBirth.Year,
                Address = s.Address
            }).ToList();

            return Ok(result);
        }
        else
        {
            IQueryable<Student> query = dbContext.Students;

            if (age.HasValue)
            {
                var cutoffDate = DateTime.UtcNow.AddYears(-age.Value);
                query = query.Where(s => s.DateOfBirth.Year <= cutoffDate.Year);
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(s =>
                    s.Address != null && EF.Functions.ILike(s.Address, $"%{address}%"));
            }
            
            var result = await query.Select(s => new StudentFilteredDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Age = DateTime.UtcNow.Year - s.DateOfBirth.Year,
                Address = s.Address
            }).ToListAsync();

            return Ok(result);
        }
    }
}