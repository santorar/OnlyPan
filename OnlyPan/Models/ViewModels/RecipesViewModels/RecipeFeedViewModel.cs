namespace OnlyPan.Models.ViewModels.RecipesViewModels;

public class RecipeFeedViewModel
{
    public int IdRecipe { get; set; }
    public string? Name { get; set; }
    public double Rating { get; set; }
    public string? Description { get; set; }
    public byte[]? Photo { get; set; }
}