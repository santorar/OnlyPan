namespace OnlyPan.Models.Dtos.UserDtos;

public class ProfileRolDto
{
    public int IdUser { get; set; }
    public int IdNewRol { get; set; }
    public string? CurrentRol { get; set; }
    
    public string? NewRol { get; set; }
    
    public string? Name { get; set; }
    
    public string? Email { get; set; }
    
    public string? Biography { get; set; }
    
    public byte[]? Photo { get; set; }
    
    public int Followers { get; set; }
    
    public int Followed { get; set; }
}