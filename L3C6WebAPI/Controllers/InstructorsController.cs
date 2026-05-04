using L3C6WebAPI.DTO;
using L3C6WebAPI.Services.Interfaces;
using L3C6WebAPI.Data;
using L3C6WebAPI.Data.Entities;
using L3C6WebAPI.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace L3C6WebAPI.Controllers;
 
[Route("api/[controller]")]
[ApiController]
public class InstructorsController(IInstructorService instructorService) : ControllerBase
{
 
    // POST /api/instructors
    [HttpPost]
    public async Task<IActionResult> AddInstructor(CreateInstructorDto instructorDto)
    {
       var response =  await instructorService.AddInstructorAsync(instructorDto);
       return Ok(response);
    }
    

}
 