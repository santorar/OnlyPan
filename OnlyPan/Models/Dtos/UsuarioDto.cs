using System;
using System.Collections.Generic;

namespace OnlyPan.Models.Dtos;

public class UsuarioDto
{
    public int IdUsuario { get; set; }

    public int Rol { get; set; }

    public DateTime? FechaInscrito { get; set; }

    public byte[]? Foto { get; set; }

    public string? Biografia { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Estado { get; set; }

    public string Contrasena { get; set; } = null!;

    public string? CodigoActivacion { get; set; }

    public bool? Activo { get; set; }

    public string? ContrasenaToken { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Estado EstadoNavigation { get; set; } = null!;

    public virtual ICollection<RecetaChef> RecetaChefs { get; set; } = new List<RecetaChef>();

    public virtual ICollection<ReplicaUsuario> ReplicaUsuarios { get; set; } = new List<ReplicaUsuario>();

    public virtual Rol RolNavigation { get; set; } = null!;

    public virtual ICollection<SeguirUsuario> SeguirUsuarioIdSeguidoNavigations { get; set; } = new List<SeguirUsuario>();

    public virtual ICollection<SeguirUsuario> SeguirUsuarioIdSeguidorNavigations { get; set; } = new List<SeguirUsuario>();

    public virtual ICollection<SolicitudRol> SolicitudRolIdUsuarioAprovadorNavigations { get; set; } = new List<SolicitudRol>();

    public virtual ICollection<SolicitudRol> SolicitudRolIdUsuarioSolicitudNavigations { get; set; } = new List<SolicitudRol>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
