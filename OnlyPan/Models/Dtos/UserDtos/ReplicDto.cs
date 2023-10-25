namespace OnlyPan.Models.Dtos.UserDtos;

public class ReplicDto : RecipeDto
{
    public int IdReplic { get; set; }
    public string? Comentary { get; set; }
    public DateTime? DateReplicated { get; set; }
}