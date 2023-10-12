using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class SeguirUsuario
{
    public int IdSeguidor { get; set; }

    public int IdSeguido { get; set; }

    public DateTime? FechaSeguimiento { get; set; }

    public virtual Usuario IdSeguidoNavigation { get; set; } = null!;

    public virtual Usuario IdSeguidorNavigation { get; set; } = null!;
}
