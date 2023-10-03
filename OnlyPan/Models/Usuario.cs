using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;

namespace OnlyPan.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int Rol { get; set; }

    public DateTime? FechaInscrito { get; set; }

    public byte[] Foto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int? Estado { get; set; }
    [NotMapped]
    public bool SesionActiva { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Estado? EstadoNavigation { get; set; }

    public virtual ICollection<RecetaChef> RecetaChefs { get; set; } = new List<RecetaChef>();

    public virtual ICollection<ReplicaUsuario> ReplicaUsuarios { get; set; } = new List<ReplicaUsuario>();

    public virtual Rol RolNavigation { get; set; } = null!;

    public virtual ICollection<SeguirUsuario> SeguirUsuarioSeguidoNavigations { get; set; } = new List<SeguirUsuario>();

    public virtual ICollection<SeguirUsuario> SeguirUsuarioSeguidorNavigations { get; set; } = new List<SeguirUsuario>();

    public virtual ICollection<SolicitudRol>? SolicitudRolUsuarioAprovadorNavigations { get; set; } = new List<SolicitudRol>();

    public virtual ICollection<SolicitudRol> SolicitudRolUsuarioSolicitudNavigations { get; set; } = new List<SolicitudRol>();

    public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}
