namespace OnlyPan.Models.Dtos;

public class CommentDto
{
    public int IdComment { get; set; }
    public int IdUser { get; set; }
    public int IdRecipe { get; set; }
    public string? UserName { get; set; }
    public string? Comment { get; set; }
}