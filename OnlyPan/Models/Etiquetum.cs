using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Etiquetum
{
    public int IdEtiqueta { get; set; }

    public string? NombreEtiqueta { get; set; }

    public virtual ICollection<Recetum> Receta { get; set; } = new List<Recetum>();
}
