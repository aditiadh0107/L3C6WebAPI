using L3C6WebAPI.DTO;
using L3C6WebAPI.Data.Entities;

namespace L3C6WebAPI.Services.Interfaces;

public interface IInstructorService
{
    public Task<string> AddInstructorAsync(CreateInstructorDto instructorDto);
}