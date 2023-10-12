using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? NombreEstado { get; set; }

    public string? DescripcionEstado { get; set; }

    public virtual ICollection<Recetum> Receta { get; set; } = new List<Recetum>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
    public virtual ICollection<SolicitudRol> SolicitudRols { get; set; } = new List<SolicitudRol>();
}
