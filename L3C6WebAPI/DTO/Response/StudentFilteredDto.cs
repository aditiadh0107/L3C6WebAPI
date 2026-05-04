namespace L3C6WebAPI.DTO.Response;

public class StudentFilteredDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string? Address { get; set; }
}