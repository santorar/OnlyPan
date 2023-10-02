using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class ReplicaUsuario
{
    public int IdReplica { get; set; }

    public string Usuario { get; set; }

    public int Receta { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaConsulta { get; set; }

    public virtual Recetum RecetaNavigation { get; set; }

    public virtual Usuario UsuarioNavigation { get; set; }
}
