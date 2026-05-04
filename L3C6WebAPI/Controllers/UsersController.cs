using System.Security.Claims;
using L3C6WebAPI.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace L3C6WebAPI.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(UserManager<Users> userManager, IWebHostEnvironment env) : ControllerBase
{
    
    [HttpPost("{id}/profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(string id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        
        var uploadsDir = Path.Combine(env.ContentRootPath, "Uploads", "ProfilePictures");
        Directory.CreateDirectory(uploadsDir);

       
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{id}{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);

        
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        
        user.ProfilePicturePath = filePath;
        await userManager.UpdateAsync(user);

        return Ok(new { Message = "Profile picture uploaded successfully." });
    }

    
    [HttpGet("{id}/profile-picture")]
    public async Task<IActionResult> GetProfilePicture(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        if (string.IsNullOrEmpty(user.ProfilePicturePath) || !System.IO.File.Exists(user.ProfilePicturePath))
            return NotFound("No profile picture found.");

        var extension = Path.GetExtension(user.ProfilePicturePath).ToLower();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        var fileBytes = await System.IO.File.ReadAllBytesAsync(user.ProfilePicturePath);
        return File(fileBytes, contentType, $"profile{extension}");
    }
}
