using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Comentario
{
    public int IdInteraccion { get; set; }

    public string Usuario { get; set; }

    public int Receta { get; set; }

    public DateTime? FechaInteracion { get; set; }

    public string Comentario1 { get; set; }

    public int Estado { get; set; }

    public virtual Estado EstadoNavigation { get; set; }

    public virtual Recetum RecetaNavigation { get; set; }

    public virtual Usuario UsuarioNavigation { get; set; }
}
