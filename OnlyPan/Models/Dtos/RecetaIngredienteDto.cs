using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class RecetaIngredienteDto
{
    public int IdReceta { get; set; }

    public int IdIngrediente { get; set; }

    public int? Cantidad { get; set; }

    public virtual Ingrediente IdIngredienteNavigation { get; set; } = null!;

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;
}
