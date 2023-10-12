using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class ValoracionDto
{
    public int IdUsuario { get; set; }

    public int IdReceta { get; set; }

    public DateTime? FechaInteracion { get; set; }

    public int? Valoration { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
