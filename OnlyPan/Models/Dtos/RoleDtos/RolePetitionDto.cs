namespace OnlyPan.Models.Dtos.RoleDtos;

public class RolePetitionDto
{
    public int IdUser { get; set; }
    public string? UserName { get; set; }   
    public string? CurrentRoleName { get; set; }
    public int IdRequesedRole { get; set; }
    public string? RequesedRoleName { get; set; }
    public DateTime? Time { get; set; }
}