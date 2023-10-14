namespace OnlyPan.Models.Dtos.UserDtos;

public class RegisterDto : UserDto
{
    public string? ActivationToken { get; set; }
}