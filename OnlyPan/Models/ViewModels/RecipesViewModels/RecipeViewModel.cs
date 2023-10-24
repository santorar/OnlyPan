using OnlyPan.Models.Dtos;

namespace OnlyPan.Models.ViewModels.RecipesViewModels;

public class RecipeViewModel
{
    public int IdRecipe { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double Rating { get; set; }
    public string? Category { get; set; }
    public string? Tag { get; set; }
    public List<string>? Ingredients { get; set; }
    public string? Instructions { get; set; }
    public List<byte[]>? Photos { get; set; }
    public DateTime? Date { get; set; }
    public int ChefId { get; set; }
    public string? Chef { get; set; }
    public List<CommentDto>? Comments { get; set; }
    public int PersonalRating { get; set; }
}