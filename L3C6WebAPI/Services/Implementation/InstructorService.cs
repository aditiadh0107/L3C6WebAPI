using L3C6WebAPI.Data;
using L3C6WebAPI.Data.Entities;
using L3C6WebAPI.DTO;
using L3C6WebAPI.Services.Interfaces;

namespace L3C6WebAPI.Services.Implementation;

public class InstructorService(AppDbContext dbContext) : IInstructorService
{
    public async Task<string> AddInstructorAsync(CreateInstructorDto instructorDto)
    {
        
        Instructor instructor = new Instructor
        {
            FirstName = instructorDto.FirstName,
            LastName = instructorDto.LastName,
            Email = instructorDto.Email,
            HireDate = instructorDto.HireDate
        };
        
        dbContext.Instructors.Add(instructor);
        await dbContext.SaveChangesAsync();
        return  "Successfully added Instructor";
    }
}