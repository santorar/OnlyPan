namespace OnlyPan.Models.Dtos;

public class RecipeFeedDto
{
    public int IdRecipe { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[]? Photo { get; set; }
}