using OnlyPan.Models.ViewModels.RecipesViewModels;

namespace OnlyPan.Models.ViewModels.UserViewModels;

public class ReplicViewModel : RecipeViewModel
{
    public int IdReplic { get; set; }
    public string? Comentary { get; set; }
    public DateTime? DateReplicated { get; set; }
}