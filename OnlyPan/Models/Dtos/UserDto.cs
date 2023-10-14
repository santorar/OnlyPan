namespace OnlyPan.Models.Dtos;

public class UserDto
{
    public int IdUsuario { get; set; }
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
    public int Rol { get; set; }
}