namespace OnlyPan.Models.ViewModels.AdminViewModels;

public class RecipeModerateViewModel
{
    public int IdRecipe { get; set; }
    public string? Name { get; set; }
    public string? ChefName { get; set; }
    public string? State { get; set; }
    public DateTime? Date { get; set; }
}