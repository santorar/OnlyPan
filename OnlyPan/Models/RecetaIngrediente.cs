using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class RecetaIngrediente
{
    public int IdLista { get; set; }

    public int? Ingrediente { get; set; }

    public decimal? Cantidad { get; set; }

    public virtual Ingrediente? IngredienteNavigation { get; set; }

    public virtual ICollection<Recetum> Receta { get; set; } = new List<Recetum>();
}
