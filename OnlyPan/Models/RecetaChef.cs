using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class RecetaChef
{
    public int IdReceta { get; set; }

    public int IdChef { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Usuario IdChefNavigation { get; set; } = null!;

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;
}
