namespace OnlyPan.Models.ViewModels.RolViewModels;

public class RolePetitionViewModel
{
    
    public int IdUser { get; set; }
    public int IdRequesedRole { get; set; }
    public string? UserName { get; set; }   
    public string? CurrentRoleName { get; set; }
    public string? RequesedRoleName { get; set; }
    public DateTime? Time { get; set; }
}