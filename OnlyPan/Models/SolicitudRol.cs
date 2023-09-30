using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class SolicitudRol
{
    public int IdSolicitud { get; set; }

    public int? UsuarioSolicitud { get; set; }

    public int? RolSolicitado { get; set; }

    public int? EstadoSolicitud { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaAprovacion { get; set; }

    public int? UsuarioAprovador { get; set; }

    public virtual Estado? EstadoSolicitudNavigation { get; set; }

    public virtual Rol? RolSolicitadoNavigation { get; set; }

    public virtual Usuario? UsuarioAprovadorNavigation { get; set; }

    public virtual Usuario? UsuarioSolicitudNavigation { get; set; }
}
