namespace OnlyPan.Models.Dtos.UserDtos;

public class UserDto
{
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Biography { get; set; }
    public int Rol { get; set; }
    public byte[]? Photo { get; set; }
}