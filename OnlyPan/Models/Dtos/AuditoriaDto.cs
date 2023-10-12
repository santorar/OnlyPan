namespace OnlyPan.Models.Dtos;

public class AuditoriaDto
{
    public int IdAuditoria { get; set; }

    public int? IdUsuario { get; set; }

    public string? Accion { get; set; }

    public string? Tabla { get; set; }

    public string? Comando { get; set; }

    public DateTime? Fecha { get; set; }
}
