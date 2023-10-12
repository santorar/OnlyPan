using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public int? IdUsuario { get; set; }

    public string? Accion { get; set; }

    public string? Tabla { get; set; }

    public string? Comando { get; set; }

    public DateTime? Fecha { get; set; }
}
