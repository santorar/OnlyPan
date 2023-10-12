﻿using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class RolDto
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<SolicitudRol> SolicitudRols { get; set; } = new List<SolicitudRol>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
