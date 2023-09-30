namespace OnlyPan.Models;

public class Rol
{
  public int IdRol { get; set; }

  public string? NombreRol { get; set; }

  public virtual ICollection<SolicitudRol> SolicitudRols { get; set; } = new List<SolicitudRol>();

  public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}