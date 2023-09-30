namespace OnlyPan.Models;

public class RecetaChef
{
  public int IdActuacion { get; set; }

  public int? Chef { get; set; }

  public int? Receta { get; set; }

  public DateTime? FechaActualizacion { get; set; }

  public virtual Usuario? ChefNavigation { get; set; }

  public virtual Recetum? RecetaNavigation { get; set; }
}