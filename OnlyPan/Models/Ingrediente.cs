using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string? NombreIngrediente { get; set; }

    public string? DescripcionIngrediente { get; set; }

    public virtual ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
}
