namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ProfileRolViewModel
{
    public int IdUser { get; set; }
    
    public int IdRolNew { get; set; }
    
    public string? CurrentRol { get; set; }
    
    public string? NewRol { get; set; }
    
    public string? Name { get; set; }
    
    public string? Email { get; set; }
    
    public string? Biography { get; set; }
    
    public int Followers { get; set; }
    public int Followed { get; set; }
    public byte[]? Photo { get; set; }
}