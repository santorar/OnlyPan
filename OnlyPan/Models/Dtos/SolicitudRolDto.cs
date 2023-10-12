﻿using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class SolicitudRolDto
{
    public int IdUsuarioSolicitud { get; set; }

    public int IdRolSolicitud { get; set; }
    public int IdEstado { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaAprovacion { get; set; }

    public int? IdUsuarioAprovador { get; set; }

    public virtual Rol IdRolSolicitudNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAprovadorNavigation { get; set; }

    public virtual Usuario IdUsuarioSolicitudNavigation { get; set; } = null!;
    public virtual Estado IdEstadoNavigation { get; set; } = null!;
}
