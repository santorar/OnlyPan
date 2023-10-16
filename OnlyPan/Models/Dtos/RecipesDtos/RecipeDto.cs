namespace OnlyPan.Models.Dtos;

public class RecipeDto
{
    public int IdRecipe { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int IdCategory { get; set; }
    public string? Category { get; set; }
    public int IdTag { get; set; }
    public string? Tag { get; set; }
    public List<string>? Ingredients { get; set; }
    public List<int>? IdsIngredients { get; set; }
    public List<int>? IngredientsQuantity { get; set; }
    public List<string>? IngredientsUnit { get; set; }
    public string? Instructions { get; set; }
    public byte[]? Photo { get; set; }
    public DateTime? Date { get; set; }
    public int IdUser { get; set; }
    public string? Chef { get; set; }
    public List<CommentDto>? CommentsDto { get; set; }
}