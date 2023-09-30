using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Etiquetum
{
    public int IdEtiqueta { get; set; }

    public int? Receta { get; set; }

    public string? Etiqueta { get; set; }

    public virtual ICollection<Recetum> RecetaNavigation { get; set; } = new List<Recetum>();
}
