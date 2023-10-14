namespace OnlyPan.Models.Dtos;

public class RegisterDto
{
    public byte[]? Foto { get; set; }
    
    public string Nombre { get; set; } = null!;
    
    public string Correo { get; set; } = null!;
    
    public string Contrasena { get; set; } = null!;

    public string? CodigoActivacion { get; set; }
}