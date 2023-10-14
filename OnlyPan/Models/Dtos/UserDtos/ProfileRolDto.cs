namespace OnlyPan.Models.Dtos.UserDtos;

public class ProfileRolDto : UserDto
{
    public int IdNewRol { get; set; }
    
    public string? CurrentRol { get; set; }
    
    public string? NewRol { get; set; }
    
    public int Followers { get; set; }
    
    public int Followed { get; set; }
}