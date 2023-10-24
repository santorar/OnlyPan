using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Unidad
{
    public int IdUnidad { get; set; }

    public string? NombreCorto { get; set; }

    public string? NombreLargo { get; set; }

    public virtual ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
}
