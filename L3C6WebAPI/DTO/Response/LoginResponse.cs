namespace L3C6WebAPI.DTO.Response;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    
    public string? Token { get; set; }

}