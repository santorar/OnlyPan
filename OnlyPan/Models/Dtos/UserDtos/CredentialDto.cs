namespace OnlyPan.Models.Dtos.UserDtos;

public class CredentialDto
{
    public int IdUsuario { get; set; }
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
    public int Rol { get; set; }
}