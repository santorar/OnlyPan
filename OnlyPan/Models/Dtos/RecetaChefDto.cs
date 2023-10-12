using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class RecetaChefDto
{
    public int IdReceta { get; set; }

    public int IdChef { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Usuario IdChefNavigation { get; set; } = null!;

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;
}
