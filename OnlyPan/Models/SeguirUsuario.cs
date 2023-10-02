using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public class SeguirUsuario
{
    public int IdSeguir { get; set; }

    public string Seguido { get; set; }

    public string Seguidor { get; set; }

    public DateTime FechaSeguimiento { get; set; }

    public int SeguidoresChef { get; set; }

    public int Estado { get; set; }

    public virtual Estado EstadoNavigation { get; set; }

    public virtual Usuario SeguidoNavigation { get; set; }

    public virtual Usuario SeguidorNavigation { get; set; }
}
