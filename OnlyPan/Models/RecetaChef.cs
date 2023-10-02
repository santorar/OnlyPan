using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class RecetaChef
{
    public int IdActuacion { get; set; }

    public string Chef { get; set; }

    public int Receta { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual Usuario ChefNavigation { get; set; }

    public virtual Recetum RecetaNavigation { get; set; }
}
