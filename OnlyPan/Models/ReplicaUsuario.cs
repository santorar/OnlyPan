﻿using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class ReplicaUsuario
{
    public int IdReplica { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdReceta { get; set; }

    public DateTime? FechaReplica { get; set; }

    public string? Comentario { get; set; }

    public virtual Recetum? IdRecetaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
