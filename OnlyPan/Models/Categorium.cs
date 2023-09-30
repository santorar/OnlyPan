using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public int? Receta { get; set; }

    public string? Categoria { get; set; }

    public virtual ICollection<Recetum> RecetaNavigation { get; set; } = new List<Recetum>();
}
