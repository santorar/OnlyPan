using System;
using System.Collections.Generic;

namespace OnlyPan.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public string Usuario { get; set; }

    public string Accion { get; set; }

    public string Tabla { get; set; }

    public string Sq { get; set; }

    public DateTime Fecha { get; set; }
}
