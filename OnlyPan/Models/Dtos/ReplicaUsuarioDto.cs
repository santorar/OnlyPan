using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class ReplicaUsuarioDto
{
    public int IdReplica { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdReceta { get; set; }

    public DateTime? FechaReplica { get; set; }

    public string? Comentario { get; set; }

    public virtual Recetum IdReplicaNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
