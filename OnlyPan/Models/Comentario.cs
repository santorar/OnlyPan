using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Comentario
{
    public int IdUsuario { get; set; }

    public int IdReceta { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Comentario1 { get; set; }

    public int? Estado { get; set; }

    public int IdComentario { get; set; }

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
