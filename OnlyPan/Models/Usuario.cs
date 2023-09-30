namespace OnlyPan.Models;

public class Usuario
{
  public int IdUsuario { get; set; }

  public int? Rol { get; set; }

  public DateTime? FechaInscrito { get; set; }

  public string? Foto { get; set; }

  public string? Nombre { get; set; }

  public string? Correo { get; set; }

  public string? Contraseña { get; set; }

  public int? Estado { get; set; }

  public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

  public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

  public virtual Estado? EstadoNavigation { get; set; }

  public virtual ICollection<RecetaChef> RecetaChefs { get; set; } = new List<RecetaChef>();

  public virtual ICollection<ReplicaUsuario> ReplicaUsuarios { get; set; } = new List<ReplicaUsuario>();

  public virtual Rol? RolNavigation { get; set; }

  public virtual ICollection<SeguirUsuario> SeguirUsuarioSeguidoNavigations { get; set; } = new List<SeguirUsuario>();

  public virtual ICollection<SeguirUsuario> SeguirUsuarioSeguidorNavigations { get; set; } = new List<SeguirUsuario>();

  public virtual ICollection<SolicitudRol> SolicitudRolUsuarioAprovadorNavigations { get; set; } =
    new List<SolicitudRol>();

  public virtual ICollection<SolicitudRol> SolicitudRolUsuarioSolicitudNavigations { get; set; } =
    new List<SolicitudRol>();

  public virtual ICollection<Valoracion> Valoracions { get; set; } = new List<Valoracion>();
}