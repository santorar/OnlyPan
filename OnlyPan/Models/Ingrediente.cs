namespace OnlyPan.Models;

public class Ingrediente
{
  public int IdIngrediente { get; set; }

  public int? Receta { get; set; }

  public string? Ingrediente1 { get; set; }

  public virtual ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
}